using Elastic02.Utility;
using Nest;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System.ComponentModel;

namespace Elastic02.Models.Test
{
    public class RoadNamePush
    {
        [Number(Index = true)]
        public int RoadID { get; set; } = 0;

        [Number(Index = true)]
        public int ProvinceID { get; set; } = 0;

        [Text(Index = true, Fielddata = true)]
        public string? RoadName { get; set; } = string.Empty;

        [Text(Index = true, Fielddata = true)]
        public string? NameExt { get; set; } = string.Empty;

        [Text(Index = true, Fielddata = true)]
        public string? Address { get; set; } = string.Empty;

        [Number(Index = true)]
        public decimal Lng { get; set; } = 0;

        [Number(Index = true)]
        public decimal Lat { get; set; } = 0;
    }

    [ElasticsearchType(IdProperty = nameof(Id)), Description("roadname-location")]
    public class RoadName : RoadNamePush
    {
        [GeoPoint]
        public GeoLocation Location { get; set; }

        [Text(Index = true, Fielddata = true)]
        public string? AddressLower { get; set; } = string.Empty;

        [Text(Index = true, Fielddata = true)]
        public string Keywords { get; set; } = string.Empty;

        public string? KeywordsAscii { get; set; } = string.Empty;
        public RoadName(RoadNamePush other)
        {
            RoadID = other.RoadID;
            ProvinceID = other.ProvinceID;
            RoadName = other.RoadName;
            NameExt = other.NameExt;
            Address = other.Address;
            AddressLower = other?.Address?.ToLower();
            Lng = other?.Lng ?? 0;
            Lat = other?.Lat ?? 0;

            //Location = other.Lat.ToString() + ", " + other.Lng.ToString();
            Location = new GeoLocation((double)Lat, (double)Lng);
            if (!string.IsNullOrEmpty(other?.NameExt))
            {
                Keywords = other?.RoadName?.ToString().ToLower() + " , " + other?.NameExt?.ToString().ToLower();
            }
            else
            {
                Keywords = other?.RoadName?.ToString().ToLower() ?? "";
                //Keywords = other.RoadName + " , ";
            }

            if (!string.IsNullOrEmpty(other?.Address))
            {
                Keywords += " , " + other?.Address?.ToString().ToLower();
            }else
            {
                Keywords += " , ";
            }
            //else
            //{
            //    Keywords += other?.Address ?? "";
            //}

            KeywordsAscii = LatinToAscii.Latin2Ascii(Keywords);
        }

    }
}
