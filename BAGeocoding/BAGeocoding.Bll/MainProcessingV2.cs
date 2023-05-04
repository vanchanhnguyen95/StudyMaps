using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

using BAGeocoding.Bll.MapObj;

using BAGeocoding.Entity.DataService;
using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.Public;
using BAGeocoding.Entity.Router;
using BAGeocoding.Entity.Utility;

using BAGeocoding.Utility;

using RTree.Engine.Entity;
using BAGeocoding.Entity.Enum.Route;
//using BAGeocoding.Dal.MapRoute;

namespace BAGeocoding.Bll
{
    public class MainProcessingV2
    {
        /// <summary>
        /// Khởi tạo dữ liệu
        /// </summary>
        public static async Task<bool> InitData()
        {
            try
            {
                if (RunningParams.ProcessState == EnumProcessState.Success)
                    return true;
                else if (RunningParams.ProcessState == EnumProcessState.Processing)
                    return false;
                else if (RunningParams.ProcessState == EnumProcessState.Error)
                    return false;
                RunningParams.ProcessState = EnumProcessState.Processing;
               
                BackgroundWorker bg = new BackgroundWorker();
                bg.DoWork += new DoWorkEventHandler(InitData_DoWork);
                bg.RunWorkerAsync();
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("MainProcessing.InitData, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Tiến trình ngầm khởi tạo dữ liệu
        /// </summary>
        private static void InitData_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BAGEncoding.BackupLogs();
                LogFile.WriteError("Application init start !!!");
                //// Dữ liệu cấu hình
                //if (RouterManager.LoadConfig() == false)
                //    RunningParams.ProcessState = EnumProcessState.Error;
                // Tải dữ liệu vùng
                if (RegionManager.LoadData() == false)
                    RunningParams.ProcessState = EnumProcessState.Error;
                // Tải dữ liệu khu đô thị
                else if (PlaceManager.LoadData() == false)
                    RunningParams.ProcessState = EnumProcessState.Error;
                // Tải dữ liệu từ khóa tìm kiếm
                else if (KeySearchManager.LoadData() == false)
                    RunningParams.ProcessState = EnumProcessState.Error;
                // Tải dữ liệu địa điểm
                else if (PointManager.LoadData() == false)
                    RunningParams.ProcessState = EnumProcessState.Error;
                // Tải dữ liệu đường
                else if (SegmentManager.LoadData() == false)
                    RunningParams.ProcessState = EnumProcessState.Error;
                else
                    RunningParams.ProcessState = EnumProcessState.Success;

                // Logs khởi tạo dữ liệu
                if(RunningParams.ProcessState == EnumProcessState.Success)
                    LogFile.WriteError("Application init success !!!");
                else
                    LogFile.WriteError("Application init error !!!");
            }
            catch (Exception ex)
            {
                LogFile.WriteError("MainProcessing.InitData_DoWork, ex: " + ex.ToString());
                RunningParams.ProcessState = EnumProcessState.None;
            }       
        }
        
        /// <summary>
        /// Khởi tạo dữ liệu
        /// </summary>
        public static bool InitRoute()
        {
            try
            {
                if (RunningParams.ProcessState == EnumProcessState.Success)
                    return true;
                else if (RunningParams.ProcessState == EnumProcessState.Processing)
                    return false;
                else if (RunningParams.ProcessState == EnumProcessState.Error)
                    return false;
                RunningParams.ProcessState = EnumProcessState.Processing;
                BackgroundWorker bgr = new BackgroundWorker();
                bgr.DoWork += new DoWorkEventHandler(InitRoute_DoWork);
                bgr.RunWorkerAsync();
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("MainProcessing.InitData, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Tiến trình ngầm khởi tạo dữ liệu
        /// </summary>
        private static void InitRoute_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Backup các file logs hiện tại
                RouterManager.BackupLogs();
                LogFile.WriteError("Application init start !!!");
                // Dữ liệu cấu hình
                if (RouterManager.LoadConfig() == false)
                    RunningParams.ProcessState = EnumProcessState.Error;
                // Tải dữ liệu tìm đường
                else if (RouterManager.LoadData() == false)
                    RunningParams.ProcessState = EnumProcessState.Error;
                // Khởi tạo ghi logs tìm đường
                else if (BAGRouting.RLogsInit() == false)
                    RunningParams.ProcessState = EnumProcessState.Error;
                // Trả về kết quả
                else
                    RunningParams.ProcessState = EnumProcessState.Success;

                // Logs khởi tạo dữ liệu
                if (RunningParams.ProcessState == EnumProcessState.Success)
                    LogFile.WriteError("Application init success !!!");
                else
                    LogFile.WriteError("Application init error !!!");
            }
            catch (Exception ex)
            {
                LogFile.WriteError("MainProcessing.InitRoute_DoWork, ex: " + ex.ToString());
                RunningParams.ProcessState = EnumProcessState.None;
            }
        }

        ///// <summary>
        ///// Kiểm tra xác thực
        ///// </summary>
        //public static bool CheckAuthen(UTLAuthHeader authHeader, int dataExt = 0)
        //{
        //    if (RunningParams.CheckAuthen == false)
        //        return true;
        //    else if (authHeader == null)
        //        return false;
        //    else if (RunningParams.RouterData.Authen.Count == 0)
        //        return false;
        //    else if (RunningParams.RouterData.Authen.ContainsKey(authHeader.Username) == false)
        //        return false;
        //    UTLAuthen authenInfo = (UTLAuthen)RunningParams.RouterData.Authen[authHeader.Username];
        //    if (authenInfo.Password.Equals(authHeader.Password) == false)
        //        return false;
        //    else if (dataExt > 0)
        //        return ((authenInfo.DataExt & dataExt) > 0);
        //    else
        //        return true;
        //}

        ///// <summary>
        ///// Kiểm tra xác thực
        ///// </summary>
        //public static bool CheckAuthen(UTLAuthHeader authHeader, int dataExt, ref UTLAuthen authenInfo)
        //{
        //    if (RunningParams.CheckAuthen == false)
        //        return true;
        //    else if (authHeader == null)
        //        return false;
        //    else if (RunningParams.RouterData.Authen.Count == 0)
        //        return false;
        //    else if (RunningParams.RouterData.Authen.ContainsKey(authHeader.Username) == false)
        //        return false;
        //    authenInfo = (UTLAuthen)RunningParams.RouterData.Authen[authHeader.Username];
        //    if (authenInfo.Password.Equals(authHeader.Password) == false)
        //        return false;
        //    else if (dataExt > 0)
        //        return ((authenInfo.DataExt & dataExt) > 0);
        //    else
        //        return true;
        //}

        /// <summary>
        /// Kiểm tra xác thực
        /// </summary>
        public static bool CheckRegister(BARRegister requestInfo, ref BARRouteLogs logsdata)
        {
            try
            {
                if (RunningParams.CheckRegister == false)
                    return true;
                else if (RunningParams.RouterData.Register.Count == 0)
                    return false;
                else if (RunningParams.RouterData.Register.ContainsKey(requestInfo.KeyStr) == false)
                    return false;
                BARRegister registerInfo = (BARRegister)RunningParams.RouterData.Register[requestInfo.KeyStr];
                if (registerInfo.IPAddress.ContainsKey(requestInfo.IPStr) == false)
                    return false;
                logsdata.RegisterID = Convert.ToInt16(registerInfo.IPAddress[requestInfo.IPStr]);
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Copy file dữ liệu
        /// </summary>
        public static bool FileCopy(string pathStr, string fileName, ref string result)
        {
            try
            {
                result = string.Format(@"{0}Temp", pathStr);
                if (Directory.Exists(result) == false)
                    Directory.CreateDirectory(result);
                result = string.Format(@"{0}\{1}_{2}.ba", result, fileName.Substring(0, fileName.LastIndexOf(".")), DateTime.Now.Ticks);
                if (File.Exists(result) == true)
                    File.Delete(result);
                File.Copy(string.Format(@"{0}{1}", pathStr, fileName), result, true);
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("MainProcessing.FileCopy({0}, {1}), ex: {2}", pathStr, fileName, ex.ToString()));
                return false;
            }
        }
        
        /// <summary>
        /// Tìm kiếm địa chỉ theo tọa độ
        /// </summary>
        public static List<RPBLAddressResult> GLAddressByGeo(string lngStr, string latStr, string keyStr, string lanStr)
        {
            try
            {
                short provinceID = 0;
                EnumBAGLanguage language = (lanStr == "vn") ? EnumBAGLanguage.Vn : EnumBAGLanguage.En;
                string[] lngList = lngStr.Split(Constants.DEFAULT_SPLIT_DATA);
                string[] latList = latStr.Split(Constants.DEFAULT_SPLIT_DATA);

                double its = Constants.DISTANCE_INTERSECT_ROAD;
                List<RPBLAddressResult> resultList = new List<RPBLAddressResult>();
                for (int i = 0; i < lngList.Length; i++)
                {
                    BAGPoint pts = new BAGPoint(lngList[i], latList[i]);
                    if (pts.IsValid() == false)
                    {
                        resultList.Add(new RPBLAddressResult() { });
                        continue;
                    }
                    RTRectangle rec = new RTRectangle(pts.Lng - its, pts.Lat - its, pts.Lng + its, pts.Lat + its, 0.0f, 0.0f);
                    RPBLAddressResult resultItem = BAGEncoding.RegionByGeo(rec, pts, language, ref provinceID);
                    if (resultItem != null)
                    {
                        #region ==================== Tìm kiếm theo dữ liệu của BinhAnh ====================
                        // Ưu tiên tìm khu đô thị
                        PBLAddressResult placeItem = BAGEncoding.PlaceByGeo(rec, pts, language);
                        if (placeItem != null)
                            resultItem.Road = placeItem.Road;
                        // Nếu không tìm theo đường
                        else if (RunningParams.ProvinceData.Segm.ContainsKey(provinceID) == true)
                        {
                            DTSSegment segmentDT = (DTSSegment)RunningParams.ProvinceData.Segm[provinceID];
                            RPBLAddressResult temp = BAGEncoding.RoadByGeo(segmentDT.KDTree, segmentDT.RTree, rec, pts, its, language);
                            if (temp != null)
                            {
                                resultItem.Building = temp.Building;
                                resultItem.Road = temp.Road;
                            }
                            else if (lngList.Length == 1)
                            {
                                for (int step = 2; step < 4; step++)
                                {
                                    its = Constants.DISTANCE_INTERSECT_ROAD * step;
                                    rec = new RTRectangle(pts.Lng - its, pts.Lat - its, pts.Lng + its, pts.Lat + its, 0.0f, 0.0f);
                                    temp = BAGEncoding.RoadByGeo(segmentDT.KDTree, segmentDT.RTree, rec, pts, its, language);
                                    if (temp != null)
                                    {
                                        resultItem.Building = temp.Building;
                                        resultItem.Road = temp.Road;
                                        break;
                                    }
                                }
                            }
                        }
                        resultList.Add(resultItem);
                        #endregion
                    }
                    else
                    {
                        #region ==================== Tìm kiếm theo dữ liệu của Google ====================
                        resultItem = GGLEndcoding.AddressByGeo(pts, keyStr, language);
                        if (resultItem != null)
                            resultList.Add(resultItem);
                        else
                            resultList.Add(new RPBLAddressResult { });
                        #endregion
                    }
                }
                return resultList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("MainProcessing.GLAddressByGeo({0}, {1}, {2}), ex: {3}", lngStr, latStr, lanStr, ex.ToString()));
                return null;
            }
        }


        /// <summary>
        /// Tìm kiếm địa chỉ theo tọa độ
        /// </summary>
        public static List<PBLRegionResult> RegionByGeo(string lngStr, string latStr, string lanStr)
        {
            try
            {
                short provinceID = 0;
                EnumBAGLanguage language = (lanStr == "vn") ? EnumBAGLanguage.Vn : EnumBAGLanguage.En;
                string[] lngList = lngStr.Split(Constants.DEFAULT_SPLIT_DATA);
                string[] latList = latStr.Split(Constants.DEFAULT_SPLIT_DATA);

                #region ==================== Tiến hành xử lý dịch vụ ====================
                double its = Constants.DISTANCE_INTERSECT_ROAD;
                List<PBLRegionResult> resultList = new List<PBLRegionResult>();
                for (int i = 0; i < lngList.Length; i++)
                {
                    BAGPoint pts = new BAGPoint(lngList[i], latList[i]);
                    if (pts.IsValid() == false)
                    {
                        resultList.Add(new PBLRegionResult { });
                        continue;
                    }
                    RTRectangle rec = new RTRectangle(pts.Lng - its, pts.Lat - its, pts.Lng + its, pts.Lat + its, 0.0f, 0.0f);
                    RPBLAddressResult resultItem = BAGEncoding.RegionByGeo(rec, pts, language, ref provinceID);
                    if (resultItem != null)
                        resultList.Add(new PBLRegionResult(resultItem));
                    else
                        resultList.Add(new PBLRegionResult { });
                }
                return resultList;
                #endregion
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("MainProcessing.RegionByGeo({0}, {1}, {2}), ex: {3}", lngStr, latStr, lanStr, ex.ToString()));
                return null;
            }
        }

        /// <summary>
        /// Tìm kiếm địa chỉ theo tọa độ
        /// </summary>
        public static List<RPBLAddressResult> AddressByGeo(string lngStr, string latStr)
        {
            try
            {
                short provinceID = 0;
                EnumBAGLanguage language = EnumBAGLanguage.Vn;
                string[] lngList = lngStr.Split(Constants.DEFAULT_SPLIT_DATA);
                string[] latList = latStr.Split(Constants.DEFAULT_SPLIT_DATA);


                #region ==================== Tiến hành xử lý dịch vụ ====================
                double its = Constants.DISTANCE_INTERSECT_ROAD;
                List<RPBLAddressResult> resultList = new List<RPBLAddressResult>();
                for (int i = 0; i < lngList.Length; i++)
                {
                    BAGPoint pts = new BAGPoint(lngList[i], latList[i]);
                    if (pts.IsValid() == false)
                    {
                        resultList.Add(new RPBLAddressResult { });
                        continue;
                    }
                    RTRectangle rec = new RTRectangle(pts.Lng - its, pts.Lat - its, pts.Lng + its, pts.Lat + its, 0.0f, 0.0f);
                    RPBLAddressResult resultItem = BAGEncoding.RegionByGeo(rec, pts, language, ref provinceID);
                    if (resultItem != null)
                    {
                        #region ==================== Tìm kiếm theo dữ liệu của BinhAnh ====================
                            // Ưu tiên tìm khu đô thị
                            PBLAddressResult placeItem = BAGEncoding.PlaceByGeo(rec, pts, language);
                            if (placeItem != null)
                                resultItem.Road = placeItem.Road;
                            // Nếu không tìm theo đường
                            else if (RunningParams.ProvinceData.Segm.ContainsKey(provinceID) == true)
                            {
                                DTSSegment segmentDT = (DTSSegment)RunningParams.ProvinceData.Segm[provinceID];
                                RPBLAddressResult temp = BAGEncoding.RoadByGeo(segmentDT.KDTree, segmentDT.RTree, rec, pts, its, language);
                                if (temp != null)
                                {
                                    resultItem.Building = temp.Building;
                                    resultItem.Road = temp.Road;
                                    resultItem.MinSpeed = temp.MinSpeed;
                                    resultItem.MaxSpeed = temp.MaxSpeed;
                                    resultItem.DataExt = temp.DataExt;
                                }
                                else// if (lngList.Length == 1) // ANHPT: Luông tìm kiếm để chính xác nhất
                                {
                                    for (int step = 2; step < 4; step++)
                                    {
                                        its = Constants.DISTANCE_INTERSECT_ROAD * step;
                                        rec = new RTRectangle(pts.Lng - its, pts.Lat - its, pts.Lng + its, pts.Lat + its, 0.0f, 0.0f);
                                        temp = BAGEncoding.RoadByGeo(segmentDT.KDTree, segmentDT.RTree, rec, pts, its, language);
                                        if (temp != null)
                                        {
                                            resultItem.Building = temp.Building;
                                            resultItem.Road = temp.Road;
                                            resultItem.MinSpeed = temp.MinSpeed;
                                            resultItem.MaxSpeed = temp.MaxSpeed;
                                            resultItem.DataExt = temp.DataExt;
                                            break;
                                        }
                                    }
                                }
                            }
                            resultList.Add(resultItem);
                        #endregion
                    }
                    else
                    {
                        #region ==================== Tìm kiếm theo dữ liệu của Google ====================
                        resultItem = GGLEndcoding.AddressByGeo(pts, string.Empty, language);
                        if (resultItem != null)
                            resultList.Add(resultItem);
                        else
                            resultList.Add(new RPBLAddressResult { });
                        #endregion
                    }
                }
                #endregion
                return resultList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("MainProcessing.AddressByGeo({0}, {1}), ex: {2}", lngStr, latStr, ex.ToString()));
                return null;
            }
        }


