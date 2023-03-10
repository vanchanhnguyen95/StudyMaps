using Elastic02.Models.Test;
using Nest;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Runtime.ExceptionServices;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Elastic02.Services.Test
{
    public class HaNoiRoadService : IHaNoiRoadService
    {
        public int NumberOfShards { get; set; } = 5;
        public int NumberOfReplicas { get; set; } = 1;
        private readonly ElasticClient _client;
        private readonly string _indexName;
        private List<HaNoiRoadPoint> hanoiPointsErr;

        private readonly IConfiguration _configuration;

        public HaNoiRoadService(ElasticClient client, IConfiguration configuration)
        {
            _client = client;
            _indexName = GetIndexName();
            _configuration = configuration;
        }

        private string GetIndexName()
        {
            var type = typeof(HaNoiRoadPoint);

            var customAttributes = (DescriptionAttribute[])type
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (customAttributes != null && customAttributes.Length > 0)
                return customAttributes[0].Description;

            throw new Exception($"{nameof(HaNoiRoadPoint)} description attribute is missing.");
        }

        public async Task<string> CreateIndex(string indexName)
        {
            try
            {
                if (string.IsNullOrEmpty(indexName)) indexName = _indexName;
                //var indexResponse = await _client.Indices.CreateAsync(Indices.Index(_indexName), c => c
                // .Map<HaNoiRoadPoint>(mm => mm.AutoMap())
                //.Settings(s => s
                //    .Analysis(a => a
                //        .CharFilters(cf => cf
                //            .Mapping("programming_language", mca => mca
                //                .Mappings(new[]
                //                {
                //                    "hai ba trung => hai bà trưng",
                //                    "C# => Csharp"
                //                })
                //            )
                //          )
                //        .TokenFilters(tf => tf
                //            .AsciiFolding("ascii_folding", tk => new AsciiFoldingTokenFilter
                //            {
                //                PreserveOriginal = true
                //            })
                //            .Synonym("synonym_filter", sf => new SynonymTokenFilter
                //            {
                //                Synonyms = new List<string>()
                //                {"ha noi, hà nội, Hà Nội, Ha Noi, thủ đô, Thủ Đô, thu do, hn, hanoi",
                //                    "tphcm,tp.hcm,tp hồ chí minh,sài gòn,saigon"
                //                }
                //            })
                //        )
                //        .Analyzers(an => an
                //            .Custom("my_vi_analyzer", ca => ca
                //                .CharFilters("html_strip","programming_language")
                //                .Tokenizer("vi_tokenizer")
                //                .Filters("synonym_filter", "icu_folding", "lowercase", "ascii_folding")
                //            )
                //        )
                //    )
                //)
                //);

                //var indexResponse = await _client.Indices.CreateAsync(Indices.Index(indexName), c => c
                //   .Map<HaNoiRoadPoint>(mm => mm.AutoMap())
                //   .Settings(s => s
                //       .NumberOfReplicas(NumberOfReplicas)
                //       .NumberOfShards(NumberOfShards)
                //       .Analysis(a => a
                //           .CharFilters(cf => cf
                //               .Mapping("programming_language", mca => mca
                //                   .Mappings(new[]
                //                   {
                //                        "c# => csharp",
                //                        "C# => Csharp"
                //                   })
                //               )
                //             )
                //           .TokenFilters(tf => tf
                //               .AsciiFolding("ascii_folding", tk => new AsciiFoldingTokenFilter
                //               {
                //                   PreserveOriginal = true
                //               })
                //           //.Synonym("synonym_filter", sf => new SynonymTokenFilter
                //           //{
                //           //    Synonyms = new List<string>()
                //           //    {"ha noi, hà nội, Hà Nội, Ha Noi, thủ đô, Thủ Đô, thu do, hn, hanoi",
                //           //         "tphcm,tp.hcm,tp hồ chí minh,sài gòn,saigon"
                //           //    }
                //           //})
                //           )
                //           .Analyzers(an => an
                //               .Custom("keyword_analyzer", ca => ca
                //                   .CharFilters("programming_language")
                //                   .Tokenizer("keyword")
                //                   .Filters("lowercase"))
                //               .Custom("vi_analyzer_road", ca => ca
                //                   .CharFilters("programming_language")
                //                   .Tokenizer("vi_tokenizer")
                //                   .Filters("lowercase", "icu_folding", "ascii_folding")
                //                   //.Filters("synonym_filter","lowercase", "icu_folding", "ascii_folding")
                //               )
                //           )
                //       )
                //    )
                //);

                    var indexResponse = await _client.Indices.CreateAsync(Indices.Index(indexName), c => c
                   .Map<HaNoiRoadPoint>(mm => mm.AutoMap())
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
                           //.Synonym("synonym_filter", sf => new SynonymTokenFilter
                           //{
                           //    //Synonyms = new List<string>()
                           //    //{"ha noi, hà nội, Hà Nội, Ha Noi, thủ đô, Thủ Đô, thu do, hn, hanoi",
                           //    //     "tphcm,tp.hcm,tp hồ chí minh,sài gòn,saigon"
                           //    //}
                           //    //SynonymsPath = pathHaNoiRoad
                           //    SynonymsPath = "analysis/synonyms_hanoiroad.txt"
                           //})
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
                               //.Filters("synonym_filter","lowercase", "icu_folding", "ascii_folding")
                               //.Filters("synonym_filter","lowercase", "icu_folding", "ascii_folding")
                               )
                           )
                       )
                    )
                );

                return indexResponse.ApiCall.HttpStatusCode.ToString() ?? "OK";
            }
            catch (Exception ex) { return ex.ToString(); }

        }

        public async Task<List<HaNoiRoadPush>> GetDataSuggestion(double lat, double lng, GeoDistanceType type, string distance, int size, string keyword)
        {
            try
            {
                List<HaNoiRoadPoint> res = new List<HaNoiRoadPoint>();
                List<HaNoiRoadPush> result = new List<HaNoiRoadPush>();

                // Tìm kiếm theo tọa độ
                if (string.IsNullOrEmpty(keyword))
                {
                    res = await GetDataByLocation(lat, lng, type, distance, size);
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

                if (res.Any())
                    res.ForEach(item => result.Add(new HaNoiRoadPush(item)));

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // Tìm kiếm theo tọa độ
        private async Task<List<HaNoiRoadPoint>> GetDataByLocation(double lat, double lng, GeoDistanceType type, string distance, int size)
        {
            try
            {
                //var geo = await _client.SearchAsync<HaNoiRoadPoint>(
                //s => s.Index(_indexName)
                //    .Size(size)
                //    .Query(
                //    q => q.GeoDistance(g => g
                //    .Boost(1.1)
                //    .Name("named_query")
                //    .Field(p => p.location)
                //    .DistanceType(type)
                //    .Location(lat, lng)
                //    .Distance(distance)
                //    .ValidationMethod(GeoValidationMethod.IgnoreMalformed)
                //    ))
                //    .Sort(s => s.Descending(SortSpecialField.Score))
                //);
                var geo = await _client.SearchAsync<HaNoiRoadPoint>(s => s.Index(_indexName)
                   .Size(size)
                   .PostFilter(q => q.GeoDistance(
                        g => g.Boost(1.1).Name("named_query")
                        .Field(p => p.location).DistanceType(type).Location(lat, lng)
                        .Distance(distance).ValidationMethod(GeoValidationMethod.IgnoreMalformed)
                    ))
                   .Sort(s => s.Descending(SortSpecialField.Score))
                   .Scroll(1)
                   );

                return geo.Documents.ToList();
            }
            catch
            {
                return null;
            }
        }

        // Tìm kiếm theo từ khóa
        private async Task<List<HaNoiRoadPoint>> GetDataByKeyWord(int size, string keyword)
        {
            try
            {
                //var geo = await _client.SearchAsync<HaNoiRoadPoint>(
                //s => s.Index(_indexName)
                //    .Size(size)
                //    .Sort(s => s.Descending(SortSpecialField.Score))
                //        .Query(q => q.Match(m => m.Field(f => f.name)
                //        .Boost(1.1)
                //    .Name("named_query").Analyzer("my_vi_analyzer").Query(keyword).Fuzziness(Fuzziness.Auto)))
                //     .Sort(s => s.Descending(SortSpecialField.Score))
                //);
                //var geo = await _client.SearchAsync<HaNoiRoadPoint>(s =>
                //s.Index(_indexName)
                //            //.Query(q => q.Match(m => m.Field(f => f.name).Query(keyword)
                //            .Query(q => q.Match(m => m
                //            .Field(f => f.keyword).Query(keyword)
                //            .Analyzer("vi_analyzer")//my_vi_analyzer
                //            .Fuzziness(Fuzziness.Auto)
                //        )
                //    ).Size(size)
                //    .Sort(s => s.Descending(SortSpecialField.Score))
                //);

                //var geo2 = await _client.SearchAsync<HaNoiRoadPoint>(s => s.Index(_indexName)
                //.Size(size)
                //.Query(q => q.Bool(
                //            b => b.Must(
                //            //mu => mu.Match( ma => ma.Field( f => f.name).Query(keyword).Analyzer("vi_analyzer").Fuzziness(Fuzziness.Auto)
                //            mu => mu.Match( ma => ma.Field( f => f.keyword).Query(keyword).Analyzer("vi_analyzer").Fuzziness(Fuzziness.Auto)
                //                    .AutoGenerateSynonymsPhraseQuery()
                //                    .Boost(1.1)
                //        )
                //      )
                //    )
                //  )
                //.Sort(s => s.Descending(SortSpecialField.Score))
                //);

                //var geo3 = await _client.SearchAsync<HaNoiRoadPoint>(s => s.Index(_indexName)
                //.Size(size)
                //.Query(q => q.Bool(
                //            b => b.Must( mu => mu.Match(ma =>
                //                        ma.Field(f => f.keyword).Analyzer("vi_analyzer").Query(keyword).Fuzziness(Fuzziness.Auto)
                //                        .AutoGenerateSynonymsPhraseQuery()
                                        
                //                )
                //            )
                //            //.Must(mu => mu.Match(ma => ma.Field(f => f.name).Analyzer("vi_analyzer").Query(keyword)
                //            //        .AutoGenerateSynonymsPhraseQuery()
                //        //))
                //    )
                //  )
                //.Sort(s => s.Descending(SortSpecialField.Score))
                //);

                var geo = await _client.SearchAsync<HaNoiRoadPoint>(s => s.Index(_indexName)
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
        private async Task<List<HaNoiRoadPoint>> GetDataByLocationKeyWord(double lat, double lng, GeoDistanceType type, string distance, int size, string keyword)
        {
            try
            {
                //var geo = await _client.SearchAsync<HaNoiRoadPoint>(
                //s => s.Index(_indexName)
                //    .Size(size)
                //    //.Query(q => q.Match(m => m.Field(f => f.name).Analyzer("my_vi_analyzer").Query(keyword).Fuzziness(Fuzziness.Auto)))
                //    .Query(q => q.Match(m => m.Field(f => f.name).Analyzer("vi_analyzer").Query(keyword).Fuzziness(Fuzziness.Auto)))
                //    .PostFilter(
                //        q => q.GeoDistance(
                //                g => g.Boost(1.1).Name("named_query")
                //                .Field(p => p.location).DistanceType(type).Location(lat, lng)
                //                .Distance(distance).ValidationMethod(GeoValidationMethod.IgnoreMalformed)
                //        ))
                //    .Sort(s => s.Descending(SortSpecialField.Score))
                //);

                //var geo2 = await _client.SearchAsync<HaNoiRoadPoint>(s => s.Index(_indexName)
                //.Size(size)
                //.Query(q => q.Bool(
                //            b => b.Must(
                //            mu => mu.Match(ma => ma.Field(f => f.name).Query(keyword).Analyzer("vi_analyzer").Fuzziness(Fuzziness.Auto)
                //                    .AutoGenerateSynonymsPhraseQuery()
                //                    .Boost(1.1)
                //                    .Name("named_query")
                //        )
                //      )
                //    )
                //  )
                //.PostFilter(q => q.GeoDistance(
                //                g => g.Boost(1.1).Name("named_query")
                //                .Field(p => p.location).DistanceType(type).Location(lat, lng)
                //                .Distance(distance).ValidationMethod(GeoValidationMethod.IgnoreMalformed)
                // ))
                //.Sort(s => s.Descending(SortSpecialField.Score))
                //).ConfigureAwait(false);

                var geo = await _client.SearchAsync<HaNoiRoadPoint>(s => s.Index(_indexName)
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

                return geo.Documents.ToList();
            }
            catch
            {
                return null;
            }
        }

        public async Task<string> BulkAsync(List<HaNoiRoadPoint> haNoiRoads)
        {
            try
            {
                hanoiPointsErr = new List<HaNoiRoadPoint>();
                var existsIndex = await _client.Indices.ExistsAsync(_indexName);
                if (!existsIndex.Exists)
                    await CreateIndex(_indexName);

                _client.BulkAll(haNoiRoads, b => b
                 .Index(_indexName)
                 // how long to wait between retries
                 .BackOffTime("30s")
                 // how many retries are attempted if a failure occurs
                 .BackOffRetries(2)
                 //// refresh the index once the bulk operation completes
                 .RefreshOnCompleted()
                 // how many concurrent bulk requests to make
                 .MaxDegreeOfParallelism(Environment.ProcessorCount)
                 // number of items per bulk request
                 .Size(haNoiRoads.Count())
                 //.BufferToBulk(async (descriptor, list) =>
                 //{
                 //    // customise the individual operations in the bulk
                 //    // request before it is dispatched
                 //    foreach (var item in list)
                 //    {
                 //        // index each document into either even-index or odd-index
                 //        //descriptor.Index<HaNoiRoadPoint>(bi => bi
                 //        //   .Index("hanoiroad_point_2")
                 //        //   .Document(item)
                 //        //);
                 //        await CreateHaNoiRoadPoint(item);
                 //    }
                 //})
                 .RetryDocumentPredicate((item, road) =>
                 {
                     bool hasCreate = CreateHaNoiRoadPoint(road, _indexName);

                     if (!hasCreate)
                         hanoiPointsErr.Add(road);

                     // decide if a document should be retried in the event of a failure
                     return item.Error.Index == _indexName;
                 })
                 .DroppedDocumentCallback((item, road) =>
                 {
                     // if a document cannot be indexed this delegate is called
                     //Console.WriteLine($"Unable to index: {item} {road}");
                     bool hasCreate = CreateHaNoiRoadPoint(road, _indexName);

                     if (!hasCreate)
                         hanoiPointsErr.Add(road);
                 })
                 .ContinueAfterDroppedDocuments()
             )
             // Perform the indexing, waiting up to 15 minutes. 
             // Whilst the BulkAll calls are asynchronous this is a blocking operation
             .Wait(TimeSpan.FromMinutes(15), async next =>
             {
                 // do something on each response e.g. write number of batches indexed to console
                 if (hanoiPointsErr.Any())
                 {
                     foreach (HaNoiRoadPoint item in hanoiPointsErr)
                     {
                         var isCreate = await CreateHaNoiRoadPointAsync(item, _indexName);
                         if (!isCreate)
                         {
                             hanoiPointsErr.Remove(item);
                             hanoiPointsErr.Add(item);
                         } 
                         
                         hanoiPointsErr.Remove(item);
                     }
                     hanoiPointsErr = null;
                 }

             });

                if (!hanoiPointsErr.Any())
                {
                    hanoiPointsErr = null;
                    return "Success";
                }
                    
                foreach (HaNoiRoadPoint item in hanoiPointsErr)
                {
                    var isCreate = await CreateHaNoiRoadPointAsync(item, _indexName);

                    if (!isCreate)
                    {
                        hanoiPointsErr.Remove(item);
                        hanoiPointsErr.Add(item);
                    }

                    hanoiPointsErr.Remove(item);
                }
                hanoiPointsErr = null;

                return "Success";
            }
            catch (Exception ex)
            { return $"Error: {ex.ToString()}"; }
        }

        public async Task<bool> CreateAsync(List<HaNoiRoadPush> haNoiRoads)
        {
            try
            {
                var existsIndex = await _client.Indices.ExistsAsync(_indexName);
                if (!existsIndex.Exists)
                    await CreateIndex(_indexName);

                foreach (HaNoiRoadPush item in haNoiRoads)
                {
                    var response = await _client.CreateAsync(new HaNoiRoadPoint(item), q => q.Index(_indexName));
                    if (response.ApiCall?.HttpStatusCode == 409)
                    {
                        await _client.UpdateAsync<HaNoiRoadPoint>(item.id.ToString(), a => a.Index(_indexName).Doc(new HaNoiRoadPoint(item)));
                    }
                }
                return true;
            }
            catch
            { return false; }
        }

        public async Task<bool> BulkAsync2(ICollection<HaNoiRoadPoint> haNoiRoads)
        {
            try
            {
                hanoiPointsErr = new List<HaNoiRoadPoint>();

                var existsIndex = await _client.Indices.ExistsAsync("hanoiroad_point_2");
                if (!existsIndex.Exists)
                {
                    var indexResponse = await _client.Indices.CreateAsync(Indices.Index("hanoiroad_point_2"), c => c
                   .Map<HaNoiRoadPoint>(mm => mm.AutoMap())
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
                           //.Synonym("synonym_filter", sf => new SynonymTokenFilter
                           //{
                           //    //Synonyms = new List<string>()
                           //    //{"ha noi, hà nội, Hà Nội, Ha Noi, thủ đô, Thủ Đô, thu do, hn, hanoi",
                           //    //     "tphcm,tp.hcm,tp hồ chí minh,sài gòn,saigon"
                           //    //}
                           //    //SynonymsPath = pathHaNoiRoad
                           //    SynonymsPath = "analysis/synonyms_hanoiroad.txt"
                           //})
                           )
                           .Analyzers(an => an
                               .Custom("keyword_analyzer", ca => ca
                                   .CharFilters("programming_language")
                                   .Tokenizer("keyword")
                                   .Filters("lowercase"))
                               .Custom("vi_analyzer", ca => ca
                                   .CharFilters("programming_language")
                                   .Tokenizer("vi_tokenizer")
                                   .Filters("lowercase", "icu_folding", "ascii_folding")
                               //.Filters("synonym_filter","lowercase", "icu_folding", "ascii_folding")
                               //.Filters("synonym_filter","lowercase", "icu_folding", "ascii_folding")
                               )
                           )
                       )
                    )
                );
                }

                _client.BulkAll(haNoiRoads, b => b
                .Index("hanoiroad_point_2")
                // how long to wait between retries
                .BackOffTime("30s")
                // how many retries are attempted if a failure occurs
                .BackOffRetries(2)
                //// refresh the index once the bulk operation completes
                .RefreshOnCompleted()
                // how many concurrent bulk requests to make
                .MaxDegreeOfParallelism(Environment.ProcessorCount)
                // number of items per bulk request
                .Size(haNoiRoads.Count())
                //.BufferToBulk(async (descriptor, list) =>
                //{
                //    // customise the individual operations in the bulk
                //    // request before it is dispatched
                //    foreach (var item in list)
                //    {
                //        // index each document into either even-index or odd-index
                //        //descriptor.Index<HaNoiRoadPoint>(bi => bi
                //        //   .Index("hanoiroad_point_2")
                //        //   .Document(item)
                //        //);
                //        await CreateHaNoiRoadPoint(item);
                //    }
                //})
                .RetryDocumentPredicate( (item, road) =>
                {
                    bool hasCreate = CreateHaNoiRoadPoint(road);

                    if(!hasCreate)
                        hanoiPointsErr.Add(road);

                    // decide if a document should be retried in the event of a failure
                    return item.Error.Index == "hanoiroad_point_2";
                })
                .DroppedDocumentCallback((item, road) =>
                {
                    // if a document cannot be indexed this delegate is called
                    Console.WriteLine($"Unable to index: {item} {road}");
                    bool hasCreate = CreateHaNoiRoadPoint(road, "hanoiroad_point_2");

                    if (!hasCreate)
                        hanoiPointsErr.Add(road);
                })
                .ContinueAfterDroppedDocuments()
            )   
            // Perform the indexing, waiting up to 15 minutes. 
            // Whilst the BulkAll calls are asynchronous this is a blocking operation
            .Wait(TimeSpan.FromMinutes(15), async next =>
            {
                // do something on each response e.g. write number of batches indexed to console
                if (hanoiPointsErr.Any())
                {
                    foreach (HaNoiRoadPoint item in hanoiPointsErr)
                    {
                        var isCreate = await CreateHaNoiRoadPointAsync(item, "hanoiroad_point_2");
                        if (!isCreate)
                            hanoiPointsErr.Remove(item);
                        hanoiPointsErr.Add(item);
                    }
                    hanoiPointsErr = null;
                }    

            });

                if (!hanoiPointsErr.Any())
                    return true;

                foreach (HaNoiRoadPoint item in hanoiPointsErr)
                {
                    var isCreate = await CreateHaNoiRoadPointAsync(item, "hanoiroad_point_2");
                    if (!isCreate)
                        hanoiPointsErr.Remove(item);
                    hanoiPointsErr.Add(item);
                }
                hanoiPointsErr = null;

                return true;

            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> BulkAsync3(ICollection<HaNoiRoadPoint> haNoiRoads)
        {
            try
            {
                hanoiPointsErr = new List<HaNoiRoadPoint>();

                var existsIndex = await _client.Indices.ExistsAsync("hanoiroad_point_2");
                if (!existsIndex.Exists)
                {
                    var indexResponse = await _client.Indices.CreateAsync(Indices.Index("hanoiroad_point_2"), c => c
                   .Map<HaNoiRoadPoint>(mm => mm.AutoMap())
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
                           //.Synonym("synonym_filter", sf => new SynonymTokenFilter
                           //{
                           //    //Synonyms = new List<string>()
                           //    //{"ha noi, hà nội, Hà Nội, Ha Noi, thủ đô, Thủ Đô, thu do, hn, hanoi",
                           //    //     "tphcm,tp.hcm,tp hồ chí minh,sài gòn,saigon"
                           //    //}
                           //    //SynonymsPath = pathHaNoiRoad
                           //    SynonymsPath = "analysis/synonyms_hanoiroad.txt"
                           //})
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
                               //.Filters("synonym_filter","lowercase", "icu_folding", "ascii_folding")
                               //.Filters("synonym_filter","lowercase", "icu_folding", "ascii_folding")
                               )
                           )
                       )
                    )
                );
                }
                int i = 0;
                var bulkAllObservable = _client.BulkAll(haNoiRoads, b => b
                    .Index("hanoiroad_point_2")
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
                    .DroppedDocumentCallback(async (bulkResponseItem, road) =>
                    {
                        i++;
                        bool isCreate = await CreateHaNoiRoadPointAsync(road, "hanoiroad_point_2");
                        while (!isCreate)
                            isCreate = await CreateHaNoiRoadPointAsync(road, "hanoiroad_point_2");

                        //Console.WriteLine($"Unable to index: {bulkResponseItem} {road}");
                        Console.WriteLine($"Count error: {i}");
                    })
                    .ContinueAfterDroppedDocuments()
                )
                .Wait(TimeSpan.FromMinutes(15), next =>
                {
                    // do something e.g. write number of pages to console
                })
                ;
                i = 0;

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// Thêm mới dữ liệu đường Hà Nội
        /// </summary>
        /// <param name="roadPoint"></param>
        /// <param name="indexName"></param>
        /// <returns></returns>
        private bool CreateHaNoiRoadPoint(HaNoiRoadPoint roadPoint, string indexName = null)
        {
            try
            {
                if (string.IsNullOrEmpty(indexName)) indexName = _indexName;

                var response = _client.Index(new HaNoiRoadPoint(roadPoint), q => q.Index(indexName));
                if (response.ApiCall?.HttpStatusCode == 409)
                {
                    _client.Update<HaNoiRoadPoint>(roadPoint.id.ToString(), a => a.Index(indexName).Doc(new HaNoiRoadPoint(roadPoint)));
                }
                return response.IsValid;
            }
            catch (Exception ex) { return false; }
            
        }

        /// <summary>
        /// Thêm mới dữ liệu đường Hà Nội
        /// </summary>
        /// <param name="roadPoint"></param>
        /// <param name="indexName"></param>
        /// <returns></returns>
        private async Task<bool> CreateHaNoiRoadPointAsync(HaNoiRoadPoint roadPoint, string indexName = null)
        {
            try
            {
                if (string.IsNullOrEmpty(indexName)) indexName = _indexName;

                var response = await _client.IndexAsync(new HaNoiRoadPoint(roadPoint), q => q.Index(indexName));
                if (response.ApiCall?.HttpStatusCode == 409)
                {
                    await _client.UpdateAsync<HaNoiRoadPoint>(roadPoint.id.ToString(), a => a.Index(indexName).Doc(new HaNoiRoadPoint(roadPoint)));
                }
                return response.IsValid;
            }
            catch (Exception ex) { return false; }

        }
    }
}
