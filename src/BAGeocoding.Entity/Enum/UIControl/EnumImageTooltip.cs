using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.UIControl
{
    public enum EnumImageTooltip
    {
        [EnumItem("Hỏi", FieldName = "Question")]
        Question = 0,

        [EnumItem("Thông tin", FieldName = "Info")]
        Info = 1,

        [EnumItem("Cảnh báo", FieldName = "Warnning")]
        Warnning = 2,

        [EnumItem("Lỗi", FieldName = "Error")]
        Error = 3,
    }
}