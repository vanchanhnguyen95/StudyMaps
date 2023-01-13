using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using BAGeocoding.Bll.ImportData;

//using BAGeocoding.Dal.MapObj;
//using BAGeocoding.Dal.SearchData;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.Enum.MapObject;
using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.Router;
using BAGeocoding.Entity.Utility;

using BAGeocoding.Utility;

using OSGeo.OGR;

namespace BAGeocoding.Bll.WriteData
{
    public class WriteDataManager
    {
        private static List<short> ProvinceList { get; set; }

        ///// <summary>
        ///// Ghi dữ liệu vùng (Tỉnh, Huyện, Xã)
        ///// </summary>
        //public static bool WriteRegion(string fileName)
        //{
        //    try
        //    {
        //        #region ==================== 1. Lấy dữ liệu vùng ====================
        //        // 1. Dữ liệu tỉnh/thành
        //        List<BAGProvince> provinceList = ProvinceDAO.GetAll();
        //        if (provinceList == null || provinceList.Count == 0)
        //            return false;
        //        // 2. Dữ liệu quận/huyện
        //        List<BAGDistrict> districtList = DistrictDAO.GetAll();
        //        if (districtList == null || districtList.Count == 0)
        //            return false;
        //        // 3. Dữ liệu xã/phường
        //        List<BAGCommune> communeList = CommuneDAO.GetAll();
        //        if (communeList == null || communeList.Count == 0)
        //            return false;
        //        // 4. Dữ liệu vùng tìm kiếm
        //        List<BAGTile> tileList = TileDAO.GetByPage();
        //        if (tileList == null || tileList.Count == 0)
        //            return false;
        //        // 5. Dữ liệu quận huyện ưu tiên thấp khi tìm kiếm
        //        List<BAGDistrict> districtLow = DistrictDAO.GetPriorityLow();
        //        #endregion

        //        #region ==================== 2. Tiến hành ghi dữ liệu ====================
        //        using (FileStream stream = new FileStream(fileName, FileMode.Create))
        //        {
        //            using (BinaryWriter writer = new BinaryWriter(stream))
        //            {
        //                writer.Write((byte)5);

        //                // 1. Ghi dữ liệu tỉnh/thành
        //                writer.Write(BitConverter.GetBytes(provinceList.Count));
        //                for (int i = 0; i < provinceList.Count; i++)
        //                {
        //                    provinceList[i].PointList = new List<BAGPoint>();
        //                    writer.Write(provinceList[i].ToBinary());
        //                }

        //                // 2. Ghi dữ liệu quận/huyện
        //                writer.Write(BitConverter.GetBytes(districtList.Count));
        //                for (int i = 0; i < districtList.Count; i++)
        //                {
        //                    districtList[i].PointList = new List<BAGPoint>();
        //                    writer.Write(districtList[i].ToBinary());
        //                }

        //                // 3. Ghi dữ liệu xã/phường
        //                writer.Write(BitConverter.GetBytes(communeList.Count));
        //                for (int i = 0; i < communeList.Count; i++)
        //                    writer.Write(communeList[i].ToBinary());

        //                // 4. Ghi dữ liệu vùng tìm kiếm
        //                writer.Write(BitConverter.GetBytes(tileList.Count));
        //                for (int i = 0; i < tileList.Count; i++)
        //                    writer.Write(tileList[i].ToBinary());

        //                // 5. Ghi dữ liệu quận/huyện ưu tiên thấp khi tìm kiếm
        //                if (districtLow != null && districtLow.Count > 0)
        //                {
        //                    writer.Write(BitConverter.GetBytes(districtLow.Count));
        //                    for (int i = 0; i < districtLow.Count; i++)
        //                        writer.Write(BitConverter.GetBytes(districtLow[i].DistrictID));
        //                }
        //                else
        //                    writer.Write(BitConverter.GetBytes(0));
                        
