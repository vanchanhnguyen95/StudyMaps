using System.Collections;

using BAGeocoding.Entity.Router;

using KDTree;
using RTree.Engine;

namespace BAGeocoding.Entity.DataService
{
    public class DTSRouter
    {
        // Xác thực kết nối
        public Hashtable Authen { get; set; }
        // Key đăng ký sử dụng
        public Hashtable Register { get; set; }
        // Danh sách các đối tượng đường (Province) ([ID, Name, Points[]])
        public Hashtable Objs { get; set; }
        // Lưu lượng sử dụng
        public Hashtable Traffic { get; set; }


        // KDTree để tìm node gần nhất
        public KDTree<BARNode> KDTree { get; set; }
        // RTree để tìm kiếm theo tọa độ
        public RTree<BARSegment> RTree { get; set; }


        public DTSRoutTree Normal { get; set; }
        public DTSRoutTree IsWalk { get; set; }
        public DTSRoutTree IgFerry { get; set; }

        public DTSRouter()
        {
            Authen = new Hashtable();
            Register = new Hashtable();
            Objs = new Hashtable();
            Traffic = new Hashtable();

            KDTree = new KDTree<BARNode>(2);
            RTree = new RTree<BARSegment>();

            Normal = new DTSRoutTree();
            IsWalk = new DTSRoutTree();
            IgFerry = new DTSRoutTree();
        }
    }

    public class DTSRoutTree
    {
        // KDTree để tìm node gần nhất
        public KDTree<BARNode> KDTree { get; set; }
        // RTree để tìm kiếm theo tọa độ
        public RTree<BARSegment> RTree { get; set; }

        public DTSRoutTree()
        {
            KDTree = new KDTree<BARNode>(2);
            RTree = new RTree<BARSegment>();
        }
    }
}