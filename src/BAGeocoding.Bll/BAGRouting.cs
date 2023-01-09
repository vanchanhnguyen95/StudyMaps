using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;

using KDTree;
using RTree.Engine.Entity;

using BAGeocoding.Dal.MapRoute;

using BAGeocoding.Entity.Enum.MapObject;
using BAGeocoding.Entity.Enum.Route;
using BAGeocoding.Entity.Router;

using BAGeocoding.Utility;

namespace BAGeocoding.Bll
{
    public class BAGRouting
    {
        private static BackgroundWorker RLogBWorker { get; set; }
        private static List<BARRouteLogs> RLogsData { get; set; }
        private static List<BARRouteLogs> RLogsError { get; set; }

        /// <summary>
        /// Tìm khoảng cách đường đi ngắn nhất giữa hai điểm
        /// </summary>
        public static BARResultS SRoute(BARParamS paramObj)
        {
            try
            {
                // 1. Xác định các node
                BARDetech detechStart = BAGRouting.SegmentDetect(true, paramObj.from.typ, new BARPoint { Lng = paramObj.from.lng, Lat = paramObj.from.lat });
                if (detechStart == null)
                    return new BARResultS(EnumBARErrorCode.StartNode);
                BARDetech detechEnd = BAGRouting.SegmentDetect(false, paramObj.to.typ, new BARPoint { Lng = paramObj.to.lng, Lat = paramObj.to.lat });
                if (detechEnd == null)
                    return new BARResultS(EnumBARErrorCode.EndNode);

                // 2. Xử lý trường hợp trùng segment
                BARResultS resultObj = SSegmentDupplicate(detechStart, detechEnd, paramObj.back);
                if (resultObj.state == true)
                    return resultObj;

                // 3. Tiến hành tìm đường
                BARInput dataInput = RouteInitParam(EnumBARType.Adjust, 0, detechStart, detechEnd);
                string routeResult = RouteCalc(paramObj.opts, RunningParams.Object2Json<BARInput>(dataInput));                
                if (routeResult == null || routeResult.Length == 0)
                    return new BARResultS(EnumBARErrorCode.RouteError);

                // 4. Trả về kết quả
                BAROutputS output = RunningParams.Json2Object<BAROutputS>(routeResult);
                if (output.state == true)
                    return new BARResultS { state = true, distance = output.length };
                else
                    return new BARResultS(EnumBARErrorCode.RouteNone);

            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGRouting.SRoute, ex: " + ex.ToString());
                return new BARResultS { state = false };
            }
        }

        /// <summary>
        /// Tìm khoảng cách đường đi ngắn nhất giữa hai điểm (Theo google)
        /// </summary>
        public static BARResultS SRouteGG(BARParamS paramObj)
        {
            try
            {
                GGRDirectionRes route = RouteGoogle(paramObj);
                if (route.status == "OK" && route.routes.Count > 0 && route.routes[0].legs.Count > 0)
                    return new BARResultS { state = true, distance = route.routes[0].legs[0].distance.value, type = "gg" };
                else
                    return new BARResultS(EnumBARErrorCode.RouteNone) { type = "gg" };
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGRouting.SRouteGG, ex: " + ex.ToString());
                return new BARResultS(EnumBARErrorCode.RouteError) { type = "gg" };
            }
        }

        /// <summary>
        /// Kiểm tra trường hợp trùng segment
        /// </summary>
        private static BARResultS SSegmentDupplicate(BARDetech detechStart, BARDetech detechEnd, int backward = 0)
        {
            if (detechStart.NodeInfo.SegmentID != detechEnd.NodeInfo.SegmentID)
                return new BARResultS { state = false };
            else if (RunningParams.RouterData.Objs.ContainsKey(detechStart.NodeInfo.SegmentID) == false)
                return new BARResultS { state = false };

            BARSegment segmentInfo = (BARSegment)RunningParams.RouterData.Objs[detechStart.NodeInfo.SegmentID];
            if (segmentInfo.AllowCar.Forward == true && detechStart.D2End + backward > detechEnd.D2End)
                return new BARResultS { state = true, distance = (int)Math.Abs(detechStart.D2End - detechEnd.D2End) };
            else if (segmentInfo.AllowCar.Reverse == true && detechStart.D2Start + backward > detechEnd.D2Start)
                return new BARResultS { state = true, distance = (int)Math.Abs(detechStart.D2Start - detechEnd.D2Start) };
            else
                return new BARResultS { state = false };
        }

