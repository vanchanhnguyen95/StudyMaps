using Nest;
using System.ComponentModel;

namespace Elastic02.Models.Test
{
    public class Crs
    {
        public string type { get; set; }
        public Properties properties { get; set; }
    }

    public class Feature
    {
        public string type { get; set; }
        public Properties properties { get; set; }
        public Geometry geometry { get; set; }
    }

    public class Geometry
    {
        public string type { get; set; }
        public List<List<List<object>>> coordinates { get; set; }
    }

    public class Properties
    {
        // Tinh
        public string? name { get; set; }
        public float? gid { get; set; }
        public string? code { get; set; }
        public string? ten_tinh { get; set; }

        // Huyen
        //public string name { get; set; }
        public int? OBJECTID { get; set; }
        public string? f_code { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public double? Pop_2009 { get; set; }
        public string? Code_re { get; set; }
        //public int? gid { get; set; }
        //public string code { get; set; }
        //public string ten_tinh { get; set; }

        // Giao thong
        //public string name { get; set; }
        //public int gid { get; set; }
        public string? ma { get; set; }
        public string? ten { get; set; }
        public string? loai_duong { get; set; }
        public string? cap_duong { get; set; }
        public double? chieu_dai { get; set; }
        public string? map_id { get; set; }
    }
 
    public class GeoJsonVietNam
    {
        public string? typedata { get; set; }//P:Province; D: District, T: traffic
        //public string? type { get; set; }
        //public Crs? crs { get; set; }
        //public List<Feature>? features { get; set; }
    }

    [ElasticsearchType(IdProperty = nameof(Id)), Description("vietnamshape")]
    public class VietNamShape
    {
        [Number(Index = true)]
        public int? id { get; set; } = 0;

        [Number(Index = true)]
        public int provinceid { get; set; } = 0;

        [Text(Index = true, Fielddata = true, Analyzer = "vi_analyzer_road")]
        public string? province { get; set; }

        [Text(Index = true, Fielddata = true, Analyzer = "vi_analyzer_road")]
        public string? name { get; set; } = string.Empty;

        [Text(Index = true, Fielddata = true, Analyzer = "vi_analyzer_road")]
        public string? typename { get; set; } = string.Empty;

        [GeoShape]
        public string? location { get; set; } = string.Empty;

        //public string? ename { get; set; } = string.Empty;
        [Text(Index = true, Fielddata = true, Analyzer = "vi_analyzer_road")]
        public string keywords { get; set; } = string.Empty;

        public VietNamShape() {}

        public VietNamShape(VietNamShape orther) {
            id = orther.id;
            provinceid = orther.provinceid;
            province = orther.province;
            name = orther.name;
            typename = orther.typename;
            location = orther.location;
            keywords = orther.keywords;
        }

        public VietNamShape(HaNoiShape orther)
        {
            id = orther.id;
            provinceid = 16;
            name = orther.name;
            typename = @"Đường Hà Nội";
            location = orther.location;
            keywords = orther.keywords;
        }

        public VietNamShape(HaNoiShapePush orther)
        {
            id = orther.id;
            provinceid = 16;
            name = orther.name;
            typename = @"Đường Hà Nội";
            //keywords = orther.keywords;

            if (!string.IsNullOrEmpty(orther.extend))
            {
                keywords = orther?.name?.ToString() + ", " + orther?.extend?.ToString();
            }
            else
            {
                keywords = orther.name ?? "";
            }

            if (orther?.shapeid == 1)//Điểm
            {
                double[] arrayCoords = new double[] { (orther?.lng ?? 0), (orther?.lat ?? 0) };
                string loc = string.Join(" ", arrayCoords);

                location = $"POINT ( {loc} )";
            }
            else if (orther?.shapeid == 2)//Đường
            {
                List<string> lsyLine = new List<string>();
                string lineString = string.Empty;
                for (int i = 0; i < orther?.coords?.Count; i++)
                {
                    double[] arrayCoords = new double[] { orther.coords[i].lng, orther.coords[i].lat };
                    string loc = string.Join(" ", arrayCoords);
                    lsyLine.Add(loc);
                }

                lineString = string.Join(",", lsyLine);
                location = $"LINESTRING  (  {lineString}  )";
            }
            else if (orther?.shapeid == 3)// Vùng
            {
                List<string> lsyLine = new List<string>();
                string lineString = string.Empty;
                for (int i = 0; i < orther?.coords?.Count; i++)
                {
                    double[] arrayCoords = new double[] { orther.coords[i].lng, orther.coords[i].lat };
                    string loc = string.Join(" ", arrayCoords);
                    lsyLine.Add(loc);
                }

                lineString = string.Join(",", lsyLine);
                location = "POLYGON  ((" + lineString + "))";
            }

        }
    }

    public class VietNamResponse
    {
        public string? name { get; set; } = string.Empty;
        [GeoShape]
        public string? location { get; set; } = string.Empty;
        public string? typename { get; set; } = string.Empty;
        public int? provinceid { get; set; } = 0;

        public string? province { get; set; } = string.Empty;
      
        public VietNamResponse(VietNamShape orther)
        {
            name    = orther.name;
            location = orther.location;
            typename = orther.typename;
            provinceid = orther.provinceid;
            province = orther.province;
        }

    }


}
