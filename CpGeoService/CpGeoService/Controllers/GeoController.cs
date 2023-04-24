using CpGeoService.Interfaces;
using CpGeoService.Model;
using Microsoft.AspNetCore.Mvc;

namespace CpGeoService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeoController : ControllerBase
    {
        private readonly IGeocodeRepository _geocodeRepository;
        public GeoController(IGeocodeRepository geocodeRepository)
        {
            _geocodeRepository = geocodeRepository;
        }

        [HttpPost]
        [Route("GeoByListAddress")]
        public async Task<IActionResult> GeoByListAddressAsync(List<InputAddress> addressList)
        {
            return Ok(await _geocodeRepository.GeoByAddressAsync(addressList));
        }
    }
}
