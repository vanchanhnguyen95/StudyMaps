using Nest;
using System.ComponentModel;

namespace Elastic02.Models.Test
{
    public class HaNoiRoadPush
    {
        [Number(Index = true)]
        public float id { get; set; } = 0;

        [Text(Index = true, Fielddata = true, Analyzer = "vi_analyzer_road")]
        public string? name { get; set; } = string.Empty;

        [Text(Index = true, Fielddata = true, Analyzer = "vi_analyzer_road")]
        public string? extend { get; set; } = string.Empty;

        [Number(Index = true)]
        public double lng { get; set; } = 0;

        [Number(Index = true)]
        public double lat { get; set; } = 0;

        public HaNoiRoadPush() { }
        public HaNoiRoadPush(HaNoiRoadPoint other) {
            id = other.id;
            name = other.name;
            lat = other.lat;
            lng = other.lng;
            extend= other.extend;
        }

    }

    /// <summary>
    /// IdProperty for elasticsearchType will override default property generation by elastic search and will use 
    /// assigned property as id for document level. 
    /// Description holds the index name
    /// NOTE: Id value should be unique and index name should be in Lower Case
    /// </summary>
    [ElasticsearchType(IdProperty = nameof(Id)), Description("hanoiroad_point")]
    public class HaNoiRoadPoint : HaNoiRoadPush
    {
        [GeoPoint]
        public string location { get; set; } = string.Empty;

        //public string? ename { get; set; } = string.Empty;
        [Text(Index = true, Fielddata = true, Analyzer = "vi_analyzer_road")]
        public string keywords { get; set; } = string.Empty;

        public HaNoiRoadPoint(HaNoiRoadPush other)
        {
            id = other.id;
            name = other.name;
            extend = other.extend;
            lat= other.lat;
            lng = other.lng;
            //location = new GeoLocation(other.lat, other.lng);
            location = other.lat.ToString() + ", " + other.lng.ToString();
            if (!string.IsNullOrEmpty(other.extend))
            {
                //keyword = $"{other.name} , {other.extend}";
                keywords = other?.name?.ToString() + ", " + other?.extend?.ToString();
            }
            else
            {
                keywords = other.name??"";
            }
        }
    }

}
