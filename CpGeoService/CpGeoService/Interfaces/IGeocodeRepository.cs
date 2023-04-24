using CpGeoService.Model;

namespace CpGeoService.Interfaces
{
    public interface IGeocodeRepository
    {
        // Tìm kiếm tọa độ theo danh sách địa chỉ
        Task<ResultGeoByAddressMerge> GeoByAddressAsync(List<InputAddress> addressList);
    }
}
