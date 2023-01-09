using System.Collections;

using BAGeocoding.Entity.MapObj;

using RTree.Engine;

namespace BAGeocoding.Entity.DataService
{
    public class DTSPoint
    {
        public Hashtable KeyName { get; set; }
        public Hashtable KeyInfo { get; set; }
        public Hashtable Objs { get; set; }
        public RTree<int> RTree { get; set; }

        public DTSPoint()
        {
            KeyName = new Hashtable();
            KeyInfo = new Hashtable();
            Objs = new Hashtable();
            RTree = new RTree<int>();
        }
    }
}
