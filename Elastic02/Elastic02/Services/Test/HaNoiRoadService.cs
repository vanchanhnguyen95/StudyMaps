using Elastic02.Mapping.Test;
using Elastic02.Models.Test;
using Nest;
using System.ComponentModel;

namespace Elastic02.Services.Test
{
    public class HaNoiRoadService : IHaNoiRoadService
    {
        private readonly ElasticClient _client;
        private readonly string _indexName;

        public HaNoiRoadService(ElasticClient client)
        {
            _client = client;
            _indexName = GetIndexName();
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

        public async Task<string> CreateIndex()
        {
            try
            {
                var indexResponse = await _client.Indices.CreateAsync(Indices.Index(_indexName), c => c
                 .Map<HaNoiRoadPoint>(mm => mm.AutoMap())
                .Settings(s => s
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
                            .Synonym("synonym_filter", sf => new SynonymTokenFilter
                            {
                                Synonyms = new List<string>()
                                {"ha noi, hà nội, Hà Nội, Ha Noi, thủ đô, Thủ Đô, thu do, hn, hanoi",
                                    "tphcm,tp.hcm,tp hồ chí minh,sài gòn,saigon"
                                }
                            })
                        )
                        .Analyzers(an => an
                            .Custom("my_vi_analyzer", ca => ca
                                .CharFilters("programming_language")
                                .Tokenizer("vi_tokenizer")
                                .Filters("synonym_filter", "lowercase", "ascii_folding")
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
                List<HaNoiRoadPoint> res= new List<HaNoiRoadPoint>();
                List<HaNoiRoadPush> result = new List<HaNoiRoadPush>();

                // Tìm kiếm theo tọa độ
                if (string.IsNullOrEmpty(keyword))
                {
                    res = await GetDataByLocation(lat, lng, type, distance, size);
                }
                // Tìm kiếm theo từ khóa
                else if (lat == 0 || lng == 0)
                {
                    res =  await GetDataByKeyWord(size, keyword);
                }
                // Tìm kiếm theo tọa độ và từ khóa
                else
                {
                    res = await GetDataByLocationKeyWord(lat, lng, type, distance, size, keyword);
                }
                
                if(res.Any())
                    res.ForEach(item => result.Add(new HaNoiRoadPush(item)));

                return result;
            }
            catch
            {
                return null;
            }
        }

        // Tìm kiếm theo tọa độ
        private async Task<List<HaNoiRoadPoint>> GetDataByLocation(double lat, double lng, GeoDistanceType type, string distance, int size)
        {
            try
            {
                var geo = await _client.SearchAsync<HaNoiRoadPoint>(
                s => s.Index(_indexName)
                    .Size(size)
                    .Query(
                    q => q.GeoDistance(g => g
                    .Boost(1.1)
                    .Name("named_query")
                    .Field(p => p.location)
                    .DistanceType(type)
                    .Location(lat, lng)
                    .Distance(distance)
                    .ValidationMethod(GeoValidationMethod.IgnoreMalformed)
                    ))
                    .Sort(s => s.Descending(SortSpecialField.Score))
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
                var geo = await _client.SearchAsync<HaNoiRoadPoint>(
                s => s.Index(_indexName)
                    .Size(size)
                    .Sort(s => s.Descending(SortSpecialField.Score))
                        .Query(q => q.Match(m => m.Field(f => f.name)
                        .Boost(1.1)
                    .Name("named_query").Query(keyword).Fuzziness(Fuzziness.Auto)))
                     .Sort(s => s.Descending(SortSpecialField.Score))
                );
                return geo.Documents.ToList();
            }
            catch
            {
                return null;
            }
        }

        // Tìm kiếm theo tọa độ và từ khóa
        private async Task<List<HaNoiRoadPoint>> GetDataByLocationKeyWord(double lat, double lng, GeoDistanceType type, string distance, int pageSize, string keyword)
        {
            try
            {
                var geo = await _client.SearchAsync<HaNoiRoadPoint>(
                s => s.Index(_indexName)
                    .Size(pageSize)
                    .Query(q => q.Match(m => m.Field(f => f.name).Query(keyword).Fuzziness(Fuzziness.Auto)))
                    .PostFilter(
                        q => q.GeoDistance(
                                g => g.Boost(1.1).Name("named_query")
                                .Field(p => p.location).DistanceType(type).Location(lat, lng)
                                .Distance(distance).ValidationMethod(GeoValidationMethod.IgnoreMalformed)
                        ))
                    .Sort(s => s.Descending(SortSpecialField.Score))
                );
                return geo.Documents.ToList();
            }
            catch
            {
                return null;
            }
        }

    }
}
