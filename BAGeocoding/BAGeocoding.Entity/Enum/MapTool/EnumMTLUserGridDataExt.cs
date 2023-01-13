using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.MapTool
{
    public enum EnumMTLUserGridDataExt : byte
    {
        [EnumItem("Quyền chỉnh sửa dữ liệu", FieldName = "Edit")]
        Edit = 1,

        [EnumItem("Quyền biên tập điểm", FieldName = "ComposePoi")]
        ComposePoi = 2,

        [EnumItem("Quyền biên tập đường", FieldName = "ComposeSeg")]
        ComposeSeg = 3
    }
}