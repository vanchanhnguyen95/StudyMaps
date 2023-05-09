using BAGeocoding.Api.Interfaces;
using BAGeocoding.Api.Models.RoadName;
using BAGeocoding.Utility;
using Nest;
using System.ComponentModel;
using static BAGeocoding.Api.Models.RoadName.RoadName;

namespace BAGeocoding.Api.Services
{
    public class RoadNameService : IRoadNameService
    {
        public int NumberOfShards { get; set; } = 5;
        public int NumberOfReplicas { get; set; } = 1;
        private readonly ElasticClient _client;
        private readonly string _indexName;
        private readonly IConfiguration _configuration;
        private readonly IVietNamShapeService _vnShapeService;
        //private readonly ILogService _logService;

        private string GetIndexName()
        {
            var type = typeof(RoadName);

            var customAttributes = (DescriptionAttribute[])type
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (customAttributes != null && customAttributes.Length > 0)
                return customAttributes[0].Description;

            throw new Exception($"{nameof(RoadName)} description attribute is missing.");
        }

        private async Task<bool> IndexAsync(RoadName item, string indexName = null)
        {
            try
            {
                if (string.IsNullOrEmpty(indexName)) indexName = _indexName;

                var response = await _client.IndexAsync(new RoadName(item), q => q.Index(indexName));
                if (response.ApiCall?.HttpStatusCode == 409)
                {
                    await _client.UpdateAsync<RoadName>(item.RoadID.ToString(), a => a.Index(indexName).Doc(new RoadName(item)));
                }
                return response.IsValid;
            }
            catch { return false; }

        }

        public async Task<string> CreateIndex(string indexName)
        {
            try
            {
                if (string.IsNullOrEmpty(indexName)) indexName = _indexName;

                await _client.Indices.DeleteAsync(Indices.Index(indexName));

                var indexResponse = await _client.Indices.CreateAsync(Indices.Index(indexName), c => c
               .Map<RoadName>(mm => mm.AutoMap())
               .Settings(s => s
                   .NumberOfReplicas(NumberOfReplicas)
                   .NumberOfShards(NumberOfShards)
                   .Analysis(a => a
                       .CharFilters(cf => cf
                           .Mapping("programming_language", mca => mca
                               .Mappings(new[]
                               {
                                        "c# => csharp",
                                        "C# => Csharp"
                               })
                           )
                         )
                       .CharFilters(cf => cf
                           .Mapping("province_name", mca => mca
                               .Mappings(new[]
                               {
                                    "hà nội => ha noi",
                                    "Hà Nội => Ha Noi"
                               })
                           )
                         )
                       .TokenFilters(tf => tf
                           .AsciiFolding("ascii_folding", tk => new AsciiFoldingTokenFilter
                           {
                               PreserveOriginal = true
                           })
                           .Synonym("synonym_address", sf => new SynonymTokenFilter
                           {
                               Synonyms = new List<string>()
                               { "ha noi, hà nội, Hà Nội, Ha Noi, hn, hanoi, tp. ha noi, thành phố ha noi",
                                    "tphcm,tp.hcm,tp hồ chí minh,sài gòn, saigon"
                               }
                           })
                           .Stop("stop_filter", st => new StopTokenFilter
                           {
                               StopWords = new List<string>()
                               { "H.","h.","Q.","q.","TP.","tp.","TX.","tx."
                               }
                           })
                       )
                       .Analyzers(an => an
                           .Custom("keyword_analyzer", ca => ca
                               .CharFilters("html_strip")
                               .Tokenizer("keyword")
                               .Filters("lowercase"))
                           .Custom("my_combined_analyzer", ca => ca
                               .CharFilters("html_strip")
                               .Tokenizer("vi_tokenizer")
                               .Filters("lowercase", "stop", "ascii_folding")
                           )
                           .Custom("vn_analyzer3", ca => ca
                               .CharFilters("html_strip")
                               .Tokenizer("vi_tokenizer")
                               .Filters("lowercase", "ascii_folding", "stop_filter")
                           )
                           .Custom("vi_analyzer2", ca => ca
                                .CharFilters("province_name")
                                .Tokenizer("vi_tokenizer")
                                .Filters("lowercase", "ascii_folding")
                            )
                           .Custom("vn_analyzer", ca => ca
                               .CharFilters("html_strip")
                               .Tokenizer("vi_tokenizer")
                               .Filters("lowercase", "ascii_folding")
                           )
                           .Custom("address_analyzer", ca => ca
                               //.CharFilters("html_strip", "province_name")
                               .Tokenizer("vi_tokenizer")
                               .Filters("synonym_address", "lowercase", "ascii_folding")
                           )
                       )
                   )
                )
            );

                return indexResponse.ApiCall.HttpStatusCode.ToString() ?? "OK";
            }
            catch (Exception ex) { return ex.ToString(); }
        }

