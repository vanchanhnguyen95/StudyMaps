using BAGeocoding.Bll;
using BAGeocoding.Entity.Public;

namespace BAGeocoding.Api.Services
{
    public interface IGeoService
    {
        // Tìm kiếm địa chỉ theo tọa độ
        Task<List<RPBLAddressResult>> AddressByGeoAsync(string? lngStr, string? latStr);
        // Tìm kiếm tọa độ theo địa chỉ
        Task<RPBLAddressResult> GeoByAddressAsync(string? keyStr, string? lanStr);

        // Tìm kiếm tọa độ theo địa chỉ
        Task<RPBLAddressResultV2> GeoByAddressAsyncV2(string? keyStr, string? lanStr);
    }

    public class GeoService : IGeoService
    {
        public GeoService() { }

        public async Task<List<RPBLAddressResult>> AddressByGeoAsync(string? lngStr, string? latStr)
        {
            return await Task.Run(() => MainProcessing.AddressByGeo(lngStr ?? "", latStr ?? ""));
        }

        public async Task<RPBLAddressResult> GeoByAddressAsync(string? keyStr, string? lanStr)
        {
            return await Task.Run(() => MainProcessing.GeoByAddress(keyStr ?? "", lanStr ?? ""));
        }

        public async Task<RPBLAddressResultV2> GeoByAddressAsyncV2(string? keyStr, string? lanStr)
        {
            return await Task.Run(() => MainProcessing.GeoByAddressV2(keyStr ?? "", lanStr ?? ""));
        }

    }
}
