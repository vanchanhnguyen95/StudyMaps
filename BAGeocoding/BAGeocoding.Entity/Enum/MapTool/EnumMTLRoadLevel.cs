using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.MapTool
{
    public enum EnumMTLRoadLevel : short
    {
        [EnumItem("Đường hầm", FieldName = "Subway")]
        Subway = -1,

        [EnumItem("Bình thường", FieldName = "Normal")]
        Normal = 0,

        [EnumItem("Trên cao", FieldName = "Above")]
        Above = 1
    }
}
