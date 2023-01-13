
using BAGeocoding.Entity.MapObj;
namespace BAGeocoding.Entity.RestfulApi.ElasticSearch
{
    public class BAGElasticPointBase
    {
        public double lng { get; set; }
        public double lat { get; set; }

        public BAGElasticPointBase() { }

        public BAGElasticPointBase(BAGElasticPointBase other)
        {
            lng = other.lng;
            lat = other.lat;
        }

        public BAGElasticPointBase(BAGPoint other)
        {
            lng = other.Lng;
            lat = other.Lat;
        }
    }
}
