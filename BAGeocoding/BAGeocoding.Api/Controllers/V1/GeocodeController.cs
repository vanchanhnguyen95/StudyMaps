using BAGeocoding.Api.Services;
using BAGeocoding.Api.ViewModels;
using BAGeocoding.Bll;
using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.Public;
using Microsoft.AspNetCore.Mvc;

namespace BAGeocoding.Api.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class GeocodeController : ControllerBase
    {
        private readonly IGeoService _geoService;

        public GeocodeController(IGeoService geoService)
        {
            _geoService = geoService;
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        [Route("AddressByGeo")]
        public async Task<List<PBLAddressResult>> AddressByGeo([FromBody] AddressByGeoVm? body)
        {
            var result = new List<PBLAddressResult>();
            if (RunningParams.ProcessState != EnumProcessState.Success)
            {
                return result;
            }
            var addresses = await _geoService.AddressByGeoAsync(body?.lng, body?.lat);
            if (addresses?.Count > 0)
            {
                addresses.ForEach(item => result.Add(new PBLAddressResult(item)));
            }

            return result;
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        [Route("GeoByAddress")]
        public async Task<PBLAddressResult> GeoByAddress([FromBody] GeoByAddressVm? body)
        {
            var result = new PBLAddressResult();
            if (RunningParams.ProcessState != EnumProcessState.Success)
            {
                return result;
            }
            var geo = await _geoService.GeoByAddressAsync(body?.address, "vn");
            if (geo == null) return result;

            var objTest = new PBLAddressResult(geo);
            return new PBLAddressResult(geo);
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        [Route("GeoByAddressV2")]
        public async Task<PBLAddressResultV2> GeoByAddressV2([FromBody] GeoByAddressVm? body)
        {
            var result = new PBLAddressResultV2();
            if (RunningParams.ProcessState != EnumProcessState.Success)
            {
                return result;
            }
            var geo = await _geoService.GeoByAddressAsyncV2(body?.address, "vn");
            if (geo == null) return result;

            var objTest = new PBLAddressResultV2(geo);
            return new PBLAddressResultV2(geo);
        }
    }
}
