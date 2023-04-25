using CpGeoService.Model;

namespace CpGeoService.Interfaces
{
    public interface IGeoPBDVRepository
    {
        // Tìm kiếm tọa độ theo địa chỉ
        Task<DataPNC> GeoByAddressAsync(string? address);
    }
}
