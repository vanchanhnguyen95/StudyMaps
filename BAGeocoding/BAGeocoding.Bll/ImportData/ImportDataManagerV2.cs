using BAGeocoding.Dal.MapObj;
using BAGeocoding.Dal.SearchData;
using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.SearchData;
using BAGeocoding.Utility;
using OSGeo.OGR;
using RTree.Engine;
using RTree.Engine.Entity;
using System.Collections;
using System.Text;

namespace BAGeocoding.Bll.ImportData
{
    public class ImportDataManagerV2
    {
        /// <summary>
        /// Import dữ liệu vùng (Tỉnh, Huyện, Xã)
        /// </summary>
        public static async Task<bool> ImportRegion(EnumBAGRegionType typeID, string fileMap, string fileName)
        {
            try
            {
                Hashtable htObjectName = ReadObjectName(typeID, fileName);
                if (typeID != EnumBAGRegionType.Tile && (htObjectName == null || htObjectName.Count == 0))
                    return false;
                switch (typeID)
                {
                    case EnumBAGRegionType.Province:
                        await ProvinceDAOV2.Clear();
                        await RegionKeyDAOV2.Clear(EnumBAGRegionType.Province);
                        break;
                    case EnumBAGRegionType.District:
                        await DistrictDAOV2.Clear();
                        await RegionKeyDAOV2.Clear(EnumBAGRegionType.District);
                        break;
                    case EnumBAGRegionType.Commune:
                        await CommuneDAOV2.Clear();
                        await RegionKeyDAOV2.Clear(EnumBAGRegionType.Commune);
                        break;
                    case EnumBAGRegionType.Tile:
                        await TileDAOV2.Clear();
                        break;
                    default:
                        break;
                }

                OrgAPI ds = new OrgAPI(fileMap, 0);
                int nIndex = 1;
                if (RunningParams.ImportFileType == EnumBAGFileType.Shp)
                    nIndex = 0;
                //int nFeature = ds.GetFeatureCount() + nIndex;//Chanh
                long nFeature = ds.GetFeatureCount() + nIndex;
                List<int> processList = new List<int>();
                for (int i = nIndex; i < nFeature; i++)
                {
                    Feature f = ds.GetFeatureById(i);
                    Geometry geo = f.GetGeometryRef();
                    if (geo == null)
                        continue;
                    //int gtype = geo.GetGeometryType();//Chanh
                    wkbGeometryType gtype = geo.GetGeometryType();
                    //if (gtype == ogr.wkbLineString || gtype == ogr.wkbPoint)/Chanh
                    if (gtype == wkbGeometryType.wkbLineString || gtype == wkbGeometryType.wkbPoint)
                    {
                        int nCount = geo.GetPointCount();
                        switch (typeID)
                        {
                            case EnumBAGRegionType.Province:
                                #region ==================== Read Province ====================
                                BAGProvinceV2 province = new BAGProvinceV2();
                                province.ProvinceID = Convert.ToInt16(f.GetFieldAsInteger("ProvinceID"));
                                if (province.ProvinceID == 9999)
                                    province.ProvinceID = 254;
                                if (province.ProvinceID > 255)
                                    continue;
                                else if (processList.Exists(item => item == province.ProvinceID) == true)
                                    continue;
                                processList.Add(province.ProvinceID);
                                if (province.ProvinceID < 256)
                                {
                                    if (htObjectName.ContainsKey(province.ProvinceID) == true)
                                        province.VName = (string)htObjectName[province.ProvinceID];
                                    else
                                        province.VName = string.Empty;
                                    province.EName = LatinToAscii.Latin2Ascii(province.VName);
                                    province.PointList = new List<BAGPointV2>();
                                    //for (int j = 0; j < nCount; j++)
                                    //{
                                    //    province.PointList.Add(new BAGPoint(geo.GetX(j), geo.GetY(j)));
                                    //    if (j > 0)
                                    //    {
                                    //        province.LngStr += ",";
                                    //        province.LatStr += ",";
                                    //        province.GeoStr += ",";
                                    //    }
                                    //    province.LngStr += string.Format("{0:N8}", geo.GetX(j));
                                    //    province.LatStr += string.Format("{0:N8}", geo.GetY(j));
                                    //    province.GeoStr += string.Format("{0:N8} {1:N8}", geo.GetX(j), geo.GetY(j));
                                    //}

                                    /*Chanh Start */
                                    //if (ProvinceDAO.Add(province) == false)
                                    //    LogFile.WriteError("");
                                    //else
                                    //{
                                    //    List<string> keyList = StringUlt.SplitKeySearch(province.EName);
                                    //    for (byte j = 0; j < keyList.Count; j++)
                                    //    {
                                    //        RegionKeyDAO.Add(new BAGRegionKey
                                    //        {
                                    //            EnumTypeID = EnumBAGRegionType.Province,
                                    //            KeyStr = keyList[j],
                                    //            ObjectID = province.ProvinceID,
                                    //            IndexID = j,
                                    //            Rate = DataUtl.KeySearchCalcRate(j, keyList.Count)
                                    //        });
                                    //    }
                                    //}
                                    /*Chanh End */
                                    if (await ProvinceDAOV2.Add(province) == false)
                                        LogFile.WriteError("");
                                    else
                                    {
                                        List<string> keyList = StringUlt.SplitKeySearch(province.EName);
                                        for (byte j = 0; j < keyList.Count; j++)
                                        {
                                            await RegionKeyDAOV2.Add(new BAGRegionKeyV2
                                            {
                                                EnumTypeID = EnumBAGRegionType.Province,
                                                KeyStr = keyList[j],
                                                ObjectID = province.ProvinceID,
                                                IndexID = j,
                                                Rate = DataUtl.KeySearchCalcRate(j, keyList.Count)
                                            });
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case EnumBAGRegionType.District:
                                #region ==================== Read District ====================
                                BAGDistrictV2 district = new BAGDistrictV2();
                                district.DistrictID = Convert.ToInt16(f.GetFieldAsInteger("DistrictID"));
                                district.ProvinceID = Convert.ToInt16(f.GetFieldAsInteger("ProvinceID"));
                                if (district.ProvinceID == 9999)
                                    district.ProvinceID = 254;
                                if (district.ProvinceID > 255)
                                    continue;
                                else if (processList.Exists(item => item == district.DistrictID) == true)
                                    continue;
                                processList.Add(district.DistrictID);
                                if (htObjectName.ContainsKey(district.DistrictID) == true)
                                    district.VName = (string)htObjectName[district.DistrictID];
                                else
                                    district.VName = string.Empty;
                                district.EName = LatinToAscii.Latin2Ascii(district.VName);
                                district.PointList = new List<BAGPoint>();
                                //for (int j = 0; j < nCount; j++)
                                //{
                                //    if (j > 0)
                                //    {
                                //        district.LngStr += ",";
                                //        district.LatStr += ",";
                                //        district.GeoStr += ",";
                                //    }
                                //    district.LngStr += string.Format("{0:N8}", geo.GetX(j));
                                //    district.LatStr += string.Format("{0:N8}", geo.GetY(j));
                                //    district.GeoStr += string.Format("{0:N8} {1:N8}", geo.GetX(j), geo.GetY(j));
                                //}

                                if (await DistrictDAOV2.Add(district) == false)
                                    LogFile.WriteError("");
                                else
                                {
                                    List<string> keyList = StringUlt.SplitKeySearch(district.EName);
                                    for (byte j = 0; j < keyList.Count; j++)
                                    {
                                        await RegionKeyDAOV2.Add(new BAGRegionKeyV2
                                        {
                                            EnumTypeID = EnumBAGRegionType.District,
                                            KeyStr = keyList[j],
                                            ObjectID = district.DistrictID,
                                            IndexID = j,
                                            Rate = DataUtl.KeySearchCalcRate(j, keyList.Count)
                                        });
                                    }
                                }

                                #endregion
                                break;
                            case EnumBAGRegionType.Commune:
                                #region ==================== Read Commune ====================
                                try
                                {
                                    BAGCommuneV2 commune = new BAGCommuneV2();
                                    commune.CommuneID = Convert.ToInt16(f.GetFieldAsInteger("CommuneID"));
                                    if (processList.Exists(item => item == commune.CommuneID) == true)
                                        continue;
                                    processList.Add(commune.CommuneID);
                                    if (htObjectName.ContainsKey(commune.CommuneID) == true)
                                        commune.VName = (string)htObjectName[commune.CommuneID];
                                    else
                                        commune.VName = string.Empty;
                                    commune.EName = LatinToAscii.Latin2Ascii(commune.VName);
                                    commune.DistrictID = Convert.ToInt16(f.GetFieldAsInteger("DistrictID"));
                                    commune.PointList = new List<BAGPointV2>();
                                    //for (int j = 0; j < nCount; j++)
                                    //{
                                    //    commune.PointList.Add(new BAGPoint(geo.GetX(j), geo.GetY(j)));
                                    //    if (j > 0)
                                    //    {
                                    //        commune.LngStr += ",";
                                    //        commune.LatStr += ",";
                                    //        commune.GeoStr += ",";
                                    //    }
                                    //    commune.LngStr += string.Format("{0:N8}", geo.GetX(j));
                                    //    commune.LatStr += string.Format("{0:N8}", geo.GetY(j));
                                    //    commune.GeoStr += string.Format("{0:N8} {1:N8}", geo.GetX(j), geo.GetY(j));
                                    //}
                                    if (await CommuneDAOV2.Add(commune) == true)
                                    {
                                        //for (int j = 0; j < commune.PointList.Count; j++)
                                        //    RegionPointDAO.Add(typeID, commune.CommuneID, commune.PointList[j], j + 1);

                                        List<string> keyList = StringUlt.SplitKeySearch(commune.EName);
                                        for (byte j = 0; j < keyList.Count; j++)
                                        {
                                            await RegionKeyDAOV2.Add(new BAGRegionKeyV2
                                            {
                                                EnumTypeID = EnumBAGRegionType.Commune,
                                                KeyStr = keyList[j],
                                                ObjectID = commune.CommuneID,
                                                IndexID = j,
                                                Rate = DataUtl.KeySearchCalcRate(j, keyList.Count)
                                            });
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogFile.WriteData(ex.ToString());
                                }
                                #endregion
                                break;
                            case EnumBAGRegionType.Tile:
                                #region ==================== Read Tile ====================
                                try
                                {
                                    BAGTileV2 tile = new BAGTileV2();
                                    tile.TileID = Convert.ToInt32(f.GetFieldAsInteger("TileID"));
                                    tile.CommuneID = Convert.ToInt16(f.GetFieldAsInteger("CommuneID"));
                                    if (processList.Exists(item => item == tile.TileID) == true)
                                        continue;
                                    processList.Add(tile.TileID);
                                    tile.PointList = new List<BAGPointV2>();
                                    for (int j = 0; j < nCount; j++)
                                    {
                                        tile.PointList.Add(new BAGPointV2(geo.GetX(j), geo.GetY(j)));
                                        if (j > 0)
                                        {
                                            tile.LngStr += ",";
                                            tile.LatStr += ",";
                                            tile.GeoStr += ",";
                                        }
                                        tile.LngStr += string.Format("{0:N8}", geo.GetX(j));
                                        tile.LatStr += string.Format("{0:N8}", geo.GetY(j));
                                        tile.GeoStr += string.Format("{0:N8} {1:N8}", geo.GetX(j), geo.GetY(j));
                                    }
                                    await TileDAOV2.Add(tile);
                                }
                                catch (Exception ex)
                                {
                                    LogFile.WriteData(ex.ToString());
                                }
                                #endregion
                                break;
                            default:
                                break;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("ImportDataManager.ImportRegion, ex: " + ex.ToString());
                return false;
            }
        }

        public static RTree<BAGDistrict> GetDistrictRTree(string fileName)
        {
            try
            {
                OrgAPI ds = new OrgAPI(fileName, 0);
                RTree<BAGDistrict> rtDistrict = new RTree<BAGDistrict>();
                int nIndex = 1;
                if (RunningParams.ImportFileType == EnumBAGFileType.Shp)
                    nIndex = 0;
                //int nFeature = ds.GetFeatureCount() + nIndex;//Chanh
                long nFeature = ds.GetFeatureCount() + nIndex;
                for (int i = nIndex; i < nFeature; i++)
                {
                    Feature f = ds.GetFeatureById(i);
                    Geometry geo = f.GetGeometryRef();
                    if (geo == null)
                        continue;
                    //else if (geo.GetGeometryType() == ogr.wkbLineString) // Chanh
                    else if (geo.GetGeometryType() == wkbGeometryType.wkbLineString)
                    {
                        int nCount = geo.GetPointCount();
                        BAGDistrict district = new BAGDistrict();
                        district.DistrictID = (short)f.GetFieldAsInteger("DistrictID");
                        district.PointList = new List<BAGPoint>();
                        for (int j = 0; j < nCount; j++)
                            district.PointList.Add(new BAGPoint(geo.GetX(j), geo.GetY(j)));
                        rtDistrict.Add(district.GetRectangle(), district);
                    }
                }
                return rtDistrict;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("ImportDataManager.GetDistrictRTree, ex: " + ex.ToString());
                return null;
            }
        }

        public static List<BAGGridView> GetGridList(string fileName)
        {
            try
            {
                OrgAPI ds = new OrgAPI(fileName, 0);
                List<BAGGridView> gridList = new List<BAGGridView>();
                int nIndex = 1;
                if (RunningParams.ImportFileType == EnumBAGFileType.Shp)
                    nIndex = 0;
                //int nFeature = ds.GetFeatureCount() + nIndex;//Chanh
                long nFeature = ds.GetFeatureCount() + nIndex;
                for (int i = nIndex; i < nFeature; i++)
                {
                    Feature f = ds.GetFeatureById(i);
                    Geometry geo = f.GetGeometryRef();
                    if (geo == null)
                        continue;
                    //else if (geo.GetGeometryType() == ogr.wkbLineString)//Chanh
                    else if (geo.GetGeometryType() == wkbGeometryType.wkbLineString)
                    {
                        int nCount = geo.GetPointCount();
                        BAGGridView gridItem = new BAGGridView();
                        gridItem.GridID = (short)f.GetFieldAsInteger("Et_id");
                        gridItem.Name = gridItem.GridID.ToString();// f.GetFieldAsString("Et_index");
                        gridItem.PointList = new List<BAGPoint>();
                        BAGPoint leftBot = new BAGPoint();
                        BAGPoint rightTop = new BAGPoint();
                        for (int j = 0; j < nCount; j++)
                        {
                            BAGPoint pointTemp = new BAGPoint(geo.GetX(j), geo.GetY(j));
                            if (j == 0)
                            {
                                rightTop = new BAGPoint(pointTemp);
                                leftBot = new BAGPoint(pointTemp);
                            }
                            else
                            {
                                //Ben trai
                                if (pointTemp.Lng < leftBot.Lng)
                                    leftBot.Lng = pointTemp.Lng;
                                //Phia duoi
                                if (pointTemp.Lat > leftBot.Lat)
                                    leftBot.Lat = pointTemp.Lat;
                                //Ben phai
                                if (pointTemp.Lng > rightTop.Lng)
                                    rightTop.Lng = pointTemp.Lng;
                                //Phia tren
                                if (pointTemp.Lat < rightTop.Lat)
                                    rightTop.Lat = pointTemp.Lat;
                            }
                        }
                        gridItem.PointList.Add(leftBot);
                        gridItem.PointList.Add(rightTop);
                        gridItem.LngStr = string.Format("{0:N8}@{1:N8}", leftBot.Lng, rightTop.Lng);
                        gridItem.LatStr = string.Format("{0:N8}@{1:N8}", leftBot.Lat, rightTop.Lat);

                        gridList.Add(gridItem);
                    }
                }
                return gridList;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("ImportDataManager.GetGridRTree, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Import dữ liệu đoạn đường
        /// </summary>
        public static bool ImportSegment(RTree<BAGDistrict> rtDistrict, BAGProvince provinceInfo, string fileMap, string fileName)
        {
            try
            {
                if (rtDistrict == null || rtDistrict.Count == 0)
                    return false;
                Hashtable htObjectName = ReadObjectName(EnumBAGRegionType.Segment, fileName);
                if (htObjectName == null || htObjectName.Count == 0)
                    return false;
                /* Chanh Start*/
                //Hashtable htProvince = ProvinceDAO.GetForCheck();
                //if (htProvince == null)
                //    return false;
                //else if (htProvince.ContainsKey(provinceInfo.ProvinceID) == false)
                //    return false;

                //// .1 Đối tượng đầu tiên => Hủy tất cả đoạn đường trong tỉnh/thành
                //SegmentDAO.Clear(provinceInfo.ProvinceID);
                /* Chanh End*/

                OrgAPI ds = new OrgAPI(fileMap, 0);
                Hashtable htDistrict = new Hashtable();
                int nIndex = 1;
                if (RunningParams.ImportFileType == EnumBAGFileType.Shp)
                    nIndex = 0;
                int provinceID = 0;
                //int nFeature = ds.GetFeatureCount() + nIndex; // Chanh
                long nFeature = ds.GetFeatureCount() + nIndex;
                List<BAGSegment> segmentList = new List<BAGSegment>();
                for (int i = nIndex; i < nFeature; i++)
                {
                    Feature f = ds.GetFeatureById(i);
                    Geometry geo = f.GetGeometryRef();
                    if (geo == null)
                        continue;
                    //else if (geo.GetGeometryType() == ogr.wkbLineString)// Chanh
                    else if (geo.GetGeometryType() == wkbGeometryType.wkbLineString)
                    {
                        #region ==================== Đọc thông tin từ file bản đồ ====================
                        BAGSegment segmentInfo = new BAGSegment();
                        segmentInfo.TemplateID = f.GetFieldAsInteger("ID");
                        segmentInfo.ProvinceID = (short)f.GetFieldAsInteger("ProvinceID");
                        if (segmentInfo.ProvinceID != provinceInfo.ProvinceID)
                        {
                            LogFile.WriteProcess(string.Format("Lệch mã tỉnh: ProvinceID: {0} - ID: {1} <----> {2}", segmentInfo.ProvinceID, segmentInfo.TemplateID, provinceID));
                            return false;
                        }
                        segmentInfo.SegmentID = segmentInfo.ProvinceID * Constants.PROVINCE_SHIFT_FOR_SEGMENT + segmentInfo.TemplateID;
                        if (segmentInfo.ProvinceID > 252)
                            continue;
                        else if (htObjectName.ContainsKey(segmentInfo.TemplateID) == true)
                            segmentInfo.VName = (string)htObjectName[segmentInfo.TemplateID];
                        else
                            segmentInfo.VName = string.Empty;
                        segmentInfo.EName = LatinToAscii.Latin2Ascii(segmentInfo.VName);
                        segmentInfo.ClassFunc = (byte)f.GetFieldAsInteger("ClassFunc");
                        segmentInfo.Direction = (byte)f.GetFieldAsInteger("Direction");
                        try
                        {
                            segmentInfo.DataExt = f.GetFieldAsInteger("DataExt");
                        }
                        catch { segmentInfo.DataExt = 0; }
                        segmentInfo.MinSpeed = (byte)f.GetFieldAsInteger("MinSpeed");
                        segmentInfo.MaxSpeed = (byte)f.GetFieldAsInteger("MaxSpeed");
                        segmentInfo.StartLeft = (short)f.GetFieldAsInteger("StartLeft");
                        segmentInfo.StartRight = (short)f.GetFieldAsInteger("StartRight");
                        segmentInfo.EndLeft = (short)f.GetFieldAsInteger("EndLeft");
                        segmentInfo.EndRight = (short)f.GetFieldAsInteger("EndRight");
                        segmentInfo.PointList = new List<BAGPoint>();
                        int nCount = geo.GetPointCount();
                        for (int j = 0; j < nCount; j++)
                        {
                            segmentInfo.PointList.Add(new BAGPoint(geo.GetX(j), geo.GetY(j)));
                            if (j > 0)
                            {
                                segmentInfo.LngStr += ",";
                                segmentInfo.LatStr += ",";
                                segmentInfo.Length += (float)segmentInfo.PointList[j - 1].Distance(segmentInfo.PointList[j]);
                            }
                            segmentInfo.LngStr += string.Format("{0:N8}", segmentInfo.PointList[segmentInfo.PointList.Count - 1].Lng);
                            segmentInfo.LatStr += string.Format("{0:N8}", segmentInfo.PointList[segmentInfo.PointList.Count - 1].Lat);
                        }
                        segmentInfo.DistrictID = GetDistrictID(rtDistrict, segmentInfo);
                        segmentList.Add(segmentInfo);
                        #endregion
                    }
                }

                #region ==================== Tiến hành lưu vào CSDL ====================
                for (int i = 0; i < segmentList.Count; i++)
                {
                    /*Chanh Start*/
                    // .2 Tiến hành thêm đoạn đường
                    //if (SegmentDAO.Add(segmentList[i]))
                    //{
                    //    // .2.1 Thêm điểm
                    //    SegmentPointDAO.AddStr(segmentList[i]);
                    //    // .2.2 Từ khóa tìm kiềm
                    //    List<string> keyList = StringUlt.SplitKeySearch(segmentList[i].EName);
                    //    for (byte j = 0; j < keyList.Count; j++)
                    //    {
                    //        BAGSegmentKey segmentKey = new BAGSegmentKey
                    //        {
                    //            ProvinceID = segmentList[i].ProvinceID,
                    //            KeyStr = keyList[j],
                    //            SegmentID = segmentList[i].SegmentID,
                    //            IndexID = j,
                    //            Rate = DataUtl.KeySearchCalcRate(j, keyList.Count)
                    //        };
                    //        /*Chanh Start*/
                    //        //if (SegmentKeyDAO.Add(segmentKey, Constants.PROVINCE_SHIFT_FOR_KEY * segmentList[i].ProvinceID) == false)
                    //        //    LogFile.WriteProcess(string.Format("ProvinceID: {0} - TemplateID: {1} - SegmentID: {2} - VName: {3} - Key: {4}", segmentList[i].ProvinceID, segmentList[i].TemplateID, segmentList[i].SegmentID, segmentList[i].VName, keyList[j]));
                    //        /*Chanh End*/
                    //    }
                    //}
                    /*Chanh End*/
                }
                #endregion

                // .3 Hiệu chỉnh lại số nhà
                //SegmentDAO.AdjustBuilding(provinceInfo.ProvinceID); Chanh

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("ImportDataManager.ImportSegment, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Import dữ liệu đường (Nguyên bản)
        /// </summary>
        public static bool ImportSegmentOriginal(List<BAGGridView> gridList, string fileMap, string fileName)
        {
            try
            {
                if (gridList == null || gridList.Count == 0)
                    return false;
                RTree<BAGGridView> gridRTree = new RTree<BAGGridView>();
                for (int i = 0; i < gridList.Count; i++)
                    gridRTree.Add(gridList[i].GetRectangle(), gridList[i]);

                Hashtable htObjectName = ReadObjectName(EnumBAGRegionType.Segment, fileName);
                if (htObjectName == null || htObjectName.Count == 0)
                    return false;

                OrgAPI ds = new OrgAPI(fileMap, 0);
                Hashtable htDistrict = new Hashtable();
                int nIndex = 1;
                if (RunningParams.ImportFileType == EnumBAGFileType.Shp)
                    nIndex = 0;
                //int nFeature = ds.GetFeatureCount() + nIndex;Chanh
                long nFeature = ds.GetFeatureCount() + nIndex;

                for (int i = nIndex; i < nFeature; i++)
                {
                    Feature f = ds.GetFeatureById(i);
                    Geometry geo = f.GetGeometryRef();
                    if (geo == null)
                        continue;
                    //else if (geo.GetGeometryType() == ogr.wkbLineString)//Chanh
                    else if (geo.GetGeometryType() == wkbGeometryType.wkbLineString)
                    {
                        #region ==================== Đọc thông tin từ file bản đồ ====================
                        BAGSegment segment = new BAGSegment();
                        segment.TemplateID = (int)f.GetFieldAsInteger("ID");
                        segment.ProvinceID = (short)f.GetFieldAsInteger("ProvinceID");
                        segment.SegmentID = segment.ProvinceID * Constants.PROVINCE_SHIFT_FOR_SEGMENT + segment.TemplateID;
                        if (segment.ProvinceID > 252)
                            continue;
                        else if (htObjectName.ContainsKey(segment.TemplateID) == true)
                            segment.VName = (string)htObjectName[segment.TemplateID];
                        else
                            segment.VName = string.Empty;
                        segment.VName = segment.VName.Replace(",", " ");
                        segment.EName = LatinToAscii.Latin2Ascii(segment.VName);
                        segment.Direction = (byte)f.GetFieldAsInteger("Direction");
                        segment.ClassFunc = (byte)f.GetFieldAsInteger("ClassFunc");
                        segment.StartLeft = (short)f.GetFieldAsInteger("StartLeft");
                        segment.StartRight = (short)f.GetFieldAsInteger("StartRight");
                        segment.EndLeft = (short)f.GetFieldAsInteger("EndLeft");
                        segment.EndRight = (short)f.GetFieldAsInteger("EndRight");
                        segment.PointList = new List<BAGPoint>();
                        int nCount = geo.GetPointCount();
                        for (int j = 0; j < nCount; j++)
                        {
                            segment.PointList.Add(new BAGPoint(geo.GetX(j), geo.GetY(j)));
                            if (j > 0)
                            {
                                segment.LngStr += "@";
                                segment.LatStr += "@";
                                segment.Length += (float)segment.PointList[j - 1].Distance(segment.PointList[j]);
                                segment.SegLength += (float)segment.PointList[j - 1].Distance(segment.PointList[j]);
                            }
                            segment.LngStr += string.Format("{0:N8}", segment.PointList[segment.PointList.Count - 1].Lng);
                            segment.LatStr += string.Format("{0:N8}", segment.PointList[segment.PointList.Count - 1].Lat);
                        }
                        segment.GridStr = GetGridListID(gridRTree, segment);

                        segment.LevelID = (byte)f.GetFieldAsInteger("Level");
                        segment.KindID = (byte)f.GetFieldAsInteger("Kind");
                        segment.RegionLev = (byte)f.GetFieldAsInteger("RegionLev");
                        segment.Wide = (byte)f.GetFieldAsInteger("Wide");
                        segment.MinSpeed = (byte)f.GetFieldAsInteger("MinSpeed");
                        segment.MaxSpeed = (byte)f.GetFieldAsInteger("MaxSpeed");
                        segment.Fee = 0;// (int)f.GetFieldAsInteger("Fee");
                        segment.IsNumber = (f.GetFieldAsInteger("IsNumber") == 1);
                        segment.IsBridge = (f.GetFieldAsInteger("IsBridge") == 1);
                        segment.IsPrivate = (f.GetFieldAsInteger("IsPrivate") == 1);
                        segment.IsPed = (f.GetFieldAsInteger("IsPed") == 1);
                        segment.AllowPed = (f.GetFieldAsInteger("AllowPed") == 1);
                        segment.AllowWalk = true;// (f.GetFieldAsInteger("AllowWalk") == 1);
                        segment.AllowBicycle = true;// (f.GetFieldAsInteger("AllowBicycle") == 1);
                        segment.AllowMoto = (f.GetFieldAsInteger("AllowMoto") == 1);
                        segment.AllowCar = (f.GetFieldAsInteger("AllowCar") == 1);
                        segment.DirCar = segment.Direction;// (byte)f.GetFieldAsInteger("DirCar");
                        segment.AllowBus = (f.GetFieldAsInteger("AllowBus") == 1);
                        segment.DirBus = segment.Direction;// (byte)f.GetFieldAsInteger("DirBus");
                        segment.AllowTruck = (f.GetFieldAsInteger("AllowTruck") == 1);
                        segment.DirTruck = segment.Direction;// (byte)f.GetFieldAsInteger("DirTruck");
                        segment.AllowTaxi = segment.AllowCar;// (f.GetFieldAsInteger("AllowTaxi") == 1);
                        segment.DirTaxi = segment.Direction;// (byte)f.GetFieldAsInteger("DirTaxi");

                        segment.IsDone = (f.GetFieldAsInteger("FinishID") == 1);
                        #endregion

                        #region ==================== Tiến hành lưu vào CSDL ====================
                        // .1 Đối tượng đầu tiên => Hủy tất cả đoạn đường trong tỉnh/thành
                        /*Chanh Start*/
                        //if (i == nIndex)
                        //{
                        //    EDTSegmentDAO.Clear(segment.ProvinceID);

                        //    EDTGridViewDAO.Clear(segment.ProvinceID);
                        //    for (int k = 0; k < gridList.Count; k++)
                        //        EDTGridViewDAO.Add(gridList[k]);
                        //}
                        //// .2 Tiến hành thêm đoạn đường
                        //if (EDTSegmentDAO.Add(segment))
                        //    EDTSegmentPointDAO.AddStr(segment);
                        //// .3 Đối tượng cuối cùng => Hiệu chỉnh lại số nhà
                        //if (i == nFeature - 1)
                        //    EDTSegmentDAO.AdjustBuilding(segment.ProvinceID);
                        /*Chanh End*/
                        #endregion
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("ImportDataManager.ImportSegmentOriginal, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Import dữ liệu đường (Nguyên bản)
        /// </summary>
        public static bool ImportPolylineOriginal(string fileMap)
        {
            try
            {
                OrgAPI ds = new OrgAPI(fileMap, 0);
                Hashtable htDistrict = new Hashtable();
                int nIndex = 1;
                if (RunningParams.ImportFileType == EnumBAGFileType.Shp)
                    nIndex = 0;
                //int nFeature = ds.GetFeatureCount() + nIndex;//Chanh
                long nFeature = ds.GetFeatureCount() + nIndex;

                for (int i = nIndex; i < nFeature; i++)
                {
                    Feature f = ds.GetFeatureById(i);
                    Geometry geo = f.GetGeometryRef();
                    if (geo == null)
                        continue;
                    //else if (geo.GetGeometryType() == ogr.wkbLineString)// Chanh
                    else if (geo.GetGeometryType() == wkbGeometryType.wkbLineString)
                    {
                        #region ==================== Đọc thông tin từ file bản đồ ====================
                        BAGPolyline polyline = new BAGPolyline();
                        polyline.PolylineID = (short)f.GetFieldAsInteger("Id");
                        polyline.PointList = new List<BAGPoint>();
                        int nCount = geo.GetPointCount();
                        string coorStr = string.Empty;
                        for (int j = 0; j < nCount; j++)
                        {
                            polyline.PointList.Add(new BAGPoint(geo.GetX(j), geo.GetY(j)));
                            if (j > 0)
                            {
                                polyline.LngStr += "@";
                                polyline.LatStr += "@";

                                coorStr += ",";
                            }
                            polyline.LngStr += string.Format("{0:N8}", polyline.PointList[polyline.PointList.Count - 1].Lng);
                            polyline.LatStr += string.Format("{0:N8}", polyline.PointList[polyline.PointList.Count - 1].Lat);

                            coorStr += string.Format("{0:N8} {1:N8}", polyline.PointList[polyline.PointList.Count - 1].Lng, polyline.PointList[polyline.PointList.Count - 1].Lat);
                        }
                        #endregion

                        #region ==================== Tiến hành lưu vào CSDL ====================
                        LogFile.WriteProcess(string.Format("{0}, {1}", polyline.PolylineID, coorStr));
                        //PolylineDAO.Add(polyline);
                        #endregion
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("ImportDataManager.ImportPolylineOriginal, ex: " + ex.ToString());
                return false;
            }
        }

        public static bool ImportPointOfInterest(BAGProvince provinceInfo, string fileMap, string fileName)
        {
            try
            {
                /*Chanh Start*/
                //// .1 Đối tượng đầu tiên => Hủy tất cả đoạn đường trong tỉnh/thành
                //PointOfInterestDAO.Clear(provinceInfo.ProvinceID);

                //OrgAPI ds = new OrgAPI(fileMap, 0);
                //Hashtable htDistrict = new Hashtable();
                //int nIndex = 1;
                //if (RunningParams.ImportFileType == EnumBAGFileType.Shp)
                //    nIndex = 0;
                //int nFeature = ds.GetFeatureCount() + nIndex;
                //List<BAGPointOfInterest> pointList = new List<BAGPointOfInterest>();
                //for (int i = nIndex; i < nFeature; i++)
                //{
                //    Feature f = ds.GetFeatureById(i);
                //    Geometry geo = f.GetGeometryRef();
                //    if (geo == null)
                //        continue;
                //    else if (geo.GetGeometryType() == ogr.wkbPoint)
                //    {
                //        #region ==================== Đọc thông tin từ file bản đồ ====================

                //        #endregion
                //    }
                //}

                //#region ==================== Tiến hành lưu vào CSDL ====================
                //for (int i = 0; i < pointList.Count; i++)
                //    PointOfInterestDAO.Add(pointList[i]);
                /*Chanh End*/
                return true;
                //#endregion

            }
            catch (Exception ex)
            {
                LogFile.WriteData("ImportDataManager.ImportPointOfInterest, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Đọc tên đối tượng
        /// </summary>
        public static Hashtable ReadObjectName(EnumBAGRegionType typeID, string fileName)
        {
            string dataTemp = string.Empty;
            try
            {
                int indexId = -1;
                int indexName = -1;
                int segmentID = 0;
                short objectID = 0;
                Hashtable htObjectName = new Hashtable();
                StreamReader streamReader = new StreamReader(fileName, Encoding.UTF8);
                while (streamReader.EndOfStream == false)
                {
                    List<string> lineData = StringUlt.StringAnalyze(streamReader.ReadLine(), ',');

                    if (indexId < 0 || indexName < 0)
                    {
                        #region ==================== Lay chi muc du lieu ====================
                        for (int i = 0; i < lineData.Count; i++)
                        {
                            switch (lineData[i].Trim().ToUpper())
                            {
                                case "PROVINCEID":
                                    if (typeID == EnumBAGRegionType.Province)
                                        indexId = i;
                                    break;
                                case "DISTRICTID":
                                    if (typeID == EnumBAGRegionType.District)
                                        indexId = i;
                                    break;
                                case "COMMUNEID":
                                    if (typeID == EnumBAGRegionType.Commune)
                                        indexId = i;
                                    break;
                                case "ID":
                                case "SEGMENTID":
                                    if (typeID == EnumBAGRegionType.Segment)
                                        indexId = i;
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
                        if (typeID == EnumBAGRegionType.Segment)
                        {
                            segmentID = Convert.ToInt32(lineData[indexId]);
                            if (htObjectName.ContainsKey(segmentID) == false)
                                htObjectName.Add(segmentID, lineData[indexName].Trim());
                            else
                                LogFile.WriteProcess(string.Format("Lỗi trùng dữ liệu tên đối tượng, SegmentID: {0}", segmentID));
                        }
                        else
                        {
                            objectID = Convert.ToInt16(lineData[indexId]);
                            if (htObjectName.ContainsKey(objectID) == false)
                                htObjectName.Add(objectID, lineData[indexName].Trim());
                            else
                                LogFile.WriteProcess("Lỗi trùng dữ liệu tên đối tượng");
                        }
                        #endregion
                    }
                }
                return htObjectName;
            }
            catch (Exception ex)
            {
                LogFile.WriteData(string.Format("ImportDataManager.ReadObjectName({0}), ex: {1}", fileName, ex));
                return null;
            }
        }

        /// <summary>
        /// Xác định quận/huyện cho đoạn đường
        /// </summary>
        public static short GetDistrictID(RTree<BAGDistrict> rtree, BAGSegment segment)
        {
            try
            {
                Hashtable htDistrictID = new Hashtable();
                for (int i = 0; i < segment.PointList.Count; i++)
                {
                    RTRectangle rec = new RTRectangle(
                        segment.PointList[i].Lng - Constants.DISTANCE_INTERSECT,
                        segment.PointList[i].Lng + Constants.DISTANCE_INTERSECT,
                        segment.PointList[i].Lat - Constants.DISTANCE_INTERSECT,
                        segment.PointList[i].Lat + Constants.DISTANCE_INTERSECT);
                    List<BAGDistrict> result = rtree.Intersects(rec);
                    if (result != null)
                    {
                        if (result.Count == 1)
                        {
                            if (htDistrictID.ContainsKey(result[0].DistrictID))
                                htDistrictID[result[0].DistrictID] = (int)htDistrictID[result[0].DistrictID] + 1;
                            else
                                htDistrictID.Add(result[0].DistrictID, 1);
                        }
                        else
                        {
                            bool bExists = false;
                            for (int j = 0; j < result.Count - 1; j++)
                            {
                                if (MapUtilityManager.CheckInsidePolygon(result[j].PointList, segment.PointList[i]))
                                {
                                    bExists = true;
                                    if (htDistrictID.ContainsKey(result[j].DistrictID))
                                        htDistrictID[result[j].DistrictID] = (int)htDistrictID[result[j].DistrictID] + 1;
                                    else
                                        htDistrictID.Add(result[j].DistrictID, 1);
                                }
                            }
                            if (!bExists)
                            {
                                if (htDistrictID.ContainsKey(result[result.Count - 1].DistrictID))
                                    htDistrictID[result[result.Count - 1].DistrictID] = (int)htDistrictID[result[result.Count - 1].DistrictID] + 1;
                                else
                                    htDistrictID.Add(result[result.Count - 1].DistrictID, 1);
                            }
                        }
                    }
                }
                if (htDistrictID.Count == 0)
                    return -1;
                else
                {
                    short districtID = -1;
                    int countPoint = 0;
                    foreach (object key in htDistrictID.Keys)
                    {
                        if ((int)htDistrictID[key] > countPoint)
                        {
                            districtID = (short)key;
                            countPoint = (int)htDistrictID[key];
                        }
                    }
                    return districtID;
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteError("ImportDataManager.GetDistrictID, ex: " + ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Xác định lưới của đoạn đường
        /// </summary>
        public static string GetGridListID(RTree<BAGGridView> rtree, BAGSegment segment)
        {
            try
            {
                float distanceIntersect = 0.0000000001f;
                List<int> gridListID = new List<int>();
                for (int i = 0; i < segment.PointList.Count; i++)
                {
                    RTRectangle rec = new RTRectangle(segment.PointList[i].Lng - distanceIntersect, segment.PointList[i].Lat - distanceIntersect, segment.PointList[i].Lng + distanceIntersect, segment.PointList[i].Lat + distanceIntersect);
                    List<BAGGridView> result = rtree.Intersects(rec);
                    if (result == null || result.Count == 0)
                        continue;
                    else if (gridListID.Exists(item => item == result[0].GridID) == false)
                        gridListID.Add(result[0].GridID);
                }
                if (gridListID.Count == 0)
                    return string.Empty;
                else
                    return string.Join("@", gridListID);
            }
            catch (Exception ex)
            {
                LogFile.WriteError("ImportDataManager.GetGridListID, ex: " + ex.ToString());
                return string.Empty;
            }
        }
    }
}
