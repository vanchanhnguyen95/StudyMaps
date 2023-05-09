using BAGeocoding.Api.Models;
using BAGeocoding.Bll;
using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.Public;
using BAGeocoding.Utility;

namespace BAGeocoding.Api.Services
{

    public interface IGeoService
    {
        // Tìm kiếm địa chỉ theo tọa độ
        Task<List<RPBLAddressResult>> AddressByGeoAsync(string? lngStr, string? latStr);

        Task<Result<object>> AddressByGeoAsyncV2(string? lngStr, string? latStr);

        // Tìm kiếm tọa độ theo địa chỉ
        Task<RPBLAddressResult> GeoByAddressAsync(string? keyStr, string? lanStr);

        // Tìm kiếm tọa độ theo địa chỉ
        Task<Result<object>> GeoByAddressAsyncV2(string? keyStr, string? lanStr);
    }

    public class GeoService : IGeoService
    {
        private const string INIT_DATA_SUCCESS = "1";//Đã khởi tạo dữ liệu
        private const string INIT_DATA_FAIL = "2";//Chưa khởi tạo xong dữ liệu
        public GeoService() {
            if (RunningParams.ProcessState != EnumProcessState.Success)
                MainProcessing.InitData();
        }

        public async Task<List<RPBLAddressResult>> AddressByGeoAsync(string? lngStr, string? latStr)
        {
            //if (RunningParams.ProcessState != EnumProcessState.Success)
            //{
            //    MainProcessing.InitData();
            //    return await Task.Run(() => MainProcessing.AddressByGeo(lngStr ?? "", latStr ?? ""));
            //}
            return await Task.Run(() => MainProcessing.AddressByGeo(lngStr ?? "", latStr ?? ""));
        }

        public async Task<Result<object>> AddressByGeoAsyncV2(string? lngStr, string? latStr)
        {
            var ret = await Task.Run(() => MainProcessing.AddressByGeoV2(lngStr ?? "", latStr ?? ""));
            if (RunningParams.ProcessState != EnumProcessState.Success)
            {
                MainProcessing.InitData();
                return Result<object>.Error(INIT_DATA_FAIL, "Chưa khởi tạo xong dữ liệu", RunningParams.ProcessState.ToString());
            }

            if (!ret.Any())
            {
                LogFile.WriteNoDataAddressByGeo($"AddressByGeo, [ lat:{latStr}, lng:{lngStr} ]");
                return Result<object>.Success(new RPBLAddressResultV2(), INIT_DATA_SUCCESS, "Đã khởi tạo xong dữ liệu", RunningParams.ProcessState.ToString());
            }

            var data = new List<RPBLAddressResultV2>(ret);
            return Result<object>.Success(data, INIT_DATA_SUCCESS, "Đã khởi tạo dữ liệu", RunningParams.ProcessState.ToString());
            //if (RunningParams.ProcessState != EnumProcessState.Success)
            //{
            //    MainProcessing.InitData();
            //    return await Task.Run(() => MainProcessing.AddressByGeo(lngStr ?? "", latStr ?? ""));
            //}
            //return await Task.Run(() => MainProcessing.AddressByGeoV2(lngStr ?? "", latStr ?? ""));
        }

        public async Task<RPBLAddressResult> GeoByAddressAsync(string? keyStr, string? lanStr)
        {
            //return await Task.Run(() => MainProcessing.GeoByAddress(keyStr ?? "", lanStr ?? ""));
            //if (RunningParams.ProcessState != EnumProcessState.Success)
            //{
            //    MainProcessing.InitData();
            //    return await Task.Run(() => MainProcessing.GeoByAddress(keyStr ?? "", lanStr ?? ""));
            //}
            return await Task.Run(() => MainProcessing.GeoByAddress(keyStr ?? "", lanStr ?? ""));
        }

        public async Task<Result<object>> GeoByAddressAsyncV2(string? keyStr, string? lanStr)
        {
            var ret = await Task.Run(() => MainProcessing.GeoByAddressV2(keyStr ?? "", lanStr ?? ""));
            if (RunningParams.ProcessState != EnumProcessState.Success)
            {
                MainProcessing.InitData();
                return Result<object>.Error(INIT_DATA_FAIL, "Chưa khởi tạo xong dữ liệu", RunningParams.ProcessState.ToString());
            }

            if (ret == null)
            {
                LogFile.WriteNoDataGeobyAddress($"GeoByAddress, [ key:{keyStr} ]");
                return Result<object>.Success(new PBLAddressResultV2(), INIT_DATA_SUCCESS, "Đã khởi tạo dữ liệu", RunningParams.ProcessState.ToString());
            }     

            var data = new PBLAddressResultV2(ret);
            return Result<object>.Success(data, INIT_DATA_SUCCESS, "Đã khởi tạo dữ liệu", RunningParams.ProcessState.ToString());
        }
    }
}
