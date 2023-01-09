using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.Route
{
    public enum EnumBARErrorCode : byte
    {
        [EnumItem("Lỗi tài khoản xác thực", FieldName = "Auth")]
        Auth = 1,

        [EnumItem("Lỗi mã key xác thực", FieldName = "KeyStr")]
        KeyStr = 2,


        [EnumItem("Lỗi dữ liệu đầu vào", FieldName = "DataInput")]
        DataInput = 9,

        [EnumItem("Không xác định được điểm đầu", FieldName = "StartNode")]
        StartNode = 10,

        [EnumItem("Không xác định được điểm cuối", FieldName = "EndNode")]
        EndNode = 11,


        [EnumItem("Không tìm thấy kết quả", FieldName = "RouteNone")]
        RouteError = 33,

        [EnumItem("Không tìm thấy kết quả", FieldName = "RouteNone")]
        RouteNone = 34
    }
}