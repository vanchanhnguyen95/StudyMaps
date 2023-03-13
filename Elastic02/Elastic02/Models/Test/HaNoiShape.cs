using Nest;
using System.ComponentModel;

namespace Elastic02.Models.Test
{
    public class PointBase
    {
        public double lng { get; set; }
        public double lat { get; set; }

        public PointBase() { }

        public PointBase(PointBase other)
        {
            lng = other.lng;
            lat = other.lat;
        }

        public PointBase(LngLat other)
        {
            lng = other.Lng;
            lat = other.Lat;
        }
    }

    public class HaNoiShapePush
    {
        [Number(Index = true)]
        public float? id { get; set; } = 0;

        [Text(Index = true, Fielddata = true, Analyzer = "vi_analyzer_road")]
        public string? name { get; set; } = string.Empty;

        [Text(Index = true, Fielddata = true, Analyzer = "vi_analyzer_road")]
        public string? extend { get; set; } = string.Empty;

        [Number(Index = true)]
        public double? lng { get; set; } = 0;

        [Number(Index = true)]
        public double? lat { get; set; } = 0;
        [Number(Index = true)]
        public short? shapeid { get; set; } = 1;

        public List<PointBase>? coords { get; set; }

        public HaNoiShapePush() {
            coords = new List<PointBase>();
        }
        public HaNoiShapePush(HaNoiShapePush other)
        {
            id = other.id;
            name = other.name;
            lat = other.lat;
            lng = other.lng;
            extend = other.extend;
            shapeid = other.shapeid;
            coords = other.coords;
        }

    }

    [ElasticsearchType(IdProperty = nameof(Id)), Description("hanoi_shape")]
    public class HaNoiShape : HaNoiShapePush
    {
        [GeoShape]
        public string? location { get; set; } = string.Empty;

        //public string? ename { get; set; } = string.Empty;
        [Text(Index = true, Fielddata = true, Analyzer = "vi_analyzer_road")]
        public string keywords { get; set; } = string.Empty;

        public HaNoiShape(HaNoiShapePush other)
        {
            id = other.id;
            name = other.name;
            lat = other.lat;
            lng = other.lng;
            extend = other.extend;
            coords = other.coords;
            shapeid = other.shapeid;
            //coords.Add(new GeoLocation(other.lat, other.lng));

            if (!string.IsNullOrEmpty(other.extend))
            {
                //keyword = $"{other.name} , {other.extend}";
                keywords = other?.name?.ToString() + ", " + other?.extend?.ToString();
            }
            else
            {
                keywords = other.name ?? "";
            }

            if (other?.shapeid == 1)//Điểm
            {
                //double[] arrayCoords = new double[] { other?.coords[0]?.Longitude ?? (other?.lng ?? 0), other?.coords[0]?.Latitude ?? (other?.lat ?? 0) };
                double[] arrayCoords = new double[] { (other?.lng ?? 0), (other?.lat ?? 0) };
                string loc = string.Join(" ", arrayCoords);

                //location = "POINT (" + loc + ")";
                location = $"POINT ( { loc } )";
            }
            else if (other?.shapeid == 2)//Đường
            {
                lat = other?.coords?[0]?.lat ?? 0;
                lng = other?.coords?[0]?.lng ?? 0;

                List<string> lsyLine = new List<string>();
                string lineString = string.Empty;
                for (int i = 0; i < other?.coords?.Count; i++)
                {
                    double[] arrayCoords = new double[] { other.coords[i].lng, other.coords[i].lat };
                    string loc = string.Join(" ", arrayCoords);
                    lsyLine.Add(loc);
                }

                lineString = string.Join(",", lsyLine);
                //location = "LINESTRING  (" + lineString + ")";
                location = $"LINESTRING  (  { lineString }  )";
            }
            else if (other?.shapeid == 3)// Vùng
            {

                lat = other?.coords?[0]?.lat;
                lng = other?.coords?[0]?.lng;

                List<string> lsyLine = new List<string>();
                string lineString = string.Empty;
                for (int i = 0; i < other?.coords?.Count; i++)
                {
                    double[] arrayCoords = new double[] { other.coords[i].lng, other.coords[i].lat };
                    string loc = string.Join(" ", arrayCoords);
                    lsyLine.Add(loc);
                }

                lineString = string.Join(",", lsyLine);
                location = "POLYGON  ((" + lineString + "))";
                //location = $"POLYGON  ((  { lineString }  ))";
            }
        }
    }
}
