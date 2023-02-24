using ElasticProject.Data.Entity.MapObj;
using ElasticProject.Data.Interface;
using Microsoft.Extensions.Configuration;
using Nest;

namespace ElasticProject.Data.Service
{
    public class ElsGeoshapeService : IElsGeoshapeService
    {
        private readonly IConfiguration _configuration;
        private readonly IElasticClient _client;

        private ElasticClient CreateInstance()
        {
            string host = _configuration.GetSection("ElasticsearchServer:Host").Value??"";
            string port = _configuration.GetSection("ElasticsearchServer:Port").Value ?? "";
            string username = _configuration.GetSection("ElasticsearchServer:Username").Value ?? "";
            string password = _configuration.GetSection("ElasticsearchServer:Password").Value ?? "";
            var settings = new ConnectionSettings(new Uri(host + ":" + port));
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                settings.BasicAuthentication(username, password);

            return new ElasticClient(settings);
        }

        public ElsGeoshapeService(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = CreateInstance();
        }

        public async Task<string> BulkAsyncElasticRequestGeoshape(string indexName, List<ElasticRequestPushGeoshape> geoshapesPush)
        {
            try
            {
                // Check xem index đã tạo chưa, nếu chưa tạo thì phải tạo trước mới bulk được

                List<ElasticRequestCreateGeoshape> geoshapes = new List<ElasticRequestCreateGeoshape>();
                if (geoshapesPush?.Count > 0)
                    geoshapesPush.ForEach(item => geoshapes.Add(new ElasticRequestCreateGeoshape(item)));
               
                var response = await _client.BulkAsync(b => b.Index(indexName).IndexMany(geoshapes));
                if (response.ApiCall.Success) return "Success";
                return "False";
            }
            catch (Exception ex) { return ex.ToString(); }
        }
    }
}
