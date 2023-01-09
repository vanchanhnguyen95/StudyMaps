using BAGeocoding.Entity.Enum;

namespace BAGeocoding.Entity.Router
{
    public class BARDetech
    {
        public EnumBAGAnchor Anchor { get; set; }
        public BARPoint Point { get; set; }
        public int PointIndex { get; set; }
        public double Distance { get; set; }
        public BARNode NodeInfo { get; set; }
        public double D2Start { get; set; }
        public double D2End { get; set; }
        
        public BARDetech() { }

        public BARDetech(BARDetech other)
        {
            Anchor = other.Anchor;
            Point = new BARPoint(other.Point);
            PointIndex = other.PointIndex;
            Distance = other.Distance;
            NodeInfo = new BARNode(other.NodeInfo);

            D2Start = other.D2Start;
            D2End = other.D2End;
        }

        public void Update(BARDetech other)
        {
            Anchor = other.Anchor;
            Point = new BARPoint(other.Point);
            Distance = other.Distance;
            PointIndex = other.PointIndex;
            NodeInfo.D2Start = other.NodeInfo.D2Start;
        }
    }
}
