using CpGeoService.Model;

namespace CpGeoService.Interfaces
{
    public interface IGeoPBDV2Repository
    {
        // Tìm kiếm tọa độ theo địa chỉ
        Task<DataPNC> GeoByAddressAsync(string? address);
        //Task<Result<ResultGeoByAddressPBDV2>> GeoByAddressAsync(string address);
    }
}
