using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum
{
    public enum EnumMapObjectType : byte
    {
        [EnumItem("Đường phố", FieldName = "RoadSegment")]
        RoadSegment = 32,

        [EnumItem("Địa điểm", FieldName = "PointOfInterest")]
        PointOfInterest = 33
    }
}
