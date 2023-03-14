using Elastic02.Models.Test;
using Nest;

namespace Elastic02.Services.Test
{
    public interface IVietNamShapeService
    {
        Task<string> CreateIndex(string indexName);

        Task<string> BulkAsync(List<VietNamShape> vietNamShapes);

        Task<string> AddAsync(List<VietNamShape> vietNamShapes);
        // Tìm kiếm theo Tọa độ / Từ Khóa / Tọa độ và từ khóa
        Task<List<VietNamShape>> GetDataSuggestion(double lat, double lng, GeoDistanceType type, string distance, int size, string keyword, GeoShapeRelation relation);
    }
}
