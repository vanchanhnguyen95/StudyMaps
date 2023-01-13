using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Router
{
    public class BAREpsilon
    {
        public int Meter { get; set; }
        public double LngLat { get; set; }

        public BAREpsilon(int meter) 
        {
            Meter = meter;
            LngLat = DataUtl.ConvertMeterToLngLat(meter);
        }
    }
}