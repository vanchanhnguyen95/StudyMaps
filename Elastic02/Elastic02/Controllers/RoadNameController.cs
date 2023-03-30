using Elastic02.Models.Test;
using Elastic02.Services.Test;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Newtonsoft.Json;

namespace Elastic02.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoadNameController : ControllerBase
    {
        private readonly IRoadNameService _roadNameService;

        public RoadNameController(IRoadNameService roadNameService)
        {
            _roadNameService = roadNameService;
        }

        [HttpPost]
        [Route("BulkAsync")]
        //public async Task<IActionResult> BulkAsync([FromBody] List<RoadNamePush> roadPushs)
        public async Task<IActionResult> BulkAsync(string path = @"D:\Project\Els01\Db\pastedText.json")
        {

            if (string.IsNullOrEmpty(path)) path = @"D:\Project\Els01\Db\pastedText.json";
            //string path = @"D:\Project\Els01\Db\RoadName.json";
            var jsonData = System.IO.File.ReadAllText(path);
            var roadPushs = JsonConvert.DeserializeObject<List<RoadNamePush>>(jsonData);

            return Ok(await _roadNameService.BulkAsync(roadPushs?? new List<RoadNamePush>()));
        }

        [HttpPost]
        [Route("BulkAsyncFromData")]
        public async Task<IActionResult> BulkAsyncFromData([FromBody] List<RoadNamePush> roadPushs)
        {
            //if (string.IsNullOrEmpty(path)) path = @"D:\Project\Els01\Db\pastedText.json";
            ////string path = @"D:\Project\Els01\Db\RoadName.json";
            //var jsonData = System.IO.File.ReadAllText(path);
            //var roadPushs = JsonConvert.DeserializeObject<List<RoadNamePush>>(jsonData);

            return Ok(await _roadNameService.BulkAsync(roadPushs));
            //return Ok(await _roadNameService.BulkAsyncMultiProvince(roadPushs ?? new List<RoadNamePush>()));
        }

        [HttpGet]
        [Route("GetDataSuggestion")]
        public async Task<IActionResult> GetDataSuggestion(double lat = 0, double lng = 0, string distance = "100km", int size = 5, string keyword = null)
        {
            //double lat = 21.006423010707078, double lng = 105.83878960584113, string distance = "30000m", int pageSize = 10, string keyWord = null
            return Ok(await _roadNameService.GetDataSuggestion(lat, lng, distance, size, keyword));
        }

        [HttpGet]
        [Route("GetRouting")]
        public async Task<IActionResult> GetRouting(double latStart = 0, double lngStart = 0, double latEnd = 0, double lngEnd = 0, int size = 10)
        {
            //poingStart = new GeoLocation(20.97263381, 105.77930601);
            //pointEnd = new GeoLocation(20.99874272, 105.81312923);

            GeoLocation poingStart = new GeoLocation(latStart, lngStart);
            GeoLocation pointEnd = new GeoLocation(latEnd, lngEnd);

            return Ok(await _roadNameService.GetRouting(poingStart, pointEnd, size));
        }
    }
}
