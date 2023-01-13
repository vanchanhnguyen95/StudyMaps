using System.Collections;

namespace BAGeocoding.Entity.Router
{
    public class BARCheckPoint
    {
        public int IndexID { get; set; }
        public int NodeID { get; set; }
        public int SegmentID { get; set; }
        public BARPoint Coords { get; set; }

        public Hashtable SegmentHT { get; set; }

        public BARCheckPoint()
        {
            Coords = new BARPoint();
            SegmentHT = new Hashtable();
        }

        public BARCheckPoint(BARCheckPoint other)
        {
            IndexID = other.IndexID;
            NodeID = other.NodeID;
            SegmentID = other.SegmentID;
            Coords = new BARPoint(other.Coords);
            SegmentHT = new Hashtable();
            foreach (object key in other.SegmentHT.Keys)
                SegmentHT.Add(key, other.SegmentHT[key]);
        }
    }
}