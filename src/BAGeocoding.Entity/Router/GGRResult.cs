using System.Collections.Generic;

namespace BAGeocoding.Entity.Router
{
    public class GGRDirectionRes
    {
        public string status { set; get; }
        public List<GGRRoute> routes { set; get; }
    }

    public class GGRRoute
    {
        public List<GGRLeg> legs { set; get; }
        public GGRPolyline overview_polyline { set; get; }
        public string summary { set; get; }
    }

    public class GGRPolyline
    {
        public string points { set; get; }
    }

    public class GGRLeg
    {
        public GGRTextVal distance { set; get; }
        public GGRTextVal duration { set; get; }
    }

    public class GGRTextVal
    {
        public string text { set; get; }
        public int value { set; get; }
    }
}