using System.Collections;

using BAGeocoding.Entity.MapObj;

using RTree.Engine;

namespace BAGeocoding.Entity.DataService
{
    public class DTSTile
    {
        //Danh sách các đối tượng vùng tìm kiếm ([ID, CommuneID, Points[]])
        public Hashtable Objs { get; set; }
        //RTree để tìm kiếm theo tọa độ
        public RTree<BAGTile> RTree { get; set; }

        public DTSTile()
        {
            Objs = new Hashtable();
            RTree = new RTree<BAGTile>();
        }
    }
}
