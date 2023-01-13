using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using OSGeo.OGR;

//using BAGeocoding.Dal.MapObj;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.Utility;

using BAGeocoding.Utility;
using RTree.Engine;

namespace BAGeocoding.Bll.ImportData
{
    public class ImportDataPlaceManager
    {
        ///// <summary>
        ///// Tiến hành import đối tượng
        ///// </summary>
        //public static bool ImportPlace(List<UTLConditionImportPlace> condition)
        //{
        //    try
        //    {
        //        int provinceID = 0;
        //        for (int i = 0; i < condition.Count; i++)
        //        {
        //            if (condition[i].State == false)
        //                continue;
        //            provinceID = 0;
        //            if (ImportPlace(condition[i].Folder, ref provinceID) == false)
        //            {
        //                if (provinceID > 0 && provinceID < Int16.MaxValue)
        //                    PlaceDAO.Clear((short)provinceID);
        //                return false;
        //            }
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogFile.WriteData("ImportDataManager.ImportPlace, ex: " + ex.ToString());
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// Tiến hành import đối tượng
        ///// </summary>
        //private static bool ImportPlace(string folderStr, ref int provinceID)
        //{
        //    try
        //    {
        //        #region ==================== 1. Đọc thông tin từ file bản đồ ====================
        //        LogsProcess("---------------------------------------------------------------------------------------------------------------------------------------------------");
        //        LogsProcess(folderStr);
        //        // 1. Thông tin khu đô thị
        //        List<BAGPlace> urbanList = ReadPlaceList(EnumBAGPlaceType.Urban, string.Format(@"{0}\Place_Polyline_Urban.shp", folderStr), string.Format(@"{0}\Place_Polyline_Urban.txt", folderStr));
        //        if (urbanList == null || urbanList.Count == 0)
        //        {
        //            LogsProcess("Không đọc được thông tin khu đô thị");
        //            return false;
        //        }
        //        int parentID = urbanList[0].ParentID;
        //        if (urbanList.Exists(item => item.ParentID != parentID) == true)
        //        {
        //            LogsProcess("Lỗi thông tin khu đô thị (lệnh tỉnh)");
        //            return false;
        //        }
        //        provinceID = parentID;
        //        // 2. Thông tin lô đất
        //        List<BAGPlace> portionList = ReadPlaceList(EnumBAGPlaceType.Portion, string.Format(@"{0}\Place_Polyline_Portion.shp", folderStr), string.Format(@"{0}\Place_Polyline_Portion.txt", folderStr));
        //        if (portionList == null || portionList.Count == 0)
        //        {
        //            LogsProcess("Không đọc được thông tin lô đất");
        //            return false;
        //        }
        //        // 3. Thông tin ô đất
        //        List<BAGPlace> plotList = ReadPlacePlot(EnumBAGPlaceType.Plot, string.Format(@"{0}\Place_Polyline_Plot.shp", folderStr), string.Format(@"{0}\Place_Polyline_Plot.txt", folderStr), string.Format(@"{0}\Place_Point_Plot.shp", folderStr));
        //        if (plotList == null)
        //        {
        //            LogsProcess("Không đọc được thông tin khu ô đất");
        //            return false;
        //        }
        //        RTree<BAGPlace> rtreePlot = new RTree<BAGPlace>();
        //        for (int i = 0; i < plotList.Count; i++)
        //            rtreePlot.Add(plotList[i].GetRectangle(), plotList[i]);
        //        #endregion

        //        #region ==================== 2. Tiến hành thêm vào CSDL ====================
        //        int realID = 0;
        //        int indexID = 0;

        //        #region ==================== 2.1 Thông tin khu đô thị ====================
        //        for (int i = 0; i < urbanList.Count; i++)
        //        {
        //            if (PlaceDAO.Add(urbanList[i], ref realID) == false)
        //                return false;
        //            urbanList[i].RealID = realID;
        //            PlacePointDAO.AddStr(realID, urbanList[i]);
        //        }
        //        #endregion

        //        #region ==================== 2.2. Thông tin lô đất ====================
        //        for (int i = 0; i < portionList.Count; i++)
        //        {
        //            // 2.1 Tìm đối tượng khu đô thị
        //            indexID = urbanList.FindIndex(item => item.PlaceID == portionList[i].ParentID);
        //            if (indexID < 0)
        //            {
        //                LogsProcess(string.Format("Thiếu thông tin khu đô thị của lô đất, PlaceID: {0}, ParentID: {1}", portionList[i].PlaceID, portionList[i].ParentID));
        //                return false;
        //            }
        //            // 2.2 Gán lại parentID (ID khi lưu vào CSDL)
        //            portionList[i].ParentID = urbanList[indexID].RealID;
        //            // 2.3 Lưu vào CSDL
        //            if (PlaceDAO.Add(portionList[i], ref realID) == false)
        //                return false;
        //            portionList[i].RealID = realID;
        //            PlacePointDAO.AddStr(realID, portionList[i]);
        //        }
        //        #endregion

