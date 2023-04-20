using Elastic02.Models.Test;
using Elastic02.Utility;
using Nest;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Text;

namespace Elastic02.Services.Test
{
    public interface IProvinceService
    {
        Task<string> CreateIndex(string indexName);

        Task<string> BulkAsync(List<Province> vietNamShapes);

        //Task<string> AddAsync(List<VietNamShape> vietNamShapes);
        // Tìm kiếm theo Tọa độ / Từ Khóa / Tọa độ và từ khóa
        Task<List<Province>> GetDataSuggestion(int size, string keyword);
        Task<string> GetProvinceId(string keyword);
    }

    public class ProvinceService : IProvinceService
    {

        private int NumberOfShards { get; set; } = 5;
        private int NumberOfReplicas { get; set; } = 1;
        private readonly ElasticClient _client;
        private readonly string _indexName;
        private readonly IConfiguration _configuration;
        private string proinceHaNoi = "16";

        private string GetIndexName()
        {
            var type = typeof(ProvinceAnalysis);

            var customAttributes = (DescriptionAttribute[])type
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (customAttributes != null && customAttributes.Length > 0)
                return customAttributes[0].Description;

            throw new Exception($"{nameof(ProvinceAnalysis)} description attribute is missing.");
        }

        public async Task<string> CreateIndex(string indexName)
        {
            try
            {
                if (string.IsNullOrEmpty(indexName)) indexName = _indexName;

                await _client.Indices.DeleteAsync(Indices.Index(indexName));

                var indexResponse = await _client.Indices.CreateAsync(Indices.Index(indexName), c => c
               .Map<ProvinceAnalysis>(mm => mm.AutoMap())
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
                            .PatternCapture("pattern_capture", pt => new PatternCaptureTokenFilter
                            {
                                Patterns = new[] { @"\d+", @"\\\\" },
                                PreserveOriginal = true,
                            })
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
                           .Custom("number_analyzer", ca => ca
                               //.CharFilters("html_strip", "province_name")
                               .Tokenizer("standard")
                               .Filters("lowercase", "pattern_capture")
                           )
                       )
                   )
                )
            );

