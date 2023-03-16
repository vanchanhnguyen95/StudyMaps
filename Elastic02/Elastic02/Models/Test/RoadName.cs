using Nest;
using System.ComponentModel;

namespace Elastic02.Models.Test
{
    public class RoadNamePush
    {
        [Number(Index = true)]
        public int RoadID { get; set; } = 0;

        [Number(Index = true)]
        public int ProvinceID { get; set; } = 0;

        [Text(Index = true, Fielddata = true, Analyzer = "vi_analyzer_road")]
        public string? RoadName { get; set; } = string.Empty;

        [Text(Index = true, Fielddata = true, Analyzer = "vi_analyzer_road")]
        public string? NameExt { get; set; } = string.Empty;

        [Number(Index = true)]
        public decimal Lng { get; set; } = 0;

        [Number(Index = true)]
        public decimal Lat { get; set; } = 0;
    }

    [ElasticsearchType(IdProperty = nameof(Id)), Description("roadname")]
    public class RoadName : RoadNamePush
    {
        [GeoPoint]
        public string Location { get; set; } = string.Empty;

        [Text(Index = true, Fielddata = true, Analyzer = "vi_analyzer_road")]
        public string Keywords { get; set; } = string.Empty;
        public RoadName(RoadNamePush other)
        {
            RoadID = other.RoadID;
            ProvinceID = other.ProvinceID;
            RoadName = other.RoadName;
            NameExt = other.NameExt;
            Lng = other.Lng;
            Lat = other.Lat;

            Location = other.Lat.ToString() + ", " + other.Lng.ToString();
            if (!string.IsNullOrEmpty(other.NameExt))
            {
                Keywords = other?.RoadName?.ToString() + " " + other?.NameExt?.ToString();
            }
            else
            {
                Keywords = other.RoadName ?? "";
            }
        }

    }
}
