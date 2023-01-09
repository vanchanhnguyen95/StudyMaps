using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using BAGeocoding.Bll;

using BAGeocoding.Entity.Public;
using BAGeocoding.Entity.Utility;

using BAGeocoding.Utility;

namespace BAGeocoding.Service
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class geocoding : System.Web.Services.WebService
    {
        public UTLAuthHeader AuthHeader;

        [WebMethod(), SoapHeader("AuthHeader")]
        public PBLAddressResult[] GLAddressByGeo(string lngStr, string latStr, string keyStr)
        {
            try
            {
                // 1. Kiểm tra đầu vào
                // 1.1 Trạng thái khởi tạo dịch vụ
                if (MainProcessing.InitData() == false)
                    return null;
                // 1.2 Thông tin xác thực
                else if (MainProcessing.CheckAuthen(AuthHeader) == false)
                {
                    LogFile.WriteError(string.Format("{0} => Authenticate invalid !!!", HttpContext.Current.Request.UserHostAddress));
                    return null;
                }
                // 1.3 Dữ liệu đầu vào
                else if (lngStr.Trim().Length == 0 || latStr.Trim().Length == 0)
                {
                    LogFile.WriteError(string.Format("{0}: {1} - {2} => data error", HttpContext.Current.Request.UserHostAddress, lngStr, latStr));
                    return null;
                }

                // 2. Tìm kiếm và trả về kết quả
                List<RPBLAddressResult> sourceList = MainProcessing.GLAddressByGeo(lngStr, latStr, keyStr, "vn");
                if (sourceList == null || sourceList.Count == 0)
                    return null;
                List<PBLAddressResult> resultList = new List<PBLAddressResult>();
                for (int i = 0; i < sourceList.Count; i++)
                    resultList.Add(new PBLAddressResult(sourceList[i]));
                return resultList.ToArray();
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("{0} => geocoding.GLAddressByGeo({1}, {2}, {3}), ex: {4}", HttpContext.Current.Request.UserHostAddress, lngStr, latStr, keyStr, ex.ToString()));
                return null;
            }
        }

        [WebMethod(), SoapHeader("AuthHeader")]
        public PBLRegionResult[] RegionByGeo(string lngStr, string latStr, string lanStr)
        {
            try
            {
                // 1. Kiểm tra đầu vào
                // 1.1 Trạng thái khởi tạo dịch vụ
                if (MainProcessing.InitData() == false)
                    return null;
                // 1.2 Thông tin xác thực
                else if (MainProcessing.CheckAuthen(AuthHeader, 1) == false)
                {
                    LogFile.WriteError(string.Format("{0} => Authenticate invalid !!!", HttpContext.Current.Request.UserHostAddress));
                    return null;
                }
                // 1.3 Dữ liệu đầu vào
                else if (lngStr.Trim().Length == 0 || latStr.Trim().Length == 0)
                {
                    LogFile.WriteError(string.Format("{0}: {1} - {2} => data error", HttpContext.Current.Request.UserHostAddress, lngStr, latStr));
                    return null;
                }

                // 2. Tìm kiếm và trả về kết quả
                List<PBLRegionResult> resultList = MainProcessing.RegionByGeo(lngStr, latStr, lanStr);
                if (resultList != null && resultList.Count > 0)
                    return resultList.ToArray();
                else
                    return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("{0} => geocoding.RegionByGeo({1}, {2}, {3}), ex: {4}", HttpContext.Current.Request.UserHostAddress, lngStr, latStr, lanStr, ex.ToString()));
                return null;
            }
        }

        [WebMethod(), SoapHeader("AuthHeader")]
        public PBLAddressResult[] AddressByGeo(string lngStr, string latStr, string lanStr)
        {
            try
            {
                // 1. Kiểm tra đầu vào
                UTLAuthen authenInfo = new UTLAuthen() { QuotaBA = -1, QuotaGG = 0 };
                // 1.1 Trạng thái khởi tạo dịch vụ
                if (MainProcessing.InitData() == false)
                    return null;
                // 1.2 Thông tin xác thực
                else if (MainProcessing.CheckAuthen(AuthHeader, 0, ref authenInfo) == false)
                {
                    LogFile.WriteError(string.Format("{0} => Authenticate invalid !!!", HttpContext.Current.Request.UserHostAddress));
                    return null;
                }
                // 1.3 Dữ liệu đầu vào
                else if (lngStr.Trim().Length == 0 || latStr.Trim().Length == 0)
                {
                    LogFile.WriteError(string.Format("{0}: {1} - {2} => data error", HttpContext.Current.Request.UserHostAddress, lngStr, latStr));
                    return null;
                }

                // 2. Tìm kiếm và trả về kết quả
                List<PBLAddressResult> resultList = new List<PBLAddressResult>();
                List<RPBLAddressResult> sourceList = MainProcessing.AddressByGeo(authenInfo, lngStr, latStr, lanStr);
                if (sourceList != null && sourceList.Count > 0)
                {
                    sourceList.ForEach(item => resultList.Add(new PBLAddressResult(item)));
                    return resultList.ToArray();
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("{0} => geocoding.AddressByGeo({1}, {2}, {3}), ex: {4}", HttpContext.Current.Request.UserHostAddress, lngStr, latStr, lanStr, ex.ToString()));
                return null;
            }
        }

        [WebMethod(), SoapHeader("AuthHeader")]
        public PBLAddressResult GeoByAddress(string keyStr, string lanStr)
        {
            try
            {
                // 1. Kiểm tra đầu vào
                // 1.1 Trạng thái khởi tạo dịch vụ
                if (MainProcessing.InitData() == false)
                    return null;
                // 1.2 Thông tin xác thực
                else if (MainProcessing.CheckAuthen(AuthHeader) == false)
                {
                    LogFile.WriteError(string.Format("{0} => Authenticate invalid !!!", HttpContext.Current.Request.UserHostAddress));
                    return null;
                }
                // 1.3 Dữ liệu đầu vào
                else if (keyStr == null || keyStr.Length < 3)
                {
                    LogFile.WriteError(string.Format("{0}: {1} - {2} => data error", HttpContext.Current.Request.UserHostAddress, keyStr, lanStr));
                    return null;
                }

                // 2. Tìm kiếm và trả về kết quả
                RPBLAddressResult result = MainProcessing.GeoByAddress(keyStr, lanStr);
                if (result != null)
                    return new PBLAddressResult(result);
                else
                    return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("{0} => geocoding.GeoByAddress({1}, {2}), ex: {4}", HttpContext.Current.Request.UserHostAddress, keyStr, lanStr, ex.ToString()));
                return null;
            }
        }


        [WebMethod(), SoapHeader("AuthHeader")]
        public FPBLAddressResult[] FAddressByGeo(string lngStr, string latStr, string lanStr)
        {
            try
            {
                // 1. Kiểm tra đầu vào
                UTLAuthen authenInfo = new UTLAuthen() { QuotaBA = -1, QuotaGG = 0 };
                // 1.1 Trạng thái khởi tạo dịch vụ
                if (MainProcessing.InitData() == false)
                    return null;
                // 1.2 Thông tin xác thực
                else if (MainProcessing.CheckAuthen(AuthHeader, 1, ref authenInfo) == false)
                {
                    LogFile.WriteError(string.Format("{0} => Authenticate invalid !!!", HttpContext.Current.Request.UserHostAddress));
                    return null;
                }
                // 1.3 Dữ liệu đầu vào
                else if (lngStr.Trim().Length == 0 || latStr.Trim().Length == 0)
                {
                    LogFile.WriteError(string.Format("{0}: {1} - {2} => data error", HttpContext.Current.Request.UserHostAddress, lngStr, latStr));
                    return null;
                }

                // 2. Tìm kiếm và trả về kết quả
                List<RPBLAddressResult> sourceList = MainProcessing.AddressByGeo(authenInfo, lngStr, latStr, lanStr);
                if (sourceList == null || sourceList.Count == 0)
                    return null;
                List<FPBLAddressResult> resultList = new List<FPBLAddressResult>();
                for (int i = 0; i < sourceList.Count; i++)
                    resultList.Add(new FPBLAddressResult(sourceList[i]));
                return resultList.ToArray();
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("{0} => geocoding.FAddressByGeo({1}, {2}, {3}), ex: {4}", HttpContext.Current.Request.UserHostAddress, lngStr, latStr, lanStr, ex.ToString()));
                return null;
            }
        }

        [WebMethod(), SoapHeader("AuthHeader")]
        public FPBLAddressResult FGeoByAddress(string keyStr, string lanStr)
        {
            try
            {
                // 1. Kiểm tra đầu vào
                // 1.1 Trạng thái khởi tạo dịch vụ
                if (MainProcessing.InitData() == false)
                    return null;
                // 1.2 Thông tin xác thực
                else if (MainProcessing.CheckAuthen(AuthHeader, 1) == false)
                {
                    LogFile.WriteError(string.Format("{0} => Authenticate invalid !!!", HttpContext.Current.Request.UserHostAddress));
                    return null;
                }
                // 1.3 Dữ liệu đầu vào
                else if (keyStr == null || keyStr.Length < 3)
                {
                    LogFile.WriteError(string.Format("{0}: {1} - {2} => data error", HttpContext.Current.Request.UserHostAddress, keyStr, lanStr));
                    return null;
                }

                // 2. Tìm kiếm và trả về kết quả
                //LogFile.WriteData(keyStr);
                return new FPBLAddressResult(MainProcessing.GeoByAddress(keyStr, lanStr));
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("{0} => geocoding.GeoByAddress({1}, {2}), ex: {4}", HttpContext.Current.Request.UserHostAddress, keyStr, lanStr, ex.ToString()));
                return null;
            }
        }
        
        [WebMethod(), SoapHeader("AuthHeader")]
        public SPBLAddressResult[] SAddressByGeo(string lngStr, string latStr, string lanStr)
        {
            try
            {
                // 1. Kiểm tra đầu vào
                UTLAuthen authenInfo = new UTLAuthen() { QuotaBA = -1, QuotaGG = 0 };
                // 1.1 Trạng thái khởi tạo dịch vụ
                if (MainProcessing.InitData() == false)
                    return null;
                // 1.2 Thông tin xác thực
                else if (MainProcessing.CheckAuthen(AuthHeader, 0, ref authenInfo) == false)
                {
                    LogFile.WriteError(string.Format("{0} => Authenticate invalid !!!", HttpContext.Current.Request.UserHostAddress));
                    return null;
                }
                // 1.3 Dữ liệu đầu vào
                else if (lngStr.Trim().Length == 0 || latStr.Trim().Length == 0)
                {
                    LogFile.WriteError(string.Format("{0}: {1} - {2} => data error", HttpContext.Current.Request.UserHostAddress, lngStr, latStr));
                    return null;
                }

                // 2. Tìm kiếm và trả về kết quả
                List<SPBLAddressResult> resultList = new List<SPBLAddressResult>();
                List<RPBLAddressResult> sourceList = MainProcessing.AddressByGeo(authenInfo, lngStr, latStr, lanStr);
                if (sourceList != null && sourceList.Count > 0)
                {
                    sourceList.ForEach(item => resultList.Add(new SPBLAddressResult(item)));
                    return resultList.ToArray();
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("{0} => geocoding.SAddressByGeo({1}, {2}, {3}), ex: {4}", HttpContext.Current.Request.UserHostAddress, lngStr, latStr, lanStr, ex.ToString()));
                return null;
            }
        }

        [WebMethod(), SoapHeader("AuthHeader")]
        public SPBLAddressResult SGeoByAddress(string keyStr, string lanStr)
        {
            try
            {
                // 1. Kiểm tra đầu vào
                // 1.1 Trạng thái khởi tạo dịch vụ
                if (MainProcessing.InitData() == false)
                    return null;
                // 1.2 Thông tin xác thực
                else if (MainProcessing.CheckAuthen(AuthHeader, 1) == false)
                {
                    LogFile.WriteError(string.Format("{0} => Authenticate invalid !!!", HttpContext.Current.Request.UserHostAddress));
                    return null;
                }
                // 1.3 Dữ liệu đầu vào
                else if (keyStr == null || keyStr.Length < 3)
                {
                    LogFile.WriteError(string.Format("{0}: {1} - {2} => data error", HttpContext.Current.Request.UserHostAddress, keyStr, lanStr));
                    return null;
                }

                // 2. Tìm kiếm và trả về kết quả
                return new SPBLAddressResult(MainProcessing.GeoByAddress(keyStr, lanStr));
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("{0} => geocoding.SGeoByAddress({1}, {2}), ex: {4}", HttpContext.Current.Request.UserHostAddress, keyStr, lanStr, ex.ToString()));
                return null;
            }
        }
    }
}