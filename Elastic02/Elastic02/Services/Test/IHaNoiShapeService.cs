using Elastic02.Models.Test;
using Nest;

namespace Elastic02.Services.Test
{
    public interface IHaNoiShapeService
    {
        Task<string> CreateIndex(string indexName);

        Task<string> BulkAsync(List<HaNoiShape> haNoiRoads);
        // Tìm kiếm theo Tọa độ / Từ Khóa / Tọa độ và từ khóa
        Task<List<HaNoiShapePush>> GetDataSuggestion(double lat, double lng, GeoDistanceType type, string distance, int size, string keyword, GeoShapeRelation relation);
    }
}
