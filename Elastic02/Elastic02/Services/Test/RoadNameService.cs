using Elastic02.Models.Test;
using Elastic02.Utility;
using Nest;
using System.Collections.Generic;
using System.ComponentModel;

namespace Elastic02.Services.Test
{
    public class RoadNameService : IRoadNameService
    {
        public int NumberOfShards { get; set; } = 5;
        public int NumberOfReplicas { get; set; } = 1;
        private readonly ElasticClient _client;
        private readonly string _indexName;
        private readonly IConfiguration _configuration;
        private readonly IVietNamShapeService _vnShapeService;
        private readonly ILogService _logService;

        private string GetIndexName()
        {
            var type = typeof(RoadName);

            var customAttributes = (DescriptionAttribute[])type
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (customAttributes != null && customAttributes.Length > 0)
                return customAttributes[0].Description;

            throw new Exception($"{nameof(RoadName)} description attribute is missing.");
        }

        public RoadNameService(ElasticClient client, IConfiguration configuration, IVietNamShapeService vnShapeService, ILogService logService)
        {
            _client = client;
            _indexName = GetIndexName();
            _configuration = configuration;
            _vnShapeService = vnShapeService;
            _logService = logService;
        }

        public async Task<string> BulkAsync(List<RoadNamePush> roadPushs)
        {
            try
            {
                _logService.WriteLog($"BulkAsync Start", LogLevel.Info);
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
                            _logService.WriteLog($"BulkAsync Err, road: {road.RoadID} - {road.RoadName}", LogLevel.Error);
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
                _logService.WriteLog($"BulkAsync End", LogLevel.Info);
                return "Success";
            }
            catch (Exception ex)
            {
                _logService.WriteLog($"BulkAsync - {ex.ToString()}", LogLevel.Error);
                return "Bulk False";
            }
        }

        public async Task<string> BulkAsyncByProvince(List<RoadName> roads, string indexName = null)
        {
            try
            {
                if (!roads.Any())
                    return "Success - No data bulk";

                // Check xem khởi tạo index chưa, nếu chưa khởi tạo thì phải khởi tạo index mới được
                await CreateIndex(indexName);

                var bulkAllObservable = _client.BulkAll(roads, b => b
                    .Index(indexName)
                    // how long to wait between retries
                    .BackOffTime("30s")
                    // how many retries are attempted if a failure occurs
                    .BackOffRetries(2)
                    // refresh the index once the bulk operation completes
                    .RefreshOnCompleted()
                    // how many concurrent bulk requests to make
                    .MaxDegreeOfParallelism(Environment.ProcessorCount)
                    // number of items per bulk request
                    .Size(20000)
                    // decide if a document should be retried in the event of a failure
                    //.RetryDocumentPredicate((item, road) =>
                    //{
                    //    return item.Error.Index == "even-index" && person.FirstName == "Martijn";
                    //})
                    // if a document cannot be indexed this delegate is called
                    .DroppedDocumentCallback(async (bulkResponseItem, road) =>
                    {
                        bool isCreate = true;
                        isCreate = await CreateAsync(road, indexName);
                        while (isCreate == false)
                        {
                            isCreate = await IndexAsync(road, indexName);
                            //Console.OutputEncoding = Encoding.UTF8;
                            //Console.WriteLine($"{road.RoadName}");
                        }
                    })
                    .Timeout(Environment.ProcessorCount)
                    .ContinueAfterDroppedDocuments()
                )
                .Wait(TimeSpan.FromMinutes(15), next =>
                {
                    // do something e.g. write number of pages to console
                });

                return "Success";
            }
            catch (Exception ex)
            {
                _logService.WriteLog($"BulkAsync - {ex.ToString()}", LogLevel.Error);
                return ex.ToString();
            }
        }