        //                writer.Close();
        //            }
        //            stream.Close();
        //        }
        //        #endregion

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogFile.WriteData("WriteDataManager.WriteRegion, ex: " + ex.ToString());
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// Ghi dữ liệu khu đô thị
        ///// </summary>
        //public static bool WritePlace(string fileName)
        //{
        //    try
        //    {
        //        #region ==================== 1. Lấy dữ liệu vùng ====================
        //        // 1. Dữ liệu KĐT
        //        List<BAGPlace> urbanList = PlaceDAO.GetByType(EnumBAGPlaceType.Urban);
        //        if (urbanList == null || urbanList.Count == 0)
        //            return false;
        //        // 2. Dữ liệu lô đất
        //        List<BAGPlace> portionList = PlaceDAO.GetByType(EnumBAGPlaceType.Portion);
        //        if (portionList == null || portionList.Count == 0)
        //            return false;
        //        // 3. Dữ liệu ô đất
        //        List<BAGPlace> plotList = PlaceDAO.GetByType(EnumBAGPlaceType.Plot);
        //        if (plotList == null || plotList.Count == 0)
        //            return false;
        //        #endregion

        //        #region ==================== 2. Tiến hành ghi dữ liệu ====================
        //        using (FileStream stream = new FileStream(fileName, FileMode.Create))
        //        {
        //            using (BinaryWriter writer = new BinaryWriter(stream))
        //            {
        //                writer.Write((byte)3);

        //                // 1. Ghi dữ liệu KĐT
        //                writer.Write(BitConverter.GetBytes(urbanList.Count));
        //                for (int i = 0; i < urbanList.Count; i++)
        //                {
        //                    urbanList[i].PointList = new List<BAGPoint>();
        //                    writer.Write(urbanList[i].ToBinary());
        //                }

        //                // 2. Ghi dữ liệu lô đất
        //                writer.Write(BitConverter.GetBytes(portionList.Count));
        //                for (int i = 0; i < portionList.Count; i++)
        //                {
        //                    portionList[i].PointList = new List<BAGPoint>();
        //                    writer.Write(portionList[i].ToBinary());
        //                }

        //                // 3. Ghi dữ liệu ô đất
        //                writer.Write(BitConverter.GetBytes(plotList.Count));
        //                for (int i = 0; i < plotList.Count; i++)
        //                    writer.Write(plotList[i].ToBinary());
                        
        //                writer.Close();
        //            }
        //            stream.Close();
        //        }
        //        #endregion

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogFile.WriteData("WriteDataManager.WritePlace, ex: " + ex.ToString());
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// Ghi dữ liệu đường
        ///// </summary>
        //public static bool WriteSegment(string fileName, bool speedFlag)
        //{
        //    try
        //    {
        //        #region ==================== 1. Lấy dữ liệu vùng ====================
        //        Hashtable htSegment = new Hashtable();
        //        List<BAGProvince> provinceList = ProvinceDAO.GetByDataExt(EnumMOBProvinceDataExt.DataSearch);
        //        if (provinceList == null || provinceList.Count == 0)
        //            return false;
        //        for (int i = 0; i < provinceList.Count; i++)
        //        {
        //            List<BAGSegment> segmentList = SegmentDAO.GetByProvince(provinceList[i].ProvinceID);
        //            if (segmentList == null || segmentList.Count == 0)
        //                continue;
        //            htSegment.Add(provinceList[i].ProvinceID, segmentList);
        //        }
        //        #endregion

        //        #region ==================== 2. Tiến hành ghi dữ liệu ====================
        //        using (FileStream stream = new FileStream(fileName, FileMode.Create))
        //        {
        //            using (BinaryWriter writer = new BinaryWriter(stream))
        //            {
        //                writer.Write(Convert.ToByte(htSegment.Count));
        //                foreach (object key in htSegment.Keys)
        //                {
        //                    //Lay du lieu
        //                    List<BAGSegment> segmentList = (List<BAGSegment>)htSegment[key];
        //                    //Write ProvinceID
        //                    writer.Write(Convert.ToByte(key));
        //                    //Write number segment
        //                    writer.Write(Convert.ToInt32(segmentList.Count));
        //                    //Write segment object
        //                    for (int i = 0; i < segmentList.Count; i++)
        //                        writer.Write(segmentList[i].ToBinary(speedFlag));
        //                }
        //                writer.Close();
        //            }
        //            stream.Close();
        //        }
        //        #endregion

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogFile.WriteData("WriteDataManager.WriteSegment, ex: " + ex.ToString());
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// Ghi dữ liệu từ khóa tìm kiếm
        ///// </summary>
        //public static bool WriteKeys(string fileName, bool forSTaxi)
        //{
        //    try
        //    {
        //        #region ==================== 1. Lấy dữ liệu tìm kiếm ====================
        //        // 1. Dữ liệu tỉnh/thành
        //        List<UTLSearchKey> provinceList = RegionKeyDAO.GetByType(EnumBAGRegionType.Province);
        //        if (provinceList == null || provinceList.Count == 0)
        //            return false;
        //        // 2. Dữ liệu quận/huyện
        //        List<UTLSearchKey> districtList = RegionKeyDAO.GetByType(EnumBAGRegionType.District);
        //        if (districtList == null || districtList.Count == 0)
        //            return false;
        //        // 3. Dữ liệu xã/phường
        //        List<UTLSearchKey> communeList = RegionKeyDAO.GetByType(EnumBAGRegionType.Commune);
        //        if (communeList == null || communeList.Count == 0)
        //            return false;

