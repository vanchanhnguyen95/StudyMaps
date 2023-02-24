namespace Elastic.Demo.Api.Models
{
    public class CommonStats
    {
        public double? Average { get; set; }
        public long Count { get; set; }
        public double? Max { get; set; }
        public double? Min { get; set; }
        public double Sum { get; set; }
        public double Median { get; set; }
        public List<GroupStats> GroupStats { get; set; }
    }
}
