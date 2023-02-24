using ElasticProject.Data.Entity.MapObj;
using ElasticProject.Data.Interface;
using ElasticProject.Data.Mapping;
using Microsoft.Extensions.Configuration;
using Nest;

namespace ElasticProject.Data.Service
{
    public class HealthCareService : IHealthCareService
    {
        private readonly IConfiguration _configuration;
        private readonly IElasticClient _client;
        public HealthCareService(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = CreateInstance();
        }
        private ElasticClient CreateInstance()
        {
            string host = _configuration.GetSection("ElasticsearchServer:Host").Value??"";
            string port = _configuration.GetSection("ElasticsearchServer:Port").Value??"";
            string username = _configuration.GetSection("ElasticsearchServer:Username").Value??"";
            string password = _configuration.GetSection("ElasticsearchServer:Password").Value??"";
            var settings = new ConnectionSettings(new Uri(host + ":" + port));
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                settings.BasicAuthentication(username, password);

            return new ElasticClient(settings);
        }

        public async Task<string> CreateIndex(string indexName)
        {
            try
            {
                var anyy = await _client.Indices.ExistsAsync(indexName);
                if (anyy.Exists)
                    return "indexName Exists";
                var createIndexResponse = _client.Indices.Create(indexName, c => c
                .HealthcareMapping()
                .Settings(s => s
                .Analysis(a => a
                .CharFilters(cf => cf
                .Mapping("healhcareES_custom", mcf => mcf
                .Mappings(
                "Bs=> Bác sĩ",
                "bs=> Bác sĩ",
                "BS => Bác sĩ",
                "Bac si=>Bác sĩ",
                "bac si=>Bác sĩ",
                "PK=>Phòng khám",
                "pk=>Phòng khám",
                "phong kham=>Phòng khám",
                "BV=> Bệnh viện",
                "bv=> Bệnh viện",
                "TMH=>Tai mũi họng",
                "tmh=>Tai mũi họng",
                "RHM=> Răng hàm mặt",
                "rhm=> Răng hàm mặt",
                "X-ray=> X quang",
                "x-quang=> X quang",
                "Sản phụ khoa=> khám thai")))
                .Analyzers(an => an
                .Custom("vn_analyzer", ca => ca
                .CharFilters("html_strip", "healhcareES_custom")
                .Tokenizer("vi_tokenizer")
                .Filters("lowercase", "stop", "icu_folding"))
                //asciifolding icu_folding
                ))));
                return "OK";
            }
            catch (Exception ex) { return ex.ToString(); }

        }

        // Tìm kiếm theo tọa độ
        public async Task<List<HealthCareModel>> GetDataSearchGeo(double lat, double ln, GeoDistanceType type, string distance, int pageSize)
        {
            try
            {
                var geo = await _client.SearchAsync<HealthCareModel>(
                s => s.Index("healthcare").Size(pageSize).Sort(s => s.Descending(SortSpecialField.Score)).Query(
                    q => q.GeoDistance(g => g
                    .Boost(1.1).Name("named_query")
                    .Field(p => p.geoLocation)
                    .DistanceType(type)
                    .Location(lat, ln)
                    .Distance(distance)
                    .ValidationMethod(GeoValidationMethod.IgnoreMalformed)
                    ))
                );

                return geo.Documents.ToList();
            }
            catch
            {
                return null;
            }
        }


        // Tìm kiếm theo tọa độ và từ khóa
        public async Task<List<HealthCareModel>> GetDataSearchGeo(double lat, double ln, GeoDistanceType type, string distance, int pageSize, string keyword)
        {
            try
            {
                var geo = await _client.SearchAsync<HealthCareModel>(
                s => s.Index("healthcare").Size(pageSize).Sort(s => s.Descending(SortSpecialField.Score)).Query(q => q.Match(m => m.Field(f => f.keywords).Query(keyword).Fuzziness(Fuzziness.Auto))).PostFilter(
                    q => q.GeoDistance(
                        g => g
                        .Boost(1.1).Name("named_query")
                        .Field(p => p.geoLocation).DistanceType(type).Location(lat, ln)
                            .Distance(distance).ValidationMethod(GeoValidationMethod.IgnoreMalformed)
                        ))
                );
                return geo.Documents.ToList();
            }
            catch
            {
                return null;
            }
        }

        public async Task<string> InsertData(string indexName, List<HealthCareModel> healthCares)
        {
            try
            {
                foreach (HealthCareModel item in healthCares)
                {
                    item.geoLocation = new GeoLocation(item.Latitude, item.Longitude);
                }
                //.BulkAsync(b => b.Index("products").IndexMany(products));
                //_client.BulkAsync(b => b.Index("healthCares").IndexMany(healthCares));
                var response = await _client.IndexManyAsync(healthCares, indexName);
                ////var response = await _client.CreateAsync(model, q => q.Index("healthcare"));
                //var response = await _client.CreateAsync(healthCares[0], q => q.Index("healthcare"));
                //if (response.ApiCall?.HttpStatusCode == 409)
                //{
                //    await _client.UpdateAsync<HealthCareModel>(healthCares[0].id, a => a.Index("healthcare").Doc(healthCares[0]));
                //}

                //await _client.IndexDocumentAsync(model);
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }

        public async Task<List<string>> GetDataSuggestion(string keyword, int pageSize)
        {
            try
            {
                var response = await _client.SearchAsync<HealthCareModel>(
                  s => s.Index("healthcare").Query(
                    q => q.Match(
                     m => m.Field(f => f.specialist).Query(keyword).Fuzziness(Fuzziness.Auto)
                    )
                  ).Size(pageSize).Sort(s => s.Descending(SortSpecialField.Score))
                );
                var rs = response.Documents.Select(x => $"{x.name} - {x.specialist}");
                return rs.ToList();
            }
            catch (Exception ex) { return null; }
        }
    }
}
