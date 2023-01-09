using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using OSGeo.OGR;

using BAGeocoding.Entity;
using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.Enum.MapTool;
using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.MapTool;
using BAGeocoding.Utility;

namespace BAGeocoding.Bll.ImportData
{
    public class WRKPlanApprovedManager
    {
        /// <summary>
        /// Import dữ liệu điểm
        /// </summary>
        public static List<WRKPlanPoint> LoadPoint(string fileMap, string fileName)
        {
            try
            {
                // 2. Lấy dữ liệu tên
                Hashtable htObjectName = ReadObjectName(EnumBAGObjectType.Point, fileName);
                if (htObjectName == null || htObjectName.Count == 0)
                    return null;
                // 3. Đọc dữ liệu
                OrgAPI ds = new OrgAPI(fileMap, 0);
                int nIndex = 1;
                if (RunningParams.ImportFileType == EnumBAGFileType.Shp)
                    nIndex = 0;
                int nFeature = ds.GetFeatureCount() + nIndex;
                List<WRKPlanPoint> result = new List<WRKPlanPoint>();
                for (int i = nIndex; i < nFeature; i++)
                {
                    Feature f = ds.GetFeatureById(i);
                    Geometry geo = f.GetGeometryRef();
                    if (geo == null)
                        continue;
                    int gtype = geo.GetGeometryType();
                    if (gtype == ogr.wkbPoint)
                    {
                        #region ==================== Đọc thông tin từ file bản đồ ====================
                        WRKPlanPoint point = new WRKPlanPoint();
                        point.PLNPoiID = (long)f.GetFieldAsDouble("PLNPoiID");
                        if (point.PLNPoiID == 0)
                            continue;
                        else if (result.Exists(item => item.PLNPoiID == point.PLNPoiID) == true)
                            continue;
                        point.KindID = (short)f.GetFieldAsInteger("Kind");
                        if (htObjectName.ContainsKey(point.PLNPoiID) == true)
                        {
                            WRKPlanObjBase objectInfo = (WRKPlanObjBase)htObjectName[point.PLNPoiID];
                            point.Name = objectInfo.Name;
                            point.NoteOld = objectInfo.NoteOld;
                            point.NoteNew = objectInfo.NoteNew;
                        }
                        else
                        {
                            point.Name = string.Empty;
                            point.NoteNew = string.Empty;
                        }
                        point.Coords = new BAGPoint(geo.GetX(0), geo.GetY(0));
                        point.ActionID = 0;
                        point.ApprovedState = (byte)f.GetFieldAsInteger("AppStatus");
                        #endregion

                        result.Add(point);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("WRKPlanDataManager.ApprovedPoint, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Import dữ liệu đường
        /// </summary>
        public static List<WRKPlanSegment> LoadSegment(string fileMap, string fileName)
        {
            try
            {
                // 1. Lấy dữ liệu tên
                Hashtable htObjectName = ReadObjectName(EnumBAGObjectType.Polyline, fileName);
                if (htObjectName == null || htObjectName.Count == 0)
                    return null;
                // 3. Đọc file dữ liệu
                OrgAPI ds = new OrgAPI(fileMap, 0);
                int nIndex = 1;
                if (RunningParams.ImportFileType == EnumBAGFileType.Shp)
                    nIndex = 0;
                int nFeature = ds.GetFeatureCount() + nIndex;
                List<WRKPlanSegment> result = new List<WRKPlanSegment>();
                for (int i = nIndex; i < nFeature; i++)
                {
                    Feature f = ds.GetFeatureById(i);
                    Geometry geo = f.GetGeometryRef();
                    if (geo == null)
                        continue;
                    int gtype = geo.GetGeometryType();
                    if (gtype == ogr.wkbLineString)
                    {
                        #region ==================== Đọc thông tin từ file bản đồ ====================
                        WRKPlanSegment segment = new WRKPlanSegment();
                        segment.PLNSegID = f.GetFieldAsInteger("PLNSegID");
                        if (segment.PLNSegID == 0)
                            continue;
                        else if (result.Exists(item => item.PLNSegID == segment.PLNSegID) == true)
                            continue;
                        if (htObjectName.ContainsKey(segment.PLNSegID) == true)
                        {
                            WRKPlanObjBase objectInfo = (WRKPlanObjBase)htObjectName[segment.PLNSegID];
                            segment.Name = objectInfo.Name;
                            segment.NoteNew = objectInfo.NoteNew;
                        }
                        else
                        {
                            segment.Name = string.Empty;
                            segment.NoteNew = string.Empty;
                        }
                        segment.Direction = (byte)f.GetFieldAsInteger("Direction");
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
                            if(j > 0)
                                segment.RoadLength += (decimal)segment.PointList[j].Distance(segment.PointList[j - 1]);
                        }
                        segment.Coords = MapHelper.PolylineAlgorithmEncode(segment.PointList);

                        segment.NoteOld = string.Empty;

                        segment.StateOptsSet(EnumMTLStateOption.IsVisible, true);
                        segment.ActionID = 0;
                        segment.ApprovedState = (byte)f.GetFieldAsInteger("AppStatus");     //ApprovedState
                        segment.EditorID = RunningParams.USER.UserID;
                        #endregion

                        result.Add(segment);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("WRKPlanApprovedManager.ApprovedSegment, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Đọc tên đối tượng
        /// </summary>
        private static Hashtable ReadObjectName(EnumBAGObjectType typeID, string fileName)
        {
            int indexId = -1;
            int indexName = -1;
            int indexAdds = -1;
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
                                case "PLNSEGID":
                                    if(typeID == EnumBAGObjectType.Polyline)
                                        indexId = i;
                                    break;
                                case "PLNPOIID":
                                    if (typeID == EnumBAGObjectType.Point)
                                        indexId = i;
                                    break;
                                case "NAME":
                                    indexName = i;
                                    break;
                                case "ADDRESS":
                                    indexAdds = i;
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
                        objectID = (long)Convert.ToDecimal(lineData[indexId]);
                        if (objectID == 0)
                            continue;
                        else if (htObjectName.ContainsKey(objectID) == false)
                            htObjectName.Add(objectID, new WRKPlanObjBase { 
                                Name = lineData[indexName].Trim(), 
                                NoteOld = indexAdds > -1 ? lineData[indexAdds].Trim() : string.Empty,
                                NoteNew = indexNote > -1 ? lineData[indexNote].Trim() : string.Empty 
                            });
                        else
                            LogFile.WriteProcess(string.Format("Lỗi trùng dữ liệu tên đối tượng, ObjectID: {0}", objectID));
                        #endregion
                    }
                }
                return htObjectName;
            }
            catch (Exception ex)
            {
                LogFile.WriteData(string.Format("WRKPlanApprovedManager.ReadObjectName({0}), ex: {1}", fileName, ex));
                return null;
            }
        }
    }
}
