using Elastic02.Models.Test;
using Elastic02.Services.Test;
using Microsoft.AspNetCore.Mvc;
using Nest;
using NetTopologySuite.Features;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using FeatureCollection = NetTopologySuite.Features.FeatureCollection;

namespace Elastic02.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class GeojsonController : ControllerBase
    {
        private readonly IVietNamShapeService _vnShapeService;
        private readonly IHaNoiShapeService _haNoiService;

        public GeojsonController(IVietNamShapeService vnShapeService, IHaNoiShapeService haNoiService)
        {
            _vnShapeService = vnShapeService;
            _haNoiService = haNoiService;
        }

        [HttpPost]
        [Route("PostMultiFile2")]
        public async Task<IActionResult> PostMultiFile2([FromBody] List<HaNoiShapePush> haNoiShapePush = null)
        {
            List<string> paths = new List<string>();
            //paths.Add(@"D:\GeoJson\airport.geojson");
            //paths.Add(@"D:\GeoJson\atm_rice_covid-19.geojson");
            paths.Add(@"D:\GeoJson\diaphantinh.geojson");
            //paths.Add(@"D:\GeoJson\district.geojson");
            //paths.Add(@"D:\GeoJson\ga.geojson");
            //paths.Add(@"D:\GeoJson\giaothong.geojson");
            //paths.Add(@"D:\GeoJson\harborgeojson.geojson");
            //paths.Add(@"D:\GeoJson\hydropower_2020.geojson");
            //paths.Add(@"D:\GeoJson\khu-bao-ton-quoc-gia.geojson");
            //paths.Add(@"D:\GeoJson\sanbay.geojson");

            //if (!files.Any()) return BadRequest("Invalid file");

            try
            {
                List<VietNamShape> vnShape = new List<VietNamShape>();

                if (haNoiShapePush.Any())
                    haNoiShapePush.ForEach(item => vnShape.Add(new VietNamShape(item)));

                int i = haNoiShapePush.Count() +1;
                foreach (string path in paths)
                {
                    var jsonData = System.IO.File.ReadAllText(path);
                    var vnCheck = JsonConvert.DeserializeObject<GeoJsonVietNam>(jsonData);

                    var geoJsonReader = new GeoJsonReader();
                    var featureCollection = geoJsonReader.Read<FeatureCollection>(jsonData);

                    if (featureCollection == null) Ok("Fail");

                    // Lặp qua từng đối tượng Feature trong FeatureCollection và trích xuất thông tin về đối tượng địa lý
                    foreach (var feature in featureCollection)
                    {
                        //VietNamShape item = GetNamShape(feature, type);
                        vnShape.Add(GetNamShape(feature, vnCheck.typedata, i));
                        i++;
                    }
                }

                return Ok(await _vnShapeService.BulkAsync(vnShape));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }

        [HttpPost]
        [Route("PostMultiFile")]
        public async Task<IActionResult> PostMultiFile (List<IFormFile> files,[FromBody] List<HaNoiShapePush> haNoiShapePush = null)
        {
            if (!files.Any()) return BadRequest("Invalid file");

            try
            {
                List<VietNamShape> vnShape = new List<VietNamShape>();

                // Lấy dữ liệu từ index hanoishape
                if (haNoiShapePush.Any())
                    haNoiShapePush.ForEach(item => vnShape.Add(new VietNamShape(item)));

                int i = 1;
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
        public async Task<List<VietNamResponse>> GetDataSuggestion(double lat = 0, double lng = 0, string distance = "300km", int size = 5, string keyword = null, GeoShapeRelation relation = GeoShapeRelation.Intersects)
        {
            List<VietNamResponse> responses= new List<VietNamResponse>();
            List<VietNamShape> vietNamShapes = await _vnShapeService.GetDataSuggestion(lat, lng, GeoDistanceType.Arc, distance, size, keyword, relation);
            if (vietNamShapes.Any())
                vietNamShapes.ForEach(item => responses.Add(new VietNamResponse(item)));

            return responses;
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
                item.provinceid = Convert.ToInt32((feature.Attributes["provinceid"].ToString()) ?? "0"); ;
                item.name = feature.Attributes["ten_tinh"].ToString() ?? "";
                item.keywords = feature.Attributes["ten_tinh"].ToString() ?? "";
            } else if(type == "D")//District
            {
                item.id = i;
                item.typename = @"Huyện";
                item.name = feature.Attributes["District"].ToString();
                item.keywords = feature.Attributes["District"].ToString() ?? "" + ", " + feature.Attributes["Province"].ToString() ?? "";
            } else if (type == "T")//Traffic
            {
                item.id = i;
                item.typename = @"Đường giao thông";
                item.name = feature.Attributes["ten"].ToString() ?? "";
                item.keywords = feature.Attributes["ten"].ToString() ?? "";
            }
            else if (type == "H")//harbor
            {
                item.id = i;
                item.typename = @"Bến cảng";

                var na = feature.Attributes["Name"];
                string nastrin = string.Empty;
                if(na != null)
                {
                    nastrin = na.ToString()?? "";
                }

                item.name = nastrin;
                item.keywords = nastrin;
            }
            else if (type == "A")//airport
            {
                item.id = i;
                item.typename = @"Cảng hàng không";
                item.name = feature.Attributes["Name"].ToString() ?? "";
                item.keywords = feature.Attributes["Name"].ToString() ?? "" + ", " + feature.Attributes["City"].ToString() ?? "";
            }
            else if (type == "TS")//train station
            {
                item.id = i;
                item.typename = @"Ga đường sắt";
                item.name = feature.Attributes["Ten_Ga"].ToString() ?? "";
                item.keywords = feature.Attributes["Ten_Ga"].ToString() ?? "";
            }
            else if (type == "NP")//National Parks
            {
                item.id = i;
                item.typename = @"Vườn Quốc gia";
                item.name = feature.Attributes["Ten"].ToString() ?? ""; ;
                item.keywords = feature.Attributes["Ten"].ToString() ?? "";
            }
            //hydropower_2020
            else if (type == "HD")//hydropower
            {
                item.id = i;
                item.typename = @"Thủy điện";
                item.name = feature.Attributes["Vietnamese"].ToString() ?? ""; ;
                item.keywords = feature.Attributes["Vietnamese"].ToString() ?? "";
            }
            else if (type == "AG")//atm_rice_covid-19
            {
                item.id = i;
                item.typename = @"ATM Gạo";
                item.name = feature.Attributes["Name_VI"].ToString() ?? ""; ;
                item.keywords = feature.Attributes["Name_VI"].ToString() ?? "";
            }

            return item;

        }
    }
}
