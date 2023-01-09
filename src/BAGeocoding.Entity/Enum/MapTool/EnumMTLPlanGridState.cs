using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.MapTool
{
    public enum EnumMTLPlanGridState : byte
    {
        [EnumItem("Khởi tạo", FieldName = "Generate")]
        Generate = 0,

        [EnumItem("Đi thực địa", FieldName = "Processing")]
        Processing = 1,

        [EnumItem("Gửi chờ duyệt", FieldName = "Pending")]
        Pending = 2,

        [EnumItem("Tiến hành duyệt", FieldName = "Doing")]
        Doing = 3,


        [EnumItem("Trả làm lại", FieldName = "Giveback")]
        Giveback = 7,

        [EnumItem("Duyệt dữ liệu", FieldName = "Approved")]
        Approved = 8
    }
}