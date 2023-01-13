namespace BAGeocoding.Entity.RestfulApi.MapData
{
    public class BARFALatLng
    {
        public double lat { get; set; }
        public double lng { get; set; }

        public bool IsValid()
        {
            if (lat > 90)
                return false;
            else if (lat < -90)
                return false;
            else if (lng > 180)
                return false;
            else if (lng < -180)
                return false;
            else
                return 
                    true;
        }
    }
}