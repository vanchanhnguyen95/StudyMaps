using Nest;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace ElasticProject.Data.Entity.MapObj
{
    public class HealthCareModel
    {
        public string id { get; set; }
        public string name { get; set; }
        //public string address { get; set; }
        public string keywords { get; set; }
        public string specialist { get; set; }
        //public string TypeHealthCare { get; set; }
        //public string PhoneNumber { get; set; }
        //public string Viewed { get; set; }
        //public string WorkPlace { get; set; }

        [DataMember(Name = "lat")]
        public double Latitude { get; set; } = 0;

        [DataMember(Name = "lon")]
        public double Longitude { get; set; } = 0;
        public GeoLocation geoLocation { get; set; } = new GeoLocation(1, 1);
        //public GeoShapeBase geoShape { get; set; }
        //public GeoShapeAttribute geoShapeAttribute { get; set; }
        //public GeoShapeProperty geoShapeProperty { get; set; }
        //public GeoShapeBase geoShape { get; set; }


        //public Distance distancedistance { get; set; };
        //public string Slug { get; set; }
    }

    //public class GeoShape : IGeoShape
    //{
    //    public string Type => throw new NotImplementedException();
    //}
}
