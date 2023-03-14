using Elastic02.Models.Test;
using Elastic02.Services.Test;
using Microsoft.AspNetCore.Mvc;
using Nest;
using NetTopologySuite.Features;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using System;
using FeatureCollection = NetTopologySuite.Features.FeatureCollection;

namespace Elastic02.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class GeojsonController : ControllerBase
    {
        private readonly IVietNamShapeService _vnShapeService;

        public GeojsonController(IVietNamShapeService vnShapeService)
        {
            _vnShapeService = vnShapeService;
        }

        [HttpPost]
        [Route("PostMultiFile")]
        public async Task<IActionResult> PostMultiFile (List<IFormFile> files)
        {
            if (!files.Any()) return BadRequest("Invalid file");

            try
            {
                int i = 1;
                List<VietNamShape> vnShape = new List<VietNamShape>();

                foreach (IFormFile file in files)
                {
                    using var stream = file.OpenReadStream();
                    using var reader = new StreamReader(stream);

                    FeatureCollection featureCollection;
                    var jsonData = await reader.ReadToEndAsync();

                    var vnCheck = JsonConvert.DeserializeObject<GeoJsonVietNam>(jsonData);

                    featureCollection = new GeoJsonReader().Read<FeatureCollection>(jsonData);

                    if (featureCollection == null) Ok("Fail");

                    // Lặp qua từng đối tượng Feature trong FeatureCollection và trích xuất thông tin về đối tượng địa lý
                    foreach (var feature in featureCollection)
                    {
                        //VietNamShape item = GetNamShape(feature, type);
                        vnShape.Add(GetNamShape(feature, vnCheck.typedata, i));
                        i++;
                    }
                   
                }
                i = 0;
                return Ok(await _vnShapeService.BulkAsync(vnShape));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }

        [HttpGet]
        [Route("GetDataSuggestion")]
        public async Task<List<VietNamShape>> GetDataSuggestion(double lat = 0, double lng = 0, string distance = "300km", int size = 5, string keyword = null, GeoShapeRelation relation = GeoShapeRelation.Intersects)
        {
            return await _vnShapeService.GetDataSuggestion(lat, lng, GeoDistanceType.Arc, distance, size, keyword, relation);
        }


        private VietNamShape GetNamShape(IFeature feature, string type = "P", int i = 1)
        {
            // Đối tượng địa lý được lưu trữ trong biến "feature"
            // Truy cập đối tượng địa lý trong thuộc tính "Geometry" của Feature
            var writer = new WKTWriter();
            var geometry = feature.Geometry;

            VietNamShape item = new VietNamShape();
            item.location = writer.Write(geometry);

            if(type == "P")//Province
            {
                //item.id = Convert.ToInt32((feature.Attributes["gid"].ToString()) ?? "0");
                item.id = i;
                item.typename = "Tỉnh";
                item.name = feature.Attributes["ten_tinh"].ToString();
                item.keywords = feature.Attributes["ten_tinh"].ToString() ?? "";
            } else if(type == "D")//District
            {
                //item.id = Convert.ToInt32((feature.Attributes["OBJECTID"].ToString()) ?? "0");
                item.id = i;
                item.typename = @"Huyện";
                item.name = feature.Attributes["District"].ToString();
                item.keywords = feature.Attributes["District"].ToString() + ", " + feature.Attributes["Province"].ToString();
            } else if (type == "T")//Traffic
            {
                //item.id = Convert.ToInt32((feature.Attributes["gid"].ToString()) ?? "0");
                item.id = i;
                item.typename = @"Đường giao thông";
                item.name = feature.Attributes["ten"].ToString();
                item.keywords = feature.Attributes["ten"].ToString() ?? "";
            }

            return item;

        }
    }
}
