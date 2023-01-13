using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.ActionData
{
    public enum EnumWPLImportData : byte
    {
        [EnumItem("Import thành công", FieldName = "Success")]
        Success = 0,


        [EnumItem("Chưa gán grid cho kế hoạch", FieldName = "GridMissing")]
        GridMissing = 1,

        [EnumItem("Lỗi xử lý grid của kế hoạch", FieldName = "GridError")]
        GridError = 2,


        [EnumItem("Không có dữ liệu tên đối tượng", FieldName = "NameMissing")]
        NameMissing = 5,

        [EnumItem("Lỗi xử lý đọc tên đối tượng", FieldName = "NameError")]
        NameError = 6,


        [EnumItem("Lỗi đọc dữ liệu từ file bản đồ", FieldName = "ObjectError")]
        ObjectError = 9
    }
}