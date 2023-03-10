using Nest;
using System.ComponentModel;

namespace Elastic02.Models.Test
{
    public class HaNoiShapePush
    {
        [Number(Index = true)]
        public float id { get; set; } = 0;

        [Text(Index = true, Fielddata = true, Analyzer = "vi_analyzer_road")]
        public string? name { get; set; } = string.Empty;

        [Text(Index = true, Fielddata = true, Analyzer = "vi_analyzer_road")]
        public string? extend { get; set; } = string.Empty;

        //[Number(Index = true)]
        //public double lng { get; set; } = 0;

        //[Number(Index = true)]
        //public double lat { get; set; } = 0;
        [Number(Index = true)]
        public short? shapeid { get; set; } = 1;

        public List<GeoLocation> coords { get; set; }

        public HaNoiShapePush() {
            coords = new List<GeoLocation>();
        }
        public HaNoiShapePush(HaNoiShapePush other)
        {
            id = other.id;
            name = other.name;
            //lat = other.lat;
            //lng = other.lng;
            extend = other.extend;
            shapeid = other.shapeid;
            coords = new List<GeoLocation>();
        }

    }

    [ElasticsearchType(IdProperty = nameof(Id)), Description("haNoi_shape")]
    public class HaNoiShape : HaNoiShapePush
    {
        [GeoShape]
        public string location { get; set; }

        //public string? ename { get; set; } = string.Empty;
        [Text(Index = true, Fielddata = true, Analyzer = "vi_analyzer_road")]
        public string keywords { get; set; } = string.Empty;

        public HaNoiShape(HaNoiShapePush other)
        {
            id = other.id;
            name = other.name;
            //lat = other.lat;
            //lng = other.lng;
            extend = other.extend;
            coords = other.coords;

            if (!string.IsNullOrEmpty(other.extend))
            {
                //keyword = $"{other.name} , {other.extend}";
                keywords = other?.name?.ToString() + ", " + other?.extend?.ToString();
            }
            else
            {
                keywords = other.name ?? "";
            }

            if (other.shapeid == 1)//Điểm
            {
                double[] arrayCoords = new double[] { other.coords[0].Longitude, other.coords[0].Latitude };
                string loc = string.Join(" ", arrayCoords);

                //location = "POINT (" + loc + ")";
                location = $"POINT (\" { loc } \")";
            }
            else if (other.shapeid == 2)//Đường
            {
                List<string> lsyLine = new List<string>();
                string lineString = string.Empty;
                for (int i = 0; i < other.coords.Count; i++)
                {
                    double[] arrayCoords = new double[] { other.coords[i].Longitude, other.coords[i].Latitude };
                    string loc = string.Join(" ", arrayCoords);
                    lsyLine.Add(loc);
                }

                lineString = string.Join(",", lsyLine);
                //location = "LINESTRING  (" + lineString + ")";
                location = $"LINESTRING  (\"  { lineString}  \")";


            }
            else if (other.shapeid == 3)// Vùng
            {
                List<string> lsyLine = new List<string>();
                string lineString = string.Empty;
                for (int i = 0; i < other.coords.Count; i++)
                {
                    double[] arrayCoords = new double[] { other.coords[i].Longitude, other.coords[i].Latitude };
                    string loc = string.Join(" ", arrayCoords);
                    lsyLine.Add(loc);
                }

                lineString = string.Join(",", lsyLine);
                //location = "POLYGON  ((" + lineString + "))";
                location = $"POLYGON  ((\"  {lineString}  \"))";
            }
        }
    }
}