        public async Task<List<RoadNamePush>> GetDataSuggestion(double lat, double lng, GeoDistanceType type, string distance, int size, string keyword, GeoShapeRelation relation)
        {
            try
            {
                List<RoadName> res = new List<RoadName>();
                List<RoadNamePush> result = new List<RoadNamePush>();

                //res = await GetDataByKeyWord(size, keyword);

                //Tìm kiếm theo tọa độ
                if (string.IsNullOrEmpty(keyword))
                {
                    //int provinceid = await GetProvinceId(lat, lng, null);
                    //res = await GetDataByLocation(lat, lng, distance, size, provinceid);
                    res = await GetDataByLocation(lat, lng, distance, size);
                }
                // Tìm kiếm theo từ khóa
                else if (lat == 0 && lng == 0)
                {
                    res = await GetDataByKeyWord(size, keyword);
                }
                // Tìm kiếm theo tọa độ và từ khóa
                else
                {
                    //int provinceid = await GetProvinceId(lat, lng, null);
                    res = await GetDataByLocationKeyWord(lat, lng, distance, size, keyword);
                }

                if (res.Any())
                    res.ForEach(item => result.Add(new RoadName(item)));

                return result;
                //return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<RoadNamePush>> GetDataSuggestion(double lat, double lng, string distance, int size, string keyword)
        {
            try
            {
                //_logService.WriteLog($"GetDataSuggestion Start - keyword: {keyword}", LogLevel.Info);

                List<RoadName> res = new List<RoadName>();
                List<RoadNamePush> result = new List<RoadNamePush>();

                // Tìm kiếm theo tọa độ
                if (string.IsNullOrEmpty(keyword))
                {
                    res = await GetDataByLocation(lat, lng, distance, size);
                }
                // Tìm kiếm theo từ khóa
                else if (lat == 0 && lng == 0)
                {
                    res = await GetDataByKeyWord(size, keyword);
                }
                // Tìm kiếm theo tọa độ và từ khóa
                else
                {
                    res = await GetDataByLocationKeyWord(lat, lng, distance, size, keyword);
                }

                if (res.Any())
                    res.ForEach(item => result.Add(new RoadName(item)));

                //_logService.WriteLog($"GetDataSuggestion End - keyword: {keyword}", LogLevel.Info);
                return result;

            }
            catch(Exception ex)
            {
                _logService.WriteLog($"GetDataSuggestion - keyword: {keyword} Err - {ex}", LogLevel.Error);
                return new List<RoadNamePush>();
            }
        }

        public async Task<List<RoadNamePush>> GetDataSuggestion(double lat, double lng, string distance, int size, string keyword, int type)
        {
            try
            {
                //_logService.WriteLog($"GetDataSuggestion Start - keyword: {keyword}", LogLevel.Info);

                List<RoadName> res = new List<RoadName>();
                List<RoadNamePush> result = new List<RoadNamePush>();

                // Tìm kiếm theo tọa độ
                if (string.IsNullOrEmpty(keyword))
                {
                    res = await GetDataByLocation(lat, lng, distance, size, type);
                }
                // Tìm kiếm theo từ khóa
                else if (lat == 0 && lng == 0)
                {
                    res = await GetDataByKeyWord(size, keyword, type);
                }
                // Tìm kiếm theo tọa độ và từ khóa
                else
                {
                    res = await GetDataByLocationKeyWord(lat, lng, distance, size, keyword, type);
                }

                if (res.Any())
                    res.ForEach(item => result.Add(new RoadName(item)));

                //_logService.WriteLog($"GetDataSuggestion End - keyword: {keyword}", LogLevel.Info);
                return result;

            }
            catch (Exception ex)
            {
                _logService.WriteLog($"GetDataSuggestion - keyword: {keyword} Err - {ex}", LogLevel.Error);
                return new List<RoadNamePush>();
            }
        }

        // Tìm kiếm theo tọa độ
        private async Task<List<RoadName>> GetDataByLocationOld(double lat, double lng, string distance, int size)
        {
            try
            {
                int provinceid = 16;

                if (lat != 0 && lng != 0)
                    provinceid = await GetProvinceId(lat, lng, null);

                var geo = await _client.SearchAsync<RoadName>(s => s.Index(_indexName)
                   .Size(size)
                   .Sort(s => s
                        //.Ascending(f => f.NameExt)
                        .Ascending(SortSpecialField.Score)
                        .GeoDistance(
                        d => d
                        //.Field(f => f.Location)
                        .Points(new GeoLocation(lat, lng))
                        .Order(SortOrder.Ascending)
                        .Unit(DistanceUnit.Kilometers)
                        .Mode(SortMode.Min)
                        .DistanceType(GeoDistanceType.Arc)
                        .IgnoreUnmapped()
                        )
                   )
                   .PostFilter(q => q.GeoDistance(
                        g => g
                        .Distance(distance)
                        .DistanceType(GeoDistanceType.Arc)
                        .Location(lat, lng)
                        .ValidationMethod(GeoValidationMethod.IgnoreMalformed)
                        .IgnoreUnmapped()
                        .Boost(1.1)
                        .Name("named_query")
                    //.Field(p => p.Location)

                    ))

                   .Scroll(1)
                   );

                return geo.Documents.ToList();
            }
            catch
            {
                return null;
            }
        }

        private async Task<List<RoadName>> GetDataByLocation(double lat, double lng, string distance, int size, int type)
        {
            try
            {
                int provinceID = 16;

                if (lat != 0 && lng != 0)
                    provinceID = await GetProvinceId(lat, lng, null);

                List<RoadName> result = new List<RoadName>();

                if (type == -1)
                {
                    var result1 = await _client.SearchAsync<RoadName>(s => s.Index(_indexName)
                       .Size(size)
                       .Query(q => q.Bool(
                           b => b.Must(mu => mu.Match(ma =>
                                ma.Field(f => f.ProvinceID).Query(provinceID.ToString())
                           )
                        ))
                       )
                       .PostFilter(q => q.GeoDistance(
                            g => g
                            .Distance(distance)
                            .DistanceType(GeoDistanceType.Arc)
                            .Location(lat, lng)
                            .ValidationMethod(GeoValidationMethod.IgnoreMalformed)
                            .IgnoreUnmapped()
                            .Boost(1.1)
                            .Name("named_query")
                            .Field(p => p.Location)
                        ))
                       .Sort(s => s.Descending(SortSpecialField.Score).Ascending(f => f.NameExt)
                                .GeoDistance(
                                            d => d
                                            .Field(f => f.Location)
                                            .Order(SortOrder.Ascending).Points(new GeoLocation(lat, lng))
                                            .Unit(DistanceUnit.Meters).Mode(SortMode.Min)
                                            )
                       )
                       .MinScore(5.0)
                       .Scroll(1)
                       );

                    result = result1.Documents.ToList().OrderBy(x => x.Priority).ToList();
                    return result;
                }

                return await GetDataByLocation(lat, lng, distance, size);
            }
            catch
            {
                return new List<RoadName>();
            }
        }

        private async Task<List<RoadName>> GetDataByLocation(double lat, double lng, string distance, int size)
        {
            try
            {
                int provinceID = 16;

                if (lat != 0 && lng != 0)
                    provinceID = await GetProvinceId(lat, lng, null);

                var result = await _client.SearchAsync<RoadName>(s => s.Index(_indexName)
                   .Size(size)
                   .Query(q => q.Bool(
                       b => b.Must(mu => mu.Match(ma =>
                            ma.Field(f => f.ProvinceID).Query(provinceID.ToString())
                       )
                    ))
                   )
                   .PostFilter(q => q.GeoDistance(
                        g => g
                        .Distance(distance)
                        .DistanceType(GeoDistanceType.Arc)
                        .Location(lat, lng)
                        .ValidationMethod(GeoValidationMethod.IgnoreMalformed)
                        .IgnoreUnmapped()
                        .Boost(1.1)
                        .Name("named_query")
                        .Field(p => p.Location)
                    ))
                   .Sort(s => s.Descending(SortSpecialField.Score).Ascending(f => f.NameExt)
                            .GeoDistance(
                                        d => d
                                        .Field(f => f.Location)
                                        .Order(SortOrder.Ascending).Points(new GeoLocation(lat, lng))
                                        .Unit(DistanceUnit.Meters).Mode(SortMode.Min)
                                        )
                   )
                   .MinScore(5.0)
                   .Scroll(1)
                   );

                return result.Documents.ToList();
            }
            catch
            {
                return new List<RoadName>();
            }
        }

        private Job ConvertDoc(IHit<Job> hit)
        {
            Job u = new Job();

            try
            {
                u = hit.Source;
                u.JobId = hit.Id;
            }
            catch
            {
            }
            return u;
        }

        // Tìm kiếm theo từ khóa
        private async Task<List<RoadName>> GetDataByKeyWord2(int size, string keyword)
        {
            try
            {
                string? keywordAscii = string.Empty;
                keyword = keyword.ToLower();

                if (!string.IsNullOrEmpty(keyword))
                    keywordAscii = LatinToAscii.Latin2Ascii(keyword.ToLower());

                List<Job> lst = new List<Job>();

                var searchRequest = new SearchRequest<RoadName>(_indexName);


                List<QueryContainer> must = new List<QueryContainer>();

                var queryContainerList = new List<QueryContainer>();

                var matchQuery1 = new MatchQuery
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
                    //Analyzer = "standard",
                    //AutoGeneratePhraseQueries = true,
                    //CutoffFrequency = 0.001,


                    AutoGenerateSynonymsPhraseQuery = true,
                    Name = "named_query",
                    Field = Infer.Field<RoadName>(f => f.Keywords),
                    Query = keyword,
                    Fuzziness = Fuzziness.EditDistance(0),
                    Analyzer = "vn_analyzer"
                };

                var matchQuery2 = new MatchQuery
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
                    //Analyzer = "standard",
                    //AutoGeneratePhraseQueries = true,
                    //CutoffFrequency = 0.001,

                    AutoGenerateSynonymsPhraseQuery = true,
                    Name = "named_query",
                    Field = Infer.Field<RoadName>(f => f.KeywordsAscii),
                    Query = keywordAscii,
                    Fuzziness = Fuzziness.EditDistance(1),
                    Analyzer = "vn_analyzer"

                };

                var matchQuery3 = new MatchQuery
                {
                    AutoGenerateSynonymsPhraseQuery = true,
                    Name = "named_query",
                    Field = Infer.Field<RoadName>(f => f.KeywordsAscii),
                    Query = keywordAscii,
                    Fuzziness = Fuzziness.EditDistance(1),
                    Analyzer = "vn_analyzer"

                };

                queryContainerList.Add(
                    matchQuery1
                    &&
                    matchQuery2
                );

                var sort = new List<ISort>
                {
                    new FieldSort { Field = "_score", Order = SortOrder.Descending }
                };

                var boolQuery = new BoolQuery
                {
                    IsStrict = true,
                    Must = queryContainerList,
                    Boost = 1.1
                };


                searchRequest.Sort = sort;
                searchRequest.From = 0;
                searchRequest.Size = size;
                searchRequest.TrackTotalHits = true;
                searchRequest.Query = boolQuery;

                var searchResponse = await _client.SearchAsync<RoadName>(searchRequest);
                if (searchResponse.IsValid)
                    return searchResponse.Documents.ToList();



                return new List<RoadName>();
            }
            catch
            { return new List<RoadName>(); }
        }

