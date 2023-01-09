using System;
using BAGeocoding.Entity.Enum.CheckData;

namespace BAGeocoding.Entity.CheckData
{
    public class CDTSegment
    {
        public byte KindID { get; set; }
        public EnumCDTErrorKind EnumKindID { get { return (EnumCDTErrorKind)KindID; } set { KindID = (byte)value; } }
        public int SegmentID { get; set; }
        public string DataStr { get; set; }
        public string GeoStr { get; set; }
    }
}
