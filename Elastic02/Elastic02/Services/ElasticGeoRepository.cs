using Nest;
using System.ComponentModel;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;
using System.ComponentModel.Design;
using System.Runtime.Serialization.Formatters;
using System.Linq.Expressions;
using Elastic02.Models;

namespace Elastic02.Services
{
    public class ElasticGeoRepository<T> : IElasticGeoRepository<T> where T : class
    {
        private readonly ElasticClient _elasticClient;
        private readonly string _indexName;

        public ElasticGeoRepository(ElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
            _indexName = GetIndexName();
        }

        private string GetIndexName()
        {
            var type = typeof(T);

            var customAttributes = (DescriptionAttribute[])type
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (customAttributes != null && customAttributes.Length > 0)
                return customAttributes[0].Description;

            throw new Exception($"{nameof(T)} description attribute is missing.");
        }

        public async Task<bool> BulkAsync(IEnumerable<T> objects)
        {
            var existsIndex = await _elasticClient.Indices.ExistsAsync(_indexName);
            if (!existsIndex.Exists)
               await CreateIndexGeoAsync();

            var response = await _elasticClient.BulkAsync(b => b.Index(_indexName).IndexMany(objects));
            if (response.ApiCall.Success)
                return response.IsValid;
            return false;
        }

        public async Task<CreateIndexResponse> CreateIndexGeoAsync()
        {
            var indexResponse = await _elasticClient.Indices.CreateAsync(Indices.Index(_indexName), c => c
                 .Map<T>(mm => mm.AutoMap())
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
            return indexResponse;
        }

        public async Task<List<string>> GetDataSearchGeo(double lat, double ln, GeoDistanceType type, string distance, int pageSize)
        {
            try
            {
                var typeP = typeof(T);

                var customAttributes = (DescriptionAttribute[])typeP
                    .GetCustomAttributes(typeof(DescriptionAttribute), false);

                string nameOf = customAttributes[0].Description;

                var typeGeoP = typeof(ElasticRequestPushGeopoint);

                var customAttributesGeoP = (DescriptionAttribute[])typeGeoP
                    .GetCustomAttributes(typeof(DescriptionAttribute), false);

                string nameOfGeoP = customAttributes[0].Description;

                if(nameOf == nameOfGeoP)
                {
                    var geo = await _elasticClient.SearchAsync<ElasticRequestPushGeopoint>(
                    s => s.Index(_indexName).Size(pageSize).Sort(s => s.Descending(SortSpecialField.Score)).Query(
                        q => q.GeoDistance(g => g
                        .Boost(1.1).Name("named_query")
                        .Field(p => p.location)
                        .DistanceType(type)
                        .Location(lat, ln)
                        .Distance(distance)
                        .ValidationMethod(GeoValidationMethod.IgnoreMalformed)
                        ))
                    );
                }

                return new List<string>();
            }
            catch
            {
                return null;
            }
        }


    }
}
