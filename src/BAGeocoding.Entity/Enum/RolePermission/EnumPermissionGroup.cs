using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.RolePermission
{
    public enum EnumPermissionGroup : short
    {
        [EnumItem("1.Dữ liệu", FieldName = "GNRData")]
        GNRData = 1,

        [EnumItem("2.Import", FieldName = "GNRImport")]
        GNRImport = 2,

        [EnumItem("3.Convert", FieldName = "GNRConvert")]
        GNRConvert = 3,

        [EnumItem("4.Ghi dữ liệu", FieldName = "GNRWrite")]
        GNRWrite = 4,



        [EnumItem("1.Dữ liệu", FieldName = "TESTData")]
        TESTData = 257,

        [EnumItem("2.Dịch vụ", FieldName = "TESTService")]
        TESTService = 258,



        [EnumItem("1.Dữ liệu", FieldName = "TESTData")]
        MTLData = 513,
    }
}