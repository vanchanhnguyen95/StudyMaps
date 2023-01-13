using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using OSGeo.OGR;
//using BAGeocoding.Dal.MapObj;
//using BAGeocoding.Dal.SearchData;
//using BAGeocoding.Dal.SegmentEdit;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.SearchData;

using BAGeocoding.Utility;

using RTree.Engine;
using RTree.Engine.Entity;
using BAGeocoding.Entity.CheckData;
using BAGeocoding.Entity.Enum.CheckData;

namespace BAGeocoding.Bll.CheckData
{
    public class CheckDataManager
    {
        /// <summary>
        /// Import dữ liệu đoạn đường
        /// </summary>
        public static List<CDTSegment> CheckSegment(string fileMap, string fileName)
        {
            try
            {
                OrgAPI ds = new OrgAPI(fileMap, 0);
                Hashtable htSegmentInfo = ReadSegmentInfo(fileName);
                if (htSegmentInfo == null || htSegmentInfo.Count == 0)
                    return null;
                int nIndex = 1;
                if (RunningParams.ImportFileType == EnumBAGFileType.Shp)
                    nIndex = 0;
                int nFeature = ds.GetFeatureCount() + nIndex;
                string msgStr = string.Empty;
                string geoStr = string.Empty;
                List<CDTSegment> resultList = new List<CDTSegment>();
                List<BAGSegment> segmentList = new List<BAGSegment>();
                Hashtable htRoadName = new Hashtable();
                for (int i = nIndex; i < nFeature; i++)
                {
                    Feature f = ds.GetFeatureById(i);
                    Geometry geo = f.GetGeometryRef();
                    if (geo == null)
                        continue;
                    //else if (geo.GetGeometryType() == ogr.wkbLineString)
                    else if (geo.GetGeometryType() == ogr.wkbLineString)
                    {
                        #region ==================== Đọc thông tin từ file bản đồ ====================
                        BAGSegment segmentInfo = new BAGSegment();
                        segmentInfo.TemplateID = (int)f.GetFieldAsInteger("ID");
                        if (htSegmentInfo.ContainsKey(segmentInfo.TemplateID) == true)
                            segmentInfo.VName = (string)htSegmentInfo[segmentInfo.TemplateID];
                        else
                            segmentInfo.VName = string.Empty;
                        segmentInfo.StartLeft = (short)f.GetFieldAsInteger("StartLeft");
                        segmentInfo.StartRight = (short)f.GetFieldAsInteger("StartRight");
                        segmentInfo.EndLeft = (short)f.GetFieldAsInteger("EndLeft");
                        segmentInfo.EndRight = (short)f.GetFieldAsInteger("EndRight");
                        int nCount = geo.GetPointCount();
                        if (nCount > 0)
                            segmentInfo.PointList.Add(new BAGPoint(geo.GetX(0), geo.GetY(0)));
                        #endregion

                        #region ==================== Tiến hành lưu kết quả ====================
                        // Kiểm tra lỗi thiếu số nhà
                        if (segmentInfo.IsMissing(ref msgStr) == true)
                            resultList.Add(new CDTSegment { EnumKindID = EnumCDTErrorKind.Missing, SegmentID = segmentInfo.TemplateID, DataStr = msgStr, GeoStr = string.Format("{0:N8}, {1:N8}", segmentInfo.PointList[0].Lat, segmentInfo.PointList[0].Lng) });
                        else if (segmentInfo.VName.Length == 0)
                            continue;
                        else if (htRoadName.ContainsKey(segmentInfo.VName) == false)
                            htRoadName.Add(segmentInfo.VName, new List<BAGSegment> { segmentInfo });
                        else
                        {
                            msgStr = string.Empty;
                            geoStr = string.Empty;
                            List<BAGSegment> segmentTemp = (List<BAGSegment>)htRoadName[segmentInfo.VName];
                            for (int k = 0; k < segmentTemp.Count; k++)
                                segmentInfo.IsDuplicate(segmentTemp[k], ref msgStr, ref geoStr);

                            if (msgStr.Length > 0)
                            {
                                msgStr = msgStr.Substring(2);
                                msgStr = string.Format("Name: {0} (ID: {1}, SR: {2}, ER: {3}, SL: {4}, EL: {5}) - {6}", segmentInfo.VName, segmentInfo.TemplateID, segmentInfo.StartRight, segmentInfo.EndRight, segmentInfo.StartLeft, segmentInfo.EndLeft, msgStr);
                                geoStr = string.Format("({0:N8}, {1:N8}) {2}", segmentInfo.PointList[0].Lat, segmentInfo.PointList[0].Lng, geoStr);
                                resultList.Add(new CDTSegment { EnumKindID = EnumCDTErrorKind.Duplicate, SegmentID = segmentInfo.TemplateID, DataStr = msgStr, GeoStr = geoStr });
                            }
                            segmentTemp.Add(segmentInfo);
                            htRoadName[segmentInfo.VName] = segmentTemp;
                        }
                        #endregion
                    }
                }

                return resultList;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("CheckDataManager.CheckSegment, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Đọc thông tin đối tượng
        /// </summary>
        private static Hashtable ReadSegmentInfo(string fileName)
        {
            try
            {
                int objectID = 0;
                int indexID = -1;
                int indexName = -1;

                Hashtable htObjectName = new Hashtable();
                StreamReader streamReader = new StreamReader(fileName, Encoding.UTF8);
                while (streamReader.EndOfStream == false)
                {
                    List<string> lineData = StringUlt.StringAnalyze(streamReader.ReadLine(), ',');

                    if (indexID < 0 || indexName < 0)
                    {
                        #region ==================== Lay chi muc du lieu ====================
                        for (int i = 0; i < lineData.Count; i++)
                        {
                            switch (lineData[i].Trim().ToUpper())
                            {
                                case "ID":
                                    indexID = i;
                                    break;
                                case "NAME":
                                    indexName = i;
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
                        objectID = Convert.ToInt32(lineData[indexID]);
                        if (htObjectName.ContainsKey(objectID) == false)
                            htObjectName.Add(objectID, lineData[indexName].Trim());
                        else
                            LogFile.WriteProcess(string.Format("Lỗi trùng dữ liệu tên đối tượng, ObjectID: {0}", objectID));
                        #endregion

                    }
                }
                return htObjectName;
            }
            catch (Exception ex)
            {
                LogFile.WriteData(string.Format("CheckDataManager.ReadSegmentInfo({0}), ex: {1}", fileName, ex));
                return null;
            }
        }

    }
}
