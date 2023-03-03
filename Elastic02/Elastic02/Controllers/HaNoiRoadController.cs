using Elastic02.Models.Test;
using Elastic02.Services;
using Elastic02.Services.Test;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace Elastic02.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HaNoiRoadController : ControllerBase
    {
        private readonly IElasticGeoRepository<HaNoiRoadPoint> _haNoiGeoService;
        private readonly IHaNoiRoadService _haNoiRoadService;

        public HaNoiRoadController(IElasticGeoRepository<HaNoiRoadPoint> haNoiGeoService, IHaNoiRoadService haNoiRoadService)
        {
            _haNoiGeoService = haNoiGeoService;
            _haNoiRoadService = haNoiRoadService;
        }

        [HttpGet]
        [Route("CreateIndex")]
        public async Task<string> CreateIndex()
        {
            return await _haNoiRoadService.CreateIndex();
            //await _haNoiGeoService.CreateIndexGeoAsync();
            //return "OK";
        }

        [HttpPost]
        [Route("BulkHaNoiGeo")]
        public async Task<IActionResult> BulkHaNoiGeo([FromBody] List<HaNoiRoadPush> geopointsPush)
        {
            List<HaNoiRoadPoint> geopoints = new List<HaNoiRoadPoint>();

            // Check xem khởi tạo index chưa, nếu chưa khởi tạo thì phải khởi tạo index mới được
            if (geopointsPush.Any())
                geopointsPush.ForEach(item => geopoints.Add(new HaNoiRoadPoint(item)));

            return Ok(await _haNoiRoadService.BulkAsync(geopoints));
        }

        [HttpPost]
        [Route("CreateAsyncHaNoiGeo")]
        public async Task<IActionResult> CreateAsyncHaNoiGeo([FromBody] List<HaNoiRoadPush> geopointsPush)
        {
            return Ok(await _haNoiRoadService.CreateAsync(geopointsPush));
        }

        [HttpGet]
        [Route("GetDataSuggestion")]
        public async Task<List<HaNoiRoadPush>> GetDataSuggestion(double lat = 0, double lng = 0, string distance = "300km", int size = 5, string keyword = null)
        {
            //double lat = 21.006423010707078, double lng = 105.83878960584113, string distance = "30000m", int pageSize = 10, string keyWord = null
            return await _haNoiRoadService.GetDataSuggestion(lat, lng, GeoDistanceType.Arc, distance, size, keyword);
        }
    }
}
