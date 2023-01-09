using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum
{
    public enum EnumMapObjectShape
    {
        [EnumItem("Điểm", FieldName = "Point")]
        Point = 1,

        [EnumItem("Đường", FieldName = "Polyline")]
        Polyline = 2,

        [EnumItem("Vùng", FieldName = "Polygon")]
        Polygon = 3
    }
}
