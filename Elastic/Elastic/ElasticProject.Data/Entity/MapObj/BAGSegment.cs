namespace ElasticProject.Data.Entity.MapObj
{
    public class BAGSegment
    {
        public int TemplateID { get; set; }
        public int SegmentID { get; set; }
        public string VName { get; set; }
        public string EName { get; set; }
        public short ProvinceID { get; set; }
        public short DistrictID { get; set; }
        public byte Direction { get; set; }
        public byte ClassFunc { get; set; }
        public int DataExt { get; set; }
        public List<BAGPoint> PointList { get; set; }

        public short StartLeft { get; set; }
        public short StartRight { get; set; }
        public short EndLeft { get; set; }
        public short EndRight { get; set; }

        //public byte LevelID { get; set; }
        //public byte KindID { get; set; }
        //public byte RegionLev { get; set; }
        //public byte Wide { get; set; }
        //public byte MinSpeed { get; set; }
        //public byte MaxSpeed { get; set; }
        public float SegLength { get; set; }
        //public int Fee { get; set; }
        //public bool IsNumber { get; set; }
        //public bool IsBridge { get; set; }
        //public bool IsPrivate { get; set; }
        //public bool IsPed { get; set; }
        public bool IsSerial { get; set; }
        //public bool AllowPed { get; set; }
        //public bool AllowWalk { get; set; }
        //public bool AllowBicycle { get; set; }
        //public bool AllowMoto { get; set; }
        //public bool AllowCar { get; set; }
        //public byte DirCar { get; set; }
        //public bool AllowBus { get; set; }
        //public byte DirBus { get; set; }
        //public bool AllowTruck { get; set; }
        //public byte DirTruck { get; set; }
        //public bool AllowTaxi { get; set; }
        //public byte DirTaxi { get; set; }

        //public byte MinSpeed1 { get; set; }
        //public byte MaxSpeed1 { get; set; }
        //public byte MinSpeed2 { get; set; }
        //public byte MaxSpeed2 { get; set; }
        //public byte MinSpeed3 { get; set; }
        //public byte MaxSpeed3 { get; set; }
        //public byte MinSpeed4 { get; set; }
        //public byte MaxSpeed4 { get; set; }
        //public short LyTrinh { get; set; }

        public float Length { get; set; }
        //public string GridStr { get; set; }
        public string LngStr { get; set; }
        public string LatStr { get; set; }

        public bool IsDone { get; set; }

        public BAGSegment()
        {
            PointList = new List<BAGPoint>();
            Length = 0;
            LngStr = string.Empty;
            LatStr = string.Empty;
        }

    }
}
