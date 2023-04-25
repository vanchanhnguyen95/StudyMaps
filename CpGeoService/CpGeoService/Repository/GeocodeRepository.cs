using AutoMapper;
using CpGeoService.Interfaces;
using CpGeoService.Model;

namespace CpGeoService.Repository
{
    public class GeocodeRepository : IGeocodeRepository
    {
        private readonly IGeoPBDV2Repository _geoPBDV2Repository;
        private readonly IGeoPNCRepository _geoPNCRepository;
        private readonly IMapper _mapper;

        public GeocodeRepository(IGeoPBDV2Repository geoPBDV2Repository, IGeoPNCRepository geoPNCRepository, IMapper mapper) {
            _geoPBDV2Repository = geoPBDV2Repository;
            _geoPNCRepository = geoPNCRepository;
            _mapper = mapper;

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
                //result.Data = new List<DataMerg>();
                result.Data = new List<Datum>();
                result.Code =  1;
                result.Message = "Thành công";

                foreach (var item in addressList)
                {
                    if (string.IsNullOrEmpty(item.Address))
                        continue;

                    List<Datum> dtNum = new List<Datum>();
                    Datum datum = new Datum();
                    datum.Location = new List<DataMerg>();


                    DataMerg dataMerg = new DataMerg();
                    var dataPNC = _mapper.Map<DataMerg>(await _geoPNCRepository.GeoByAddressAsync(item?.Address ?? ""));
                    dataPNC.Dep = @"Phòng Nghiên cứu";

                    var dataNew = _mapper.Map<DataMerg>(await _geoPBDV2Repository.GeoByAddressAsync(item?.Address ?? ""));
                    dataNew.Dep = @"Service mới";
                    //dataMerg = await _geoPNCRepository.GeoByAddressAsync(item?.Address ?? "");
                    //dataMerg = await _geoPBDV2Repository.GeoByAddressAsync(item?.Address ?? "");
                    //result.Data.Add(dataPNC);
                    //result.Data.Add(dataNew);

                    //result.Location.
                    datum.Location.Add(dataPNC);
                    datum.Location.Add(dataNew);

                    result.Data.Add(datum);
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
