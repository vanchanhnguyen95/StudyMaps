using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.MapTool
{
    public enum EnumMTLRoadDirection : byte
    {
        [EnumItem("Hai chiều", FieldName = "Bidirectional")]
        Bidirectional = 0,

        [EnumItem("Cùng hướng", FieldName = "Forward")]
        Forward = 1,

        [EnumItem("Ngược hướng", FieldName = "Backward")]
        Backward = 2
    }
}