        private async Task<int> GetProvinceId(double lat = 0, double lng = 0, string keyword = null)
        {
            List<VietNamShape> lst;
            lst = await _vnShapeService.GetDataSuggestion(lat, lng, GeoDistanceType.Arc, "50km", 5, null, GeoShapeRelation.Intersects);

            if (lst.Any())
                return lst.Where(x => x.provinceid > 0).Select(x => x.provinceid).FirstOrDefault();

            return 16;//Hà Nội
        }

        private MatchQuery MatchQuerySuggestion(string query, Field field, Fuzziness fuzziness, string analyzer = "vn_analyzer3")
        {
            return
                new MatchQuery
                {
                    Boost = 2.0,
                    //Operator = Operator.And,
                    //Fuzziness = Fuzziness.Auto,
                    //PrefixLength = 3,
                    //MinimumShouldMatch = 1,
                    //Lenient = true,
                    ZeroTermsQuery = ZeroTermsQuery.None,
                    //AutoGenerateSynonymsPhraseQuery = true,
                    //Name = "world_query",
                    //Boosting = 1.0,
                    //AutoGeneratePhraseQueries = true,
                    //CutoffFrequency = 0.001,

                    AutoGenerateSynonymsPhraseQuery = true,
                    Name = "named_query",
                    //Field = Infer.Field<RoadName>(f => f.KeywordsAsciiNoExt),
                    Field = field,
                    Query = query,
                    Fuzziness = fuzziness,
                    Analyzer = "vn_analyzer3"
                };
        }
        private GeoDistanceQuery GeoDistanceQuerySuggestion(string field, double lat, double lng, string distance)
        {
            return new GeoDistanceQuery
            {
                Boost = 2.0,
                Name = "named_query",
                DistanceType = GeoDistanceType.Arc,
                Field = field,
                Location = new GeoLocation(lat, lng),
                Distance = distance,
                ValidationMethod = GeoValidationMethod.IgnoreMalformed
            };
        }

        public RoadNameService(ElasticClient client, IConfiguration configuration, IVietNamShapeService vnShapeService)
        {
            _client = client;
            _indexName = GetIndexName();
            _configuration = configuration;
            _vnShapeService = vnShapeService;
            //_logService = logService;
        }

