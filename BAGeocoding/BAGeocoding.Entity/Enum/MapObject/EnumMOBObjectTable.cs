using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.MapObject
{
    public enum EnumMOBObjectTable : short
    {
        [EnumItem("Bảng vùng", FieldName = "Region")]
        Region = 1,


        [EnumItem("Bảng khu đô thị", FieldName = "Urban")]
        Urban = 17,

        [EnumItem("Bảng lô đất", FieldName = "Portion")]
        Portion = 18,

        [EnumItem("Ô đất, biệt thự", FieldName = "Plot")]
        Plot = 19,


        [EnumItem("Sông polygon", FieldName = "RiverPolygon")]
        RiverPolygon = 33,

        [EnumItem("Sông polyline", FieldName = "RiverPolyline")]
        RiverPolyline = 34,


        [EnumItem("Bảng đường", FieldName = "Road")]
        Road = 49,

        [EnumItem("Đường sắt", FieldName = "Railway")]
        Railway = 50,


        [EnumItem("Bảng POI", FieldName = "POI")]
        POI = 65,
    }
}