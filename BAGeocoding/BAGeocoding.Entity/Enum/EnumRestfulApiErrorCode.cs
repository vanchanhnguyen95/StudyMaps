using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum
{
    public enum EnumRestfulApiErrorCode
    {
        [EnumItem("Thao tác thành công", FieldName = "None")]
        None = 0,

        [EnumItem("Lỗi cấu trúc dữ liệu đầu vào", FieldName = "InvalidData")]
        InvalidData = 1,


        [EnumItem("Dịch vụ đang tạm ngưng", FieldName = "ServiceStop")]
        ServiceStop = 9,


        [EnumItem("Sai thông tin key đăng ký", FieldName = "InvalidKey")]
        InvalidKey = 17,

        [EnumItem("Địa chỉ IP không có quyền", FieldName = "InvalidIp")]
        InvalidIp = 18,

        [EnumItem("Không có quyền trên phương thức", FieldName = "InvalidMethod")]
        InvalidMethod = 19,


        [EnumItem("Lỗi xử lý dữ liệu", FieldName = "ProcessData")]
        ProcessData = 128,


        [EnumItem("Lỗi không xác định", FieldName = "Unknown")]
        Unknown = 255
    }
}
