using ElasticProject.Bll.MapObj;
using ElasticProject.Data.Entity.MapObj;
using ElasticProject.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ElasticProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ElsGeopointController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private IElsGeopointService _elasticsearchService;
        public ElsGeopointController(IConfiguration configuration, IElsGeopointService elasticsearchService)
        {
            _configuration = configuration;
            _elasticsearchService = elasticsearchService;
        }

        [HttpPost]
        [Route("BulkAsync")]
        public async Task<string> Bulk(string indexName = "els-geopoint",[FromBody]List<ElasticRequestPushGeopoint> geopoints = null )
        {
            return await _elasticsearchService.BulkAsync(indexName, geopoints);
        }
    }
}
