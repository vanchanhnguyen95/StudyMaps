using BAGeocoding.Api.Models;
using BAGeocoding.Bll;
using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.Public;
using RTree.Engine.Entity;

namespace BAGeocoding.Api.Services
{
    public interface IGeoService
    {
        // Tìm kiếm địa chỉ theo tọa độ
        Task<List<RPBLAddressResult>> AddressByGeoAsync(string? lngStr, string? latStr);
        // Tìm kiếm tọa độ theo địa chỉ
        Task<RPBLAddressResult> GeoByAddressAsync(string? keyStr, string? lanStr);

        // Tìm kiếm tọa độ theo địa chỉ
        Task<Result<object>> GeoByAddressAsyncV2(string? keyStr, string? lanStr);

        Task<Result<object>> AutosuggestAddress(string? keyStr, string? lanStr, int? limit);
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

        public async Task<Result<object>> GeoByAddressAsyncV2(string? keyStr, string? lanStr)
        {
            var ret = await Task.Run(() => MainProcessing.GeoByAddressV2(keyStr ?? "", lanStr ?? ""));
            if (RunningParams.ProcessState != EnumProcessState.Success)
            {
                return Result<object>.Error("500", "Chưa khởi tạo xong dữ liệu", RunningParams.ProcessState.ToString());
            }

            if (ret == null)
            {
                return Result<object>.Success(new PBLAddressResultV2(), "200", "Đã khởi tạo xong dữ liệu", RunningParams.ProcessState.ToString());
            }
            var data = new PBLAddressResultV2(ret);
            return Result<object>.Success(data, "200", "Đã khởi tạo dữ liệu", RunningParams.ProcessState.ToString());
        }

        public async Task<Result<object>> AutosuggestAddress(string? keyStr, string? lanStr, int? limit)
        {
            List<RPBLAddressResultV2> ret = await Task.Run(() => MainProcessing.AutosuggestAddress(keyStr ?? "", lanStr ?? "", limit??10));
            if (RunningParams.ProcessState != EnumProcessState.Success)
            {
                return Result<object>.Error("500", "Chưa khởi tạo xong dữ liệu", RunningParams.ProcessState.ToString());
            }

            if (ret == null)
            {
                return Result<object>.Success(new List<PBLAddressResultV2>(), "200", "Đã khởi tạo dữ liệu", RunningParams.ProcessState.ToString());
            }

            var data = new List<PBLAddressResultV2>();
            foreach(RPBLAddressResultV2 item in ret)
            {
                data.Add(new PBLAddressResultV2(item));
            }
            //var data = new List<PBLAddressResultV2>(ret);
            return Result<object>.Success(data, "200", "Đã khởi tạo dữ liệu", RunningParams.ProcessState.ToString());
        }
    }
}
