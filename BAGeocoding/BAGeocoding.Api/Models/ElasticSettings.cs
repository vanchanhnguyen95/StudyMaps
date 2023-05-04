namespace BAGeocoding.Api.Models
{
    public class ElasticSettings
    {
        public string EsEndPoint { get; set; } = string.Empty;//  "EsEndPoint": "http://localhost:9200",
        public string Host { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
