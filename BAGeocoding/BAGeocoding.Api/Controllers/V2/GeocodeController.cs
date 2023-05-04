using BAGeocoding.Api.Models;
using BAGeocoding.Api.Services;
using BAGeocoding.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BAGeocoding.Api.Controllers.V2
{
    [ApiVersion("2.0")]
    public class GeocodeController : BaseController
    {
        private readonly IGeoService _geoService;

        public GeocodeController(IGeoService geoService)
        {
            _geoService = geoService;
        }

        [HttpPost]
        [MapToApiVersion("2.0")]
        [Route("GeoByAddress2")]
        public async Task<Result<object>> GeoByAddress2([FromBody] GeoByAddressVm? body)
        {
            return await _geoService.GeoByAddressAsyncV2(body?.address, "vn");
        }
    }
}
