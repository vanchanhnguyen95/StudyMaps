using BAGeocoding.Entity.Enum;

namespace BAGeocoding.Entity.MapObj
{
    public class BAGDistance
    {
        public BAGSegment Segment { get; set; }
        public EnumBAGAnchor Anchor { get; set; }
        public BAGPoint Point { get; set; }
        public int PointIndex { get; set; }
        public float Distance { get; set; }
        public bool IsLeft { get; set; }
        public float Percen { get; set; }
        
        public BAGDistance() { }

        public BAGDistance(EnumBAGAnchor anchor, BAGPoint point, double distance)
        {
            Anchor = anchor;
            Point = point;
            Distance = (float)distance;
        }
    }

    public class BAGDistanceV2
    {
        public BAGSegment Segment { get; set; }
        public EnumBAGAnchor Anchor { get; set; }
        public BAGPointV2 Point { get; set; }
        public int PointIndex { get; set; }
        public float Distance { get; set; }
        public bool IsLeft { get; set; }
        public float Percen { get; set; }

        public BAGDistanceV2() { }

        public BAGDistanceV2(EnumBAGAnchor anchor, BAGPointV2 point, double distance)
        {
            Anchor = anchor;
            Point = point;
            Distance = (float)distance;
        }
    }
}