        private async Task<List<RoadName>> GetDataByKeyWord(int size, string keyword, int type)
        {
            try
            {
                string keywordAscii = string.Empty;

                if (!string.IsNullOrEmpty(keyword))
                    keywordAscii = LatinToAscii.Latin2Ascii(keyword.ToLower());

                if (type == -1)
                    return await GetDataByKeyWordNoNameExt(size, keyword);

                return await GetDataByKeyWord(size, keyword);
            }
            catch
            {
                return new List<RoadName>();
            }
        }

        private async Task<List<RoadName>> GetDataByKeyWordNoNameExt(int size, string keyword)
        {
            try
            {
                string keywordAscii = string.Empty;

                if (!string.IsNullOrEmpty(keyword))
                    keywordAscii = LatinToAscii.Latin2Ascii(keyword.ToLower());

                var result = await _client.SearchAsync<RoadName>(s => s.Index(_indexName)
                    .Size(size)
                    .Query(q => q.Bool(
                        b => b.Must(mu =>
                        mu.Match(ma =>
                        ma.Field(f => f.KeywordsAsciiNoExt).Name("named_query").Analyzer("vn_analyzer3").Query(keywordAscii).Fuzziness(Fuzziness.Auto)
                        .AutoGenerateSynonymsPhraseQuery()
                        )
                        &&
                        mu.Match(ma =>
                        ma.Field(f => f.KeywordsAscii).Name("named_query").Analyzer("vn_analyzer").Query(keywordAscii).Fuzziness(Fuzziness.EditDistance(0))
                        .AutoGenerateSynonymsPhraseQuery()
                        )
                        && mu.Match(ma =>
                        ma.Field(f => f.Keywords).Name("named_query").Analyzer("vn_analyzer").Query(keyword).Fuzziness(Fuzziness.EditDistance(1))
                        .AutoGenerateSynonymsPhraseQuery()
                        )

                        //&& mu.Match(ma =>
                        //ma.Field(f => f.KeywordsNoExt).Name("named_query").Analyzer("vn_analyzer").Query(keyword).Fuzziness(Fuzziness.Auto)
                        //.AutoGenerateSynonymsPhraseQuery()
                        //)
                        //&& mu.Match(ma =>
                        //ma.Field(f => f.RoadName).Name("named_query").Analyzer("vn_analyzer").Query(keyword)
                        //.AutoGenerateSynonymsPhraseQuery()
                        //)
                        )

                     )
                    )
                   .MinScore(5.0)
                   .Sort(s => s.Descending(SortSpecialField.Score))
                   .Scroll(1));
                List<RoadName> res = result.Documents.ToList();
                return res.OrderBy(x => x.Priority).ToList();
            }
            catch
            {
                return new List<RoadName>();
            }
        }

