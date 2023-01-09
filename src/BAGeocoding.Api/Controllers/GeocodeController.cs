using System.Collections.Generic;
using System.Web.Http;
using BAGeocoding.Bll;
using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.Public;
using BAGeocoding.Entity.Utility;

namespace BAGeocoding.Api.Controllers
{
    public class GeocodeController : ApiController
    {
        [HttpGet]
        public List<PBLAddressResult> AddressByGeo(string lat, string lng)
        {
            var result = new List<PBLAddressResult>();
            if (RunningParams.ProcessState != EnumProcessState.Success)
            {
                return result;
            }
            var addresses = MainProcessing.AddressByGeo(lng, lat);
            if (addresses?.Count > 0)
            {
                addresses.ForEach(item => result.Add(new PBLAddressResult(item)));
            }

            return result;
        }

        [HttpGet]
        public PBLAddressResult GeoByAddress(string address)
        {
            var result = new PBLAddressResult();
            if (RunningParams.ProcessState != EnumProcessState.Success)
            {
                return result;
            }
            var geo = MainProcessing.GeoByAddress(address, "vn");
            if (geo == null) return result;
            return new PBLAddressResult(geo);
        }
    }
}
