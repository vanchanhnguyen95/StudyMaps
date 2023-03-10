using Elastic02.Models.Test;
using Elastic02.Services.Test;
using Elastic02.Services;
using Microsoft.AspNetCore.Mvc;

namespace Elastic02.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HaNoiShapeController : ControllerBase
    {
        private readonly IElasticGeoRepository<HaNoiRoadPoint> _haNoiGeoService;
        private readonly IHaNoiShapeService _haNoiService;

        public HaNoiShapeController(IElasticGeoRepository<HaNoiRoadPoint> haNoiGeoService, IHaNoiShapeService haNoiService)
        {
            _haNoiGeoService = haNoiGeoService;
            _haNoiService = haNoiService;
        }

        [HttpPost]
        [Route("Bulk")]
        public async Task<IActionResult> Bulk([FromBody] List<HaNoiShapePush> haNoiShapePush)
        {
            List<HaNoiShape> haNoiShapes = new List<HaNoiShape>();

            if (haNoiShapePush.Any())
                haNoiShapePush.ForEach(item => haNoiShapes.Add(new HaNoiShape(item)));

            return Ok(await _haNoiService.BulkAsync(haNoiShapes));
        }
    }
}
