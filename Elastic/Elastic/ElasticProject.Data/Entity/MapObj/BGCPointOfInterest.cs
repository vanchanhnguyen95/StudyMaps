using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticProject.Data.Entity.MapObj
{
    public class BGCPointOfInterest
    {
        public int PoiID { get; set; }
        public byte ProvinceID { get; set; }
        public BGCKindInfo KindInfo { get; set; }
        public string Name { get; set; }
        public short House { get; set; }
        public string Road { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Anchor { get; set; }
        public string Info { get; set; }
        public string Node { get; set; }
        public string ShortKey { get; set; }
        public BGCLngLat Coord { get; set; }

        public BGCPointOfInterest()
        {
            KindInfo = new BGCKindInfo();
            Coord = new BGCLngLat();
        }

        public BGCPointOfInterest(BGCPointOfInterest other)
        {
            PoiID = other.PoiID;
            ProvinceID = other.ProvinceID;
            KindInfo = new BGCKindInfo(other.KindInfo);
            Name = other.Name;
            House = other.House;
            Road = other.Road;
            Address = other.Address;
            Tel = other.Tel;
            Anchor = other.Anchor;
            Info = other.Info;
            Node = other.Node;
            ShortKey = other.ShortKey;
            Coord = new BGCLngLat(other.Coord);
        }

    }
}
