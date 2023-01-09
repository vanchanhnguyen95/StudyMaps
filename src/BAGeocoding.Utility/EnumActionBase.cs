using System;

namespace BAGeocoding.Utility
{
    /// <summary>
    /// Thông tin Enum
    /// </summary>    
    public enum EnumActionBase : byte
    {
        [EnumItem("Khởi tạo", FieldName = "Generate")]
        Generate = 0,

        [EnumItem("Thêm mới", FieldName = "Create")]
        Create = 1,

        [EnumItem("Cập nhật", FieldName = "Edit")]
        Edit = 2,


        [EnumItem("Hủy", FieldName = "Delete")]
        Delete = 4,


        [EnumItem("Duyệt", FieldName = "Approved")]
        Approved = 8,
    }
}
