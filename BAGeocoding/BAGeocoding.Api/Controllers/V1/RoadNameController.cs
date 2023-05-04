using BAGeocoding.Api.Interfaces;
using BAGeocoding.Api.Models.RoadName;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Newtonsoft.Json;

namespace BAGeocoding.Api.Controllers.V1
{
    //[ApiVersion("1.0")]
    //public class RoadNameController : BaseController
    //{
    //    private readonly IRoadNameService _roadNameService;

    //    public RoadNameController(IRoadNameService roadNameService)
    //    {
    //        _roadNameService = roadNameService;
    //    }

    //    [HttpPost]
    //    [MapToApiVersion("1.0")]
    //    [Route("BulkAsync")]
    //    public async Task<IActionResult> BulkAsync(string path = @"D:\Project\Els01\Db\pastedText.json")
    //    {

    //        if (string.IsNullOrEmpty(path)) path = @"D:\Project\Els01\Db\pastedText.json";
    //        var jsonData = System.IO.File.ReadAllText(path);
    //        var roadPushs = JsonConvert.DeserializeObject<List<RoadNamePush>>(jsonData);

    //        return Ok(await _roadNameService.BulkAsync(roadPushs ?? new List<RoadNamePush>()));
    //    }

    //    [HttpGet]
    //    [MapToApiVersion("1.0")]
    //    [Route("GetDataSuggestion")]
    //    public async Task<IActionResult> GetDataSuggestion(double lat = 0, double lng = 0, string distance = "100km", int size = 5, string keyword = null, int type = -1)
    //    {
    //        // type: -1: tìm kiếm bao gồm phần mở rộng
    //        //double lat = 21.006423010707078, double lng = 105.83878960584113, string distance = "30000m", int pageSize = 10, string keyWord = null
    //        return Ok(await _roadNameService.GetDataSuggestion(lat, lng, distance, size, keyword, type));
    //    }
    //}
}
