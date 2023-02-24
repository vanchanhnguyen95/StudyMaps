using ElasticProject.Data.Entity.MapObj;
using ElasticProject.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ElasticProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ElsGeoshapeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private IElsGeoshapeService _elasticsearchService;
        public ElsGeoshapeController(IConfiguration configuration, IElsGeoshapeService elasticsearchService)
        {
            _configuration = configuration;
            _elasticsearchService = elasticsearchService;
        }

        [HttpPost]
        [Route("BulkAsync")]
        public async Task<string> Bulk(string indexName = "els-geoshape", [FromBody] List<ElasticRequestPushGeoshape> geoshapesPush = null)
        {
            return await _elasticsearchService.BulkAsyncElasticRequestGeoshape(indexName, geoshapesPush);
        }
    }
}
