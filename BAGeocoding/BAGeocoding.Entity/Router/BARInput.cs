
namespace BAGeocoding.Entity.Router
{
    public class BARInput
    {
        public int type { get; set; }
        public int method { get; set; }
        public BARInputNode from { get; set; }
        public BARInputNode to { get; set; }
    }

    public class BARInputNode
    {
        public int seg { get;set; }
        public int d0 { get; set; }
        public int d1 { get; set; }
        public int l0 { get; set; }
        public int l1 { get; set; }
    }
}