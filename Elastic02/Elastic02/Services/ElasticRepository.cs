using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Elastic02.Models;

namespace Elastic02.Services
{
    ///<inheritdoc cref="IElasticRepository<T>" />
    public class ElasticRepository<T> : IElasticRepository<T> where T : class
    {
        private readonly ElasticClient _elasticClient;
        private readonly string _indexName;

        public ElasticRepository(ElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
            _indexName = GetIndexName();
        }

        public int NumberOfShards { get; set; } = 5;
        public int NumberOfReplicas { get; set; } = 1;

        public async Task AddDocumentAsync(T value) => await _elasticClient.CreateAsync(value, d => d.Index(Indices.Index(_indexName)).Refresh(Elasticsearch.Net.Refresh.True));

        public async Task<CreateIndexResponse> CreateIndexAsync()
        {
            var indexResponse = await _elasticClient.Indices.CreateAsync(Indices.Index(_indexName),
            ci =>
            {
                ci.Map<T>(mm => mm.AutoMap());

                ci.Settings(s =>
                {
                    s.NumberOfReplicas(NumberOfReplicas);
                    s.NumberOfShards(NumberOfShards);
                    s.Analysis(an =>
                        an.Analyzers(az =>
                        az.Custom("casesensitive_text",
                    ca =>
                    {
                        ca.Tokenizer("keyword");
                        ca.Filters("lowercase"); return ca;
                    })));
                    return s;
                }); return ci;
            });
            return indexResponse;
        }

        public async Task DeleteDocumentAsync(T value)
        {
            await _elasticClient.DeleteAsync(new DeleteRequest(Indices.Index(_indexName), Id.From(value)));
            await RefreshIndex();
        }

        public async Task DeleteIndexAsync() => await _elasticClient.Indices.DeleteAsync(_indexName);

        public async Task DeleteIndexAsync(string indexName) => await _elasticClient.Indices.DeleteAsync(Indices.Index(indexName));

        public async Task<CommonStats> StatsAggregationAsync(string key, string fieldName)
        {
            var request = new SearchRequest(Indices.Index(_indexName))
            {
                Size = 0
            };
            var aggs = new AggregationDictionary
            {
                { key, new AggregationContainer { Stats = new StatsAggregation(key , new Field(fieldName)) } }
            };
            request.Aggregations = aggs;
            var response = await _elasticClient.SearchAsync<T>(request);
            var result = response.Aggregations.Stats(key) ?? new StatsAggregate();
            return new CommonStats
            {
                Average = result.Average,
                Count = result.Count,
                Max = result.Max,
                Min = result.Min,
                Sum = result.Sum
            };
        }

        public async Task<GetIndexResponse> GetAllIndicesAsync() => await _elasticClient.Indices.GetAsync(Indices.AllIndices);

        public async Task<T> GetDocumentAsync(string id)
        {
            var response = await _elasticClient.GetAsync<T>(new GetRequest(Indices.Index(_indexName), id));
            return response.Source;
        }

        public async Task<(long Count, IEnumerable<T> Documents)> GetDocumentsAsync(GridQueryModel gridQueryModel)
        {
            int frm = (gridQueryModel.Page - 1) * gridQueryModel.Limit;
            var searchRequest = new SearchRequest(Indices.Index(_indexName)) { From = 0, Size = 0 };

            long count;
            if (!string.IsNullOrEmpty(gridQueryModel.Search))
            {
                searchRequest.QueryOnQueryString = $"({gridQueryModel.Search}) OR (*{gridQueryModel.Search}*)";
                count = await GetFilterDocumentsCount(searchRequest);
            }
            else
                count = await GetDocumentsCount();

            searchRequest.From = frm; searchRequest.Size = gridQueryModel.Limit;

            if (!string.IsNullOrEmpty(gridQueryModel.SortBy) && !string.IsNullOrEmpty(gridQueryModel.Direction))
            {
                var field = new Field(gridQueryModel.SortBy);

                searchRequest.Sort = new List<ISort> {
                 new FieldSort {
                     Field = field,
                     Order = "asc".Equals(gridQueryModel.Direction, StringComparison.InvariantCultureIgnoreCase) ? SortOrder.Ascending : SortOrder.Descending
                 }
                };
            }

            var searchResponse = await _elasticClient.SearchAsync<T>(searchRequest);

            return new EsDocuments<T>(count, searchResponse.Documents);
        }

        public async Task<long> GetDocumentsCount()
        {
            var response = await _elasticClient.CountAsync(new CountRequest(Indices.Index(_indexName)));
            return response.Count;
        }

        public async Task<GetIndexResponse> GetIndexAsync()
        => await _elasticClient.Indices.GetAsync(Indices.Index(_indexName));

        public async Task<bool> IsIndexExists()
        {
            var indexResponse = await _elasticClient.Indices.ExistsAsync(Indices.Index(_indexName));

            return indexResponse.Exists;
        }

        public async Task RefreshIndex() => await _elasticClient.Indices.RefreshAsync(Indices.Index(_indexName));

        public async Task UpsertDocumentAsync(T value) => await _elasticClient.UpdateAsync<T, T>(Id.From(value),
                u => u.Index(Indices.Index(_indexName))
                .Doc(value)
                .DocAsUpsert(true)
                .RetryOnConflict(2)
                .Refresh(Refresh.True));

