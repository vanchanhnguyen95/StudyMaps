using Elastic02.Models.Test;
using Nest;

namespace Elastic02.Services.Test
{
    public interface IHaNoiRoadService
    {
        Task<string> CreateIndex();

        Task<bool> BulkAsync(List<HaNoiRoadPoint> haNoiRoads);
        Task<bool> CreateAsync(List<HaNoiRoadPush> haNoiRoads);
        // Tìm kiếm theo Tọa độ / Từ Khóa / Tọa độ và từ khóa
        Task<List<HaNoiRoadPush>> GetDataSuggestion(double lat, double lng, GeoDistanceType type, string distance, int size, string keyWord);
    }
}
