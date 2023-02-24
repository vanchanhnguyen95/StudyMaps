using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticProject.Data.Entity.MapObj
{
    public class BGCRoadName
    {
        public int RoadID { get; set; }
        public byte ProvinceID { get; set; }
        public string RoadName { get; set; }
        public string NameExt { get; set; }
        public string Address { get; set; }
        public BGCLngLat Coord { get; set; }

        public BGCRoadName()
        {
            Coord = new BGCLngLat();
        }

        public BGCRoadName(BGCRoadName other)
        {
            RoadID = other.RoadID;
            ProvinceID = other.ProvinceID;
            RoadName = other.RoadName;
            NameExt = other.NameExt;
            Address = other.Address;
            Coord = new BGCLngLat(other.Coord);
        }

    }
}
