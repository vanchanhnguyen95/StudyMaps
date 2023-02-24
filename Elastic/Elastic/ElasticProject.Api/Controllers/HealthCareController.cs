using ElasticProject.Data.Entity.MapObj;
using ElasticProject.Data.Interface;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ElasticProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthCareController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private IHealthCareService _elasticsearchService;
        public HealthCareController(IConfiguration configuration, IHealthCareService elasticsearchService)
        {
            _configuration = configuration;
            _elasticsearchService = elasticsearchService;
        }

        [HttpPost]
        [Route("CreateIndex")]
        public async Task<string> CreateIndex(string indexName = "healthcare")
        {
            return await _elasticsearchService.CreateIndex(indexName);
        }

        [HttpPost]
        [Route("InsertData")]
        public async Task<string> InsertData(string indexName = "healthcare", [FromBody] List<HealthCareModel> healthCareModels = null)
        {
            List<HealthCareModel> healthCares = healthCareModels;
            return await _elasticsearchService.InsertData(indexName, healthCares);
        }

        [HttpGet]
        [Route("GetDataSuggestion")]
        public async Task<List<string>> GetDataSuggestion(string keyWord = "", int size = 10)
        {
            return await _elasticsearchService.GetDataSuggestion(keyWord, size);
        }

        [HttpGet]
        [Route("GetDataSearchGeo")]
        public async Task<List<HealthCareModel>> GetDataSearchGeo(double lat = 21.006423010707078, double ln = 105.83878960584113, string distance = "30000m", int pageSize = 10, string keyword = null)
        {
            List<HealthCareModel> rs = null;
            if (string.IsNullOrEmpty(keyword))
            {
                rs = await _elasticsearchService.GetDataSearchGeo(lat, ln, GeoDistanceType.Arc, distance, pageSize);
            }
            else
            {
                rs = await _elasticsearchService.GetDataSearchGeo(lat, ln, GeoDistanceType.Arc, distance, pageSize, keyword);
            }

            return rs;
        }
    }
}