        private async Task<List<RoadName>> GetDataByKeyWord(int size, string keyword)
        {
            try
            {
                string keywordAscii = string.Empty;

                if (!string.IsNullOrEmpty(keyword))
                    keywordAscii = LatinToAscii.Latin2Ascii(keyword.ToLower());

                var geo = await _client.SearchAsync<RoadName>(s => SearchByKeyword(size, keyword, s, keywordAscii) );

                // Tìm lại lần nữa nếu kết quả  !IsValid
                if (!geo.IsValid)
                {
                    var geoAgain = await _client.SearchAsync<RoadName>(s => SearchByKeyword(size, keyword, s, keywordAscii));
                    return geoAgain.Documents.ToList();
                }

                return geo.Documents.ToList();
            }
            catch
            {
                return new List<RoadName>();
            }
        }

        private SearchDescriptor<RoadName> SearchByKeyword(int size, string keyword, SearchDescriptor<RoadName> s, string keywordAscii)
        {
            return s.Index(_indexName)
                    .Size(size)
                    .Query(q => q.Bool(
                        b => b.Must(mu =>
                        mu.Match(ma =>
                        ma.Field(f => f.KeywordsAscii).Name("named_query").Analyzer("vn_analyzer").Query(keywordAscii).Fuzziness(Fuzziness.EditDistance(0))
                        .AutoGenerateSynonymsPhraseQuery()
                        )
                        && mu.Match(ma =>
                        ma.Field(f => f.Keywords).Name("named_query").Analyzer("vn_analyzer").Query(keyword).Fuzziness(Fuzziness.EditDistance(1))
                        .AutoGenerateSynonymsPhraseQuery()
                        )
                        )
                    )
                    //    .Should(sh => sh
                    //    .Match(ma => ma.Field(f => f.RoadName)
                    //    .Name("named_query").Analyzer("vn_analyzer").Query(keyword)
                    //    //.Fuzziness(Fuzziness.EditDistance(1)).AutoGenerateSynonymsPhraseQuery()
                    //    //    )
                    //    )))
                    )
                   .MinScore(5.0)
                   .Sort(s => s.Descending(SortSpecialField.Score))
                   .Scroll(1);
        }