        /// <summary>
        /// Tìm thông tin đường đi ngắn nhất giữa hai điểm
        /// </summary>
        public static BARResultF FRoute(BARParamS paramObj)
        {
            try
            {
                // 1. Xác định các node
                BARDetech detechStart = BAGRouting.SegmentDetect(true, paramObj.from.typ, new BARPoint { Lng = paramObj.from.lng, Lat = paramObj.from.lat });
                if (detechStart == null)
                    return new BARResultF(EnumBARErrorCode.StartNode);
                BARDetech detechEnd = BAGRouting.SegmentDetect(false, paramObj.to.typ, new BARPoint { Lng = paramObj.to.lng, Lat = paramObj.to.lat });
                if (detechEnd == null)
                    return new BARResultF(EnumBARErrorCode.EndNode);

                // 2. Xử lý trường hợp trùng segment
                BARResultF resultObj = FSegmentDupplicate(detechStart, detechEnd, paramObj.back);
                if (resultObj.state == true)
                {
                    resultObj.from = new BARResultPoint(paramObj.from);
                    resultObj.to = new BARResultPoint(paramObj.to);
                    return resultObj;
                }

                // 3. Tiến hành tìm đường
                BARInput dataInput = RouteInitParam(EnumBARType.Adjust, 1, detechStart, detechEnd);
                string routeInput = RunningParams.Object2Json<BARInput>(dataInput);
                string routeResult = RouteCalc(paramObj.opts, routeInput);
                if (routeResult == null || routeResult.Length == 0)
                    return new BARResultF(EnumBARErrorCode.RouteError);
                
                // 4. Trả về kết quả
                // 4.1 Tìm lỗi
                BAROutputF output = RunningParams.Json2Object<BAROutputF>(routeResult);
                if (output.state == false)
                    return new BARResultF(EnumBARErrorCode.RouteNone);
                // 4.2 Tìm thành công
                BARResultF result = new BARResultF { state = true, distance = output.length };
                result.points.Add(new BARResultPoint(detechStart.Point));
                int indexCount = output.detail.Length;
                for (int i = 0; i < indexCount; i++)
                {
                    bool direction = (output.detail[i] % 2 == 0);
                    int segmentID = Convert.ToInt32(output.detail[i] / 2);
                    if(RunningParams.RouterData.Objs.ContainsKey(segmentID) == false)
                        return new BARResultF { state = false };
                    BARSegment segmentInfo = (BARSegment)RunningParams.RouterData.Objs[segmentID];

                    #region ==================== Lấy thông tin segment ====================
                    if (i == 0)
                    {
                        #region ==================== Trường hợp segment đầu tiên ====================
                        if (direction == true)
                        {
                            for (int j = detechStart.PointIndex; j < segmentInfo.PointList.Count; j++)
                                result.points.Add(new BARResultPoint(segmentInfo.PointList[j]));
                        }
                        else
                        {
                            for (int j = detechStart.PointIndex - 1; j > -1; j--)
                                result.points.Add(new BARResultPoint(segmentInfo.PointList[j]));
                        }
                        result.SAdd(segmentInfo, detechStart.Point);
                        #endregion
                    }
                    else if (i == indexCount - 1)
                    {
                        #region ==================== Trường hợp segment cuối cùng ====================
                        if (direction == true)
                        {
                            for (int j = 0; j < detechEnd.PointIndex; j++)
                                result.points.Add(new BARResultPoint(segmentInfo.PointList[j]));
                            result.SAdd(segmentInfo, segmentInfo.PointList[0]);
                        }
                        else
                        {
                            for (int j = segmentInfo.PointList.Count - 1; j > detechEnd.PointIndex - 1; j--)
                                result.points.Add(new BARResultPoint(segmentInfo.PointList[j]));
                            result.SAdd(segmentInfo, segmentInfo.PointList[segmentInfo.PointList.Count - 1]);
                        }
                        result.points.Add(new BARResultPoint(detechEnd.Point));
                        #endregion
                    }
                    else
                    {
                        #region ==================== Các segment tiếp theo ====================
                        if (direction == true)
                        {
                            for (int j = 0; j < segmentInfo.PointList.Count; j++)
                                result.points.Add(new BARResultPoint(segmentInfo.PointList[j]));

                            result.SAdd(segmentInfo, segmentInfo.PointList[0]);
                        }
                        else
                        {
                            for (int j = segmentInfo.PointList.Count - 1; j > -1; j--)
                                result.points.Add(new BARResultPoint(segmentInfo.PointList[j]));

                            result.SAdd(segmentInfo, segmentInfo.PointList[segmentInfo.PointList.Count - 1]);
                        }
                        #endregion
                    }
                    #endregion
                }
                result.from = new BARResultPoint(paramObj.from);
                result.to = new BARResultPoint(paramObj.to);
                return result;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGRouting.SRoute, ex: " + ex.ToString());
                return new BARResultF { state = false };
            }
        }

