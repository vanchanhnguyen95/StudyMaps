using Elastic02.Models.Test;
using Nest;

namespace Elastic02.Services.Test
{
    public interface IRoadNameService
    {
        Task<string> BulkAsync(List<RoadNamePush> roadPushs);
        Task<string> BulkAsyncMultiProvince(List<RoadNamePush> roadPushs);

        // Tìm kiếm theo Tọa độ / Từ Khóa / Tọa độ và từ khóa
        Task<List<RoadName>> GetDataSuggestion(double lat, double lng, GeoDistanceType type, string distance, int size, string keyword, GeoShapeRelation relation);
        Task<List<RoadNamePush>> GetDataSuggestion(double lat, double lng, GeoDistanceType type, string distance, int size, string keyword, GeoShapeRelation relation, int provinceID);

    }
}
