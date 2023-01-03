using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace WebApiGeocoding.Models
{
    public class HaNoiPoint
    {
        [BsonId]
        public ObjectId Id { get; set; }
        //public List<ObjectId> AddressIds { get; set; }
        public string Name { get; set; } // Tên địa chỉ đầy đủ
        public string Street { get; set; } // Tên đường (số nhà, địa danh)
        public string Ward { get; set; }// Tên phường
        public string District { get; set; }// Tên quận
        public string Province { get; set; }// Tên tỉnh, thành phố trực thuộc trung ương
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Speed { get; set; }//tốc độ cho phép
        public GeoJsonPoint<GeoJson2DGeographicCoordinates> Location { get; private set; }
        public void SetLocation(double lon, double lat) => _SetLocation(lon, lat);

        private void _SetLocation(double lon, double lat)
        {
            Latitude = lat;
            Longitude = lon;
            Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(
                new GeoJson2DGeographicCoordinates(lon, lat));
        }
    }
}
