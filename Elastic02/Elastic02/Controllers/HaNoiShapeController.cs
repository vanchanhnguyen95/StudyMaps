using Elastic02.Models.Test;
using Elastic02.Services.Test;
using Elastic02.Services;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace Elastic02.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HaNoiShapeController : ControllerBase
    {
        private readonly IElasticGeoRepository<HaNoiShape> _haNoiGeoService;
        private readonly IHaNoiShapeService _haNoiService;

        public HaNoiShapeController(IElasticGeoRepository<HaNoiShape> haNoiGeoService, IHaNoiShapeService haNoiService)
        {
            _haNoiGeoService = haNoiGeoService;
            _haNoiService = haNoiService;
        }

        [HttpPost]
        [Route("Bulk")]
        public async Task<IActionResult> Bulk([FromBody] List<HaNoiShapePush> haNoiShapePush = null)
        {
            List<HaNoiShape> haNoiShapes = new List<HaNoiShape>();

            if (haNoiShapePush.Any())
                haNoiShapePush.ForEach(item => haNoiShapes.Add(new HaNoiShape(item)));

            return Ok(await _haNoiService.BulkAsync(haNoiShapes));
        }

        [HttpGet]
        [Route("GetDataSuggestion")]
        public async Task<List<HaNoiShapePush>> GetDataSuggestion(double lat = 0, double lng = 0, string distance = "300km", int size = 5, string keyword = null, GeoShapeRelation relation = GeoShapeRelation.Contains)
        {
            //double lat = 21.006423010707078, double lng = 105.83878960584113, string distance = "30000m", int pageSize = 10, string keyWord = null
            return await _haNoiService.GetDataSuggestion(lat, lng, GeoDistanceType.Arc, distance, size, keyword, relation);
        }
    }
}
