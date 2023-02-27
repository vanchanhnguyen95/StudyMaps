using Nest;
using System.ComponentModel;

namespace Elastic02.Models
{
    public class ElasticRequestPush
    {
        /// <summary>
        /// If text field needs to be sortable then Fielddata should set to TRUE 
        /// and by default if there is no mapping defined then elasticsearch will 
        /// use the standard analyzer which will result as case insensitive 
        /// search then output will not as expected. To overcome this behaviour 
        /// we have defined custom Analyzer which will override default one.
        /// Index true will make property to available for search, 
        /// Fielddata true for sorting, aggregations, or scripting.
        /// </summary> 
        //[Text(Index = true, Fielddata = true, Analyzer = "my_vi_analyzer")]
        public byte typeid { get; set; }
        [Number(Index = true)]
        public int indexid { get; set; }
        public byte shapeid { get; set; }
        [Text(Index = true, Fielddata = true)]
        public string kindname { get; set; }
        [Text(Index = true, Fielddata = true)]
        public string name { get; set; }
        [Text(Index = true, Fielddata = true)]
        public string address { get; set; }
        [Text(Index = true, Fielddata = true)]
        public string shortkey { get; set; }
        public byte provinceid { get; set; }
        public byte priority { get; set; }

        public List<ElasticPointBase> coords { get; set; }

        public ElasticRequestPush()
        {
            coords = new List<ElasticPointBase>();
        }
    }

    /// <summary>
    /// IdProperty for elasticsearchType will override default property generation by elastic search and will use 
    /// assigned property as id for document level. 
    /// Description holds the index name
    /// NOTE: Id value should be unique and index name should be in Lower Case
    /// </summary>
    [ElasticsearchType(IdProperty = nameof(Id)), Description("push_geopoint")]
    public class ElasticRequestPushGeopoint : ElasticRequestPush
    {
        [GeoPoint]
        public GeoLocation location { get; set; }


        public ElasticRequestPushGeopoint(ElasticRequestPush other)
        {
            typeid = other.typeid;
            indexid = other.indexid;
            shapeid = other.shapeid;
            kindname = other.kindname;
            name = other.name;
            address = other.address;
            shortkey = other.shortkey;
            provinceid = other.provinceid;
            priority = other.priority;
            coords = other.coords;
            //location = string.Join(",", coords); "\"" 
            location = coords[0].lat.ToString() + "," + coords[0].lng.ToString();
        }
    }

    [ElasticsearchType(IdProperty = nameof(Id)), Description("push_geoshape")]
    public class ElasticRequestPushGeoshape : ElasticRequestPush
    {
        [GeoShape]
        public string location { get; set; }

        public ElasticRequestPushGeoshape(ElasticRequestPush other)
        {
            typeid = other.typeid;
            indexid = other.indexid;
            shapeid = other.shapeid;
            kindname = other.kindname;
            name = other.name;
            address = other.address;
            shortkey = other.shortkey;
            provinceid = other.provinceid;
            priority = other.priority;
            coords = other.coords;

            if (other.shapeid == 1)//Điểm
            {
                double[] arrayCoords = new double[] { other.coords[0].lng, other.coords[0].lat };
                string loc = string.Join(" ", arrayCoords);

                location = "POINT (" + loc + ")";
            }
            else if (other.shapeid == 2)//Đường
            {
                List<string> lsyLine = new List<string>();
                string lineString = string.Empty;
                for (int i = 0; i < other.coords.Count; i++)
                {
                    double[] arrayCoords = new double[] { other.coords[i].lng, other.coords[i].lat };
                    string loc = string.Join(" ", arrayCoords);
                    lsyLine.Add(loc);
                }

                lineString = string.Join(",", lsyLine);
                location = "LINESTRING  (" + lineString + ")";

            }
            else if (other.shapeid == 3)// Vùng
            {
                List<string> lsyLine = new List<string>();
                string lineString = string.Empty;
                for (int i = 0; i < other.coords.Count; i++)
                {
                    double[] arrayCoords = new double[] { other.coords[i].lng, other.coords[i].lat };
                    string loc = string.Join(" ", arrayCoords);
                    lsyLine.Add(loc);
                }

                lineString = string.Join(",", lsyLine);
                location = "POLYGON  ((" + lineString + "))";
            }
        }
    }

}
