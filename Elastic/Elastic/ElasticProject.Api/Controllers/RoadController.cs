using ElasticProject.Bll.MapObj;
using ElasticProject.Data.Entity.MapObj;
using ElasticProject.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ElasticProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoadController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private IRoadService _elasticsearchService;
        public RoadController(IConfiguration configuration, IRoadService elasticsearchService)
        {
            _configuration = configuration;
            _elasticsearchService = elasticsearchService;
        }

        //[HttpPost]
        //[Route("DeleteIndex")]
        //public async Task<string> DeleteIndex(string indexName = "road")
        //{
        //    return await _elasticsearchService.DeleteIndex(indexName);
        //}

        //[HttpPost]
        //[Route("CreateIndex")]
        //public async Task<string> CreateIndex(string indexName = "road2")
        //{
        //   return await _elasticsearchService.CreateIndex(indexName);
        //}

        [HttpPost]
        [Route("Bulk")]
        public async Task<string> Bulk(string indexName = "road2")
        {
            string filePath = _configuration.GetSection("FilePath:Segment").Value??"";
            List<BGCElasticRequestCreate> elasticRequestCreates = SegmentManager.LoadData(filePath);
            return await _elasticsearchService.InsertBulkElasticRequest(indexName, elasticRequestCreates);
        }

        [HttpGet]
        [Route("GetRoads")]
        public async Task<List<BGCElasticRequestCreate>> GetRoads(string indexName = "road")
        {
           return await _elasticsearchService.GetRoads(indexName);
        }

        [HttpGet]
        [Route("GetDataSuggestion")]
        public async Task<List<string>> GetDataSuggestion(string indexName = "road", string keyWord = "", int size = 10)
        {
            return await _elasticsearchService.GetDataSuggestion(indexName, keyWord, size);
        }

    }

}