        // Tìm kiếm theo tọa độ và từ khóa
        private async Task<List<RoadName>> GetDataByLocationKeyWord(double lat, double lng, string distance, int size, string keyword, int type)
        {
            try
            {
                if (type == -1)
                    return await GetDataByLocationKeyWordType(lat, lng, distance, size, keyword);

                return await GetDataByLocationKeyWord(lat, lng, distance, size, keyword);
            }
            catch
            {
                return null;
            }
        }

        private async Task<List<RoadName>> GetDataByLocationKeyWord(double lat, double lng, string distance, int size, string keyword)
        {
            try
            {
                var result = await _client.SearchAsync<RoadName>(s => s.Index(_indexName)
                   .Size(size)
                   .Query(q => q.Bool(
                        b => b.Must(mu => mu.Match(ma =>
                        ma.Field(f => f.KeywordsAscii).Name("named_query").Query(keyword).Analyzer("vn_analyzer").Fuzziness(Fuzziness.Auto)
                        .AutoGenerateSynonymsPhraseQuery())
                        && mu.Match(ma =>
                        ma.Field(f => f.RoadName).Query(keyword)
                        .AutoGenerateSynonymsPhraseQuery())
                    )))
                   .PostFilter(q => q.GeoDistance(
                        g => g.Boost(1.1).Name("named_query")
                        .Field(p => p.Location)
                        .DistanceType(GeoDistanceType.Arc).Location(lat, lng)
                        .Distance(distance).ValidationMethod(GeoValidationMethod.IgnoreMalformed)
                    ))
                   .Sort(s => s.Descending(SortSpecialField.Score))
                   .MinScore(5.0)
                   .Scroll(1)
                   );

                return result.Documents.ToList();
            }
            catch
            {
                return null;
            }
        }

