using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum
{
    public enum EnumBAGPlaceType
    {
        [EnumItem("Khu đô thị", FieldName = "Urban")]
        Urban = 1,

        [EnumItem("Lô đất", FieldName = "Portion")]
        Portion = 2,

        [EnumItem("Ô đất", FieldName = "Plot")]
        Plot = 3
    }
}