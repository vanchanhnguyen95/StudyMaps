using ElasticProject.Data.Entity.MapObj;
using Nest;

namespace ElasticProject.Data.Interface
{
    public interface IHealthCareService
    {
        Task<string> CreateIndex(string indexName);
        Task<string> InsertData(string indexName, List<HealthCareModel> healthCares);

        Task<List<string>> GetDataSuggestion(string keyword, int pageSize);

        // Tìm kiếm theo tọa độ
        Task<List<HealthCareModel>> GetDataSearchGeo(double lat, double ln, GeoDistanceType type, string distance, int pageSize);
        // Tìm kiếm theo tọa độ và từ khóa
        Task<List<HealthCareModel>> GetDataSearchGeo(double lat, double ln, GeoDistanceType type, string distance, int pageSize, string keyword);
    }
}
