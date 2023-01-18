using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using OSGeo.OGR;

using RTree.Engine;
using RTree.Engine.Entity;

//using BAGeocoding.Dal.MapTool;

using BAGeocoding.Entity;
using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.Enum.ActionData;
using BAGeocoding.Entity.Enum.MapTool;
using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.MapTool;
using BAGeocoding.Entity.MapTool.Plan;

using BAGeocoding.Utility;
using BAGeocoding.Entity.ConvertData;

namespace BAGeocoding.Bll.ImportData
{
    public class WRKPlanDataManager
    {
        ///// <summary>
        ///// Import dữ liệu điểm
        ///// </summary>
        //public static bool ImportPoint(WRKPlan planInfo, string fileMap, string fileName, ref EnumWPLImportData state)
        //{
        //    try
        //    {
        //        // 1. Xây dựng RTree
        //        state = EnumWPLImportData.Success;
        //        RTree<MCLGrid> gridRTree = InitGridRTree(planInfo, ref state);
        //        if (gridRTree == null || gridRTree.Count == 0)
        //            return false;
        //        // 2. Lấy dữ liệu tên
        //        Hashtable htObjectName = ReadObjectName(EnumBAGObjectType.Point, fileName, ref state);
        //        if (htObjectName == null || htObjectName.Count == 0)
        //            return false;
        //        // 3. Đọc dữ liệu
        //        OrgAPI ds = new OrgAPI(fileMap, 0);
        //        int nIndex = 1;
        //        if (RunningParams.ImportFileType == EnumBAGFileType.Shp)
        //            nIndex = 0;
        //        int nFeature = ds.GetFeatureCount() + nIndex;
        //        for (int i = nIndex; i < nFeature; i++)
        //        {
        //            Feature f = ds.GetFeatureById(i);
        //            Geometry geo = f.GetGeometryRef();
        //            if (geo == null)
        //                continue;
        //            int gtype = geo.GetGeometryType();
        //            if (gtype == ogr.wkbPoint)
        //            {
        //                #region ==================== Đọc thông tin từ file bản đồ ====================
        //                WRKPlanPoint point = new WRKPlanPoint();
        //                point.MigrateID = f.GetFieldAsInteger("ID");
        //                point.PlanID = planInfo.PlanID;
        //                point.KindID = (short)f.GetFieldAsInteger("Kind");
        //                if (htObjectName.ContainsKey(point.MigrateID) == true)
        //                {
        //                    WRKPlanObjBase objectInfo = (WRKPlanObjBase)htObjectName[point.MigrateID];
        //                    point.Name = objectInfo.Name;
        //                    point.NoteNew = objectInfo.NoteNew;
        //                }
        //                else
        //                {
        //                    point.Name = string.Empty;
        //                    point.NoteNew = string.Empty;
        //                }
        //                point.Coords = new BAGPoint(geo.GetX(0), geo.GetY(0));

        //                point.ImageSrc = string.Empty;
        //                point.NoteOld = string.Empty;

        //                point.StateOptsSet(EnumMTLStateOption.IsVisible, true);
        //                point.ActionID = 0;
        //                point.EnumApprovedState = EnumMTLApprovedState.Generate;
        //                point.EditorID = RunningParams.USER.UserID;
        //                point.SyncTokenID = string.Empty;
        //                #endregion

        //                #region ==================== Bổ sung thông tin Grid và thêm vào CSDL ====================
        //                DetectGridInfo(gridRTree, ref point);
        //                if (point.GridEdit == 0)
        //                    LogFile.WriteData(string.Format("Đường nằm ngoài grid, SegmentID = {0}", point.MigrateID));
        //                else if (WRKPlanPointDAO.Generate(point) == false)
        //                    LogFile.WriteData(string.Format("Lỗi import dữ liệu đường, SegmentID = {0}", point.MigrateID));
        //                #endregion
        //            }
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogFile.WriteData("WRKPlanDataManager.ImportPoint, ex: " + ex.ToString());
        //        return false;
        //    }
        //}

