using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum
{
    public enum EnumRestfulApiMethod : int
    {
        [EnumItem("Dữ liệu trống", FieldName = "Blank")]
        Blank = 0,

        [EnumItem("Lấy địa chỉ IP", FieldName = "IPAddressGet")]
        IPAddressGet = 1,


        [EnumItem("Lấy vùng theo tọa độ", FieldName = "RegionGet")]
        RegionGet = 512,
    }
}
