using BAGeocoding.Entity.Enum.MapTool;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapTool.Plan
{
    public class WRKTrackFile : SQLDataUlt
    {
        public int PlanID { get; set; }
        public int UserID { get; set; }
        public byte TypeID { get; set; }
        public EnumMTLTrackType EnumTypeID { get { return (EnumMTLTrackType)TypeID; } set { TypeID = (byte)value; } }
        public string FilePath { get; set; }
        public int EditTime { get; set; }
    }
}