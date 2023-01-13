namespace BAGeocoding.Entity.Router
{
    public class BAROutputS
    {
        public bool state { get; set; }
        public string message { get; set; }
        public int length { get; set; }
    }

    public class BAROutputF
    {
        public bool state { get; set; }
        public string message { get; set; }
        public int length { get; set; }
        public int[] detail { get; set; }
    }
}