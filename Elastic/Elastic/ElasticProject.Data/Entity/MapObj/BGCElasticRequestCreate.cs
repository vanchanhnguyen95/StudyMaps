namespace ElasticProject.Data.Entity.MapObj
{
    public class BGCElasticRequestCreate
    {
        //public byte typeid { get; set; }
        //public int indexid { get; set; }
        //public byte shapeid { get; set; }
        public string kindname { get; set; }
        public string name { get; set; }
        //public string address { get; set; }
        //public string shortkey { get; set; }
        public byte provinceid { get; set; }
        //public byte priority { get; set; }
        public List<BGCElasticPointBase> coords { get; set; } = new List<BGCElasticPointBase>();

        //public BGCElasticRequestCreate()
        //{
        //    coords = new List<BGCElasticPointBase>();
        //}

        //public BGCElasticRequestCreate(BGCPointOfInterest other)
        //{
        //    typeid = (byte)33;//(byte)EnumMapObjectType.PointOfInterest;
        //    indexid = other.PoiID;
        //    shapeid = (byte)1;//(byte)EnumMapObjectShape.Point;
        //    kindname = other.KindInfo.Name;
        //    name = string.Format("{0} {1}", other.KindInfo.Name, other.Name);
        //    address = other.Info;
        //    shortkey = other.ShortKey;
        //    provinceid = other.ProvinceID;
        //    priority = (byte)1;//(byte)EnumElasticSearchPriority.Normal;
        //    coords = new List<BGCElasticPointBase>();
        //    coords.Add(new BGCElasticPointBase(other.Coord));
        //}

        //public BGCElasticRequestCreate(BGCElasticRequestCreate other)
        //{
        //    typeid = other.typeid;
        //    indexid = other.indexid;
        //    shapeid = other.shapeid;
        //    kindname = other.kindname;
        //    name = other.name;
        //    address = other.address;
        //    shortkey = other.shortkey;
        //    provinceid = other.provinceid;
        //    priority = other.priority;
        //    coords = new List<BGCElasticPointBase>();
        //    for (int i = 0; i < other.coords.Count; i++)
        //        coords.Add(new BGCElasticPointBase(other.coords[i]));
        //}

        //public BGCElasticRequestCreate(BGCRoadName other)
        //{
        //    typeid = (byte)32;//(byte)EnumMapObjectType.RoadSegment;
        //    indexid = other.RoadID;
        //    shapeid = (byte)2;//(byte)EnumMapObjectShape.Polyline;
        //    kindname = "Đường";
        //    if (other.NameExt.Length == 0)
        //    {
        //        name = other.RoadName;
        //        address = string.Format("{0}, {1}", other.RoadName, other.Address);
        //    }
        //    else
        //    {
        //        name = string.Format("{0}, {1}", other.RoadName, other.NameExt);
        //        address = string.Format("{0}, {1}, {2}", other.RoadName, other.NameExt, other.Address);
        //    }
        //    shortkey = string.Empty;
        //    provinceid = other.ProvinceID;
        //    priority = (byte)1;//(byte)EnumElasticSearchPriority.Normal;
        //    coords = new List<BGCElasticPointBase>();
        //    coords.Add(new BGCElasticPointBase(other.Coord));
        //}
    }
}