        //        // 4. Dữ liệu điểm

        //        // 5. Dữ liệu tên đường đặc biệt
        //        List<BAGRoadSpecial> roadSpecialList = RoadSpecialDAO.GetAll();

        //        // 6. Lấy dữ liệu đường
        //        Hashtable htSegment = new Hashtable();
        //        List<BAGProvince> provinceTemp = ProvinceDAO.GetByDataExt(forSTaxi == true ? EnumMOBProvinceDataExt.ForSTaxi : EnumMOBProvinceDataExt.DataSearch);
        //        if (provinceTemp == null || provinceTemp.Count == 0)
        //            return false;
        //        for (int i = 0; i < provinceTemp.Count; i++)
        //        {
        //            //if (CheckProvince(provinceTemp[i].ProvinceID) == false)
        //            //    continue;
        //            List<UTLSearchKey> segmentList = SegmentKeyDAO.GetByProvince(provinceTemp[i].ProvinceID);
        //            if (segmentList == null || segmentList.Count == 0)
        //                continue;
        //            htSegment.Add(provinceTemp[i].ProvinceID, segmentList);
        //        }
        //        if (htSegment.Count == 0)
        //            return false;
        //        #endregion

        //        #region ==================== 2. Tiến hành ghi dữ liệu ====================
        //        using (FileStream stream = new FileStream(fileName, FileMode.Create))
        //        {
        //            using (BinaryWriter writer = new BinaryWriter(stream))
        //            {
        //                writer.Write(BitConverter.GetBytes(7));

        //                // 1. Ghi dữ liệu tỉnh/thành
        //                writer.Write(BitConverter.GetBytes(provinceList.Count));
        //                for (int i = 0; i < provinceList.Count; i++)
        //                    writer.Write(provinceList[i].ToBinary());

        //                // 2. Ghi dữ liệu quận/huyện
        //                writer.Write(BitConverter.GetBytes(districtList.Count));
        //                for (int i = 0; i < districtList.Count; i++)
        //                    writer.Write(districtList[i].ToBinary(false, true));

        //                // 3. Ghi dữ liệu xã/phường
        //                writer.Write(BitConverter.GetBytes(communeList.Count));
        //                for (int i = 0; i < communeList.Count; i++)
        //                    writer.Write(communeList[i].ToBinary());

        //                // 4. Dữ liệu tìm kiếm điểm
        //                writer.Write(BitConverter.GetBytes(0));

        //                // 5. Dữ liệu thông tin điểm
        //                writer.Write(BitConverter.GetBytes(0));

        //                // 6. Dữ liệu tên đường đặc biệt
        //                if (roadSpecialList != null && roadSpecialList.Count > 0)
        //                {
        //                    writer.Write(BitConverter.GetBytes(roadSpecialList.Count));
        //                    for (int i = 0; i < roadSpecialList.Count; i++)
        //                        writer.Write(roadSpecialList[i].ToBinary());
        //                }
        //                else
        //                    writer.Write(BitConverter.GetBytes(0));