        ///// <summary>
        ///// Tìm kiếm địa chỉ theo tọa độ
        ///// </summary>
        //public static List<RPBLAddressResult> AddressByGeo(UTLAuthen authenInfo, string lngStr, string latStr, string lanStr)
        //{
        //    try
        //    {
        //        short provinceID = 0;
        //        EnumBAGLanguage language = (lanStr == "vn") ? EnumBAGLanguage.Vn : EnumBAGLanguage.En;
        //        string[] lngList = lngStr.Split(Constants.DEFAULT_SPLIT_DATA);
        //        string[] latList = latStr.Split(Constants.DEFAULT_SPLIT_DATA);
                
        //        #region ==================== Lấy dữ liệu lưu lượng sử dụng ====================
        //        DTSTraffic trafficInfo = new DTSTraffic(authenInfo.AuthenID);
        //        if (RunningParams.RouterData.Traffic.ContainsKey(authenInfo.AuthenID) == true)
        //            trafficInfo = (DTSTraffic)RunningParams.RouterData.Traffic[authenInfo.AuthenID];
        //        if (trafficInfo.IsCurrent() == false)
        //        {
        //            DTSTrafficDAO.Add(new DTSTraffic(trafficInfo));
        //            trafficInfo = new DTSTraffic(authenInfo.AuthenID);
        //        }
        //        #endregion

