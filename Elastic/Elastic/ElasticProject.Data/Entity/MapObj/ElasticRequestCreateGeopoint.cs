using Nest;

namespace ElasticProject.Data.Entity.MapObj
{
    public class ElasticRequestPushGeopoint
    {
        public byte typeid { get; set; }
        public int indexid { get; set; }
        public byte shapeid { get; set; }
        [Text]
        public string kindname { get; set; }
        [Text]
        public string name { get; set; }
        [Text]
        public string address { get; set; }
        [Text]
        public string shortkey { get; set; }
        public byte provinceid { get; set; }
        public byte priority { get; set; }

        public List<ElasticPointBase> coords { get; set; }

        public ElasticRequestPushGeopoint()
        {
            coords = new List<ElasticPointBase>();
        }
    }

    public class ElasticRequestCreateGeopoint : ElasticRequestPushGeopoint
    {
        //public GeoLocation location { get; set; }
        [GeoPoint]
        public string location { get; set; }


        public ElasticRequestCreateGeopoint(ElasticRequestPushGeopoint other)
        {
            typeid = other.typeid;
            indexid = other.indexid;
            shapeid = other.shapeid;
            kindname = other.kindname;
            name = other.name;
            address = other.address;
            shortkey = other.shortkey;
            provinceid = other.provinceid;
            priority = other.priority;
            coords = other.coords;
            //location = string.Join(",", coords); "\"" 
            location = coords[0].lat.ToString() + "," + coords[0].lng.ToString();
        }
    }
}
