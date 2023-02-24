namespace ElasticProject.Data.Entity.MapObj
{
    public class BGCProvince
    {
        public byte ProvinceID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Abbreviate { get; set; }
        public string Description { get; set; }

        public bool Checked { get; set; }

        public BGCProvince() { }

        public BGCProvince(BGCProvince other)
        {
            ProvinceID = other.ProvinceID;
            Name = other.Name;
            Abbreviate = other.Abbreviate;
            Description = other.Description;
        }
    }
}