        //                // 7. Dữ liệu đường
        //                writer.Write(BitConverter.GetBytes(htSegment.Count));
        //                foreach (object key in htSegment.Keys)
        //                {
        //                    List<UTLSearchKey> segmentList = (List<UTLSearchKey>)htSegment[key];
        //                    writer.Write(BitConverter.GetBytes(Convert.ToInt16(key)));
        //                    writer.Write(BitConverter.GetBytes(segmentList.Count));
        //                    for (int i = 0; i < segmentList.Count; i++)
        //                        writer.Write(segmentList[i].ToBinary(true, true));
        //                }

        //                writer.Close();
        //            }
        //            stream.Close();
        //        }
        //        #endregion

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogFile.WriteData("WriteDataManager.WriteKeys, ex: " + ex.ToString());
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// Ghi dữ liệu tìm đường
        ///// </summary>
        ///// <returns></returns>
        //public static bool WriteRoute(UTLConditionImportRoute condition)
        //{
        //    try
        //    {
        //        #region ==================== 1. Đọc dữ liệu tên đường ====================
        //        Hashtable htObjectName = ImportDataManager.ReadObjectName(EnumBAGRegionType.Segment, condition.FileName);
        //        if (htObjectName == null || htObjectName.Count == 0)
        //            return false;
        //        #endregion

        //        #region ==================== 2. Đọc dữ liệu thông tin đường ====================
        //        OrgAPI ds = new OrgAPI(condition.FileMap, 0);
        //        Hashtable htDistrict = new Hashtable();
        //        int nIndex = 1;
        //        if (RunningParams.ImportFileType == EnumBAGFileType.Shp)
        //            nIndex = 0;
        //        int nFeature = ds.GetFeatureCount() + nIndex;
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
        //                segment.SegmentID = (int)f.GetFieldAsInteger("SegmentID");
        //                if (htObjectName.ContainsKey(segment.SegmentID) == true)
        //                    segment.VName = (string)htObjectName[segment.SegmentID];
        //                else
        //                    segment.VName = string.Empty;
        //                segment.EName = LatinToAscii.Latin2Ascii(segment.VName);
        //                segment.ClassFunc = (byte)f.GetFieldAsInteger("ClassFunc");
        //                if (segment.ClassFunc < 1 && segment.ClassFunc > condition.CoeffList.Count)
        //                    return false;
        //                segment.Coeff = (double)f.GetFieldAsInteger("Coeff");
        //                if (segment.Coeff == 0)
        //                    return false;
        //                else if (segment.Coeff == 1)
        //                    segment.Coeff = condition.CoeffList[segment.ClassFunc - 1];
        //                segment.AllowCar = new BARDirection((byte)f.GetFieldAsInteger("AllowCar"));
        //                segment.DataExt = (byte)f.GetFieldAsInteger("DataExt");
        //                if (segment.DataExtGet(EnumMOBSegmentDataExt.HighWay) == true)
        //                    Console.Write(segment.VName);

        //                segment.PointList = new List<BARPoint>();
        //                int nCount = geo.GetPointCount();
        //                for (int j = 0; j < nCount; j++)
        //                    segment.PointList.Add(new BARPoint(geo.GetX(j), geo.GetY(j)));
        //                segmentList.Add(segment);
        //                #endregion
        //            }
        //        }
        //        #endregion
                
        //        #region ==================== 3. Tiến hành ghi dữ liệu ====================
        //        using (FileStream stream = new FileStream(condition.FileData, FileMode.Create))
        //        {
        //            using (BinaryWriter writer = new BinaryWriter(stream))
        //            {
        //                writer.Write(BitConverter.GetBytes(segmentList.Count));
        //                for (int i = 0; i < segmentList.Count; i++)
        //                {
        //                    writer.Write(segmentList[i].ToBinary());
        //                }
        //                writer.Close();
        //            }
        //            stream.Close();
        //        }
        //        #endregion

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogFile.WriteData("WriteDataManager.WriteRoute, ex: " + ex.ToString());
        //        return false;
        //    }
        //}
        
        ///// <summary>
        ///// Ghi dữ liệu tìm đường
        ///// </summary>
        ///// <returns></returns>
        //public static bool WriteRouteMulti(UTLConditionImportRoute condition)
        //{
        //    try
        //    {
        //        List<BARSegment> segmentList = new List<BARSegment>();
        //        string[] fileList = Directory.GetFiles(condition.FolderInput, "*.shp");
        //        if (fileList == null || fileList.Length == 0)
        //            return false;
        //        for (int i = 0; i < fileList.Length; i++)
        //        {
        //            UTLConditionImportRoute temp = new UTLConditionImportRoute(condition) { FileMap = fileList[i] };
        //            temp.FileName = fileList[i].Replace(".shp", ".txt");
        //            if (WriteRouteRead(temp, ref segmentList) == false)
        //                return false;
        //        }