        //        #region ==================== Tiến hành xử lý dịch vụ ====================
        //        double its = Constants.DISTANCE_INTERSECT_ROAD;
        //        List<RPBLAddressResult> resultList = new List<RPBLAddressResult>();
        //        for (int i = 0; i < lngList.Length; i++)
        //        {
        //            BAGPoint pts = new BAGPoint(lngList[i], latList[i]);
        //            if (pts.IsValid() == false)
        //            {
        //                resultList.Add(new RPBLAddressResult { });
        //                continue;
        //            }
        //            RTRectangle rec = new RTRectangle(pts.Lng - its, pts.Lat - its, pts.Lng + its, pts.Lat + its, 0.0f, 0.0f);
        //            RPBLAddressResult resultItem = BAGEncoding.RegionByGeo(rec, pts, language, ref provinceID);
        //            if (resultItem != null)
        //            {
        //                #region ==================== Tìm kiếm theo dữ liệu của BinhAnh ====================
        //                trafficInfo.TrafficBA++;
        //                //LogFile.WriteProcess(string.Format("QuotaBA: {0}", authenInfo.QuotaBA));
        //                if (authenInfo.QuotaBA > -1 && trafficInfo.TrafficBA > authenInfo.QuotaBA)
        //                    resultList.Add(new RPBLAddressResult { });
        //                else
        //                {
        //                    // Ưu tiên tìm khu đô thị
        //                    PBLAddressResult placeItem = BAGEncoding.PlaceByGeo(rec, pts, language);
        //                    if (placeItem != null)
        //                        resultItem.Road = placeItem.Road;
        //                    // Nếu không tìm theo đường
        //                    else if (RunningParams.ProvinceData.Segm.ContainsKey(provinceID) == true)
        //                    {
        //                        DTSSegment segmentDT = (DTSSegment)RunningParams.ProvinceData.Segm[provinceID];
        //                        RPBLAddressResult temp = BAGEncoding.RoadByGeo(segmentDT.KDTree, segmentDT.RTree, rec, pts, its, language);
        //                        if (temp != null)
        //                        {
        //                            resultItem.Building = temp.Building;
        //                            resultItem.Road = temp.Road;
        //                            resultItem.MinSpeed = temp.MinSpeed;
        //                            resultItem.MaxSpeed = temp.MaxSpeed;
        //                            resultItem.DataExt = temp.DataExt;
        //                        }
        //                        else// if (lngList.Length == 1) // ANHPT: Luông tìm kiếm để chính xác nhất
        //                        {
        //                            for (int step = 2; step < 4; step++)
        //                            {
        //                                its = Constants.DISTANCE_INTERSECT_ROAD * step;
        //                                rec = new RTRectangle(pts.Lng - its, pts.Lat - its, pts.Lng + its, pts.Lat + its, 0.0f, 0.0f);
        //                                temp = BAGEncoding.RoadByGeo(segmentDT.KDTree, segmentDT.RTree, rec, pts, its, language);
        //                                if (temp != null)
        //                                {
        //                                    resultItem.Building = temp.Building;
        //                                    resultItem.Road = temp.Road;
        //                                    resultItem.MinSpeed = temp.MinSpeed;
        //                                    resultItem.MaxSpeed = temp.MaxSpeed;
        //                                    resultItem.DataExt = temp.DataExt;
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    resultList.Add(resultItem);
        //                }
        //                #endregion
        //            }
        //            else
        //            {
        //                #region ==================== Tìm kiếm theo dữ liệu của Google ====================
        //                trafficInfo.TrafficGG++;
        //                if (authenInfo.QuotaGG > -1 && trafficInfo.TrafficGG > authenInfo.QuotaGG)
        //                    resultList.Add(new RPBLAddressResult { });
        //                else
        //                {
        //                    resultItem = GGLEndcoding.AddressByGeo(pts, string.Empty, language);
        //                    if (resultItem != null)
        //                        resultList.Add(resultItem);
        //                    else
        //                        resultList.Add(new RPBLAddressResult { });
        //                }
        //                #endregion
        //            }
        //        }
        //        #endregion

