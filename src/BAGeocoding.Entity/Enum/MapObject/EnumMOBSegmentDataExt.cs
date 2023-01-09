using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.MapObject
{
    public enum EnumMOBSegmentDataExt : byte
    {
        [EnumItem("Số nhà liên tiếp", FieldName = "BuildingSerial")]
        BuildingSerial = 0,

        [EnumItem("Đường cao tốc", FieldName = "HighWay")]
        HighWay = 1
    }
}
