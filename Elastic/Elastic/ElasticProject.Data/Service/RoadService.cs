using ElasticProject.Data.Entity.MapObj;
using ElasticProject.Data.Interface;
using Microsoft.Extensions.Configuration;
using Nest;

namespace ElasticProject.Data.Service
{
    public class RoadService : IRoadService
    {
        private readonly IConfiguration _configuration;
        private readonly IElasticClient _client;
        public RoadService(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = CreateInstance();
        }

        private ElasticClient CreateInstance()
        {
            string host = _configuration.GetSection("ElasticsearchServer:Host").Value ?? "";
            string port = _configuration.GetSection("ElasticsearchServer:Port").Value ?? "";
            string username = _configuration.GetSection("ElasticsearchServer:Username").Value ?? "";
            string password = _configuration.GetSection("ElasticsearchServer:Password").Value ?? "";
            var settings = new ConnectionSettings(new Uri(host + ":" + port));
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                settings.BasicAuthentication(username, password);

            return new ElasticClient(settings);
        }

        public async Task<List<string>> GetDataSuggestion(string indexName, string keyWord, int size)
        {
            try
            {
                //• Tìm kiếm theo từ khóa bất kỳ với tiếng Việt có dấu
                //• Tìm kiếm từ khóa bất kỳ với tiếng Việt không dấu
                //• Tìm kiếm bao gồm tiếng Việt có dấu và không dấu
                //• Tìm kiếm chính xác từ khóa

                //• Tìm kiếm các từ khóa có từ viết tắt như “bs” hoặc “bv” hoặc những từ
                //khóa chuyên khoa đã cấu hình sẵn như “tmh”…
                //• Tìm kiếm với gợi ý(Autocomplete) có sử dụng fuzzy trong ElasticSearch
                //• Tìm kiếm theo GEO có cấu hình giới hạn về khoảng cách, phạm vi cần tìm
                //• Tìm kiếm theo GEO và có từ khóa kèm theo
                //• Tìm kiếm theo tỉnh

                var analyzeResponse = _client.Indices.Analyze(a => a
                    .Analyzer("vi_analyzer")
                    .Text(keyWord)
                );

                var response = await _client.SearchAsync<BGCElasticRequestCreate>(s =>
                            s.Index("road")
                            .Query(q => q.Match(m => m.Field(f => f.name).Query(keyWord)
                            .Analyzer("my_vi_analyzer")//my_vi_analyzer
                            .Fuzziness(Fuzziness.Auto)
                        )
                    ).Size(size)
                    .Sort(s => s.Descending(SortSpecialField.Score))
                );
                var rs = response.Documents.Select(x => $"{x.name}");
                return rs.ToList();


                // var response2 = await _client.SearchAsync<BGCElasticRequestCreate>(
                //     s => s.Index("road2").Query(
                //     q => q.Match(m => m.Field(f => f.name).Query(keyWord).Analyzer("my_vi_analyzer").Fuzziness(Fuzziness.Auto)))).ConfigureAwait(false);

                // var rs = response2.Documents.Select(x => $"{x.name}");

                // var response = await _client.SearchAsync<BGCElasticRequestCreate>(s => s
                //              .Index(indexName)
                //              .Analyzer("vi_analyzer")
                //              .Query(q => q
                //    //.Fuzzy(fz => fz.Field("kindname")
                //    .Fuzzy(fz => fz.Field("name")
                //        .Value(keyWord).Fuzziness(Fuzziness.EditDistance(4))
                //    )
                //));

                // return response.Documents.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<BGCElasticRequestCreate>> GetRoads(string indexName)
        {
            try
            {
                var response = await _client.SearchAsync<BGCElasticRequestCreate>(s => s
                   .From(0)
                   .Take(20)
                   .Index(indexName));
                return response.Documents.ToList();
            }
            catch
            {
                return new List<BGCElasticRequestCreate>();
            }
        }

        public async Task<string> InsertBulkElasticRequest(string indexName, List<BGCElasticRequestCreate> elasticRequestCreates)
        {
            try
            {
                // Check nếu có index rồi thì bulk insert, nếu chưa phải thêm xử lý tạo index nữa

                await _client.BulkAsync(b => b.Index("elasticRequestCreates").IndexMany(elasticRequestCreates));
                //await _client.IndexManyAsync(elasticRequestCreates, index: indexName);
                return "Success";
            }
            catch (Exception ex) { return ex.ToString(); }
        }

       
    }
}