        //        #region ==================== 3. Tiến hành ghi dữ liệu ====================
        //        using (FileStream stream = new FileStream(condition.FileData, FileMode.Create))
        //        {
        //            using (BinaryWriter writer = new BinaryWriter(stream))
        //            {
        //                writer.Write(BitConverter.GetBytes(segmentList.Count));
        //                for (int i = 0; i < segmentList.Count; i++)
        //                {
        //                    writer.Write(segmentList[i].ToBinary());
        //                }
        //                writer.Close();
        //            }
        //            stream.Close();
        //        }
        //        #endregion

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogFile.WriteData("WriteDataManager.WriteRoute, ex: " + ex.ToString());
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// Ghi dữ liệu tìm đường
        ///// </summary>
        ///// <returns></returns>
        //public static bool WriteRouteRead(UTLConditionImportRoute condition, ref List<BARSegment> segmentList)
        //{
        //    try
        //    {
        //        #region ==================== 1. Đọc dữ liệu tên đường ====================
        //        Hashtable htObjectName = ImportDataManager.ReadObjectName(EnumBAGRegionType.Segment, condition.FileName);
        //        if (htObjectName == null || htObjectName.Count == 0)
        //            return LogFile.ProcessState(string.Format("{0} -> Lỗi tên", condition.FileName));
        //        #endregion

        //        #region ==================== 2. Đọc dữ liệu thông tin đường ====================
        //        OrgAPI ds = new OrgAPI(condition.FileMap, 0);
        //        Hashtable htDistrict = new Hashtable();
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
        //            else if (geo.GetGeometryType() == ogr.wkbLineString)
        //            {
        //                #region ==================== Đọc thông tin từ file bản đồ ====================
        //                BARSegment segment = new BARSegment();
        //                segment.SegmentID = (int)f.GetFieldAsInteger("SegmentID");
        //                if (htObjectName.ContainsKey(segment.SegmentID) == true)
        //                    segment.VName = (string)htObjectName[segment.SegmentID];
        //                else
        //                    segment.VName = string.Empty;
        //                segment.EName = LatinToAscii.Latin2Ascii(segment.VName);
        //                segment.ClassFunc = (byte)f.GetFieldAsInteger("ClassFunc");
        //                if (segment.ClassFunc < 1 && segment.ClassFunc > condition.CoeffList.Count)
        //                    return LogFile.ProcessState(string.Format("{0} -> Lỗi ClassFunc", segment.SegmentID)); 
        //                segment.Coeff = (double)f.GetFieldAsInteger("Coeff");
        //                if (segment.Coeff == 0)
        //                    return LogFile.ProcessState(string.Format("{0} -> Lỗi Coeff", segment.SegmentID));
        //                else if (segment.Coeff == 1)
        //                    segment.Coeff = condition.CoeffList[segment.ClassFunc - 1];
        //                segment.AllowCar = new BARDirection((byte)f.GetFieldAsInteger("AllowCar"));
        //                segment.FerryThese = (f.GetFieldAsInteger("IsFerry") == 1 || f.GetFieldAsInteger("These") == 1);
        //                segment.DataExt = (byte)f.GetFieldAsInteger("DataExt");
        //                //if (segment.DataExtGet(EnumMOBSegmentDataExt.HighWay) == true)
        //                //    Console.Write(segment.VName);

        //                segment.PointList = new List<BARPoint>();
        //                int nCount = geo.GetPointCount();
        //                for (int j = 0; j < nCount; j++)
        //                    segment.PointList.Add(new BARPoint(geo.GetX(j), geo.GetY(j)));
        //                segmentList.Add(segment);
        //                #endregion
        //            }
        //        }
        //        #endregion

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogFile.WriteData("WriteDataManager.WriteRouteRead, ex: " + ex.ToString());
        //        return false;
        //    }
        //}
    }
}