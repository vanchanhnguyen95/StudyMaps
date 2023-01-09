using System;

namespace BAGeocoding.Utility
{
    /// <summary>
    /// Thông tin Enum
    /// </summary>    
    public enum EnumCatalogType : byte
    {
        [EnumItem("Danh sách lưới", FieldName = "Grid")]
        Grid = 1,

        [EnumItem("Danh sách nhóm điểm", FieldName = "PGroup")]
        PGroup = 2,

        [EnumItem("Danh sách loại điểm", FieldName = "PKind")]
        PKind = 3,

        [EnumItem("Danh sách tài khoản", FieldName = "User")]
        User = 4,

        [EnumItem("Thiết bị iPad", FieldName = "IPad")]
        IPad = 5,

        [EnumItem("Thuộc tính bảng", FieldName = "Field")]
        Field = 6,
    }
}