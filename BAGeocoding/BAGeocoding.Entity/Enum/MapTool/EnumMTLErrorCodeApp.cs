using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.MapTool
{
    public enum EnumMTLErrorCodeApp : byte
    {
        [EnumItem("Xử lý thành công không có lỗi", FieldName = "Ok")]
        Ok = 0,

        [EnumItem("Lỗi xử lý dữ liệu", FieldName = "Ok")]
        Error = 1,


        [EnumItem("Lỗi dữ liệu đầu vào", FieldName = "DataInvalid")]
        DataInvalid = 252,

        [EnumItem("Không có quyền truy xuất grid", FieldName = "GridDenied")]
        GridDenied = 253,

        [EnumItem("Không có quyền truy xuất kế hoạch", FieldName = "PlanDenied")]
        PlanDenied = 254
    }
}