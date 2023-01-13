using System.Collections;

using BAGeocoding.Entity.MapObj;

using RTree.Engine;

namespace BAGeocoding.Entity.DataService
{
    public class DTSCommune
    {
        //Danh sách từ khóa để tìm xã ([{key: [ID,..]},..])
        public Hashtable Keys { get; set; }
        //Danh sách các đối tượng xã (Commune) ([ID, Name, Points[]])
        public Hashtable Objs { get; set; }
        //RTree để tìm kiếm theo tọa độ
        public RTree<BAGCommune> RTree { get; set; }

        public DTSCommune()
        {
            Keys = new Hashtable();
            Objs = new Hashtable();
            RTree = new RTree<BAGCommune>();
        }
    }
}
