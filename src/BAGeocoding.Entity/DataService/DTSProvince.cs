using System.Collections;

namespace BAGeocoding.Entity.DataService
{
    public class DTSProvince
    {
        //Danh sách từ khóa để tìm tỉnh ([{key: [ID,..]},..])
        public Hashtable Keys { get; set; }
        //Danh sách các đối tượng tỉnh (Province) ([ID, Name, Points[]])
        public Hashtable Objs { get; set; }
        //Danh sách đoạn đường có tên (GSSegment) trong tỉnh (Keys[], Objs[], RTree{})
        public Hashtable Segm { get; set; }

        public DTSProvince()
        {
            Keys = new Hashtable();
            Objs = new Hashtable();
            Segm = new Hashtable();
        }
    }
}
