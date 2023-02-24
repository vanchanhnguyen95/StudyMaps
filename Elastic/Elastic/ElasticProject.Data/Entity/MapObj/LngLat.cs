using Newtonsoft.Json;

namespace ElasticProject.Data.Entity.MapObj
{
    public class LngLat
    {
        [JsonProperty("lng")]
        public double Lng { get; set; }
        [JsonProperty("lat")]
        public double Lat { get; set; }

        public LngLat() { }

        public LngLat(LngLat other)
        {
            Lng = other.Lng;
            Lat = other.Lat;
        }

        public LngLat(double lng, double lat)
        {
            Lng = Math.Round(lng, 8);//Constants.ROUND_DOUBLE_DIGIT
            Lat = Math.Round(lat, 8);
        }

        public LngLat(object lng, object lat, bool rev = false)
        {
            if (rev == true)
            {
                Lat = Math.Round(Convert.ToDouble(lng), 8);
                Lng = Math.Round(Convert.ToDouble(lat), 8);
            }
            else
            {
                Lng = Math.Round(Convert.ToDouble(lng), 8);
                Lat = Math.Round(Convert.ToDouble(lat), 8);
            }
        }
    }
}