        private async Task<List<RoadName>> GetDataByLocationKeyWordType(double lat, double lng, string distance, int size, string keyword)
        {
            try
            {
                string keywordAscii = string.Empty;

                if (!string.IsNullOrEmpty(keyword))
                    keywordAscii = LatinToAscii.Latin2Ascii(keyword.ToLower());

                int provinceID = 16;

                if (lat != 0 && lng != 0)
                {
                    provinceID = await GetProvinceId(lat, lng, null);
                }

                List<RoadName> res = new List<RoadName>();

                var result = await _client.SearchAsync<RoadName>(s => s.Index(_indexName)
                                       .Size(size)
                                       .Query(q => q.Bool(
                                            b => b.Must(mu => mu.Match(ma =>
                                            ma.Field(f => f.KeywordsAscii).Name("named_query").Query(keywordAscii).Analyzer("vn_analyzer").Fuzziness(Fuzziness.Auto)
                                            .AutoGenerateSynonymsPhraseQuery())
                                            //&& mu.Match(ma =>
                                            //ma.Field(f => f.RoadName).Query(keyword)
                                            //.AutoGenerateSynonymsPhraseQuery()
                                            //)
                                            && mu.Match(ma =>
                                            ma.Field(f => f.ProvinceID).Query(provinceID.ToString()))

                                        )))
                                       .PostFilter(q => q.GeoDistance(
                                            g => g.Boost(1.1).Name("named_query")
                                            .Field(p => p.Location)
                                            .DistanceType(GeoDistanceType.Arc).Location(lat, lng)
                                            .Distance(distance).ValidationMethod(GeoValidationMethod.IgnoreMalformed)
                                        ))
                                       .MinScore(5.0)
                                       .Sort(s => s.Descending(SortSpecialField.Score))
                                       //.Scroll(1)
                                       );
                res = result.Documents.ToList();

                return res.OrderBy(x => x.Priority).ToList();
            }
            catch
            {
                return null;
            }
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

        private async Task<bool> CreateAsync(RoadName item, string indexName = null)
        {
            try
            {
                if (string.IsNullOrEmpty(indexName)) indexName = _indexName;

                var response = await _client.CreateAsync(new RoadName(item), q => q.Index(indexName));
                if (response.ApiCall?.HttpStatusCode == 409)
                {
                    await _client.UpdateAsync<RoadName>(item.RoadID.ToString(), a => a.Index(indexName).Doc(new RoadName(item)));
                }
                return response.IsValid;
            }
            catch (Exception ex) { return false; }

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

        private async Task<int> GetProvinceId(double lat = 0, double lng = 0, string keyword = null)
        {
            List<VietNamShape> lst;
            lst = await _vnShapeService.GetDataSuggestion(lat, lng, GeoDistanceType.Arc, "50km", 5, null, GeoShapeRelation.Intersects);

            if (lst.Any())
                return lst.Where(x => x.provinceid > 0).Select(x => x.provinceid).FirstOrDefault();

            return 16;//Hà Nội
        }

        public async Task<List<RoadName>> GetRouting(GeoLocation poingStart, GeoLocation pointEnd, int size)
        {
            try
            {
                //poingStart = new GeoLocation(20.97263381, 105.77930601);
                //pointEnd = new GeoLocation(20.99874272, 105.81312923);

                //var searchResponse = await _client.SearchAsync<RoadName>(s => s
                //     .Index(_indexName)
                //     .Query(q => q
                //        .GeoDistance(g => g
                //            .Field(f => f.Location)
                //            .Distance("10km")
                //            //.Location(new GeoLocation(37.7765, -122.4144))
                //            .Location(poingStart)

                //        ) && q
                //        .GeoDistance(g => g
                //            .Field(f => f.Location)
                //            .Distance("10km")
                //            //.Location(new GeoLocation(37.7749, -122.4194))
                //            .Location(pointEnd)
                //        )
                //    )
                //);

                var searchResponse = await _client.SearchAsync<RoadName>(s => s
                    .Size(100)
                    .Index(_indexName)
                    .Sort(sort => sort
                        .GeoDistance(gd => gd.DistanceType(GeoDistanceType.Arc)
                            .Field(f => f.Location)
                            .Points(poingStart, pointEnd)
                            .Unit(DistanceUnit.Kilometers)
                            .Order(SortOrder.Ascending)
                        )
                    )
                );

                var searchResponse2 = await _client.SearchAsync<RoadName>(s => s
                .Index(_indexName)
                .Query(q => q
                    .GeoDistance(g => g
                        .Field(f => f.Location) // the field containing the geo points
                        .DistanceType(GeoDistanceType.Arc)
                        .ValidationMethod(GeoValidationMethod.IgnoreMalformed)
                        .Location(poingStart)
                        .Distance("10km")
                        .Location(pointEnd))
                )
                );

                //var path = searchResponse.Hits.FirstOrDefault()?.Source.Location;
                var path2 = searchResponse2.Documents.ToList();

                var response3 =await _client.SearchAsync<RoadName>(s => s
                 .Index(_indexName)
                .Query(q => q
                        .Bool(b => b
                            .Must(mu => mu
                                .GeoDistance(gd => gd
                                    .Field(f => f.Location)
                                    .Distance("10km")
                                    .Location(poingStart.Latitude, poingStart.Longitude)
                                ),
                                mu => mu
                                .GeoDistance(gd => gd
                                    .Field(f => f.Location)
                                    .Distance("10km")
                                    .Location(pointEnd.Latitude, pointEnd.Longitude)
                                )
                            )
                        )
                    )
                );

                var path3 = response3.Documents.ToList();

                if (searchResponse.IsValid)
                    return searchResponse.Documents.ToList();

                //foreach (var hit in searchResponse.Hits)
                //{
                //    Console.WriteLine(hit.Source);
                //}

                return new List<RoadName>();
            }
            catch
            {
                return new List<RoadName>();
            }    
        }

        
    }
}
