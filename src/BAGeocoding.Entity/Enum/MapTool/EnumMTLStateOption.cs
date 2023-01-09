using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.MapTool
{
    public enum EnumMTLStateOption : byte
    {
        [EnumItem("Trạng thái hiển thị", FieldName = "IsVisible")]
        IsVisible = 0,

        [EnumItem("Trạng thái xử lý", FieldName = "IsProcess")]
        IsProcess = 1,

        [EnumItem("Đối tượng được mới", FieldName = "IsCreatNew")]
        IsCreatNew = 2
    }
}