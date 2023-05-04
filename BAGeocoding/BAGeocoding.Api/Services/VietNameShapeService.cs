using BAGeocoding.Api.Interfaces;
using BAGeocoding.Api.Models.RoadName;
using Nest;
using System.ComponentModel;

namespace BAGeocoding.Api.Services
{
    public class VietNameShapeService : IVietNamShapeService
    {

        public int NumberOfShards { get; set; } = 5;
        public int NumberOfReplicas { get; set; } = 1;
        private readonly ElasticClient _client;
        private readonly string _indexName;
        private readonly IConfiguration _configuration;

        private string GetIndexName()
        {
            var type = typeof(VietNamShape);

            var customAttributes = (DescriptionAttribute[])type
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (customAttributes != null && customAttributes.Length > 0)
                return customAttributes[0].Description;

            throw new Exception($"{nameof(VietNamShape)} description attribute is missing.");
        }

        public async Task<string> CreateIndex(string indexName)
        {
            try
            {
                if (string.IsNullOrEmpty(indexName)) indexName = _indexName;

                var indexResponse = await _client.Indices.CreateAsync(Indices.Index(indexName), c => c
               .Map<VietNamShape>(mm => mm.AutoMap())
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
                       .TokenFilters(tf => tf
                           .AsciiFolding("ascii_folding", tk => new AsciiFoldingTokenFilter
                           {
                               PreserveOriginal = true
                           })
                       )
                       .Analyzers(an => an
                           .Custom("keyword_analyzer", ca => ca
                               .CharFilters("programming_language")
                               .Tokenizer("keyword")
                               .Filters("lowercase"))
                           .Custom("vi_analyzer_road", ca => ca
                               .CharFilters("programming_language")
                               .Tokenizer("vi_tokenizer")
                               .Filters("lowercase", "icu_folding", "ascii_folding")
                           )
                       )
                   )
                )
            );

