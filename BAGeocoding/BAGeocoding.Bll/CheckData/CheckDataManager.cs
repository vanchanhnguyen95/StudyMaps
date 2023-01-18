using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MaxRev.Gdal.Core;
using OSGeo.OGR;
using OSGeo.OSR;
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
        public static List<CDTSegment> CheckSegment(string fileMap, string fileName, string nameChange)
        {
            try
            {
                //OrgAPI ds = new OrgAPI(fileMap, 0);
                GdalBase.ConfigureAll();

                Hashtable htSegmentInfo = ReadSegmentInfo(fileName);
                //if (htSegmentInfo == null || htSegmentInfo.Count == 0)
                //    return null;
                int nIndex = 1;
                if (RunningParams.ImportFileType == EnumBAGFileType.Shp)
                    nIndex = 0;

                // sample reading shape file 
                var shpDriver = Ogr.GetDriverByName("ESRI Shapefile");

                //DataSource ds = shpDriver.Open(@"TestHL.shp", 0);
                DataSource ds = shpDriver.Open(fileName, 0);
                // GetLayer
                Layer layer = ds.GetLayerByIndex(0);
                //long nFeature = ds.GetFeatureCount() + nIndex;
                long nFeature = layer.GetFeatureCount(0) + nIndex;
                string msgStr = string.Empty;
                string geoStr = string.Empty;
                List<CDTSegment> resultList = new List<CDTSegment>();
                List<BAGSegment> segmentList = new List<BAGSegment>();
                Hashtable htRoadName = new Hashtable();


                GdalBase.ConfigureAll();
                

                // sample reading shape file 
                //var shpDriver = Ogr.GetDriverByName("ESRI Shapefile");
                //DataSource ds = shpDriver.Open(@"TestHL.shp", 0);
                //DataSource ds = shpDriver.Open(@"Out_Point_2022121914593.shp", 0);
                // GetLayer
                //Layer layer = ds.GetLayerByIndex(0);
                // GetLayerName
                string layerName = layer.GetName();
                // GetFeatureCount
                long featureCount = layer.GetFeatureCount(0);
                //GetFeatureById
                Feature featureById = layer.GetFeature(1);

                FeatureDefn fd = layer.GetLayerDefn();
                int n_f = fd.GetFieldCount();
                FieldDefn[] a_f = new FieldDefn[n_f];
                for (int i = 0; i < n_f; i++)
                {
                    a_f[i] = fd.GetFieldDefn(i);
                }

                string shapeType = layer.GetGeomType().ToString("G").Substring(3);
                Console.WriteLine($"Shape Type: {shapeType}");

                SpatialReference spatialReference = layer.GetSpatialRef();
                string projectionName = spatialReference.GetName();
                Console.WriteLine($"Projection: {projectionName}");

                SpatialReference sr = layer.GetSpatialRef();
                string srs_wkt;
                if (sr != null)
                {
                    sr.ExportToPrettyWkt(out srs_wkt, 1);
                }
                else
                    srs_wkt = "(unknown)";

                Console.WriteLine("Layer SRS WKT: " + srs_wkt);


                /* Đọc dữ liệu shape file polyline Có uni*/
                DataSource dsUnit = shpDriver.Open(fileName, 0);
                // GetLayer
                Layer layerUnit = dsUnit.GetLayerByIndex(0);
                // GetFeatureCount
                long countUni = layerUnit.GetFeatureCount(0);
                for (int i = 0; i < countUni; i++)
                {
                    Feature feUnit = layerUnit.GetFeature(i);
                    OSGeo.OGR.Geometry geoUnit = feUnit.GetGeometryRef();
                    var ty = geoUnit.GetGeometryType();
                    Console.WriteLine($"GeometryType - {ty}");

                    long segmentID = Convert.ToInt64(feUnit.GetFieldAsInteger64("SegmentID"));
                    string name = Convert.ToString(feUnit.GetFieldAsString("Name"));
                    int maxSpeed = Convert.ToInt16(feUnit.GetFieldAsInteger("MaxSpeed"));

                    Console.WriteLine($"SegmentID - {segmentID}; Name -{name};MaxSpeed-{maxSpeed}");
                    List<BAGPoint> PointList = new List<BAGPoint>();
                    BAGPoint baPoint = new BAGPoint();
                    int cntPoint = geoUnit.GetPointCount();
                    if (cntPoint > 0)
                    {
                        for (int jP = 0; jP < cntPoint; jP++)
                            PointList.Add(new BAGPoint(geoUnit.GetX(jP), geoUnit.GetY(jP)));
                    }
                    Console.WriteLine($"PointList");
                    foreach (var keyValuePair in PointList)
                    {
                        Console.WriteLine($"Lat - {keyValuePair.Lat} ; Lng {keyValuePair.Lng}.");
                    }
                    Console.WriteLine($"=======================================================");

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
                //int nFeature = ds.GetFeatureCount() + nIndex;Chanh
                long nFeature = ds.GetFeatureCount() + nIndex;
                string msgStr = string.Empty;
                string geoStr = string.Empty;
                List<CDTSegment> resultList = new List<CDTSegment>();
                List<BAGSegment> segmentList = new List<BAGSegment>();
                Hashtable htRoadName = new Hashtable();
                for (int i = nIndex; i < nFeature; i++)
                {
                    Feature f = ds.GetFeatureById(i);
                    OSGeo.OGR.Geometry geo = f.GetGeometryRef();
                    if (geo == null)
                        continue;
                    //else if (geo.GetGeometryType() == ogr.wkbLineString) // Chanh
                    else if (geo.GetGeometryType() == wkbGeometryType.wkbLineString)
                    {
                        #region ==================== Đọc thông tin từ file bản đồ ====================
                        BAGSegment segmentInfo = new BAGSegment();
                        //segmentInfo.TemplateID = (int)f.GetFieldAsInteger("ID");
                        //long segmentID = Convert.ToInt64(geo.GetFieldAsInteger64("SegmentID"));
                        segmentInfo.TemplateID = (int)f.GetFieldAsInteger64("ID");
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
