using Elastic02.Models.Test;
using Nest;
using System.ComponentModel;

namespace Elastic02.Services.Test
{
    public class HaNoiShapeService : IHaNoiShapeService
    {
        public int NumberOfShards { get; set; } = 5;
        public int NumberOfReplicas { get; set; } = 1;
        private readonly ElasticClient _client;
        private readonly string _indexName;
        private readonly IConfiguration _configuration;

        public HaNoiShapeService(ElasticClient client, IConfiguration configuration)
        {
            _client = client;
            _indexName = GetIndexName();
            _configuration = configuration;
        }

        private string GetIndexName()
        {
            var type = typeof(HaNoiShape);

            var customAttributes = (DescriptionAttribute[])type
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (customAttributes != null && customAttributes.Length > 0)
                return customAttributes[0].Description;

            throw new Exception($"{nameof(HaNoiShape)} description attribute is missing.");
        }

        public async Task<string> BulkAsync(List<HaNoiShape> haNoiRoads)
        {
            try
            {
                var existsIndex = await _client.Indices.ExistsAsync(_indexName);
                if (!existsIndex.Exists)
                    await CreateIndex(_indexName);

                var bulkAllObservable = _client.BulkAll(haNoiRoads, b => b
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
                        .DroppedDocumentCallback(async (bulkResponseItem, road) =>
                        {
                            if(road.id == 1000001 || road.id == 1000002)
                                Console.WriteLine($"Count error: {road.id}");

                            bool isCreate = true;

                            isCreate = await CreateHaNoiAsync(road, _indexName);
                            while (isCreate == false)
                            {
                                Console.WriteLine($"Create: {road.id}");
                                //isCreate = await CreateHaNoiAsync(road, _indexName);
                                isCreate = await IndexHaNoiAsync(road, _indexName);
                            }    
                                

                            //Console.WriteLine($"Unable to index: {bulkResponseItem} {road}");
                            //Console.WriteLine($"Count error: {i}");
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

        public async Task<string> CreateIndex(string indexName)
        {
            try
            {
                if (string.IsNullOrEmpty(indexName)) indexName = _indexName;

                var indexResponse = await _client.Indices.CreateAsync(Indices.Index(indexName), c => c
               .Map<HaNoiShape>(mm => mm.AutoMap())
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

        /// <summary>
        /// Thêm mới dữ liệu đường Hà Nội
        /// </summary>
        /// <param name="roadPoint"></param>
        /// <param name="indexName"></param>
        /// <returns></returns>
        private async Task<bool> CreateHaNoiAsync(HaNoiShape item, string indexName = null)
        {
            try
            {
                if (string.IsNullOrEmpty(indexName)) indexName = _indexName;

                var response = await _client.CreateAsync(new HaNoiShape(item), q => q.Index(indexName));
                if (response.ApiCall?.HttpStatusCode == 409)
                {
                    await _client.UpdateAsync<HaNoiShape>(item.id.ToString(), a => a.Index(indexName).Doc(new HaNoiShape(item)));
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
        private async Task<bool> IndexHaNoiAsync(HaNoiShape item, string indexName = null)
        {
            try
            {
                if (string.IsNullOrEmpty(indexName)) indexName = _indexName;

                var response = await _client.IndexAsync(new HaNoiShape(item), q => q.Index(indexName));
                if (response.ApiCall?.HttpStatusCode == 409)
                {
                    await _client.UpdateAsync<HaNoiShape>(item.id.ToString(), a => a.Index(indexName).Doc(new HaNoiShape(item)));
                }
                return response.IsValid;
            }
            catch (Exception ex) { return false; }

        }
    }
}
