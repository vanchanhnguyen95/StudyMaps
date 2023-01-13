using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.Enum.Route;
using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.Router;
using BAGeocoding.Entity.Utility;
using BAGeocoding.Utility;
using OSGeo.OGR;
using RTree.Engine;
using RTree.Engine.Entity;

namespace BAGeocoding.Bll.ImportData
{
    public class ImportDataRouteManager
    {
        public static double Epsilon = DataUtl.ConvertMeterToLngLat(2);

        #region ==================== Kiểm tra node rời ====================
        /// <summary>
        /// Kiểm tra điểm không nối
        /// </summary>
        public static bool CheckLinker(UTLConditionImportRoute condition, List<BARSegment> segmentList)
        {
            try
            {
                if(condition.Distance > 0)
                    Epsilon = DataUtl.ConvertMeterToLngLat(condition.Distance);
                if (segmentList == null || segmentList.Count == 0)
                    return false;
                List<BARCheckPoint> errorList = new List<BARCheckPoint>();
                List<BARCheckPoint> linkerList = new List<BARCheckPoint>();
                RTree<BARCheckPoint> rtreeNode = new RTree<BARCheckPoint>();

                // A. Xử lý dữ liệu tổng
                for (int i = 0; i < segmentList.Count; i++)
                {
                    if (condition.SegmentID > 0 && segmentList[i].SegmentID == condition.SegmentID)
                        Console.Write("A");
                    // .1 Kiểm tra điểm bắt đầu
                    BARCheckPoint nodeStart = new BARCheckPoint { NodeID = rtreeNode.Count + 1, SegmentID = segmentList[i].SegmentID, Coords = new BARPoint(segmentList[i].PointList[0]) };
                    CheckLinkerNode(nodeStart, ref rtreeNode, ref linkerList, ref errorList);
                    // .2 Kiểm tra điểm kết thúc
                    BARCheckPoint nodeEnd = new BARCheckPoint { NodeID = rtreeNode.Count + 1, SegmentID = segmentList[i].SegmentID, Coords = new BARPoint(segmentList[i].PointList[segmentList[i].PointList.Count - 1]) };
                    CheckLinkerNode(nodeEnd, ref rtreeNode, ref linkerList, ref errorList);
                }

                // B. Xử lý lại các điểm lỗi
                int indexID = 0;
                List<BARCheckPoint> tempList = new List<BARCheckPoint>();
                while (errorList.Count > 0 && indexID < errorList.Count)
                {
                    BARCheckPoint nodeInfo = new BARCheckPoint(errorList[indexID]);
                    EnumBARNodeState nodeState = CheckNodeFoAdd(rtreeNode, nodeInfo, ref linkerList, ref tempList);
                    if (nodeState == EnumBARNodeState.Blank)
                    {
                        nodeInfo.IndexID = linkerList.Count;
                        if (nodeInfo.SegmentHT.ContainsKey(nodeInfo.SegmentID) == false)
                            nodeInfo.SegmentHT.Add(nodeInfo.SegmentID, null);
                        linkerList.Add(nodeInfo);
                        rtreeNode.Add(nodeInfo.Coords.ToRectangle(Epsilon), nodeInfo);
                        errorList.RemoveAt(indexID);
                        indexID = 0;
                        continue;
                    }
                    else if (nodeState == EnumBARNodeState.Duplicate)
                    {
                        errorList.RemoveAt(indexID);
                        indexID = 0;
                        continue;
                    }
                    else
                        indexID += 1;
                }

                // X. Xử lý kết quả
                if (errorList.Count == 0)
                    return true;
                WritePoint(condition, "CheckLinker", errorList);
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("ImportDataRouteManager.CheckLinker, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra node liên quan
        /// </summary>
        private static EnumBARNodeState CheckLinkerNode(BARCheckPoint nodeInfo, ref RTree<BARCheckPoint> rtreeNode, ref List<BARCheckPoint> linkerList, ref List<BARCheckPoint> errorList)
        {
            try
            {
                EnumBARNodeState nodeState = CheckNodeFoAdd(rtreeNode, nodeInfo, ref linkerList, ref errorList);
                if (nodeState == EnumBARNodeState.Blank)
                {
                    nodeInfo.IndexID = linkerList.Count;
                    if (nodeInfo.SegmentHT.ContainsKey(nodeInfo.SegmentID) == false)
                        nodeInfo.SegmentHT.Add(nodeInfo.SegmentID, null);
                    linkerList.Add(nodeInfo);
                    rtreeNode.Add(nodeInfo.Coords.ToRectangle(Epsilon), nodeInfo);
                }
                else if (nodeState == EnumBARNodeState.Nearer)
                    errorList.Add(nodeInfo);
                return nodeState;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("ImportDataRouteManager.CheckLinkerNode, ex: " + ex.ToString());
                return EnumBARNodeState.Blank;
            }
        }

        /// <summary>
        /// Kiểm tra để thêm node
        /// </summary>
        private static EnumBARNodeState CheckNodeFoAdd(RTree<BARCheckPoint> rtreeNode, BARCheckPoint nodeInfo, ref List<BARCheckPoint> linkerList, ref List<BARCheckPoint> errorList)
        {
            EnumBARNodeState nodeState = EnumBARNodeState.Nearer;
            List<BARCheckPoint> checkList = rtreeNode.Intersects(nodeInfo.Coords.ToRectangle(Epsilon));
            // 1. Trường hợp node chưa tồn tại trong danh sách
            if (checkList == null || checkList.Count == 0)
                nodeState = EnumBARNodeState.Blank;
            // 2. Trường hợp đã tồn tại
            else
            {
                for (int i = 0; i < checkList.Count; i++)
                {
                    // .1 Trường hợp trùng điểm
                    if (nodeInfo.Coords.Distance(checkList[i].Coords) == 0)
                    {
                        // .1 Bổ sung segmentID vào danh sách kiểm tra
                        if (linkerList[checkList[i].IndexID].SegmentHT.ContainsKey(nodeInfo.SegmentID) == false)
                            linkerList[checkList[i].IndexID].SegmentHT.Add(nodeInfo.SegmentID, null);
                        // .2 Thiết lập trạng thái
                        nodeState = EnumBARNodeState.Duplicate;
                        break;
                    }
                    // .2 So sánh danh sách kiểm tra
                    else if (linkerList[checkList[i].IndexID].SegmentHT.ContainsKey(nodeInfo.SegmentID) == true)
                    {
                        nodeState = EnumBARNodeState.Blank;
                        break;
                    }
                }
            }

            // Kiểm tra trường hợp điểm gần
            if (nodeState == EnumBARNodeState.Nearer)
            {
                for (int i = 0; i < errorList.Count; i++)
                {
                    if (nodeInfo.Coords.Distance(errorList[i].Coords) == 0)
                    {
                        nodeState = EnumBARNodeState.Blank;
                        break;
                    }
                }
            }

            // X. Trả về kết quả
            return nodeState;
        }
        #endregion

        #region ==================== Kiểm tra tính liên thông ====================
        /// <summary>
        /// Kiểm tra tính liên thông
        /// </summary>
        public static bool CheckConnected(UTLConditionImportRoute condition, List<BARSegment> sourceList)
        {
            try
            {
                if (sourceList == null || sourceList.Count == 0)
                    return false;
                List<BARSegment> connectList = new List<BARSegment>();
                List<BARSegment> segmentList = CopySegment(sourceList);
                int indexID = 0;
                bool processFlag = false;
                RTree<BARCheckPoint> rtreeNode = new RTree<BARCheckPoint>();
                BARCheckPoint nodeStart = new BARCheckPoint { NodeID = rtreeNode.Count + 1, SegmentID = segmentList[indexID].SegmentID, Coords = new BARPoint(segmentList[indexID].PointList[0]) };
                rtreeNode.Add(nodeStart.Coords.ToRectangle(Epsilon), nodeStart);
                BARCheckPoint nodeEnd = new BARCheckPoint { NodeID = rtreeNode.Count + 1, SegmentID = segmentList[indexID].SegmentID, Coords = new BARPoint(segmentList[indexID].PointList[segmentList[indexID].PointList.Count - 1]) };
                rtreeNode.Add(nodeEnd.Coords.ToRectangle(Epsilon), nodeEnd);
                connectList.Add(new BARSegment(segmentList[indexID]));
                segmentList.RemoveAt(indexID);
                while (true)
                {
                    if (indexID > segmentList.Count - 1)
                    {
                        if (processFlag == false)
                            break;
                        else
                        {
                            indexID = 0;
                            processFlag = false;
                        }
                    }

                    nodeStart = new BARCheckPoint { NodeID = rtreeNode.Count + 1, SegmentID = segmentList[indexID].SegmentID, Coords = segmentList[indexID].PointList[0] };
                    nodeEnd = new BARCheckPoint { NodeID = rtreeNode.Count + 1, SegmentID = segmentList[indexID].SegmentID, Coords = segmentList[indexID].PointList[segmentList[indexID].PointList.Count - 1] };
                    if (segmentList[indexID].AllowCar.Forward == true && CheckNodeExist(rtreeNode, nodeStart) == true || segmentList[indexID].AllowCar.Reverse == true && CheckNodeExist(rtreeNode, nodeEnd) == true)
                    {
                        rtreeNode.Add(nodeStart.Coords.ToRectangle(Epsilon), nodeStart);
                        rtreeNode.Add(nodeEnd.Coords.ToRectangle(Epsilon), nodeEnd);
                        connectList.Add(new BARSegment(segmentList[indexID]));
                        segmentList.RemoveAt(indexID);
                        if (indexID > segmentList.Count - 1)
                        {
                            indexID = 0;
                            processFlag = false;
                        }
                        else
                            processFlag = true;
                    }
                    else
                        indexID++;

                    if (segmentList.Count == 0)
                        break;
                }

                if (segmentList.Count == 0)
                    return true;
                else if (connectList.Count < segmentList.Count)
                    WritePolyline(condition, "CheckConnected", connectList);
                else
                    WritePolyline(condition, "CheckConnected", segmentList);
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("ImportDataRouteManager.CheckConnected, ex: " + ex.ToString());
                return false;
            }
        }
        
        /// <summary>
        /// Kiểm tra tồn tại node
        /// </summary>
        private static bool CheckNodeExist(RTree<BARCheckPoint> rtreeNode, BARCheckPoint nodeInfo)
        {
            RTRectangle rec = nodeInfo.Coords.ToRectangle(Epsilon);
            List<BARCheckPoint> checStart = rtreeNode.Intersects(rec);
            if (checStart == null || checStart.Count == 0)
                return false;
            double distance = 0;
            for (int i = 0; i < checStart.Count; i++)
            {
                distance = nodeInfo.Coords.Distance(checStart[i].Coords);
                if (distance == 0)
                    return true;
            }
            return false;
        }
        #endregion

        ///// <summary>
        ///// Đọc dữ liệu từ file
        ///// </summary>
        //public static List<BARSegment> ReadData(UTLConditionImportRoute condition, ref bool objectMulti)
        //{
        //    try
        //    {
        //        Hashtable htObjectName = ImportDataManager.ReadObjectName(EnumBAGRegionType.Segment, condition.FileName);
        //        if (htObjectName == null || htObjectName.Count == 0)
        //            return null;

        //        OrgAPI ds = new OrgAPI(condition.FileMap, 0);
        //        Hashtable htDistrict = new Hashtable();
        //        int nIndex = 1;
        //        if (RunningParams.ImportFileType == EnumBAGFileType.Shp)
        //            nIndex = 0;
        //        int nFeature = ds.GetFeatureCount() + nIndex;
        //        List<BARSegment> errorList = new List<BARSegment>();
        //        List<BARSegment> segmentList = new List<BARSegment>();
        //        for (int i = nIndex; i < nFeature; i++)
        //        {
        //            Feature f = ds.GetFeatureById(i);
        //            Geometry geo = f.GetGeometryRef();
        //            if (geo == null)
        //                continue;
        //            else if (geo.GetGeometryType() == ogr.wkbLineString)
        //            {
        //                #region ==================== Đọc thông tin từ file bản đồ ====================
        //                BARSegment segment = new BARSegment();
        //                segment.SegmentID = (int)f.GetFieldAsInteger("ID");
        //                if (htObjectName.ContainsKey(segment.SegmentID) == true)
        //                    segment.VName = (string)htObjectName[segment.SegmentID];
        //                else
        //                    segment.VName = string.Empty;
        //                segment.EName = LatinToAscii.Latin2Ascii(segment.VName);
        //                segment.ClassFunc = (byte)f.GetFieldAsInteger("ClassFunc");
        //                segment.AllowCar = new BARDirection((byte)f.GetFieldAsInteger("AllowCar"));
        //                segment.PointList = new List<BARPoint>();
        //                int nCount = geo.GetPointCount();
        //                for (int j = 0; j < nCount; j++)
        //                    segment.PointList.Add(new BARPoint(geo.GetX(j), geo.GetY(j)));
        //                segmentList.Add(segment);
        //                #endregion
        //            }
        //            else
        //            {
        //                BARSegment segment = new BARSegment();
        //                segment.SegmentID = (int)f.GetFieldAsInteger("ID");
        //                if (htObjectName.ContainsKey(segment.SegmentID) == true)
        //                    segment.VName = (string)htObjectName[segment.SegmentID];
        //                else
        //                    segment.VName = string.Empty;
        //                segment.EName = LatinToAscii.Latin2Ascii(segment.VName);
        //                segment.ClassFunc = (byte)f.GetFieldAsInteger("ClassFunc");
        //                segment.AllowCar = new BARDirection((byte)f.GetFieldAsInteger("AllowCar"));
        //                segment.PointList = new List<BARPoint>();
        //                int geoCount = geo.GetGeometryCount();
        //                for (int j = 0; j < geoCount; j++)
        //                {
        //                    BARSegment segmentx = new BARSegment(segment);
        //                    Geometry geox = geo.GetGeometryRef(j);
        //                    int nCount = geox.GetPointCount();
        //                    segmentx.PointList = new List<BARPoint>();
        //                    for (int k = 0; k < nCount; k++)
        //                        segmentx.PointList.Add(new BARPoint(geox.GetX(k), geox.GetY(k)));
        //                    errorList.Add(segmentx);
        //                }
        //            }
        //        }

        //        if (errorList.Count > 0)
        //        {
        //            objectMulti = true;
        //            WritePolyline(condition, "ObjectMulti", errorList);
        //        }

        //        return segmentList;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogFile.WriteData("ImportDataRouteManager.ReadData, ex: " + ex.ToString());
        //        return null;
        //    }
        //}

        /// <summary>
        /// Copy dữ liệu
        /// </summary>
        private static List<BARSegment> CopySegment(List<BARSegment> source)
        {
            try
            {
                if (source == null)
                    return new List<BARSegment>();
                else if (source.Count == 0)
                    return source;
                List<BARSegment> result = new List<BARSegment>();
                source.ForEach(item => result.Add(new BARSegment(item)));
                return result;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("ImportDataRouteManager.CheckConnected, ex: " + ex.ToString());
                return new List<BARSegment>();
            }
        }

        #region ==================== Ghi dữ liệu ====================
        /// <summary>
        /// Ghi dữ liệu điểm
        /// </summary>
        private static bool WritePoint(UTLConditionImportRoute condition, string fileName, List<BARCheckPoint> errorList)
        {
            try
            {
                if (errorList == null || errorList.Count == 0)
                    return false;

                #region ==================== 1. File mở rộng .mif ====================
                string fileResultMif = string.Format("{0}\\{1}.mif", condition.FolderOutput, fileName);
                if (File.Exists(fileResultMif))
                    File.Delete(fileResultMif);
                using (StreamWriter streamWrite = new StreamWriter(fileResultMif, false))
                {
                    streamWrite.WriteLine("Version 300");
                    streamWrite.WriteLine("CHARSET \"Neutral\"");
                    streamWrite.WriteLine("Delimiter \", \"");
                    streamWrite.WriteLine("CoordSys Earth Projection 1, 104");
                    streamWrite.WriteLine("Columns 2");
                    streamWrite.WriteLine("  NodeID Decimal(10, 0)");
                    streamWrite.WriteLine("  SegmentID Decimal(10, 0)");
                    streamWrite.WriteLine("Data");
                    streamWrite.WriteLine("");

                    for (int i = 0; i < errorList.Count; i++)
                    {
                        streamWrite.WriteLine(string.Format("Point {0} {1}", errorList[i].Coords.Lng, errorList[i].Coords.Lat));
                        streamWrite.WriteLine("    Symbol (34, 0, 12)");
                    }
                }
                #endregion

                #region ==================== 2. File mở rộng .mid ====================
                string fileResultMid = string.Format("{0}\\{1}.mid", condition.FolderOutput, fileName);
                if (File.Exists(fileResultMid))
                    File.Delete(fileResultMid);
                using (StreamWriter streamWrite = new StreamWriter(fileResultMid, false, Encoding.UTF8))
                {
                    for (int i = 0; i < errorList.Count; i++)
                        streamWrite.WriteLine(string.Format("{0},{1}", errorList[i].NodeID, errorList[i].SegmentID));
                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("ImportDataRouteManager.WritePoint, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Ghi dữ liệu đường
        /// </summary>
        private static bool WritePolyline(UTLConditionImportRoute condition, string fileName, List<BARSegment> segmentList)
        {
            try
            {
                #region ==================== 1. File mở rộng .mif ====================
                string fileResultMif = string.Format("{0}\\{1}.mif", condition.FolderOutput, fileName);
                if (File.Exists(fileResultMif))
                    File.Delete(fileResultMif);
                using (StreamWriter streamWrite = new StreamWriter(fileResultMif, false, Encoding.ASCII))
                {
                    streamWrite.WriteLine("Version 300");
                    streamWrite.WriteLine("CHARSET \"Neutral\"");
                    streamWrite.WriteLine("Delimiter \", \"");
                    streamWrite.WriteLine("CoordSys Earth Projection 1, 104");
                    streamWrite.WriteLine("Columns 2");
                    streamWrite.WriteLine("  SegmentID Decimal(10, 0)");
                    streamWrite.WriteLine("  Name Char(200)");
                    streamWrite.WriteLine("Data");
                    streamWrite.WriteLine("");

                    for (int i = 0; i < segmentList.Count; i++)
                    {
                        streamWrite.WriteLine(string.Format("Pline {0}", segmentList[i].PointList.Count));
                        for (int j = 0; j < segmentList[i].PointList.Count; j++)
                            streamWrite.WriteLine(string.Format("{0:N8} {1:N8}", segmentList[i].PointList[j].Lng, segmentList[i].PointList[j].Lat));
                        streamWrite.WriteLine("    Pen (1, 2, 0)");
                    }
                }
                #endregion

                #region ==================== 2. File mở rộng .mif ====================
                string fileResultMid = string.Format("{0}\\{1}.mid", condition.FolderOutput, fileName);
                if (File.Exists(fileResultMid))
                    File.Delete(fileResultMid);
                using (StreamWriter streamWrite = new StreamWriter(fileResultMid, false, Encoding.UTF8))
                {
                    for (int i = 0; i < segmentList.Count; i++)
                        streamWrite.WriteLine(string.Format("{0},{1}", segmentList[i].SegmentID, segmentList[i].VName.Replace(",", "-")));
                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("ImportDataRouteManager.WritePolyline, ex: " + ex.ToString());
                return false;
            }
        }
        #endregion
    }
}