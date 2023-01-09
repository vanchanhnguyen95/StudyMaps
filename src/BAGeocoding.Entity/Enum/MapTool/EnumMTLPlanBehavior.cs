using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.MapTool
{
    public enum EnumMTLPlanBehavior : byte
    {
        [EnumItem("1.Khởi tạo", FieldName = "Generate")]
        Generate = 0,

        [EnumItem("2.Nhập liệu", FieldName = "DataInput")]
        DataInput = 1,


        [EnumItem("3.Triển khai", FieldName = "Deployment")]
        Deployment = 8,

        [EnumItem("4.Tạm dừng", FieldName = "Pause")]
        Pause = 9,


        [EnumItem("5.Biên tập", FieldName = "EditMode")]
        EditMode = 16,



        [EnumItem("8.Hủy bỏ", FieldName = "Abort")]
        Abort = 254,

        [EnumItem("9.Hoàn thành", FieldName = "Approved")]
        Finished = 255
    }
}
