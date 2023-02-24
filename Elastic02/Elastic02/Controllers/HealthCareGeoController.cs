using Elastic02.Models;
using Elastic02.Services;
using Microsoft.AspNetCore.Mvc;

namespace Elastic02.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthCareGeoController : ControllerBase
    {
        private readonly IElasticService<HealthCareGeo> _healthCareService;
        public HealthCareGeoController(IElasticService<HealthCareGeo> healthCareService)
        {
            _healthCareService = healthCareService;
        }

        [HttpGet]
        [Route("CreateIndexGeo")]
        public async Task<IActionResult> CreateIndexGeo()
        {
            await _healthCareService.CreateIndexGeoAsync();
            return Ok();
        }

        [HttpPost]
        [Route("Bulk")]
        public async Task<IActionResult> Bulk([FromBody]List<HealthCareGeo> objects)
        {
            return Ok(await _healthCareService.BulkAsync(objects));
        }
    }
}
