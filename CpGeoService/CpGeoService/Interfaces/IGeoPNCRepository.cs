using CpGeoService.Model;

namespace CpGeoService.Interfaces
{
    public interface IGeoPNCRepository
    {
        // Tìm kiếm tọa độ theo địa chỉ
        Task<DataPNC> GeoByAddressAsync(string address);
        //Task<DataPNC?> GeoByAddressAsync(InputAddress item);
        //Task<Result<ResultGeoByAddressPNC>> GeoByAddressAsync(string address);
    }
}
