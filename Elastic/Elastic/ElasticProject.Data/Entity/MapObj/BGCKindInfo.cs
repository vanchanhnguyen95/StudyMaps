namespace ElasticProject.Data.Entity.MapObj
{
    public class BGCKindInfo
    {
        public short KindID { get; set; }
        public string Name { get; set; }

        public BGCKindInfo() { }

        public BGCKindInfo(BGCKindInfo other)
        {
            KindID = other.KindID;
            Name = other.Name;
        }
    }
}
