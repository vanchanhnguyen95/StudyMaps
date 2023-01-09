using BAGeocoding.Entity.Enum.MapObject;

namespace BAGeocoding.Entity.Router
{
    public class BARNode
    {
        public int SegmentID { get; set; }
        public double D2Start { get; set; }
        public double D2End { get; set; }
        public double Coeff { get; set; }
        public bool HighWay { get; set; }
        public bool FerryThese { get; set; }
        public BARPoint Coords { get; set; }

        public BARNode() { }

        public BARNode(BARNode other)
        {
            SegmentID = other.SegmentID;
            D2Start = other.D2Start;
            D2End = other.D2End;
            Coeff = other.Coeff;
            HighWay = other.HighWay;
            FerryThese = other.FerryThese;
            if (other.Coords != null)
                Coords = new BARPoint(other.Coords);
        }

        public BARNode(BARSegment seg, int idx)
        {
            SegmentID = seg.SegmentID;
            HighWay = seg.DataExtGet(EnumMOBSegmentDataExt.HighWay);
            FerryThese = seg.FerryThese;
            Coords = new BARPoint(seg.PointList[idx]);
        }
    }
}