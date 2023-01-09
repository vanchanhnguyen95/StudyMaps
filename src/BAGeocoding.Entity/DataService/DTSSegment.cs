using System.Collections;

using BAGeocoding.Entity.MapObj;

using RTree.Engine;
using KDTree;

namespace BAGeocoding.Entity.DataService
{
    public class DTSSegment
    {
        // Danh sách từ khóa để tìm đường ([{key: [ID,..]},..])
        public Hashtable Keys { get; set; }
        // Danh sách các đối tượng đường (Province) ([ID, Name, Points[]])
        public Hashtable Objs { get; set; }
        // KDTree để xác định node gần nhất
        public KDTree<BAGPoint> KDTree { get; set; }
        // RTree để tìm kiếm theo tọa độ
        public RTree<BAGSegment> RTree { get; set; }

        public DTSSegment()
        {
            Keys = new Hashtable();
            Objs = new Hashtable();
            KDTree = new KDTree<BAGPoint>(2);
            RTree = new RTree<BAGSegment>();
        }
    }
}
