namespace ElasticProject.Data.Entity.MapObj
{
    public class ElasticPointBase
    {
        public double lng { get; set; }
        public double lat { get; set; }

        public ElasticPointBase() { }

        public ElasticPointBase(ElasticPointBase other)
        {
            lng = other.lng;
            lat = other.lat;
        }

        public ElasticPointBase(LngLat other)
        {
            lng = other.Lng;
            lat = other.Lat;
        }

        //public static implicit operator List<object>(ElasticPointBase v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
