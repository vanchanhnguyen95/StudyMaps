using System.Collections;

namespace BAGeocoding.Entity.DataService
{
    public class DTSDistrict
    {
        //Danh sách từ khóa để tìm huyện ([{key: [ID,..]},..])
        public Hashtable Keys { get; set; }
        //Danh sách các đối tượng huyện (District) ([ID, Name, Points[]])
        public Hashtable Objs { get; set; }
        
        public DTSDistrict()
        {
            Keys = new Hashtable();
            Objs = new Hashtable();
        }
    }
}
