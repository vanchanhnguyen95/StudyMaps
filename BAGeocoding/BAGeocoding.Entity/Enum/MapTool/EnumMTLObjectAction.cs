using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.MapTool
{
    public enum EnumMTLObjectAction : byte
    {
        [EnumItem("Khởi tạo dữ liệu", FieldName = "Generate")]
        Generate = 0,

        [EnumItem("Thao tác tạo mới", FieldName = "NewByAction")]
        NewByAction = 1,

        [EnumItem("Tạo mới do cắt", FieldName = "NewByCut")]
        NewByCut = 2,

        [EnumItem("Tạo mới do ghép", FieldName = "NewByJoin")]
        NewByJoin = 3,


        [EnumItem("Thao tác xóa", FieldName = "DeleteByAction")]
        DeleteByAction = 9,

        [EnumItem("Xóa do cắt", FieldName = "DeleteByCut")]
        DeleteByCut = 10,

        [EnumItem("Xóa do ghép", FieldName = "DeleteByJoin")]
        DeleteByJoin = 11,


        [EnumItem("Cập nhật t.tin", FieldName = "EditObject")]
        EditObject = 17,

        [EnumItem("Nhập số nhà", FieldName = "EnterNumber")]
        EnterNumber = 18,
    }
}