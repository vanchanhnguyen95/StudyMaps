using Elastic02.Models.Test;
using Nest;

namespace Elastic02.Services.Test
{
    public interface IRoadNameService
    {
        Task<string> BulkAsync(List<RoadNamePush> roadPushs);
        Task<string> BulkAsyncMultiProvince(List<RoadNamePush> roadPushs);

        // Tìm kiếm theo Tọa độ / Từ Khóa / Tọa độ và từ khóa
        Task<List<RoadNamePush>> GetDataSuggestion(double lat, double lng, string distance, int size, string keyword );
        //Task<List<RoadNamePush>> GetDataSuggestion(double lat, double lng, GeoDistanceType type, string distance, int size, string keyword);

        Task<List<RoadName>> GetRouting(GeoLocation poingStart, GeoLocation pointEnd,int size);

    }
}
