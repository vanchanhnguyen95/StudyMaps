using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.MapTool
{
    public enum EnumMTLRoadKind : byte
    {
        [EnumItem("Bình thường", FieldName = "Normal")]
        Normal = 0,

        [EnumItem("Cắt ngang", FieldName = "Join")]
        Join = 1,

        [EnumItem("Chuyển tuyến", FieldName = "Switch")]
        Switch = 2
    }
}
