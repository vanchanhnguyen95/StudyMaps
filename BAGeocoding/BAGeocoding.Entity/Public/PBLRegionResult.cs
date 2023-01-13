namespace BAGeocoding.Entity.Public
{
    public class PBLRegionResult
    {
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int CommuneID { get; set; }

        public PBLRegionResult() { }

        public PBLRegionResult(RPBLAddressResult other)
        {
            ProvinceID = other.ProvinceID;
            DistrictID = other.DistrictID;
        }

        public PBLRegionResult(FPBLAddressResult other)
        {
            ProvinceID = other.ProvinceID;
            DistrictID = other.DistrictID;
        }
    }
}