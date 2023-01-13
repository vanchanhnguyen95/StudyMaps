using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.MapTool
{
    public enum EnumMTLGridSyncState : byte
    {
        [EnumItem("Không có dữ liệu", FieldName = "Blank")]
        Blank = 0,

        [EnumItem("Đồng bộ bình thường", FieldName = "Normal")]
        Normal = 1,

        [EnumItem("Không được gán quyền", FieldName = "Denied")]
        Denied = 2
    }
}