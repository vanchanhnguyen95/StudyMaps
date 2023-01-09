using System.Collections.Generic;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapObj;

namespace BAGeocoding.Entity.RestfulApi.ElasticSearch
{
    public class BAGElasticRequestCreate
    {
        public string data { get; set; }
    }

    public class BAGElasticRequestItem
    {
        public byte typeid { get; set; }
        public int indexid { get; set; }
        public byte shapeid { get; set; }
        public string kindname { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string shortkey { get; set; }
        public byte provinceid { get; set; }
        public byte priority { get; set; }
        public List<BAGElasticPointBase> coords { get; set; }

        public BAGElasticRequestItem()
        {
            coords = new List<BAGElasticPointBase>();
        }

        public BAGElasticRequestItem(BAGRoadName other)
        {
            typeid = (byte)EnumMapObjectType.RoadSegment;
            indexid = other.RoadID;
            shapeid = (byte)EnumMapObjectShape.Polyline;
            kindname = "Đường";
            if (other.NameExt.Length == 0)
            {
                name = other.RoadName;
                address = string.Format("{0}, {1}", other.RoadName, other.Address);
            }
            else
            {
                name = string.Format("{0}, {1}", other.RoadName, other.NameExt);
                address = string.Format("{0}, {1}, {2}", other.RoadName, other.NameExt, other.Address);
            }
            shortkey = string.Empty;
            provinceid = other.ProvinceID;
            priority = (byte)EnumElasticSearchPriority.Normal;
            coords = new List<BAGElasticPointBase>();
            coords.Add(new BAGElasticPointBase(other.Coords));
        }

        public BAGElasticRequestItem(BAGPointOfInterest other)
        {
            typeid = (byte)EnumMapObjectType.PointOfInterest;
            indexid = other.PoiID;
            shapeid = (byte)EnumMapObjectShape.Point;
            kindname = other.KindName;
            name = string.Format("{0} {1}", other.KindName, other.Name);
            address = other.Info;
            shortkey = other.ShortKey;
            provinceid = other.ProvinceID;
            priority = (byte)EnumElasticSearchPriority.Normal;
            coords = new List<BAGElasticPointBase>();
            coords.Add(new BAGElasticPointBase(other.Coords));
        }
    }
}