using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticProject.Data.Entity.MapObj
{
    public class BGCElasticPointBase
    {
        public double lng { get; set; }
        public double lat { get; set; }

        public BGCElasticPointBase() { }

        public BGCElasticPointBase(BGCElasticPointBase other)
        {
            lng = other.lng;
            lat = other.lat;
        }

        public BGCElasticPointBase(BGCLngLat other)
        {
            lng = other.Lng;
            lat = other.Lat;
        }
    }
}
