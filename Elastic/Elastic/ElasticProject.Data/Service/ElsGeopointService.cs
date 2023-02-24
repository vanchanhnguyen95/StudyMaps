using ElasticProject.Data.Entity.MapObj;
using ElasticProject.Data.Interface;
using Microsoft.Extensions.Configuration;
using Nest;

namespace ElasticProject.Data.Service
{
    public class ElsGeopointService : IElsGeopointService
    {
        private readonly IConfiguration _configuration;
        private readonly IElasticClient _client;

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

        public ElsGeopointService(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = CreateInstance();
        }


        public async Task<string> BulkAsync(string indexName, List<ElasticRequestPushGeopoint> geopointsPush)
        {
            try
            {
                List<ElasticRequestCreateGeopoint> geopoints = new List<ElasticRequestCreateGeopoint>();
                if (geopointsPush.Any())
                    geopointsPush.ForEach(item => geopoints.Add(new ElasticRequestCreateGeopoint(item)));

                var response = await _client.BulkAsync(b => b.Index(indexName).IndexMany(geopoints));
                if (response.ApiCall.Success) return "Success";
                return "False";
            }
            catch (Exception ex) { return ex.ToString(); }
        }
    }
}
