using System;
using System.Collections;
using System.Collections.Generic;
//using BAGeocoding.Dal.MapObj;
//using BAGeocoding.Dal.MapTool;
using BAGeocoding.Entity;
using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.MapTool;
using BAGeocoding.Utility;
using OSGeo.OGR;
using RTree.Engine;
using RTree.Engine.Entity;

namespace BAGeocoding.Bll.ImportData
{
    public class CTLGridManager
    {
        private static Hashtable HTDistrict = null;
        private static Hashtable HTCommune = null;
        private static RTree<BAGTile> RTree = null;

        /// <summary>
        /// Import dữ liệu lưới
        /// </summary>
        public static bool Import(string fileMap)
        {
            try
            {
                //if (InitRTree() == false)
                //    return false;

                OrgAPI ds = new OrgAPI(fileMap, 0);
                int nIndex = 1;
                if (RunningParams.ImportFileType == EnumBAGFileType.Shp)
                    nIndex = 0;
                //int nFeature = ds.GetFeatureCount() + nIndex; // Chanh
                long nFeature = ds.GetFeatureCount() + nIndex;
                List<int> processList = new List<int>();
                for (int i = nIndex; i < nFeature; i++)
                {
                    Feature f = ds.GetFeatureById(i);
                    Geometry geo = f.GetGeometryRef();
                    if (geo == null)
                        continue;
                    //int gtype = geo.GetGeometryType();//Chanh
                    int gtype = (int)geo.GetGeometryType();
                    //if (gtype == ogr.wkbLineString)//Chanh
                    if (gtype == Ogr.wkb25DBit)
                    {
                        int nCount = geo.GetPointCount();
                        MCLGrid gridInfo = new MCLGrid();
                        gridInfo.GridID = Convert.ToInt32(f.GetFieldAsInteger("GridID"));
                        gridInfo.Name = gridInfo.GridID.ToString();
                        gridInfo.PointList = new List<BAGPoint>();
                        for (int j = 0; j < nCount; j++)
                        {
                            gridInfo.PointList.Add(new BAGPoint(geo.GetX(j), geo.GetY(j)));
                            if (j > 0)
                                gridInfo.CoordsOrignal += ",";
                            gridInfo.CoordsOrignal += string.Format("{0:N8} {1:N8}", gridInfo.PointList[gridInfo.PointList.Count - 1].Lng, gridInfo.PointList[gridInfo.PointList.Count - 1].Lat);
                        }
                        gridInfo.CoordsEncrypt = MapHelper.PolylineAlgorithmEncode(gridInfo.PointList);
                        DetectProvince(gridInfo);
                        //if (gridInfo.ProvinceStr.Length > 0)
                        //    MCLGridDAO.Add(gridInfo);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("CTLGridManager.Import, ex: " + ex.ToString());
                return false;
            }
        }

        //private static bool InitRTree()
        //{
        //    if (RTree != null && RTree.Count > 0)
        //        return true;
        //    // 1. Dữ liệu quận/huyện
        //    List<BAGDistrict> districtList = DistrictDAO.GetAll();
        //    if (districtList == null || districtList.Count == 0)
        //        return false;
        //    HTDistrict = new Hashtable();
        //    for (int i = 0; i < districtList.Count; i++)
        //        HTDistrict.Add(districtList[i].DistrictID, districtList[i]);
        //    // 2. Dữ liệu xã/phường
        //    List<BAGCommune> communeList = CommuneDAO.GetAll();
        //    if (communeList == null || communeList.Count == 0)
        //        return false;
        //    HTCommune = new Hashtable();
        //    for (int i = 0; i < communeList.Count; i++)
        //        HTCommune.Add(communeList[i].CommuneID, communeList[i]);
        //    // 3. Dữ liệu vùng tìm kiếm
        //    RTree = new RTree<BAGTile>();
        //    List<BAGTile> tileList = TileDAO.GetByPage();
        //    if (tileList == null || tileList.Count == 0)
        //        return false;
        //    for (int i = 0; i < tileList.Count; i++)
        //        RTree.Add(tileList[i].GetRectangle(), tileList[i]);
        //    return true;
        //}

        /// <summary>
        /// Xác định vùng tìm kiếm
        /// </summary>
        private static BAGTile TileByGeo(RTRectangle rec, BAGPoint pts)
        {
            try
            {
                List<BAGTile> result = RTree.Intersects(rec);
                if (result == null || result.Count == 0)
                    return null;
                else if (result.Count == 1)
                    return result[0];
                else
                {
                    for (int i = 0; i < result.Count - 1; i++)
                    {
                        if (MapUtilityManager.CheckInsidePolygon(result[i].PointList, pts) == true)
                            return result[i];
                    }
                    return result[result.Count - 1];
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteError("CTLGridManager.TileByGeo, ex: " + ex.ToString());
                return null;
            }
        }

        private static short DetectProvince(BAGPoint pts)
        {
            double its = Constants.DISTANCE_INTERSECT_ROAD;
            RTRectangle rec = new RTRectangle(pts.Lng - its, pts.Lat - its, pts.Lng + its, pts.Lat + its, 0.0f, 0.0f);
            // Xác định vùng tìm kiếm
            BAGTile tile = TileByGeo(rec, pts);
            if (tile == null)
                return 0;
            // 1. Xác định xã phường
            if (HTCommune.ContainsKey(tile.CommuneID) == false)
                return 0;
            BAGCommune commune = (BAGCommune)HTCommune[tile.CommuneID];
            // 2. Xác định quận huyện
            if (HTDistrict.ContainsKey(commune.DistrictID) == false)
                return 0;
            BAGDistrict district = (BAGDistrict)HTDistrict[commune.DistrictID];
            return district.ProvinceID;
        }

        private static short DetectDistrict(BAGPoint pts)
        {
            double its = Constants.DISTANCE_INTERSECT_ROAD;
            RTRectangle rec = new RTRectangle(pts.Lng - its, pts.Lat - its, pts.Lng + its, pts.Lat + its, 0.0f, 0.0f);
            // Xác định vùng tìm kiếm
            BAGTile tile = TileByGeo(rec, pts);
            if (tile == null)
                return 0;
            // 1. Xác định xã phường
            if (HTCommune.ContainsKey(tile.CommuneID) == false)
                return 0;
            BAGCommune commune = (BAGCommune)HTCommune[tile.CommuneID];
            return commune.DistrictID;
        }

        private static void DetectProvince(MCLGrid gridInfo)
        {
            //if (InitRTree() == false)
            //    return;
            short provinceID = 0;
            Hashtable htProvinceID = new Hashtable();
            for (int i = 0; i < gridInfo.PointList.Count; i++)
            {
                provinceID = DetectProvince(gridInfo.PointList[i]);
                if (provinceID == 0)
                    continue;
                else if (htProvinceID.ContainsKey(provinceID) == true)
                    continue;
                if (gridInfo.ProvinceStr.Length > 0)
                    gridInfo.ProvinceStr += ",";
                gridInfo.ProvinceStr += string.Format("{0}", provinceID);
                htProvinceID.Add(provinceID, null);
            }
            gridInfo.DistrictID = DetectDistrict(gridInfo.Center());
        }
    }
}
