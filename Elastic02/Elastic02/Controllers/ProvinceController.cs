using Elastic02.Models.Test;
using Elastic02.Services.Test;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using F23.StringSimilarity;

namespace Elastic02.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProvinceController : ControllerBase
    {

        private readonly IProvinceService _service;

        public ProvinceController(IProvinceService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("BulkAsync")]
        public async Task<IActionResult> BulkAsync(string path = @"D:\Project\Els01\Province.json")
        {

            if (string.IsNullOrEmpty(path)) path = @"D:\Project\Els01\Province.json";
            var jsonData = System.IO.File.ReadAllText(path);
            var roadPushs = JsonConvert.DeserializeObject<List<Province>>(jsonData);

            return Ok(await _service.BulkAsync(roadPushs ?? new List<Province>()));
        }

        [HttpGet]
        [Route("GetDataSuggestion")]
        public async Task<IActionResult> GetDataSuggestion(int size = 5, string keyword = null)
        {
            var ro = new RatcliffObershelp();

            double per = ro.Similarity("An Khánh TP. Hà Nội", "Khánh An ha noi");
            double per2 = ro.Similarity("Khánh An  TP. Hà Nội", "Khánh An ha noi");
            double per3 = ro.Similarity("Xuân Khanh  TP. Hà Nội", "Khánh An ha noi");
            //212,TRẦN HƯNG ĐẠO, TP.HÒA BÌNH
            //string[] lstkeyword = new[] { keyword };

            //string strOne = "One,Two,Three,Four";
            //string[] strArrayOne = new string[] { "" };
            ////somewhere in your code
            //strArrayOne = strOne.Split(',');

            // type: -1: tìm kiếm bao gồm phần mở rộng
            //double lat = 21.006423010707078, double lng = 105.83878960584113, string distance = "30000m", int pageSize = 10, string keyWord = null
            //return Ok(await _roadNameService.GetDataSuggestion(lat, lng, distance, size, keyword));
            return Ok(await _service.GetDataSuggestion(size, keyword));
        }
    }
}
