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
        public int? gid { get; set; }
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
            name = orther.name;
            typename = orther.typename;
            location = orther.location;
            keywords = orther.keywords;
        }
    }

    
}
