using Nest;
using System.Text;

namespace ElasticProject.Data.Entity.MapObj
{
    public  class ElasticRequestPushGeoshape
    {
        public byte typeid { get; set; }//byte
        public int indexid { get; set; }//int
        public byte shapeid { get; set; }//byte
        [Text]
        public string kindname { get; set; }
        [Text]
        public string name { get; set; }
        [Text]
        public string address { get; set; }
        [Text]
        public string shortkey { get; set; }
        public byte provinceid { get; set; }//byte
        public byte priority { get; set; }//byte

        public List<ElasticPointBase> coords { get; set; }

        public ElasticRequestPushGeoshape()
        {
            coords = new List<ElasticPointBase>();
            //location = new Location();
        }

    }


    public class ElasticRequestCreateGeoshape : ElasticRequestPushGeoshape
    {
        [GeoShape]
        public string location { get; set; }

        public ElasticRequestCreateGeoshape(ElasticRequestPushGeoshape other)
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
                string loc =  string.Join(" ", arrayCoords);

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
