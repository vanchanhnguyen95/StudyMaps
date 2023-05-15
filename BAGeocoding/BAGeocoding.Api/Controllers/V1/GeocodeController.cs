using BAGeocoding.Api.Interfaces;
using BAGeocoding.Api.Models;
using BAGeocoding.Api.Services;
using BAGeocoding.Api.ViewModels;
using BAGeocoding.Entity.Public;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace BAGeocoding.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    public class GeocodeController : BaseController
    {
        private readonly IGeoService _geoService;
        private readonly IRoadNameService _roadNameService;

        public GeocodeController(IGeoService geoService, IRoadNameService roadNameService)
        {
            _geoService = geoService;
            _roadNameService = roadNameService;
        }

        //[HttpPost]
        //[MapToApiVersion("1.0")]
        //[Route("AddressByGeoV2")]
        //public async Task<List<PBLAddressResult>> AddressByGeoV2([FromBody] AddressByGeoVm? body)
        //{
        //    var result = new List<PBLAddressResult>();
        //    //if (RunningParams.ProcessState != EnumProcessState.Success)
        //    //{
        //    //    return result;
        //    //    //MainProcessing.InitData();
        //    //}

        //    //var result = new List<PBLAddressResultV2>();

        //    //var addresses = await _geoService.AddressByGeoAsyncV2(body?.lng, body?.lat);
        //    //if(addresses?.Count > 0)
        //    //{
        //    //    addresses.ForEach(item => result.Add(new PBLAddressResultV2(item)));
        //    //}    

        //    var addresses = await _geoService.AddressByGeoAsync(body?.lng, body?.lat);
        //    if (addresses?.Count > 0)
        //    {
        //        addresses.ForEach(item => result.Add(new PBLAddressResult(item)));
        //    }

        //    return result;
        //}

        [HttpPost]
        [MapToApiVersion("1.0")]
        [Route("AddressByGeo")]
        public async Task<Result<object>> AddressByGeo([FromBody] AddressByGeoVm? body)
        {
            return await  _geoService.AddressByGeoAsyncV2(body?.lng, body?.lat); ;
        }

        //[HttpPost]
        //[MapToApiVersion("1.0")]
        //[Route("GeoByAddress2")]
        //public async Task<PBLAddressResult> GeoByAddress2([FromBody] GeoByAddressVm? body)
        //{
        //    var result = new PBLAddressResult();
        //    //if (RunningParams.ProcessState != EnumProcessState.Success)
        //    //{
        //    //    return result;
        //    //}
        //    var geo = await _geoService.GeoByAddressAsync(body?.address, "vn");
        //    if (geo == null) return result;

        //    var objTest = new PBLAddressResult(geo);
        //    return new PBLAddressResult(geo);
        //}

        [HttpPost]
        [MapToApiVersion("1.0")]
        [Route("GeoByAddress")]
        public async Task<Result<object>> GeoByAddress([FromBody] GeoByAddressVm? body)
        {
            return await _geoService.GeoByAddressAsyncV2(body?.address, "vn");
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [Route("GetDataSuggestion")]
        public async Task<IActionResult> GetDataSuggestion(double lat = 0, double lng = 0, string distance = "100km", int size = 5, string keyword = null, int type = -1)
        {
            // type: -1: tìm kiếm bao gồm phần mở rộng
            //double lat = 21.006423010707078, double lng = 105.83878960584113, string distance = "30000m", int pageSize = 10, string keyWord = null
            return Ok(await _roadNameService.GetDataSuggestion(lat, lng, distance, size, keyword, type));
        }
    }
}