        public async Task<string> BulkAsync(List<RoadNamePush> roadPushs)
        {
            try
            {
                //_logService.WriteLog($"BulkAsync Start", LogLevel.Info);
                List<RoadName> roads = new List<RoadName>();

                if (!roadPushs.Any())
                    return "Success - No data to bulk insert";

                roadPushs.ForEach(item => roads.Add(new RoadName(item)));

                //Check xem khởi tạo index chưa, nếu chưa khởi tạo thì phải khởi tạo index mới được
                await CreateIndex(_indexName);

                var bulkAllObservable = _client.BulkAll(roads, b => b
                    .Index(_indexName)
                    // how long to wait between retries
                    .BackOffTime("30s")
                    // how many retries are attempted if a failure occurs
                    .BackOffRetries(2)
                    // refresh the index once the bulk operation completes
                    .RefreshOnCompleted()
                    // how many concurrent bulk requests to make
                    .MaxDegreeOfParallelism(Environment.ProcessorCount)
                    // number of items per bulk request
                    .Size(10000)
                    // decide if a document should be retried in the event of a failure
                    //.RetryDocumentPredicate((item, road) =>
                    //{
                    //    return item.Error.Index == "even-index" && person.FirstName == "Martijn";
                    //})
                    // if a document cannot be indexed this delegate is called
                    .DroppedDocumentCallback(async (bulkResponseItem, road) =>
                    {
                        bool isCreate = true;
                        //isCreate = await CreateAsync(road, _indexName);
                        isCreate = await IndexAsync(road, _indexName);
                        while (isCreate == false)
                        {
                            //isCreate = await IndexAsync(road, _indexName);
                            isCreate = await IndexAsync(road, _indexName);
                            //_logService.WriteLog($"BulkAsync Err, road: {road.RoadID} - {road.RoadName}", LogLevel.Error);
                            //Console.OutputEncoding = Encoding.UTF8;
                            //Console.WriteLine($"{road.ProvinceID} - {road.RoadName}");
                        }
                    })
                    .Timeout(Environment.ProcessorCount)
                    .ContinueAfterDroppedDocuments()
                )
                .Wait(TimeSpan.FromMinutes(15), next =>
                {
                    // do something e.g. write number of pages to console
                });
                //_logService.WriteLog($"BulkAsync End", LogLevel.Info);
                return "Success";
            }
            catch (Exception ex)
            {
                //_logService.WriteLog($"BulkAsync - {ex.ToString()}", LogLevel.Error);
                return "Bulk False";
            }
        }

        // Tìm kiếm theo từ khóa
        public async Task<List<RoadNameOut>> GetDataSuggestion(double lat, double lng, string distance, int size, string keyword, int type)
        {
            try
            {
                List<QueryContainer> must = new List<QueryContainer>();
                List<QueryContainer> filter = new List<QueryContainer>();
                var queryContainerList = new List<QueryContainer>();
                var boolQuery = new BoolQuery();

                if (!string.IsNullOrEmpty(keyword))
                {
                    string? keywordAscii = string.Empty;
                    keyword = keyword.ToLower();

                    keywordAscii = LatinToAscii.Latin2Ascii(keyword.ToLower());

                    queryContainerList.Add(
                        MatchQuerySuggestion(keywordAscii, Infer.Field<RoadName>(f => f.KeywordsAsciiNoExt), Fuzziness.Auto, "vn_analyzer3"));

                    queryContainerList.Add(
                       MatchQuerySuggestion(keywordAscii, Infer.Field<RoadName>(f => f.KeywordsAscii), Fuzziness.EditDistance(0), "vn_analyzer"));

                    queryContainerList.Add(
                      MatchQuerySuggestion(keyword, Infer.Field<RoadName>(f => f.KeywordsAscii), Fuzziness.EditDistance(1), "vn_analyzer"));
                }

                if (lat > 0 && lng > 0)
                {
                    int provinceID = 16;
                    provinceID = await GetProvinceId(lat, lng, null);

                    queryContainerList.Add(new MatchQuery()
                    {
                        Field = "provinceID",
                        Query = provinceID.ToString()
                    }
                     );

                    filter.Add(GeoDistanceQuerySuggestion("location", lat, lng, distance));
                }

                //boolQuery.IsStrict = true;
                boolQuery.Boost = 1.1;
                boolQuery.Must = queryContainerList;
                boolQuery.Filter = filter;

                var searchResponse = await _client.SearchAsync<RoadName>(s => s.Index(_indexName)
                    .Size(size)
                    .MinScore(5.0)
                    .Scroll(1)
                    .Sort(s => s.Descending(SortSpecialField.Score))
                    .Query(q => q
                        .Bool(b => b
                            .Must(boolQuery)
                        )
                    )
                );

                List<RoadNameOut> result = new List<RoadNameOut>();
                if (searchResponse.IsValid)
                {
                    searchResponse.Documents.OrderBy(x => x.Priority).ToList().ForEach(item => result.Add(new RoadNameOut(item)));
                    //_logService.WriteLog($"GetDataSuggestion End - keyword: {keyword}", LogLevel.Info);
                    return result;
                }

                return new List<RoadNameOut>();
            }
            catch
            { return new List<RoadNameOut>(); }
        }
    }
}
