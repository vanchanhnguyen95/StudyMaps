using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.MapTool
{
    public enum EnumMTLApprovedState : byte
    {
        [EnumItem("Khởi tạo", FieldName = "Generate")]
        Generate = 0,

        [EnumItem("Cần t.tin", FieldName = "NeedInfo")]
        NeedInfo = 1,

        [EnumItem("Trả lại", FieldName = "Giveback")]
        Giveback = 2,


        [EnumItem("Hủy", FieldName = "Abort")]
        Abort = 7,

        [EnumItem("Duyệt", FieldName = "Approved")]
        Approved = 8
    }
}