        /// <summary>
        /// Import dữ liệu đường
        /// </summary>
        public static bool ImportSegment(WRKPlan planInfo, string fileMap, string fileName, ref EnumWPLImportData state)
        {
            try
            {
                // 1. Xây dựng RTree
                state = EnumWPLImportData.Success;
                RTree<MCLGrid> gridRTree = InitGridRTree(planInfo, ref state);
                if (gridRTree == null || gridRTree.Count == 0)
                    return false;
                // 2. Lấy dữ liệu tên
                Hashtable htObjectName = ReadObjectName(EnumBAGObjectType.Polyline, fileName, ref state);
                if (htObjectName == null || htObjectName.Count == 0)
                    return false;
                // 3. Đọc file dữ liệu
                OrgAPI ds = new OrgAPI(fileMap, 0);
                int nIndex = 1;
                if (RunningParams.ImportFileType == EnumBAGFileType.Shp)
                    nIndex = 0;
                //int nFeature = ds.GetFeatureCount() + nIndex;// Chanh
                long nFeature = ds.GetFeatureCount() + nIndex;
                for (int i = nIndex; i < nFeature; i++)
                {
                    Feature f = ds.GetFeatureById(i);
                    Geometry geo = f.GetGeometryRef();
                    if (geo == null)
                        continue;
                    //int gtype = geo.GetGeometryType();//Chanh
                    wkbGeometryType gtype = geo.GetGeometryType();
                    //if (gtype == ogr.wkbLineString) Chanh
                    if (gtype == wkbGeometryType.wkbLineString)
                    {
                        #region ==================== Đọc thông tin từ file bản đồ ====================
                        WRKPlanSegment segment = new WRKPlanSegment();
                        segment.MigrateID = f.GetFieldAsInteger("ID");
                        segment.PlanID = planInfo.PlanID;
                        if (htObjectName.ContainsKey(segment.MigrateID) == true)
                        {
                            WRKPlanObjBase objectInfo = (WRKPlanObjBase)htObjectName[segment.MigrateID];
                            segment.Name = objectInfo.Name;
                            segment.NoteNew = objectInfo.NoteNew;
                        }
                        else
                        {
                            segment.Name = string.Empty;
                            segment.NoteNew = string.Empty;
                        }
                        try
                        {

                            segment.Direction = (byte)f.GetFieldAsInteger("Direction");
                        }
                        catch (Exception abx)
                        {
                            LogFile.WriteError(abx.ToString());
                            return false;
                        }
                        segment.ClassFunc = (byte)f.GetFieldAsInteger("ClassFunc");
                        segment.LevelID = (short)f.GetFieldAsInteger("Level");
                        segment.KindID = (byte)f.GetFieldAsInteger("Kind");

                        segment.StartLeft = (short)f.GetFieldAsInteger("StartLeft");
                        segment.StartRight = (short)f.GetFieldAsInteger("StartRight");
                        segment.EndLeft = (short)f.GetFieldAsInteger("EndLeft");
                        segment.EndRight = (short)f.GetFieldAsInteger("EndRight");

                        segment.MinSpeed = (short)f.GetFieldAsInteger("MinSpeed");
                        segment.MaxSpeed = (short)f.GetFieldAsInteger("MaxSpeed");

                        segment.RoadOptsSet(EnumMTLRoadOption.IsNumber, f.GetFieldAsInteger("IsNumber") > 0);
                        segment.RoadOptsSet(EnumMTLRoadOption.IsBridge, f.GetFieldAsInteger("IsBridge") > 0);
                        segment.RoadOptsSet(EnumMTLRoadOption.IsPrivate, f.GetFieldAsInteger("IsPrivate") > 0);
                        segment.RoadOptsSet(EnumMTLRoadOption.IsPed, f.GetFieldAsInteger("IsPed") > 0);
                        segment.RoadOptsSet(EnumMTLRoadOption.IsFee, false);

                        segment.RoadOptsSet(EnumMTLRoadOption.AllowPed, f.GetFieldAsInteger("AllowPed") > 0);
                        segment.RoadOptsSet(EnumMTLRoadOption.AllowWalk, f.GetFieldAsInteger("AllowMoto") > 0);
                        segment.RoadOptsSet(EnumMTLRoadOption.AllowBicycle, f.GetFieldAsInteger("AllowMoto") > 0);
                        segment.RoadOptsSet(EnumMTLRoadOption.AllowMoto, f.GetFieldAsInteger("AllowMoto") > 0);
                        segment.RoadOptsSet(EnumMTLRoadOption.AllowCar, f.GetFieldAsInteger("AllowCar") > 0);
                        segment.RoadOptsSet(EnumMTLRoadOption.AllowBus, f.GetFieldAsInteger("AllowBus") > 0);
                        segment.RoadOptsSet(EnumMTLRoadOption.AllowTruck, f.GetFieldAsInteger("AllowTruck") > 0);
                        segment.RoadOptsSet(EnumMTLRoadOption.AllowTaxi, f.GetFieldAsInteger("AllowCar") > 0);

                        segment.RoadLength = 0;
                        segment.PointCount = (short)geo.GetPointCount();
                        segment.PointList = new List<BAGPoint>();
                        for (int j = 0; j < segment.PointCount; j++)
                        {
                            segment.PointList.Add(new BAGPoint(geo.GetX(j), geo.GetY(j)));
                            if (j > 0)
                                segment.RoadLength += (decimal)segment.PointList[j].Distance(segment.PointList[j - 1]);
                        }
                        segment.Coords = MapHelper.PolylineAlgorithmEncode(segment.PointList);

                        segment.NoteOld = string.Empty;

                        segment.StateOptsSet(EnumMTLStateOption.IsVisible, true);
                        segment.ActionID = 0;
                        segment.ApprovedState = (byte)f.GetFieldAsInteger("AppStatus");     //ApprovedState
                        segment.EditorID = RunningParams.USER.UserID;
                        #endregion

                        #region ==================== Bổ sung thông tin Grid và thêm vào CSDL ====================
                        /*Chanh Start*/
                        //DetectGridInfo(gridRTree, ref segment);
                        //if (segment.GridEdit == 0)
                        //    LogFile.WriteData(string.Format("Đường nằm ngoài grid, SegmentID = {0}", segment.MigrateID));
                        //else if (WRKPlanSegmentDAO.Generate(segment) == false)
                        //    LogFile.WriteData(string.Format("Lỗi import dữ liệu đường, SegmentID = {0}", segment.MigrateID));
                        /*Chanh End*/
                        #endregion

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("WRKPlanDataManager.ImportPoint, ex: " + ex.ToString());
                state = EnumWPLImportData.ObjectError;
                return false;
            }
        }

        /// <summary>
        /// Xây dựng RTree grid của kế hoạch
        /// </summary>
        private static RTree<MCLGrid> InitGridRTree(WRKPlan planInfo, ref EnumWPLImportData state)
        {
            try
            {
                /*Chanh Start*/
                //List<WRKPlanGrid> gridList = WRKPlanGridDAO.GetByPlan(planInfo.PlanID, false);
                //if (gridList == null || gridList.Count == 0)
                //{
                //    state = EnumWPLImportData.GridMissing;
                //    return null;
                //}
                /*Chanh End*/
                RTree<MCLGrid> gridRTree = new RTree<MCLGrid>();
                //for (int i = 0; i < gridList.Count; i++)
                //    gridRTree.Add(gridList[i].GridInfo.GetRectangle(), gridList[i].GridInfo);
                return gridRTree;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("WRKPlanDataManager.InitGridRTree, ex: " + ex.ToString());
                state = EnumWPLImportData.GridError;
                return null;
            }
        }

        /// <summary>
        /// Đọc tên đối tượng
        /// </summary>
        private static Hashtable ReadObjectName(EnumBAGObjectType typeID, string fileName, ref EnumWPLImportData state)
        {
            int indexId = -1;
            int indexName = -1;
            int indexNote = -1;
            long objectID = 0;
            List<string> lineData = null;
            Hashtable htObjectName = new Hashtable();
            try
            {
                StreamReader streamReader = new StreamReader(fileName, Encoding.UTF8);
                while (streamReader.EndOfStream == false)
                {
                    lineData = StringUlt.StringAnalyze(streamReader.ReadLine(), ',');

                    if (indexId < 0 || indexName < 0)
                    {
                        #region ==================== Lay chi muc du lieu ====================
                        for (int i = 0; i < lineData.Count; i++)
                        {
                            switch (lineData[i].Trim().ToUpper())
                            {
                                case "ID":
                                    indexId = i;
                                    break;
                                case "NAME":
                                    indexName = i;
                                    break;
                                case "NOTE":
                                    indexNote = i;
                                    break;
                                default:
                                    break;
                            }
                        }
                        continue;
                        #endregion
                    }
                    else
                    {
                        #region ==================== Xu ly du lieu ====================
                        objectID = Convert.ToInt64(lineData[indexId]);
                        if (htObjectName.ContainsKey(objectID) == false)
                        {
                            if (indexNote > -1)
                                htObjectName.Add(objectID, new WRKPlanObjBase { Name = lineData[indexName].Trim(), NoteNew = lineData[indexNote].Trim() });
                            else
                                htObjectName.Add(objectID, new WRKPlanObjBase { Name = lineData[indexName].Trim(), NoteNew = string.Empty });
                        }
                        else
                            LogFile.WriteProcess(string.Format("Lỗi trùng dữ liệu tên đối tượng, ObjectID: {0}", objectID));
                        #endregion
                    }
                }
                return htObjectName;
            }
            catch (Exception ex)
            {
                LogFile.WriteData(string.Format("WRKPlanDataManager.ReadObjectName({0}), ex: {1}", fileName, ex));
                state = EnumWPLImportData.NameError;
                return null;
            }
        }

        ///// <summary>
        ///// Xác định mã Grid theo điểm
        ///// </summary>
        //private static int DetectGridItem(RTree<MCLGrid> gridRTree, BAGPoint pts)
        //{
        //    double its = Constants.DISTANCE_INTERSECT_ROAD;
        //    RTRectangle rec = new RTRectangle(pts.Lng - its, pts.Lat - its, pts.Lng + its, pts.Lat + its, 0.0f, 0.0f);
        //    List<MCLGrid> result = gridRTree.Intersects(rec);
        //    if (result == null || result.Count == 0)
        //        return 0;
        //    else if (result.Count == 1)
        //        return result[0].GridID;
        //    else
        //    {
        //        for (int i = 0; i < result.Count - 1; i++)
        //        {
        //            if (MapUtilityManager.CheckInsidePolygon(result[i].PointList, pts) == true)
        //                return result[i].GridID;
        //        }
        //        return result[result.Count - 1].GridID;
        //    }
        //}

        ///// <summary>
        ///// Xác định grid cho điểm
        ///// </summary>
        //private static void DetectGridInfo(RTree<MCLGrid> gridRTree, ref WRKPlanPoint point)
        //{
        //    point.GridEdit = DetectGridItem(gridRTree, point.Coords);
        //    point.GridView = point.GridEdit.ToString();
        //}

        ///// <summary>
        ///// Xác định grid cho đường
        ///// </summary>
        //private static void DetectGridInfo(RTree<MCLGrid> gridRTree, ref WRKPlanSegment segment)
        //{
        //    int gridID = 0;
        //    Hashtable htGridID = new Hashtable();
        //    for (int i = 0; i < segment.PointList.Count; i++)
        //    {
        //        gridID = DetectGridItem(gridRTree, segment.PointList[i]);
        //        if (gridID == 0)
        //            continue;
        //        else if (htGridID.ContainsKey(gridID) == true)
        //            continue;
        //        if (segment.GridEdit == 0)
        //            segment.GridEdit = gridID;
        //        if (segment.GridView.Length > 0)
        //            segment.GridView += ",";
        //        segment.GridView += string.Format("{0}", gridID);
        //        htGridID.Add(gridID, null);
        //    }
        //}


        //#region ==================== Export dữ liệu của plan ====================
        //public static bool ExportPoint(WRKPlan planInfo, WRKPlanOptionExport exportOpt)
        //{
        //    try
        //    {
        //        // 1. Lấy dữ liệu
        //        List<WRKPlanPoint> objectList = exportOpt.EnumState == EnumMTLExportDataState.Normal ? WRKPlanPointDAO.GetForExport(planInfo.PlanID, exportOpt) : WRKPlanPointDAO.GetExportNone(planInfo.PlanID, exportOpt);
        //        if (objectList == null || objectList.Count == 0)
        //            return false;

        //        //List<JSNPoint01> tempList = new List<JSNPoint01>();
        //        //using (StreamReader streamReader = new StreamReader(@"D:\Working\BAGeocoding\BAGeocoding.Tool\bin\Debug\Data\json.txt", Encoding.UTF8))
        //        //{
        //        //    tempList = StringUlt.Json2Object<List<JSNPoint01>>(streamReader.ReadToEnd().Trim());
        //        //    streamReader.Close();
        //        //}
        //        //if (tempList.Count == 0)
        //        //    return false;
        //        //List<WRKPlanPoint> objectList = new List<WRKPlanPoint>();
        //        //for (int i = 0; i < tempList.Count; i++)
        //        //{
        //        //    WRKPlanPoint pointInfo = new WRKPlanPoint { KindID = Convert.ToInt16(Convert.ToDecimal(tempList[i].speedlimit)), Name = tempList[i].name, ImageSrc = tempList[i].id, NoteOld = tempList[i].type };
        //        //    pointInfo.Coords = new BAGPoint { Lng = Convert.ToDouble(tempList[i].longitude), Lat = Convert.ToDouble(tempList[i].latitude) };
        //        //    objectList.Add(pointInfo);
        //        //}

        //        // 2. Tiến hành ghi file
        //        // 2.1 File mở rộng .mif
        //        string fileNameMif = exportOpt.FileName.Replace(".MIF", ".mif");
        //        if (File.Exists(fileNameMif))
        //            File.Delete(fileNameMif);
        //        using (StreamWriter streamWrite = new StreamWriter(fileNameMif, false, Encoding.ASCII))
        //        {
        //            streamWrite.Write(PointFileMif(objectList));
        //        }

        //        // 2.2 File mở rộng .mid 
        //        string fileNameMid = exportOpt.FileName.Replace(".mif", ".mid").Replace(".MIF", ".mid");
        //        if (File.Exists(fileNameMid))
        //            File.Delete(fileNameMid);
        //        using (StreamWriter streamWrite = new StreamWriter(fileNameMid, false, Encoding.Unicode))
        //        {
        //            for (var i = 0; i < objectList.Count; i++)
        //                streamWrite.WriteLine(PointFileMid(objectList[i]));
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogFile.WriteData("WRKPlanDataManager.ExportPoint, ex: " + ex.ToString());
        //        return false;
        //    }
        //}

        //private static StringBuilder PointFileMif(List<WRKPlanPoint> objectList)
        //{
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendLine("VERSION 300");
        //    builder.AppendLine("CHARSET \"Neutral\"");
        //    builder.AppendLine("Delimiter \",\"");
        //    builder.AppendLine("CoordSys Earth Projection 1, 104");
        //    builder.AppendLine("Columns 8");
        //    builder.AppendLine("  PLNPoiID Integer");
        //    builder.AppendLine("  PointID Integer");
        //    builder.AppendLine("  KindID Integer");
        //    builder.AppendLine("  Name Char(256)");
        //    builder.AppendLine("  ImageSrc Char(512)");
        //    builder.AppendLine("  Note Char(1024)");
        //    builder.AppendLine("  EditorName Char(256)");
        //    builder.AppendLine("  ApprovedState Integer");
        //    builder.AppendLine("DATA");

        //    if (objectList != null)
        //    {
        //        for (var i = 0; i < objectList.Count; i++)
        //        {
        //            builder.AppendLine(string.Format("POINT {0:N8} {1:N8}", objectList[i].Coords.Lng, objectList[i].Coords.Lat));
        //            builder.AppendLine("    Symbol (34, 0, 12)");
        //        }
        //    }
        //    return builder;
        //}

        //private static string PointFileMid(WRKPlanPoint objectItem)
        //{
        //    string editerName = objectItem.EditorID.ToString();
        //    int indexID = RunningParams.CACHE.UserList.FindIndex(item => item.UserID == objectItem.EditorID);
        //    if (indexID > -1)
        //        editerName = RunningParams.CACHE.UserList[indexID].FullName;
        //    string original = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
        //                                    objectItem.PLNPoiID,
        //                                    objectItem.MigrateID,
        //                                    objectItem.KindID,
        //                                    objectItem.Name,
        //                                    objectItem.ImageSrc,
        //                                    objectItem.NoteOld.IndexOf(",") > -1 ? "\"" + objectItem.NoteOld.Replace(",", " ") + "\"" : objectItem.NoteOld,
        //                                    editerName,
        //                                    objectItem.ApprovedState);
        //    return original;

        //    // Get UTF16 bytes and convert UTF16 bytes to UTF8 bytes
        //    //byte[] utf16Bytes = Encoding.Unicode.GetBytes(original);
        //    //byte[] utf8Bytes = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, utf16Bytes);
        //    //return Encoding.UTF8.GetString(utf8Bytes);
        //}

        //public static bool ExportSegment(WRKPlan planInfo, WRKPlanOptionExport exportOpt)
        //{
        //    try
        //    {
        //        // 1. Lấy dữ liệu
        //        List<WRKPlanSegment> objectList = WRKPlanSegmentDAO.GetForExport(planInfo.PlanID, exportOpt);
        //        if (objectList == null || objectList.Count == 0)
        //            return false;

        //        // 2. Tiến hành ghi file
        //        // 2.1 File mở rộng .mif
        //        string fileNameMif = exportOpt.FileName;
        //        if (File.Exists(fileNameMif))
        //            File.Delete(fileNameMif);
        //        using (StreamWriter streamWrite = new StreamWriter(fileNameMif, false, Encoding.ASCII))
        //        {
        //            streamWrite.Write(SegmentFileMif(objectList));
        //        }

        //        // 2.2 File mở rộng .mid 
        //        string fileNameMid = exportOpt.FileName.ToLower().Replace(".mif", ".mid");
        //        if (File.Exists(fileNameMid))
        //            File.Delete(fileNameMid);
        //        using (StreamWriter streamWrite = new StreamWriter(fileNameMid, false, Encoding.Unicode))
        //        {
        //            streamWrite.Write(SegmentFileMid(objectList));
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogFile.WriteData("WRKPlanDataManager.ExportSegment, ex: " + ex.ToString());
        //        return false;
        //    }
        //}

        //private static StringBuilder SegmentFileMif(List<WRKPlanSegment> objectList)
        //{
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendLine("VERSION 300");
        //    builder.AppendLine("CHARSET \"Neutral\"");
        //    builder.AppendLine("Delimiter \",\"");
        //    builder.AppendLine("CoordSys Earth Projection 1, 104");
        //    builder.AppendLine("Columns 14");
        //    builder.AppendLine("  PLNSegID Integer");
        //    builder.AppendLine("  SegmentID Integer");
        //    builder.AppendLine("  Name Char(256)");
        //    builder.AppendLine("  Direction Integer");
        //    builder.AppendLine("  ClassFunc Integer");
        //    builder.AppendLine("  StartLeft Integer");
        //    builder.AppendLine("  EndLeft Integer");
        //    builder.AppendLine("  StartRight Integer");
        //    builder.AppendLine("  EndRight Integer");
        //    builder.AppendLine("  Note Char(1024)");
        //    builder.AppendLine("  DeleteObject Integer");
        //    builder.AppendLine("  UpdateInfo Integer");
        //    builder.AppendLine("  EditorName Char(256)");
        //    builder.AppendLine("  ApprovedState Integer");
        //    builder.AppendLine("DATA");

        //    if (objectList != null)
        //    {
        //        List<BAGPoint> pointList;
        //        for (var i = 0; i < objectList.Count; i++)
        //        {
        //            pointList = objectList[i].Coords.PolylineAlgorithmDecode().ToList();

        //            if (pointList.Count == 2)
        //            {
        //                builder.AppendLine(string.Format("LINE {0:N8} {1:N8} {2:N8} {3:N8}", pointList[0].Lng, pointList[0].Lat, pointList[1].Lng, pointList[1].Lat));
        //                builder.AppendLine(string.Format("PEN (1, 2, {0})", objectList[i].MigrateID));
        //            }
        //            else
        //            {
        //                builder.AppendLine(string.Format("PLINE {0}", pointList.Count));
        //                for (var j = 0; j < pointList.Count; j++)
        //                    builder.AppendLine(string.Format("{0:N8} {1:N8}", pointList[j].Lng, pointList[j].Lat));
        //                builder.AppendLine(string.Format("PEN (1, 2, {0})", objectList[i].MigrateID));
        //            }
        //        }
        //    }
        //    return builder;
        //}

        //private static string SegmentFileMid(List<WRKPlanSegment> objectList)
        //{
        //    string editerName = string.Empty;
        //    StringBuilder builder = new StringBuilder();
        //    for (var i = 0; i < objectList.Count; i++)
        //    {
        //        editerName = objectList[i].EditorID.ToString();
        //        int indexID = RunningParams.CACHE.UserList.FindIndex(item => item.UserID == objectList[i].EditorID);
        //        if (indexID > -1)
        //            editerName = RunningParams.CACHE.UserList[indexID].FullName;
        //        builder.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}",
        //            objectList[i].PLNSegID,
        //            objectList[i].MigrateID,
        //            objectList[i].Name.Replace(",", " "),
        //            objectList[i].Direction,
        //            objectList[i].ClassFunc,
        //            objectList[i].StartLeft,
        //            objectList[i].EndLeft,
        //            objectList[i].StartRight,
        //            objectList[i].EndRight,
        //            objectList[i].NoteOld.Replace(",", " "),
        //            objectList[i].IsDelete() ? 1 : 0,
        //            objectList[i].IsUpdate() ? 1 : 0,
        //            editerName,
        //            objectList[i].ApprovedState));
        //    }
        //    return builder.ToString();
        //}
        //#endregion
    }
}