                return indexResponse.ApiCall.HttpStatusCode.ToString() ?? "OK";
            }
            catch (Exception ex) { return ex.ToString(); }
        }

        private async Task<bool> IndexAsync(ProvinceAnalysis item, string indexName = null)
        {
            try
            {
                if (string.IsNullOrEmpty(indexName)) indexName = _indexName;

                var response = await _client.IndexAsync(new ProvinceAnalysis(item), q => q.Index(indexName));
                if (response.ApiCall?.HttpStatusCode == 409)
                {
                    await _client.UpdateAsync<ProvinceAnalysis>(item.ProvinceID.ToString(), a => a.Index(indexName).Doc(new ProvinceAnalysis(item)));
                }
                return response.IsValid;
            }
            catch { return false; }

        }

        public ProvinceService(ElasticClient client, IConfiguration configuration)
        {
            _client = client;
            _indexName = GetIndexName();
            _configuration = configuration;
        }

        public async Task<string> BulkAsync(List<Province> provincesPush)
        {
            try
            {
                List<ProvinceAnalysis> provinces = new List<ProvinceAnalysis>();

                if (!provincesPush.Any())
                    return "Success - No data to bulk insert";

                provincesPush.ForEach(item => provinces.Add(new ProvinceAnalysis(item)));

                //Check xem khởi tạo index chưa, nếu chưa khởi tạo thì phải khởi tạo index mới được
                await CreateIndex(_indexName);

                var bulkAllObservable = _client.BulkAll(provinces, b => b
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
                return "Success";
            }
            catch (Exception ex)
            {
                return "Bulk False";
            }
        }

        public async Task<List<Province>> GetDataSuggestion(int size, string keyword)
        {
            try
            {
                List<Province> result = new List<Province>();

                if (string.IsNullOrEmpty(keyword))
                    return result;

                List<ProvinceAnalysis> respon = await GetDataByKeyWord(size, keyword.Trim());

                if (!respon.Any())
                    return result;

                respon.ForEach(item => result.Add(new Province(item)));

                return result;

            }
            catch(Exception)
            {
                return new List<Province>();
            }
               
        }

        private string GetKeySearch(string keywordAscii, string analyzer = "vn_analyzer3")
        {
            var analyzeResponse = _client.Indices.Analyze(a => a
            .Index(_indexName)
            .Analyzer("number_analyzer")
            .Text(keywordAscii)
            );

            foreach (var token in analyzeResponse.Tokens)
            {
                Console.WriteLine(token.Token);
            }


            var keyAnalyzer = _client.Indices.Analyze(a => a
               .Index(_indexName)
               .Analyzer(analyzer)
               .Text(keywordAscii));
            //AnalyzeToken.Type
            foreach (var token in keyAnalyzer.Tokens)
            {
                Console.WriteLine(token.Token);
                if(token.Type == "<NUMBER>")
                {

                }

            }

            string keySearch = string.Empty;

            if (!keyAnalyzer.IsValid)
                keySearch = keywordAscii;

            if (!keyAnalyzer.Tokens.Any())
                return string.Empty;

          
            StringBuilder wordsAp = new StringBuilder();

            foreach (var token in keyAnalyzer.Tokens)
            {
                //Console.WriteLine($"Token: {token.Token}");
                wordsAp.Append(token.Token);
                wordsAp.Append(" ");
            }

            string[] words = new string[] { "" };

            words = wordsAp.ToString().Trim().Split(' ');

            int lenWords = words.Count();

            if (lenWords == 4)
                //return $" {words[lenWords - 3]} {words[lenWords - 2]} {words[lenWords - 1]}";
                return $" {words[lenWords - 2]} {words[lenWords - 1]}";

            if (lenWords > 4)
                //return $" {words[lenWords - 4]} {words[lenWords - 3]} {words[lenWords - 2]} {words[lenWords - 1]}";
                return $" {words[lenWords - 3]} {words[lenWords - 2]} {words[lenWords - 1] }";

            return string.Join("", words);
        }

        private async Task<List<ProvinceAnalysis>> GetDataByKeyWord(int size, string keyword)
        {
            try
            {
                string keywordAscii = string.Empty;

                if (!string.IsNullOrEmpty(keyword))
                    keywordAscii = LatinToAscii.Latin2Ascii(keyword.ToLower());

                string key = GetKeySearch(keywordAscii);

                var result = await _client.SearchAsync<ProvinceAnalysis>(s => s.Index(_indexName)
                    .Size(size)
                    .Query(q => q.Bool(
                        b => b.Must(mu =>
                         mu.Match(ma =>
                            ma.Field(f => f.ENameLower).Name("named_query").Analyzer("vn_analyzer3").Query(key).Fuzziness(Fuzziness.EditDistance(0))
                            .AutoGenerateSynonymsPhraseQuery()
                        )
                        //mu.Match(ma =>
                        //ma.Field(f => f.ENameLower).Name("named_query").Analyzer("vn_analyzer3").Query(keywordAscii).Fuzziness(Fuzziness.Auto)
                        //.AutoGenerateSynonymsPhraseQuery()
                        //)
                        //&&
                        //mu.Match(ma =>
                        //ma.Field(f => f.EName).Name("named_query").Analyzer("vn_analyzer").Query(keywordAscii).Fuzziness(Fuzziness.EditDistance(0))
                        //.AutoGenerateSynonymsPhraseQuery()
                        //)
                        //&& mu.Match(ma =>
                        //ma.Field(f => f.VName).Name("named_query").Analyzer("vn_analyzer").Query(keyword).Fuzziness(Fuzziness.EditDistance(1))
                        //.AutoGenerateSynonymsPhraseQuery()
                        //)

                        )
                        //.MinimumShouldMatch(98)

                     )
                    )
                   //.MinScore(5.0)
                   .Sort(s => s.Descending(SortSpecialField.Score))
                   .Scroll(1));

                return result.Documents.ToList();
            }
            catch
            {
                return new List<ProvinceAnalysis>();
            }
        }

        public async Task<string> GetProvinceId(string keyword)
        {
            List<Province> province = await GetDataSuggestion(1, keyword);
            if (!province.Any())
                return proinceHaNoi;
            return province[0].ProvinceID.ToString();

        }
    }
}