        //        #region ==================== Xử lý lưu lượng và kết quả ====================
        //        // Gán lại thông tin quota
        //        if (RunningParams.RouterData.Traffic.ContainsKey(trafficInfo.AuthenID) == true)
        //            RunningParams.RouterData.Traffic[trafficInfo.AuthenID] = trafficInfo;
        //        else
        //            RunningParams.RouterData.Traffic.Add(trafficInfo.AuthenID, trafficInfo);

        //        // Trả về kết quả
        //        return resultList;                
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        LogFile.WriteError(string.Format("MainProcessing.AddressByGeo({0}, {1}, {2}), ex: {3}", lngStr, latStr, lanStr, ex.ToString()));
        //        return null;
        //    }
        //}

        /// <summary>
        /// Tìm kiếm tọa độ theo địa chỉ
        /// </summary>
        public static RPBLAddressResult GeoByAddress(string keyStr, string lanStr)
        {
            try
            {
                // 1. Xây dựng từ khóa tìm kiếm
                BAGSearchKey keySearch = new BAGSearchKey(2, keyStr.Trim(), LatinToAscii.Latin2Ascii(keyStr.Trim().ToLower()));
                if (keySearch.IsValid == false)
                    return null;
                else if (keySearch.IsSpecial == true)
                    keySearch.IsSpecial = RunningParams.RoadSpecial.ContainsKey(keySearch.Road);

                // 2. Tìm kiếm vùng
                // 2.1 Tìm kiếm tỉnh
                short provinceID = BAGDecoding.SearchProvinceByName(keySearch.Province);
                if (provinceID < 0)
                    return null;
                // 2.2 Tìm kiếm quận/huyện
                short districtID = BAGDecoding.SearchDistrictByName(keySearch.District, (byte)provinceID);

                // 3. Tìm kiếm đường
                // 3.1 Lấy các thông tin
                DTSSegment gsSegment = (DTSSegment)RunningParams.ProvinceData.Segm[provinceID];
                EnumBAGLanguage language = (lanStr == "vn") ? EnumBAGLanguage.Vn : EnumBAGLanguage.En;
                // 3.2 Trả về kết quả
                //if (provinceID == 15 || provinceID == 16 || provinceID == 17 || provinceID == 19 || provinceID == 26)
                if(RunningParams.HTProvincePriority.ContainsKey(provinceID) == true)
                    return GeoByAddressHaNoi(keySearch, gsSegment, districtID, language);
                else
                    return GeoByAddress(keySearch, gsSegment, districtID, language);
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("MainProcessing.GeoByAddress({0}, {1}), ex: {2}", keyStr, lanStr, ex.ToString()));
                return null;
            }
        }

