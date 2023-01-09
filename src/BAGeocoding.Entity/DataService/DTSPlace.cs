using System.Collections;

using BAGeocoding.Entity.MapObj;

using RTree.Engine;

namespace BAGeocoding.Entity.DataService
{
    public class DTSPlace
    {
        //Danh sách từ khóa để tìm trong khu đô thị
        public Hashtable Keys { get; set; }
        //Danh sách các đối tượng khu đô thị
        public Hashtable Urban { get; set; }
        //Danh sách các đối tượng lô đất
        public Hashtable Portion { get; set; }
        //Danh sách các đối tượng ô đất
        public Hashtable Plot { get; set; }
        //RTree để tìm kiếm theo tọa độ
        public RTree<BAGPlace> RTree { get; set; }

        public DTSPlace()
        {
            Keys = new Hashtable();
            Urban = new Hashtable();
            Portion = new Hashtable();
            Plot = new Hashtable();
            RTree = new RTree<BAGPlace>();
        }
    }
}