        /// <summary>
        /// Tìm thông tin đường đi ngắn nhất giữa hai điểm (Theo Google)
        /// </summary>
        public static BARResultF FRouteGG(BARParamS paramObj)
        {
            try
            {
                GGRDirectionRes route = RouteGoogle(paramObj);
                if (route.status == "OK" && route.routes.Count > 0 && route.routes[0].legs.Count > 0)
                    return new BARResultF
                    {
                        state = true,
                        distance = route.routes[0].legs[0].distance.value,
                        from = new BARResultPoint(paramObj.from),
                        to = new BARResultPoint(paramObj.to),
                        points = BAGDecoding.PolylineDecode(route.routes[0].overview_polyline.points),
                        segments = new List<BARResultSegment>(),
                        type = "gg"
                    };
                else
                    return new BARResultF(EnumBARErrorCode.RouteNone) { type = "gg" };
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGRouting.FRouteGG, ex: " + ex.ToString());
                return new BARResultF(EnumBARErrorCode.RouteError) { type = "gg" };
            }
        }

        /// <summary>
        /// Kiểm tra trường hợp trùng segment
        /// </summary>
        private static BARResultF FSegmentDupplicate(BARDetech detechStart, BARDetech detechEnd, int backward = 0)
        {
            if (detechStart.NodeInfo.SegmentID != detechEnd.NodeInfo.SegmentID)
                return new BARResultF { state = false };
            else if (RunningParams.RouterData.Objs.ContainsKey(detechStart.NodeInfo.SegmentID) == false)
                return new BARResultF { state = false };
            BARSegment segmentInfo = (BARSegment)RunningParams.RouterData.Objs[detechStart.NodeInfo.SegmentID];
            if (segmentInfo.AllowCar.Forward == true && detechStart.D2End + backward > detechEnd.D2End)
            {
                BARResultF resultObj = new BARResultF { state = true, distance = (int)Math.Abs(detechStart.D2End - detechEnd.D2End) };
                resultObj.points.Add(new BARResultPoint(detechStart.Point));
                for (int i = detechStart.PointIndex; i < detechEnd.PointIndex; i++)
                    resultObj.points.Add(new BARResultPoint(segmentInfo.PointList[i]));
                resultObj.points.Add(new BARResultPoint(detechEnd.Point));
                resultObj.SAdd(segmentInfo, detechStart.Point);
                return resultObj;
            }
            else if (segmentInfo.AllowCar.Reverse == true && detechStart.D2Start + backward > detechEnd.D2Start)
            {
                BARResultF resultObj = new BARResultF { state = true, distance = (int)Math.Abs(detechStart.D2Start - detechEnd.D2Start) };
                resultObj.points.Add(new BARResultPoint(detechStart.Point));
                for (int i = detechStart.PointIndex - 1; i > detechEnd.PointIndex; i--)
                    resultObj.points.Add(new BARResultPoint(segmentInfo.PointList[i]));
                resultObj.points.Add(new BARResultPoint(detechEnd.Point));
                resultObj.SAdd(segmentInfo, detechStart.Point);
                return resultObj;
            }
            else
                return new BARResultF { state = false };
        }

        /// <summary>
        /// Khởi tạo tham số đầu vào của dịch vụ tìm đường
        /// </summary>
        private static BARInput RouteInitParam(EnumBARType type, int method, BARDetech detechStart, BARDetech detechEnd)
        {
            BARInput dataInput = new BARInput
            {
                type = (int)type,
                method = method,
                from = new BARInputNode
                {
                    seg = detechStart.NodeInfo.SegmentID,
                    d0 = (int)detechStart.NodeInfo.D2Start,
                    d1 = (int)detechStart.NodeInfo.D2End,
                    l0 = (int)detechStart.NodeInfo.D2Start,
                    l1 = (int)detechStart.NodeInfo.D2End
                },
                to = new BARInputNode
                {
                    seg = detechEnd.NodeInfo.SegmentID,
                    d0 = (int)detechEnd.NodeInfo.D2Start,
                    d1 = (int)detechEnd.NodeInfo.D2End,
                    l0 = (int)detechEnd.NodeInfo.D2Start,
                    l1 = (int)detechEnd.NodeInfo.D2End
                }
            };
            if (type == EnumBARType.Adjust)
            {
                if (dataInput.from.l0 > -1)
                    dataInput.from.l0 = (int)(dataInput.from.l0 * detechStart.NodeInfo.Coeff);
                if (dataInput.from.l1 > -1)
                    dataInput.from.l1 = (int)(dataInput.from.l1 * detechStart.NodeInfo.Coeff);
                if (dataInput.to.l0 > -1)
                    dataInput.to.l0 = (int)(dataInput.to.l0 * detechEnd.NodeInfo.Coeff);
                if (dataInput.to.l1 > 0)
                    dataInput.to.l1 = (int)(dataInput.to.l1 * detechEnd.NodeInfo.Coeff);
            }

            return dataInput;
        }

