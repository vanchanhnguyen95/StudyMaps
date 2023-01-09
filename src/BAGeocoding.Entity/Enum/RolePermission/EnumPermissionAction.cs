using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.RolePermission
{
    public enum EnumPermissionAction : byte
    {
        [EnumItem("Danh sách", FieldName = "View")]
        View = 0,

        [EnumItem("Thêm mới", FieldName = "Add")]
        Add = 1,

        [EnumItem("Cập nhật", FieldName = "Edit")]
        Edit = 2,

        [EnumItem("Xóa", FieldName = "Delete")]
        Delete = 3,

        [EnumItem("Export", FieldName = "Export")]
        Export = 4,

        [EnumItem("Logs", FieldName = "Logs")]
        Logs = 5
    }
}