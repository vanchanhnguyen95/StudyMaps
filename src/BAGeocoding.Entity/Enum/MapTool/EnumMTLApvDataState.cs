using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.MapTool
{
    public enum EnumMTLApvDataState : byte
    {
        [EnumItem("Không có đối tượng", FieldName = "Missing")]
        Missing = 1,

        [EnumItem("Bị trùng trạng thái", FieldName = "Processed")]
        Processed = 2,

        [EnumItem("Đã duyệt trước rồi", FieldName = "Apporved")]
        Apporved = 3,

        [EnumItem("Kế hoạch đã đóng", FieldName = "PlanWrong")]
        PlanWrong = 4,

        [EnumItem("Grid không đc duyệt", FieldName = "GridDenined")]
        GridDenined = 5,


        [EnumItem("Không đúng trạng thái", FieldName = "DataInvalid")]
        DataInvalid = 253,

        [EnumItem("Lỗi truy xuất CSDL", FieldName = "DBError")]
        DBError = 254,

        [EnumItem("Xử lý thành công", FieldName = "Success")]
        Success = 255
    }
}