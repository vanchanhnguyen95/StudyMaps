using CpGeoService.Interfaces;
using CpGeoService.Model;

namespace CpGeoService.Repository
{
    public class GeocodeRepository : IGeocodeRepository
    {
        private readonly IGeoPBDV2Repository _geoPBDV2Repository;
        private readonly IGeoPNCRepository _geoPNCRepository;

        public GeocodeRepository(IGeoPBDV2Repository geoPBDV2Repository, IGeoPNCRepository geoPNCRepository) {
            _geoPBDV2Repository = geoPBDV2Repository;
            _geoPNCRepository = geoPNCRepository;

        }

        public async Task<ResultGeoByAddressMerge> GeoByAddressAsync(List<InputAddress>? addressList)
        {
            try
            {
                if(!addressList.Any())
                    return new ResultGeoByAddressMerge()
                    {
                        Code = 2,
                        Message = "Không có dữ liệu"
                    };


                ResultGeoByAddressMerge result = new ResultGeoByAddressMerge();
                result.Data = new List<DataMerg>();
                result.Code =  1;
                result.Message = "Thành công";

                foreach (var item in addressList)
                {
                    if (string.IsNullOrEmpty(item.Address))
                        continue;

                    DataMerg dataMerg = new DataMerg();
                    dataMerg.DataOld = await _geoPNCRepository.GeoByAddressAsync(item?.Address ?? "");
                    dataMerg.DataNew = await _geoPBDV2Repository.GeoByAddressAsync(item?.Address ?? "");
                    result.Data.Add(dataMerg);
                }

                return result;

            }
            catch(Exception)
            {
                return new ResultGeoByAddressMerge();
            }
        }
    }
}
