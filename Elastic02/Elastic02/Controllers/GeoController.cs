using Elastic02.Models;
using Elastic02.Services;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace Elastic02.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeoController : ControllerBase
    {
        private readonly IElasticGeoRepository<ElasticRequestPushGeopoint> _geopointService;
        private readonly IElasticGeoRepository<ElasticRequestPushGeoshape> _geoshapeService;
       
        public GeoController(IElasticGeoRepository<ElasticRequestPushGeopoint> geopointService, IElasticGeoRepository<ElasticRequestPushGeoshape> geoshapeService)
        {
            _geopointService = geopointService;
            _geoshapeService = geoshapeService;
        }

        [HttpGet]
        [Route("GetDataSearchGeo")]
        public async Task<List<string>> GetDataSearchGeo(double lat = 21.006423010707078, double ln = 105.83878960584113, string distance = "30000m", int pageSize = 10, string keyword = null)
        {
            var response = await _geopointService.GetDataSearchGeo(lat, ln, GeoDistanceType.Arc, distance, pageSize);
            return response;
        }

        [HttpPost]
        [Route("BulkGeopoint")]
        public async Task<IActionResult> BulkGeopoint([FromBody] List<ElasticRequestPush> geopointsPush)
        {
            List<ElasticRequestPushGeopoint> geopoints = new List<ElasticRequestPushGeopoint>();

            // Check xem khởi tạo index chưa, nếu chưa khởi tạo thì phải khởi tạo index mới được
            if (geopointsPush.Any())
                geopointsPush.ForEach(item => geopoints.Add(new ElasticRequestPushGeopoint(item)));

            return Ok(await _geopointService.BulkAsync(geopoints));
        }

        [HttpPost]
        [Route("BulkGeoshape")]
        public async Task<IActionResult> BulkGeoshape([FromBody] List<ElasticRequestPush> geopointsPush)
        {
            List<ElasticRequestPushGeoshape> geoshapes = new List<ElasticRequestPushGeoshape>();

            // Check xem khởi tạo index chưa, nếu chưa khởi tạo thì phải khởi tạo index mới được
            if (geopointsPush.Any())
                geopointsPush.ForEach(item => geoshapes.Add(new ElasticRequestPushGeoshape(item)));

            return Ok(await _geoshapeService.BulkAsync(geoshapes));
        }
    }
}