        //        #region ==================== 2.3. Thông tin ô đất ====================
        //        for (int i = 0; i < plotList.Count; i++)
        //        {
        //            // 3.1 Tìm đối tượng lô đất
        //            indexID = portionList.FindIndex(item => item.PlaceID == plotList[i].ParentID);
        //            if (indexID < 0)
        //            {
        //                LogsProcess(string.Format("Thiếu thông tin lô đất của ô đất, PlaceID: {0}, ParentID: {1}", plotList[i].PlaceID, plotList[i].ParentID));
        //                return false;
        //            }
        //            // 3.2 Gán lại parentID (ID khi lưu vào CSDL)
        //            plotList[i].ParentID = portionList[indexID].RealID;
        //            // 3.3 Lưu vào CSDL
        //            if (PlaceDAO.Add(plotList[i], ref realID) == false)
        //                return false;
        //            plotList[i].RealID = realID;
        //            PlacePointDAO.AddStr(realID, plotList[i]);
        //        }
        //        #endregion
        //        #endregion

        //        LogsProcess("Xử lý xong");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogFile.WriteData("ImportDataManager.ImportPlace, ex: " + ex.ToString());
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// Đọc dữ liệu đối tượng
        ///// </summary>
        //private static List<BAGPlace> ReadPlaceList(EnumBAGPlaceType placeType, string fileMap, string fileName)
        //{
        //    try
        //    {
        //        Hashtable htObjectName = ReadPlaceInfo(fileName);
        //        if (htObjectName == null || htObjectName.Count == 0)
        //            return new List<BAGPlace>();

        //        int nIndex = 1;
        //        OrgAPI ds = new OrgAPI(fileMap, 0);
        //        List<BAGPlace> placeList = new List<BAGPlace>();
        //        if (RunningParams.ImportFileType == EnumBAGFileType.Shp)
        //            nIndex = 0;
        //        int nFeature = ds.GetFeatureCount() + nIndex;
        //        for (int i = nIndex; i < nFeature; i++)
        //        {
        //            Feature f = ds.GetFeatureById(i);
        //            Geometry geo = f.GetGeometryRef();
        //            if (geo == null)
        //                continue;
        //            else if (geo.GetGeometryType() == ogr.wkbLineString)
        //            {
        //                #region ==================== Đọc thông tin từ file bản đồ ====================
        //                BAGPlace placeData = new BAGPlace { EnumTypeID = placeType };
        //                placeData.PlaceID = (int)f.GetFieldAsInteger("ID");
        //                placeData.ParentID = (int)f.GetFieldAsInteger("ParentID");
        //                if (htObjectName.ContainsKey(placeData.PlaceID) == true)
        //                    placeData.UpdateInfo((BAGPlace)htObjectName[placeData.PlaceID]);
        //                else
        //                    placeData.UpdateInfo(null);
                        
        //                placeData.PointList = new List<BAGPoint>();
        //                int nCount = geo.GetPointCount();
        //                for (int j = 0; j < nCount; j++)
        //                {
        //                    placeData.PointList.Add(new BAGPoint(geo.GetX(j), geo.GetY(j)));
        //                    if (j > 0)
        //                    {
        //                        placeData.LngStr += ",";
        //                        placeData.LatStr += ",";
        //                    }
        //                    placeData.LngStr += string.Format("{0:N8}", placeData.PointList[placeData.PointList.Count - 1].Lng);
        //                    placeData.LatStr += string.Format("{0:N8}", placeData.PointList[placeData.PointList.Count - 1].Lat);

        //                    if (j < nCount - 1)
        //                    {
        //                        placeData.Center.Lng += placeData.PointList[placeData.PointList.Count - 1].Lng;
        //                        placeData.Center.Lat += placeData.PointList[placeData.PointList.Count - 1].Lat;
        //                    }
        //                }
        //                if (nCount > 2)
        //                {
        //                    placeData.Center.Lng = placeData.Center.Lng / (nCount - 1);
        //                    placeData.Center.Lat = placeData.Center.Lat / (nCount - 1);
        //                }
        //                placeList.Add(placeData);
        //                #endregion
        //            }
        //        }

        //        return placeList;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogFile.WriteData("ImportDataManager.ReadPlaceList, ex: " + ex.ToString()); 
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// Đọc dữ liệu đối tượng
        ///// </summary>
        //private static List<BAGPlace> ReadPlacePlot(EnumBAGPlaceType placeType, string fileMap, string fileName, string filePoint)
        //{
        //    #region ==================== Đọc thông tin từ file bản đồ ô đất ====================
        //    List<BAGPlace> plotList = ReadPlaceList(EnumBAGPlaceType.Plot, fileMap, fileName);
        //    if (plotList == null)
        //    {
        //        LogsProcess("Không đọc được thông tin khu ô đất");
        //        return null;
        //    }
        //    RTree<BAGPlace> rtreePlot = new RTree<BAGPlace>();
        //    for (int i = 0; i < plotList.Count; i++)
        //        rtreePlot.Add(plotList[i].GetRectangle(), plotList[i]);
        //    #endregion

