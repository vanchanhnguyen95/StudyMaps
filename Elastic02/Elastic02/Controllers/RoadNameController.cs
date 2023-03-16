using Elastic02.Models.Test;
using Elastic02.Services.Test;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Newtonsoft.Json;
using System.Collections.Generic;

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
        public async Task<IActionResult> BulkAsync(string path = @"D:\Project\Els01\Db\RoadName.json")
        {
            if(string.IsNullOrEmpty(path)) path = @"D:\Project\Els01\Db\RoadName.json";
            //string path = @"D:\Project\Els01\Db\RoadName.json";
            var jsonData = System.IO.File.ReadAllText(path);
            var roadPushs = JsonConvert.DeserializeObject<List<RoadNamePush>>(jsonData);

            return Ok(await _roadNameService.BulkAsync(roadPushs));
            //return Ok(await _roadNameService.BulkAsyncMultiProvince(roadPushs ?? new List<RoadNamePush>()));
        }

        [HttpGet]
        [Route("GetDataSuggestion")]
        public async Task<IActionResult> GetDataSuggestion(double lat = 0, double lng = 0, string distance = "100km", int size = 5, string keyword = null, GeoShapeRelation relation = GeoShapeRelation.Intersects, int provinceID = -1)
        {
            //double lat = 21.006423010707078, double lng = 105.83878960584113, string distance = "30000m", int pageSize = 10, string keyWord = null
            if (provinceID < 0)
                return Ok(await _roadNameService.GetDataSuggestion(lat, lng, GeoDistanceType.Arc, distance, size, keyword, relation));

            return Ok(await _roadNameService.GetDataSuggestion(lat, lng, GeoDistanceType.Arc, distance, size, keyword, relation, provinceID));
        }
    }
}