        /// <summary>
        /// Tìm kiếm tọa độ theo địa chỉ V2
        /// </summary>
        public static RPBLAddressResultV2 GeoByAddressV2(string keyStr, string lanStr)
        {
            try
            {
                // 1. Xây dựng từ khóa tìm kiếm
                BAGSearchKey keySearch = new BAGSearchKey(2, keyStr.Trim(), LatinToAscii.Latin2Ascii(keyStr.Trim().ToLower()));
                if (keySearch.IsValid == false)
                    return null;
                else if (keySearch.IsSpecial == true)
                    keySearch.IsSpecial = RunningParams.RoadSpecial.ContainsKey(keySearch.Road);

                // 2. Tìm kiếm vùng
                // 2.1 Tìm kiếm tỉnh
                short provinceID = BAGDecoding.SearchProvinceByName(keySearch.Province);
                if (provinceID < 0)
                    return null;
                // 2.2 Tìm kiếm quận/huyện
                short districtID = BAGDecoding.SearchDistrictByName(keySearch.District, (byte)provinceID);

                // 3. Tìm kiếm đường
                // 3.1 Lấy các thông tin
                DTSSegment gsSegment = (DTSSegment)RunningParams.ProvinceData.Segm[provinceID];
                EnumBAGLanguage language = (lanStr == "vn") ? EnumBAGLanguage.Vn : EnumBAGLanguage.En;
                // 3.2 Trả về kết quả
                //if (provinceID == 15 || provinceID == 16 || provinceID == 17 || provinceID == 19 || provinceID == 26)
                if (RunningParams.HTProvincePriority.ContainsKey(provinceID) == true)
                    return GeoByAddressHaNoiV2(keySearch, gsSegment, districtID, language);
                else
                    return GeoByAddressV2(keySearch, gsSegment, districtID, language);
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("MainProcessing.GeoByAddress({0}, {1}), ex: {2}", keyStr, lanStr, ex.ToString()));
                return null;
            }
        }

        /// <summary>
        /// Tìm kiếm tọa độ qua địa chỉ
        /// </summary>
        private static RPBLAddressResult GeoByAddress(BAGSearchKey keySearch, DTSSegment gsSegment, short districtID, EnumBAGLanguage language)
        {
            try
            {
                // 1. Tìm kiếm đường
                // 1.1 Tìm kiếm đường ưu tiên ở Hà Nội cũ
                RPBLAddressResult resultRoad = BAGDecoding.SearchRoadByName(gsSegment, keySearch, districtID);
                if (resultRoad == null)
                    return null;

                // 2. Tiến hành tìm kiếm vùng
                // 2.1 Tìm kiếm thông tin vùng
                RTRectangle rec = new RTRectangle(resultRoad.Lng - Constants.DISTANCE_INTERSECT_ROAD, resultRoad.Lat - Constants.DISTANCE_INTERSECT_ROAD, resultRoad.Lng + Constants.DISTANCE_INTERSECT_ROAD, resultRoad.Lat + Constants.DISTANCE_INTERSECT_ROAD, 0.0f, 0.0f);
                RPBLAddressResult resultRegion = BAGEncoding.RegionByGeo(rec, new BAGPoint(resultRoad.Lng, resultRoad.Lat), language);
                if (resultRegion == null)
                    return null;
                // 2.2 Bổ sung thông tin và trả về kết quả
                resultRegion.Lng = resultRoad.Lng;
                resultRegion.Lat = resultRoad.Lat;
                resultRegion.Building = resultRoad.Building;
                resultRegion.Road = resultRoad.Road;
                return resultRegion;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("MainProcessing.GeoByAddress, ex: {0}", ex.ToString()));
                return null;
            }
        }

        /// <summary>
        /// Tìm kiếm tọa độ qua địa chỉ V2
        /// </summary>
        private static RPBLAddressResultV2 GeoByAddressV2(BAGSearchKey keySearch, DTSSegment gsSegment, short districtID, EnumBAGLanguage language)
        {
            try
            {
                // 1. Tìm kiếm đường
                // 1.1 Tìm kiếm đường ưu tiên ở Hà Nội cũ
                RPBLAddressResultV2 resultRoad = BAGDecoding.SearchRoadByNameV2(gsSegment, keySearch, districtID);
                if (resultRoad == null)
                    return null;

                // 2. Tiến hành tìm kiếm vùng
                // 2.1 Tìm kiếm thông tin vùng
                RTRectangle rec = new RTRectangle(resultRoad.Lng - Constants.DISTANCE_INTERSECT_ROAD, resultRoad.Lat - Constants.DISTANCE_INTERSECT_ROAD, resultRoad.Lng + Constants.DISTANCE_INTERSECT_ROAD, resultRoad.Lat + Constants.DISTANCE_INTERSECT_ROAD, 0.0f, 0.0f);
                RPBLAddressResultV2 resultRegion = BAGEncoding.RegionByGeoV2(rec, new BAGPointV2(resultRoad.Lng, resultRoad.Lat), language);
                if (resultRegion == null)
                    return null;
                // 2.2 Bổ sung thông tin và trả về kết quả
                resultRegion.Lng = resultRoad.Lng;
                resultRegion.Lat = resultRoad.Lat;
                resultRegion.Building = resultRoad.Building;
                resultRegion.Road = resultRoad.Road;
                return resultRegion;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("MainProcessing.GeoByAddress, ex: {0}", ex.ToString()));
                return null;
            }
        }

        /// <summary>
        /// Tìm kiếm tọa độ qua địa chỉ (Áp dụng cho Hà Nội)
        /// </summary>
        private static RPBLAddressResult GeoByAddressHaNoi(BAGSearchKey keySearch, DTSSegment gsSegment, short districtID, EnumBAGLanguage language)
        {
            try
            {
                // 1. Tìm kiếm đường
                // 1.1 Tìm kiếm đường ưu tiên ở Hà Nội cũ
                RPBLAddressResult resultRoad = BAGDecoding.SearchRoadByNameHaNoi(gsSegment, keySearch, districtID);
                // 1.2 Nếu không có kết quả thì tìm ra Ha Tây cũ
                if (resultRoad == null)
                {
                    // 1.2.1 Chỉ tìm kiếm khi có giới hạn quận huyện và quận huyện đó không thuộc Hà Tây cũ
                    if (districtID == -1 || RunningParams.DistrictPriority.ContainsKey(districtID) == false)
                        resultRoad = BAGDecoding.SearchRoadByNameHaNoi(gsSegment, keySearch, -1, true);
                    // 1.2.2 Kiểm tra kết quả
                    if (resultRoad == null)
                        return null;
                }

                // 2. Tiến hành tìm kiếm vùng
                // 2.1 Tìm kiếm thông tin vùng
                RTRectangle rec = new RTRectangle(resultRoad.Lng - Constants.DISTANCE_INTERSECT_ROAD, resultRoad.Lat - Constants.DISTANCE_INTERSECT_ROAD, resultRoad.Lng + Constants.DISTANCE_INTERSECT_ROAD, resultRoad.Lat + Constants.DISTANCE_INTERSECT_ROAD, 0.0f, 0.0f);
                RPBLAddressResult resultRegion = BAGEncoding.RegionByGeo(rec, new BAGPoint(resultRoad.Lng, resultRoad.Lat), language);
                if (resultRegion == null)
                    return null;
                // 2.2 Bổ sung thông tin và trả về kết quả
                resultRegion.Lng = resultRoad.Lng;
                resultRegion.Lat = resultRoad.Lat;
                resultRegion.Building = resultRoad.Building;
                resultRegion.Road = resultRoad.Road;
                resultRegion.MinSpeed = resultRoad.MinSpeed;
                resultRegion.MaxSpeed = resultRoad.MaxSpeed;
                resultRegion.DataExt = resultRoad.DataExt;
                return resultRegion;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("MainProcessing.GeoByAddressHaNoi, ex: {0}", ex.ToString()));
                return null;
            }
        }

        /// <summary>
        /// Tìm kiếm tọa độ qua địa chỉ (Áp dụng cho Hà Nội) V2
        /// </summary>
        private static RPBLAddressResultV2 GeoByAddressHaNoiV2(BAGSearchKey keySearch, DTSSegment gsSegment, short districtID, EnumBAGLanguage language)
        {
            try
            {
                // 1. Tìm kiếm đường
                // 1.1 Tìm kiếm đường ưu tiên ở Hà Nội cũ
                RPBLAddressResultV2 resultRoad = BAGDecoding.SearchRoadByNameHaNoiV2(gsSegment, keySearch, districtID);
                // 1.2 Nếu không có kết quả thì tìm ra Ha Tây cũ
                if (resultRoad == null)
                {
                    // 1.2.1 Chỉ tìm kiếm khi có giới hạn quận huyện và quận huyện đó không thuộc Hà Tây cũ
                    if (districtID == -1 || RunningParams.DistrictPriority.ContainsKey(districtID) == false)
                        resultRoad = BAGDecoding.SearchRoadByNameHaNoiV2(gsSegment, keySearch, -1, true);
                    // 1.2.2 Kiểm tra kết quả
                    if (resultRoad == null)
                        return null;
                }

                // 2. Tiến hành tìm kiếm vùng
                // 2.1 Tìm kiếm thông tin vùng
                RTRectangle rec = new RTRectangle(resultRoad.Lng - Constants.DISTANCE_INTERSECT_ROAD, resultRoad.Lat - Constants.DISTANCE_INTERSECT_ROAD, resultRoad.Lng + Constants.DISTANCE_INTERSECT_ROAD, resultRoad.Lat + Constants.DISTANCE_INTERSECT_ROAD, 0.0f, 0.0f);
                RPBLAddressResultV2 resultRegion = BAGEncoding.RegionByGeoV2(rec, new BAGPointV2(resultRoad.Lng, resultRoad.Lat), language);
                if (resultRegion == null)
                    return null;
                // 2.2 Bổ sung thông tin và trả về kết quả
                resultRegion.Lng = resultRoad.Lng;
                resultRegion.Lat = resultRoad.Lat;
                resultRegion.Building = resultRoad.Building;
                resultRegion.Road = resultRoad.Road;
                resultRegion.MinSpeed = resultRoad.MinSpeed;
                resultRegion.MaxSpeed = resultRoad.MaxSpeed;
                resultRegion.DataExt = resultRoad.DataExt;
                return resultRegion;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("MainProcessing.GeoByAddressHaNoi, ex: {0}", ex.ToString()));
                return null;
            }
        }
    }
}