                return indexResponse.ApiCall.HttpStatusCode.ToString() ?? "OK";
            }
            catch (Exception ex) { return ex.ToString(); }
        }

        public async Task<string> BulkAsync(List<VietNamShape> vietNamShapes)
        {
            try
            {
                var existsIndex = await _client.Indices.ExistsAsync(_indexName);
                if (!existsIndex.Exists)
                    await CreateIndex(_indexName);

                var bulkAllObservable = _client.BulkAll(vietNamShapes, b => b
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
                        .Size(1000)
                        // decide if a document should be retried in the event of a failure
                        //.RetryDocumentPredicate((item, road) =>
                        //{
                        //    return item.Error.Index == "even-index" && person.FirstName == "Martijn";
                        //})
                        // if a document cannot be indexed this delegate is called
                        .DroppedDocumentCallback(async (bulkResponseItem, vietnam) =>
                        {
                            bool isCreate = true;

                            isCreate = await CreateVietNamAsync(vietnam, _indexName);
                            while (isCreate == false)
                            {
                                Console.WriteLine($"Create: {vietnam.id}");
                                isCreate = await CreateVietNamAsync(vietnam, _indexName);
                            }
                        })
                        .ContinueAfterDroppedDocuments()
                    )
                    .Wait(TimeSpan.FromMinutes(15), next =>
                    {
                        // do something e.g. write number of pages to console
                    });

                return "Success";
            }
            catch (Exception ex) { return ex.ToString(); };
        }

        /// <summary>
        /// Thêm mới dữ liệu đường Hà Nội
        /// </summary>
        /// <param name="roadPoint"></param>
        /// <param name="indexName"></param>
        /// <returns></returns>
        private async Task<bool> CreateVietNamAsync(VietNamShape item, string indexName = null)
        {
            try
            {
                if (string.IsNullOrEmpty(indexName)) indexName = _indexName;

                var response = await _client.CreateAsync(new VietNamShape(item), q => q.Index(indexName));
                if (response.ApiCall?.HttpStatusCode == 409)
                {
                    await _client.UpdateAsync<VietNamShape>(item.id.ToString(), a => a.Index(indexName).Doc(new VietNamShape(item)));
                }
                return response.IsValid;
            }
            catch (Exception ex) { return false; }

        }

        private async Task<bool> IndexVietNamAsync(VietNamShape item, string indexName = null)
        {
            try
            {
                if (string.IsNullOrEmpty(indexName)) indexName = _indexName;

                var response = await _client.IndexAsync(new VietNamShape(item), q => q.Index(indexName));
                if (response.ApiCall?.HttpStatusCode == 409)
                {
                    await _client.UpdateAsync<VietNamShape>(item.id.ToString(), a => a.Index(indexName).Doc(new VietNamShape(item)));
                }
                return response.IsValid;
            }
            catch (Exception ex) { return false; }

        }

        // Tìm kiếm theo tọa độ
        private async Task<List<VietNamShape>> GetDataByLocation(double lat, double lng, GeoDistanceType type, string distance, int size, GeoShapeRelation relation)
        {
            try
            {
                GeoCoordinate point = new GeoCoordinate(lat, lng);

                var geo = await _client.SearchAsync<VietNamShape>(
                    s => s.Index(_indexName)
                   .Size(size)
                   .PostFilter(q => q.GeoShape(g =>
                            g.Field(f => f.location)
                             .Name("named_query").Boost(1.1)
                                .Shape(s =>
                                 s.Point(point)
                            )
                            //.Relation(GeoShapeRelation.Intersects)
                            .Relation(relation)
                            .IgnoreUnmapped()
                       )
                    )
                   .Sort(s => s.Descending(SortSpecialField.Score))
                   .Scroll(1)
                   );

                return geo.Documents.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> AddAsync(List<VietNamShape> vietNamShapes)
        {
            var existsIndex = await _client.Indices.ExistsAsync(_indexName);
            if (!existsIndex.Exists)
                await CreateIndex(_indexName);

            if (!vietNamShapes.Any())
                return "Fail";

            foreach (VietNamShape vietnam in vietNamShapes)
            {
                bool isCreate = true;

                isCreate = await CreateVietNamAsync(vietnam, _indexName);
                while (isCreate == false)
                {
                    Console.WriteLine($"Create: {vietnam.id}");
                    isCreate = await IndexVietNamAsync(vietnam, _indexName);
                }
            }

            return "Success";

        }

        // Tìm kiếm theo từ khóa
        private async Task<List<VietNamShape>> GetDataByKeyWord(int size, string keyword)
        {
            try
            {
                var geo = await _client.SearchAsync<VietNamShape>(s => s.Index(_indexName)
                   .Size(size)
                   .Query(q => q.Bool(
                        b => b.Must(mu => mu.Match(ma =>
                        ma.Field(f => f.keywords).Analyzer("vi_analyzer_road").Query(keyword).Fuzziness(Fuzziness.Auto)
                        .AutoGenerateSynonymsPhraseQuery()
                        )
                        && mu.Match(ma =>
                        ma.Field(f => f.name).Analyzer("vi_analyzer_road").Query(keyword)
                        .AutoGenerateSynonymsPhraseQuery()
                        )
                    )))
                   .Sort(s => s.Descending(SortSpecialField.Score))
                   .Scroll(1)
                   );

                return geo.Documents.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        // Tìm kiếm theo tọa độ và từ khóa
        private async Task<List<VietNamShape>> GetDataByLocationKeyWord(double lat, double lng, GeoDistanceType type, string distance, int size, string keyword)
        {
            try
            {
                GeoCoordinate point = new GeoCoordinate(lat, lng);

                var geo = await _client.SearchAsync<VietNamShape>(s => s.Index(_indexName)
                   .Size(size)
                   .Query(q => q.Bool(
                        b => b.Must(mu => mu.Match(ma =>
                        ma.Field(f => f.keywords).Analyzer("vi_analyzer_road").Query(keyword).Fuzziness(Fuzziness.Auto)
                        .AutoGenerateSynonymsPhraseQuery()
                        )
                        && mu.Match(ma =>
                        ma.Field(f => f.name).Analyzer("vi_analyzer_road").Query(keyword)
                        .AutoGenerateSynonymsPhraseQuery()
                        )
                    ).Filter(fi => fi
                            //.GeoShape(s => s.Field(f => f.location).Shape(s => s.Envelope(point, point)))
                            .GeoShape(g => g
                                .Field(f => f.location)
                                .Relation(GeoShapeRelation.Intersects)
                                .Shape(s => s.Point(point)
                                ).Relation(GeoShapeRelation.Intersects).IgnoreUnmapped()
                            )
                            )
                    ))
                   .Sort(s => s.Descending(SortSpecialField.Score))
                   .Scroll(1)
                   );

                var geo2 = _client.Search<VietNamShape>(s => s
                 .Index(_indexName)
                 .Size(size)
                    .Query(q => q
                    .Bool(b => b
                        .Must(mu => mu
                            .Match(m => m
                                .Field(f => f.keywords)
                                .Query(keyword)
                                .Fuzziness(Fuzziness.Auto)
                                .AutoGenerateSynonymsPhraseQuery()
                            ) &&
                            mu.Match(ma =>
                             ma.Field(f => f.name).Query(keyword)
                            .AutoGenerateSynonymsPhraseQuery()
                            )
                        )
                        .Filter(fi => fi
                            //.GeoShape(s => s.Field(f => f.location).Shape(s => s.Envelope(point, point)))
                            .GeoShape(g => g
                                .Field(f => f.location)
                                .Relation(GeoShapeRelation.Intersects)
                                .Shape(s => s.Point(point)
                                ).Relation(GeoShapeRelation.Intersects).IgnoreUnmapped()
                            )
                        )
                    )
                ).Sort(s => s.Descending(SortSpecialField.Score))
                );

                var geo3 = await _client.SearchAsync<VietNamShape>(s => s.Index(_indexName)
                   .Size(size)
                   .Query(q => q.Bool(
                        b => b.Must(mu => mu.Match(ma =>
                        ma.Field(f => f.keywords).Query(keyword).Fuzziness(Fuzziness.Auto)
                        .AutoGenerateSynonymsPhraseQuery())
                        && mu.Match(ma =>
                        //ma.Field(f => f.name).Analyzer("vi_analyzer").Query(keyword)
                        ma.Field(f => f.name).Query(keyword)
                        .AutoGenerateSynonymsPhraseQuery())
                    )))
                   .PostFilter(q => q.GeoDistance(
                        g => g.Boost(1.1).Name("named_query")
                        .Field(p => p.location).DistanceType(type).Location(lat, lng)
                        .Distance(distance).ValidationMethod(GeoValidationMethod.IgnoreMalformed)
                    ))
                   .Sort(s => s.Descending(SortSpecialField.Score))
                   .Scroll(1)
                   );

                var geoGeometry = _client.Search<VietNamShape>(s => s
                 .Index(_indexName)
                 .Size(size)
                .Query(q => q
                    .Bool(b => b
                        .Must(mu => mu
                            .Match(m => m
                                .Field(f => f.keywords)
                                .Query(keyword)
                                .Fuzziness(Fuzziness.Auto)
                                .AutoGenerateSynonymsPhraseQuery()
                            ) &&
                            mu.Match(ma =>
                             ma.Field(f => f.name).Query(keyword)
                            .AutoGenerateSynonymsPhraseQuery()
                            )
                        ).Should(
                            sh => sh.GeoShape(
                                g => g.Field(f => f.location)
                                .Relation(GeoShapeRelation.Intersects)
                                .Shape(s => s.Point(point)
                            )
                        ))
                    )
                )
            );

                var searchResponse = _client.Search<VietNamShape>(s => s
                .Index(_indexName)
                 .Size(size)
                .Query(q => q
                    .GeoShape(g => g
                        .Field(f => f.location)
                        .Relation(GeoShapeRelation.Intersects)
                        .Shape(s => s.Point(point))
                    )
                )
            );

                return geo.Documents.ToList();
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<VietNamShape>> GetDataSuggestion(double lat, double lng, GeoDistanceType type, string distance, int size, string keyword, GeoShapeRelation relation)
        {
            try
            {
                List<VietNamShape> res = new List<VietNamShape>();
                // Tìm kiếm theo tọa độ
                if (string.IsNullOrEmpty(keyword))
                {
                    res = await GetDataByLocation(lat, lng, type, distance, size, relation);
                }
                // Tìm kiếm theo từ khóa
                else if (lat == 0 && lng == 0)
                {
                    res = await GetDataByKeyWord(size, keyword);
                }
                // Tìm kiếm theo tọa độ và từ khóa
                else
                {
                    res = await GetDataByLocationKeyWord(lat, lng, type, distance, size, keyword);
                }

                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public VietNameShapeService(ElasticClient client, IConfiguration configuration)
        {
            _client = client;
            _indexName = GetIndexName();
            _configuration = configuration;
        }
    }
}