        private async Task<long> GetFilterDocumentsCount(SearchRequest searchRequest)
        {
            var result = await _elasticClient.SearchAsync<T>(searchRequest);
            return result.HitsMetadata.Total.Value;
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

        public async Task<double> MedianAggregationAsync(string key, string fieldName)
        {
            var request = new SearchRequest(Indices.Index(_indexName))
            {
                Size = 0
            };
            var aggs = new AggregationDictionary
            {
                { key, new AggregationContainer { MedianAbsoluteDeviation = new MedianAbsoluteDeviationAggregation(key , new Field(fieldName)) } }
            };
            request.Aggregations = aggs;
            var response = await _elasticClient.SearchAsync<T>(request);
            var result = response.Aggregations.MedianAbsoluteDeviation(key);
            return result.Value ?? 0;
        }

        public async Task<List<GroupStats>> GroupByAsync(string fieldName)
        {
            var key = "group_by_field";
            var request = new SearchRequest(Indices.Index(_indexName))
            {
                Size = 0
            };

            var aggs = new AggregationDictionary
            {
                { key, new AggregationContainer { Terms = new TermsAggregation(fieldName){  Field = new Field(fieldName) } } }
            };
            request.Aggregations = aggs;
            var response = await _elasticClient.SearchAsync<T>(request);
            var items = ((BucketAggregate)response.Aggregations[key])?.Items;
            if (items == null || items.Count == 0)
                return new List<GroupStats>();

            return items.Select(q =>
            {
                var key = (KeyedBucket<object>)q;
                var gStats = new GroupStats
                {
                    Name = key.Key.ToString().CharFormat(),
                    Value = key.DocCount.Value
                };
                return gStats;
            }).ToList();
        }

        public async Task UpdateDocumentAsync(T value) => await _elasticClient.UpdateAsync(new DocumentPath<T>(value), d => d.Doc(value).Index(Indices.Index(_indexName)).Refresh(Refresh.True));

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
            ); ;


            //var indexResponse = await _elasticClient.Indices.CreateAsync(Indices.Index(_indexName), c => c
            //     .Map<T>(mm => mm.AutoMap())
            //    .Settings(s => s
            //        .Analysis(a => a
            //            .CharFilters(cf => cf
            //                .Mapping("programming_language", mca => mca
            //                    .Mappings(new[]
            //                    {
            //                        "c# => csharp",
            //                        "C# => Csharp"
            //                    })
            //                )
            //            )
            //            .Analyzers(an => an
            //                .Custom("my_vi_analyzer", ca => ca
            //                    //.CharFilters("programming_language")
            //                    .Tokenizer("vi_tokenizer")
            //                    .Filters("lowercase", "asciifolding")
            //                )
            //                .Custom("my_vi_analyzer2", ca => ca
            //                    .CharFilters("html_strip", "programming_language")
            //                    .Tokenizer("standard")
            //                    .Filters("lowercase", "stop")
            //                )

            //            )
            //        )
            //    )
            //);

            //var cusF = new Filter { Description= _indexName };

            //var fullName = new CustomAnalyzer
            //{
            //    Filter = new List<string> { "synonym", "lowercase", "asciifolding" },
            //    Tokenizer = "vi_tokenizer"
            //};



            //var indexResponse = await _elasticClient.Indices.CreateAsync(Indices.Index(_indexName),
            //ci =>
            //{
            //    ci.Map<T>(mm => mm.AutoMap());

            //    ci.Settings(s =>
            //    {
            //        s.NumberOfReplicas(NumberOfReplicas);
            //        s.NumberOfShards(NumberOfShards);
            //        s.Analysis(a => a
            //          .CharFilters(cf => cf
            //                .Mapping("synonym_filter", mcf => mcf
            //                .Mappings(
            //                     "Bs=> Bác sĩ",
            //                    "bs=> Bác sĩ"
            //                    )
            //                  )
            //           )
            //          .Analyzers(an => an
            //            .Custom("my_vi_analyzer",
            //            ca => ca.CharFilters("html_strip", "synonym_filter")
            //                    .Tokenizer("vi_tokenizer")
            //                    .Filters("lowercase", "asciifolding")
            //                    )
            //          )
            //         );

            //        return s;
            //    }); return ci;
            //});
            return indexResponse;
        }

        public async Task<bool> BulkAsync(IEnumerable<T> objects)
        {
            var existsIndex = await _elasticClient.Indices.ExistsAsync(_indexName);
            if (!existsIndex.Exists)
                await CreateIndexGeoAsync();

            var response =  await _elasticClient.BulkAsync(b => b.Index(_indexName).IndexMany(objects));
            if (response.ApiCall.Success)
                return response.IsValid;
            return false;
        }
    }

    internal struct EsDocuments<T> where T : class
    {
        public long Count;
        public IReadOnlyCollection<T> Documents;

        public EsDocuments(long count, IReadOnlyCollection<T> documents)
        {
            Count = count;
            Documents = documents;
        }

        public override bool Equals(object obj)
        {
            return obj is EsDocuments<T> other &&
                   Count == other.Count &&
                   EqualityComparer<IReadOnlyCollection<T>>.Default.Equals(Documents, other.Documents);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Count, Documents);
        }

        public void Deconstruct(out long count, out IReadOnlyCollection<T> documents)
        {
            count = Count;
            documents = Documents;
        }

        public static implicit operator (long Count, IReadOnlyCollection<T> Documents)(EsDocuments<T> value)
        {
            return (value.Count, value.Documents);
        }

        public static implicit operator EsDocuments<T>((long Count, IReadOnlyCollection<T> Documents) value)
        {
            return new EsDocuments<T>(value.Count, value.Documents);
        }
    }
}
