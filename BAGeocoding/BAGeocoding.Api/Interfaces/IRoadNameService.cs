﻿using BAGeocoding.Api.Models.RoadName;
using static BAGeocoding.Api.Models.RoadName.RoadName;

namespace BAGeocoding.Api.Interfaces
{
    public interface IRoadNameService
    {
        Task<string> BulkAsync(List<RoadNamePush> roadPushs);

        // Tìm kiếm theo Tọa độ / Từ Khóa / Tọa độ và từ khóa
        Task<List<RoadNameOut>> GetDataSuggestion(double lat, double lng, string distance, int size, string keyword, int type);

        //Task<List<RoadName>> GetRouting(GeoLocation poingStart, GeoLocation pointEnd, int size);
    }
}
