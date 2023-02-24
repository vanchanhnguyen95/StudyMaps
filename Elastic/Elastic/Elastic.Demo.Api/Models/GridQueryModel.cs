namespace Elastic.Demo.Api.Models
{
    public class GridQueryModel
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 5;
        public string SortBy { get; set; } = "";
        public string Search { get; set; } = "";
        public string Direction { get; set; } = "";
    }
}
