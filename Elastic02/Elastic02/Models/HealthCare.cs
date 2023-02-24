using Nest;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Elastic02.Models
{
    //public class HealthCareModel
    //{
    //    public string id { get; set; }
    //    public string name { get; set; }
    //    public string keywords { get; set; }
    //    public string specialist { get; set; }

    //    [DataMember(Name = "lat")]
    //    public double Latitude { get; set; } = 0;

    //    [DataMember(Name = "lon")]
    //    public double Longitude { get; set; } = 0;
    //    public GeoLocation geoLocation { get; set; } = new GeoLocation(1, 1);
    //}

    /// <summary>
    /// IdProperty for elasticsearchType will override default property generation by elastic search and will use 
    /// assigned property as id for document level. 
    /// Description holds the index name
    /// NOTE: Id value should be unique and index name should be in Lower Case
    /// </summary>
    [ElasticsearchType(IdProperty = nameof(Id)), Description("healthcaregeo")]
    public class HealthCareGeo
    {
        public HealthCareGeo(string id)
        => Id = id;

        public HealthCareGeo()
        {

        }

        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        /// <summary>
        /// If text field needs to be sortable then Fielddata should set to TRUE 
        /// and by default if there is no mapping defined then elasticsearch will 
        /// use the standard analyzer which will result as case insensitive 
        /// search then output will not as expected. To overcome this behaviour 
        /// we have defined custom Analyzer which will override default one.
        /// Index true will make property to available for search, 
        /// Fielddata true for sorting, aggregations, or scripting.
        /// </summary> 
        [Text(Index = true, Fielddata = true, Analyzer = "my_vi_analyzer")]
        public string Name { get; set; }
        [Text(Index = true, Fielddata = true, Analyzer = "my_vi_analyzer")]
        public string Keywords { get; set; }
        [Text(Index = true, Fielddata = true, Analyzer = "my_vi_analyzer")]
        public string Specialist { get; set; }
        [GeoPoint]
        public string geoLocation { get; set; }
    }
}
