using System;
using System.Data;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapObj
{
    public class BAGRect
    {
        public BAGPoint TopLeft { get; set; }
        public BAGPoint BottomRight { get; set; }

        public BAGRect()
        {
            TopLeft = new BAGPoint();
            BottomRight = new BAGPoint();
        }

        public BAGRect(BAGPoint point)
        {
            TopLeft = new BAGPoint(point);
            BottomRight = new BAGPoint(point);
        }

        public void AddPoint(BAGPoint point)
        {
            TopLeft.Lat = Math.Max(TopLeft.Lat, point.Lat);
            TopLeft.Lng = Math.Min(TopLeft.Lng, point.Lng);

            BottomRight.Lat = Math.Min(BottomRight.Lat, point.Lat);
            BottomRight.Lng = Math.Max(BottomRight.Lng, point.Lng);
        }
    }
}
