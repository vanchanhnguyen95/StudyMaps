﻿using Nest;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Elastic02.Models.Test
{
    public class HaNoiRoadPush
    {
        [Number(Index = true)]
        public float id { get; set; }

        [Text(Index = true, Fielddata = true, Analyzer = "my_vi_analyzer")]
        public string? name { get; set; } = string.Empty;
        [Text(Index = true, Fielddata = true, Analyzer = "my_vi_analyzer")]
        public string? extend { get; set; } = string.Empty;

        [Number(Index = true)]
        public double lng { get; set; }
        [Number(Index = true)]

        public double lat { get; set; }

        public HaNoiRoadPush() { }
        public HaNoiRoadPush(HaNoiRoadPoint other) {
            id = other.id;
            name = other.name;
            lat = other.lat;
            lng = other.lng;
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
        //[GeoPoint]
        public GeoLocation location { get; set; }

        public HaNoiRoadPoint(HaNoiRoadPush other)
        {
            id = other.id;
            name = other.name;
            lat= other.lat;
            lng = other.lng;
            location = new GeoLocation(other.lat, other.lng);
        }
    }

}