        //    #region ==================== Đọc thông tin từ file bản đồ điểm ====================
        //    try
        //    {
        //        int nIndex = 1;
        //        OrgAPI ds = new OrgAPI(filePoint, 0);
        //        List<BAGPoint> pointList = new List<BAGPoint>();
        //        if (RunningParams.ImportFileType == EnumBAGFileType.Shp)
        //            nIndex = 0;
        //        int placeID = 0;
        //        int nFeature = ds.GetFeatureCount() + nIndex;
        //        for (int i = nIndex; i < nFeature; i++)
        //        {
        //            Feature f = ds.GetFeatureById(i);
        //            Geometry geo = f.GetGeometryRef();
        //            if (geo == null)
        //                continue;
        //            else if (geo.GetGeometryType() == ogr.wkbPoint)
        //            {
        //                placeID = 0;
        //                BAGPoint pointInfo = new BAGPoint(geo.GetX(0), geo.GetY(0));
        //                List<BAGPlace> placeResult = rtreePlot.Intersects(pointInfo.ToRectangle(0.00005)); //0.00005 ~ 5.5 (m)
        //                if (placeResult == null)
        //                    continue;
        //                else if (placeResult.Count == 1)
        //                    placeID = placeResult[0].PlaceID;
        //                else
        //                {
        //                    for (int j = 0; j < placeResult.Count - 1; j++)
        //                    {
        //                        if (MapUtilityManager.CheckInsidePolygon(placeResult[j].PointList, pointInfo) == true)
        //                        {
        //                            placeID = placeResult[j].PlaceID;
        //                            break;
        //                        }
        //                    }
        //                    if (placeID == 0)
        //                        placeID = placeResult[placeResult.Count - 1].PlaceID;
        //                }

        //                if (placeID > 0)
        //                {
        //                    for (int j = 0; j < plotList.Count; j++)
        //                    {
        //                        if (plotList[j].PlaceID == placeID)
        //                        {
        //                            pointInfo.D2Start = pointInfo.Distance(plotList[j].Center);
        //                            plotList[j].Center = new BAGPoint(pointInfo);
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogFile.WriteData("ImportDataManager.ReadPlacePlot, ex: " + ex.ToString());
        //        return null;
        //    }
        //    #endregion

        //    return plotList;
        //}

        ///// <summary>
        ///// Đọc thông tin đối tượng
        ///// </summary>
        //private static Hashtable ReadPlaceInfo(string fileName)
        //{
        //    try
        //    {
        //        int indexID = -1;
        //        int indexName = -1;
        //        int indexAdd = -1;
        //        int indexDes = -1;

        //        int objectID = 0;
        //        Hashtable htObjectName = new Hashtable();
        //        StreamReader streamReader = new StreamReader(fileName, Encoding.UTF8);
        //        while (streamReader.EndOfStream == false)
        //        {
        //            List<string> lineData = StringUlt.StringAnalyze(streamReader.ReadLine(), ',');

        //            if (indexID < 0 || indexName < 0)
        //            {
        //                #region ==================== Lay chi muc du lieu ====================
        //                for (int i = 0; i < lineData.Count; i++)
        //                {
        //                    switch (lineData[i].Trim().ToUpper())
        //                    {
        //                        case "ID":
        //                            indexID = i;
        //                            break;
        //                        case "NAME":
        //                            indexName = i;
        //                            break;
        //                        case "ADDRESS":
        //                            indexAdd = i;
        //                            break;
        //                        case "DESCRIPTIO":
        //                            indexDes = i;
        //                            break;
        //                        default:
        //                            break;
        //                    }
        //                }
        //                continue;
        //                #endregion
        //            }
        //            else
        //            {
        //                #region ==================== Xu ly du lieu ====================
        //                objectID = Convert.ToInt32(lineData[indexID]);
        //                if (htObjectName.ContainsKey(objectID) == false)
        //                {
        //                    if (indexAdd > 0)
        //                        htObjectName.Add(objectID, new BAGPlace { Name = lineData[indexName].Trim(), Address = lineData[indexAdd].Trim(), Description = lineData[indexDes].Trim() });
        //                    else
        //                        htObjectName.Add(objectID, new BAGPlace { Name = lineData[indexName].Trim(), Address = string.Empty, Description = lineData[indexDes].Trim() });
        //                }
        //                else
        //                    LogFile.WriteProcess(string.Format("Lỗi trùng dữ liệu tên đối tượng, ObjectID: {0}", objectID));
        //                #endregion

        //            }
        //        }
        //        return htObjectName;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogFile.WriteData(string.Format("ImportDataPlaceManager.ReadPlaceInfo({0}), ex: {1}", fileName, ex));
        //        return null;
        //    }
        //}

        //private static void LogsProcess(string msg)
        //{
        //    LogFile.WriteProcess(msg);
        //}
    }
}
