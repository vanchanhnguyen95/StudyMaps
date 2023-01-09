using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.RolePermission
{
    public enum EnumPermissionPage : byte
    {
        [EnumItem("1.Tổng quan", FieldName = "General")]
        General = 1,

        [EnumItem("2.Kiểm tra", FieldName = "Check")]
        Check = 2,

        [EnumItem("3.MapTool", FieldName = "MapTool")]
        MapTool = 3
    }
}