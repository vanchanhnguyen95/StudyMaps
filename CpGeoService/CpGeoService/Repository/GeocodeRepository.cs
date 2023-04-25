using AutoMapper;
using CpGeoService.Interfaces;
using CpGeoService.Model;
//using Nest;

namespace CpGeoService.Repository
{
    public class GeocodeRepository : IGeocodeRepository
    {
        private readonly IGeoPBDVRepository _geoPBDVRepository;
        private readonly IGeoPBDV2Repository _geoPBDV2Repository;
        private readonly IGeoPNCRepository _geoPNCRepository;
        private readonly IMapper _mapper;


        public const int GridSize = 100;

        private static double CalculateDistance(double lat1 = 0, double lng1 =0, double lat2 =0, double lng2 =0)
        {
            double P1X = lat1 * (Math.PI / 180);
            double P1Y = lng1 * (Math.PI / 180);
            double P2X = lat2 * (Math.PI / 180);
            double P2Y = lng2 * (Math.PI / 180);

            double Kc = 0;
            double Temp = 0;

            if (lat1 == lat2 && lng1 == lng2) return 0;

            Kc = P2X - P1X;
            Temp = Math.Cos(Kc);
            Temp = Temp * Math.Cos(P2Y);
            Temp = Temp * Math.Cos(P1Y);

            Kc = Math.Sin(P1Y);
            Kc = Kc * Math.Sin(P2Y);
            Temp = Temp + Kc;
            Kc = Math.Acos(Temp);
            Kc = Kc * 6376000;

            //Hieu chinh quang duong km gps so voi thuc te
            //Kc = Kc * 1.0566;

            return Kc;
        }

        public GeocodeRepository(IGeoPBDVRepository geoPBDVRepository,IGeoPBDV2Repository geoPBDV2Repository, IGeoPNCRepository geoPNCRepository, IMapper mapper) {
            _geoPBDVRepository = geoPBDVRepository;
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


                    var dataOld = _mapper.Map<DataMerg>(await _geoPBDVRepository.GeoByAddressAsync(item?.Address ?? ""));
                    dataOld.Dep = @"Service cũ";

                    var dataNew = _mapper.Map<DataMerg>(await _geoPBDV2Repository.GeoByAddressAsync(item?.Address ?? ""));
                    dataNew.Dep = @"Service mới";
                    dataNew.Distance = CalculateDistance(dataNew.Lat, dataNew.Lng, dataOld.Lat, dataOld.Lng);

                    //DataMerg dataMerg = new DataMerg();
                    //var dataPNC = _mapper.Map<DataMerg>(await _geoPNCRepository.GeoByAddressAsync(item?.Address ?? ""));
                    //dataPNC.Dep = @"Phòng Nghiên cứu";
                    //dataPNC.Distance = CalculateDistance(dataPNC.Lat, dataPNC.Lng, dataNew.Lat, dataNew.Lng);

                    //result.Location.
                    datum.Location.Add(dataOld);
                    datum.Location.Add(dataNew);
                    //datum.Location.Add(dataPNC);
                    //datum.Distance = CalculateDistance(dataPNC.Lat,dataPNC.Lng,dataNew.Lat,dataNew.Lng);

                    result.Data.Add(datum);
                }

                return result;

            }
            catch(Exception)
            {
                return new ResultGeoByAddressMerge()
                {
                    Code = 2,
                    Message = $"Lỗi trong quá trình tìm kiếm dữ liệu"
                };
            }
        }
    }
}