        /// <summary>
        /// Xác định thông tin segment của điểm đầu vào
        /// </summary>
        private static BARDetech SegmentDetect(bool sts, byte typ, BARPoint pts)
        {
            try
            {
                BARNode nodeNeighbor = SegmentNode(typ, pts);
                if (nodeNeighbor != null)
                {
                    double lng = pts.Lng - nodeNeighbor.Coords.Lng;
                    double lat = pts.Lat - nodeNeighbor.Coords.Lat;
                    RTRectangle rec = pts.ToRectangle(Math.Sqrt(lng * lng + lat * lat) * 1.1d);
                    BARDetech result = SegmentDetect(sts, typ, rec, pts);
                    if (result != null)
                        return result; 
                }
                else
                {
                    for (int i = 0; i < Constants.DISTANCE_INTERSECT_LIST.Count; i++)
                    {
                        RTRectangle rec = pts.ToRectangle(Constants.DISTANCE_INTERSECT_LIST[i]);
                        BARDetech result = SegmentDetect(sts, typ, rec, pts);
                        if (result != null)
                            return result;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGRouting.SegmentDetect, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Xác định điểm gần nhất
        /// </summary>
        private static BARNode SegmentNode(byte typ, BARPoint pts)
        {
            try
            {
                NearestNeighbour<BARNode> neighborList = RunningParams.RouterData.KDTree.NearestNeighbors(pts.ToArray(), new SquareEuclideanDistanceFunction(), 32, 1e-5f);                
                if (neighborList == null)
                    return null;

                while (neighborList.MoveNext())
                {
                    if (neighborList.Current == null)
                        continue;
                    else if ((typ & 1) > 0 && neighborList.Current.HighWay == true)
                        continue;
                    else if ((typ & 2) > 0 && neighborList.Current.FerryThese == true)
                        continue;
                    return new BARNode(neighborList.Current);
                }
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGRouting.SegmentNode, ex: " + ex.ToString());
                return null;
            }
        }
        
        /// <summary>
        /// Xác định thông tin segment của điểm đầu vào
        /// </summary>
        private static BARDetech SegmentDetect(bool sts, byte typ, RTRectangle rec, BARPoint pts)
        {
            try
            {
                // Xác định segment giao với điểm check
                List<BARSegment> intersects = RunningParams.RouterData.RTree.Intersects(rec);
                if (intersects == null || intersects.Count == 0)
                    return null;
                
                // Tính toán khoảng cách điểm đến segment
                int indexID = 0;
                // .1 Xác định segment đầu tiên thỏa mãn
                for (indexID = 0; indexID < intersects.Count; indexID++)
                {
                    if ((typ & 1) > 0 && intersects[indexID].DataExtGet(EnumMOBSegmentDataExt.HighWay) == true)
                        continue;
                    else if ((typ & 2) > 0 && intersects[indexID].FerryThese == true)
                        continue;
                    break;
                }
                if (indexID > intersects.Count - 1)
                    return null;
                // .2 Tính toán với các segment còn lại
                BARDetech result = intersects[indexID].DistanceFrom(sts, pts);
                for (int i = indexID + 1; i < intersects.Count; i++)
                {
                    if ((typ & 1) > 0 && intersects[i].DataExtGet(EnumMOBSegmentDataExt.HighWay) == true)
                        continue;
                    else if ((typ & 2) > 0 && intersects[i].FerryThese == true)
                        continue;
                    BARDetech temp = intersects[i].DistanceFrom(sts, pts);
                    if (temp.Distance < result.Distance)
                    {
                        result = new BARDetech(temp);
                        result.PointIndex = temp.PointIndex;
                        result.NodeInfo.Coeff = temp.NodeInfo.Coeff;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGRouting.SegmentDetect, ex: " + ex.ToString());
                return null;
            }
        }

        #region ================ Lấy kế quả dịch vụ tìm đường ================
        /// <summary>
        /// Gọi đến dịch vụ tìm đường
        /// </summary>
        private static string RouteCalc(int options, string dataInput)
        {
            try
            {
                LogFile.WriteProcess(RunningParams.InternalHostNormal);
                using (WebClient client = new WebClient())
                {
                    client.Headers["Content-Type"] = "application/json";
                    if ((options & 2) > 0 && RunningParams.InternalHostNoFerry.Length > 0)
                        return client.UploadString(new Uri(RunningParams.InternalHostNoFerry), "POST", dataInput);
                    else if (RunningParams.InternalHostNormal.Length > 0)
                        return client.UploadString(new Uri(RunningParams.InternalHostNormal), "POST", dataInput);
                    else
                        return string.Empty;
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGRouting.RouteCalc, ex: " + ex.ToString());
                return string.Empty;
            }
        }

        /// <summary>
        /// Tìm đường theo google
        /// </summary>
        private static GGRDirectionRes RouteGoogle(BARParamS paramObj)
        {
            try
            {
                if (RunningParams.GoogleRouteKey == null || RunningParams.GoogleRouteKey.Length == 0)
                    return new GGRDirectionRes { status = "ERROR" };

                string origin = string.Format("{0},{1}", paramObj.from.lat, paramObj.from.lng);
                string destination = string.Format("{0},{1}", paramObj.to.lat, paramObj.to.lng);
                string url = string.Format("https://maps.googleapis.com/maps/api/directions/json?origin={0}&destination={1}&key={2}", origin, destination, RunningParams.GoogleRouteKey);
                using (WebClient client = new WebClient() { Encoding = Encoding.UTF8 })
                {
                    client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    client.Headers.Add("Content-Encoding: UTF-8");
                    return RunningParams.Json2Object<GGRDirectionRes>(client.DownloadString(url));
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGRouting.RouteGoogle, ex: " + ex.ToString());
                return new GGRDirectionRes();
            }
        }
        #endregion

        #region ================ Ghi logs yêu cầu dịch vụ tìm đường ================
        /// <summary>
        /// Khởi tạo tiến trình ghi logs
        /// </summary>
        public static bool RLogsInit()
        {
            try
            {
                LogFile.WriteProcess("-------------------- Khởi tạo dịch vụ ghi logs --------------------");

                RLogsData = new List<BARRouteLogs>();
                RLogsError = new List<BARRouteLogs>();

                RLogBWorker = new BackgroundWorker();
                RLogBWorker.DoWork += new DoWorkEventHandler(RLogBWorker_DoWork);

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGRouting.RLogsInit, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Xử ly logs tìm đường
        /// </summary>
        public static void RLogsProcess(BARRouteLogs logsdata)
        {
            // Thêm dữ liệu
            lock (RLogsData)
            {
                logsdata.InitTime(DateTime.Now);
                RLogsData.Add(logsdata);
                if (RLogsData.Count < 32)
                    return;
            }
            // Tiến hành lưu
            if (RLogBWorker.IsBusy == false)
                RLogBWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Xử ly logs tìm đường
        /// </summary>
        public static void RLogsProcess(List<BARRouteLogs> logsError)
        {
            // Thêm dữ liệu
            lock (RLogsError)
            {
                for (int i = 0; i < logsError.Count; i++)
                    logsError[i].InitTime(DateTime.Now);
                RLogsError.AddRange(logsError);
                if (RLogsError.Count < 2)
                    return;
            }
            // Tiến hành lưu
            if (RLogBWorker.IsBusy == false)
                RLogBWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Tiến trình ghi logs
        /// </summary>
        private static void RLogBWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                List<BARRouteLogs> logsData = new List<BARRouteLogs>();
                lock (RLogsData)
                {
                    if (RLogsData.Count > 0)
                    {
                        for (int i = 0; i < RLogsData.Count; i++)
                            logsData.Add(new BARRouteLogs(RLogsData[i]));
                        RLogsData.Clear();
                    }
                }
                if (logsData.Count > 0)
                {
                    LogFile.WriteRequest(string.Format("Bắt đầu ghi logs request: {0}", logsData.Count));
                    LOGRRequestDAO.WriteData(logsData);
                }

                List<BARRouteLogs> logsError = new List<BARRouteLogs>();
                lock (RLogsError)
                {
                    if (RLogsError.Count > 0)
                    {
                        for (int i = 0; i < RLogsError.Count; i++)
                            logsError.Add(new BARRouteLogs(RLogsError[i]));
                        RLogsError.Clear();
                    }
                }
                if (logsError.Count > 0)
                {
                    LogFile.WriteRequest(string.Format("Bắt đầu ghi logs error: {0}", logsData.Count));
                    LOGRRequestDAO.WriteError(logsError);
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGRouting.RLogBWorker_DoWork, ex: " + ex.ToString());
            }
        }
        #endregion
    }
}