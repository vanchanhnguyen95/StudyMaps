using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.MapTool
{
    public enum EnumMTLTrackType : byte
    {
        [EnumItem("Track logs di chuyển", FieldName = "Move")]
        Move = 1,

        [EnumItem("Track logs làm việc", FieldName = "Work")]
        Work = 2
    }
}