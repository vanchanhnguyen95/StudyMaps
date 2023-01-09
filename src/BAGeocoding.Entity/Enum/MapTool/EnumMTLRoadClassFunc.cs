using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.MapTool
{
    public enum EnumMTLRoadClassFunc : byte
    {
        [EnumItem("Quốc lộ", FieldName = "National")]
        National = 1,

        [EnumItem("Tỉnh lộ", FieldName = "Provincial")]
        Provincial = 2,

        [EnumItem("Có tên", FieldName = "MainRoad")]
        MainRoad = 3,

        [EnumItem("Đường nhỏ", FieldName = "RubRoad")]
        RubRoad = 4
    }
}