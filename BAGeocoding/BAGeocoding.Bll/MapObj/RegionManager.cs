using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using BAGeocoding.Entity.Enum.MapObject;
using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;

namespace BAGeocoding.Bll.MapObj
{
    public class RegionManager
    {
        private static Hashtable HTRoadByLevel = new Hashtable();

        public static bool LoadData()
        {
            try
            {
                // Tạo file tạm
                string filePath = string.Empty;
                if (MainProcessing.FileCopy(Constants.DEFAULT_DIRECTORY_DATA, Constants.DEFAULT_REGION_FILE_NAME, ref filePath) == false)
                    return false;

                // Tiến hành đọc dữ liệu
                using (FileStream stream = new FileStream(filePath, FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        int nFiles = (int)reader.ReadByte();
                        for (int i = 0; i < nFiles; i++)
                        {
                            int nameLength = 0;
                            int objectCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                            if (i == 0)
                            {
                                #region ==================== Đọc dữ liệu tỉnh/thành ====================
                                RunningParams.HTProvincePriority = new Hashtable();
                                for (int j = 0; j < objectCount; j++)
                                {
                                    BAGProvince province = new BAGProvince();
                                    //Read 2 bytes (ID)
                                    province.ProvinceID = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                                    //Read 1 byte (Length of VName)
                                    nameLength = (int)reader.ReadByte();
                                    //Read VName
                                    province.VName = Constants.UnicodeCodePage.GetString(reader.ReadBytes(nameLength));
                                    province.EName = LatinToAscii.Latin2Ascii(province.VName);
                                    province.DataExt = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                                    if (RunningParams.HTProvincePriority.ContainsKey(province.ProvinceID) == false)
                                    {
                                        if (province.DataExtGet(EnumMOBProvinceDataExt.RoadByLevel) == true)
                                            RunningParams.HTProvincePriority.Add(province.ProvinceID, null);
                                        else if (CheckMore(province.ProvinceID) == true)
                                            RunningParams.HTProvincePriority.Add(province.ProvinceID, null);

                                        if(RunningParams.HTProvincePriority.ContainsKey(province.ProvinceID) == true)
                                            LogFile.WriteFile("", "ProvincePriority", "", province.ProvinceID.ToString());
                                    }
                                    //Read 2 bytes (Length of list Points)
                                    int length = (int)BitConverter.ToInt16(reader.ReadBytes(2), 0);
                                    province.PointList = new List<BAGPoint>();
                                    //Read list Points
                                    for (int k = 0; k < length; k++)
                                        province.PointList.Add(new BAGPoint(BitConverter.ToDouble(reader.ReadBytes(8), 0), BitConverter.ToDouble(reader.ReadBytes(8), 0)));
                                    //Add object
                                    RunningParams.ProvinceData.Objs.Add(province.ProvinceID, province);

                                    RunningParams.ProvinceDataV2.Objs.Add(province.ProvinceID, province);
                                }
                                #endregion
                            }
                            else if (i == 1)
                            {
                                #region ==================== Đọc dữ liệu quận/huyện ====================
                                for (int j = 0; j < objectCount; j++)
                                {
                                    //Read District
                                    BAGDistrict district = new BAGDistrict();
                                    //Read 2 bytes (ID)
                                    district.DistrictID = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                                    //Read 1 byte (Length of VName)
                                    nameLength = (int)reader.ReadByte();
                                    //Read VName
                                    district.VName = Constants.UnicodeCodePage.GetString(reader.ReadBytes(nameLength));
                                    district.EName = LatinToAscii.Latin2Ascii(district.VName);
                                    //Read 2 bytes (ProvinceID)
                                    district.ProvinceID = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                                    //Read 2 bytes (Length of list Points)
                                    int length = (int)BitConverter.ToInt16(reader.ReadBytes(2), 0);
                                    district.PointList = new List<BAGPoint>();
                                    //Read list Points
                                    for (int k = 0; k < length; k++)
                                        district.PointList.Add(new BAGPoint(BitConverter.ToDouble(reader.ReadBytes(8), 0), BitConverter.ToDouble(reader.ReadBytes(8), 0)));
                                    //Add object
                                    RunningParams.DistrictData.Objs.Add(district.DistrictID, district);

                                    //if (RunningParams.DistrictPriority.ContainsKey(district.DistrictID) == true)
                                    //    LogFile.WriteData(string.Format("{0} - {1} - {2}", district.ProvinceID, district.DistrictID, district.VName));
                                }
                                #endregion
                            }
                            else if(i == 2)
                            {
                                #region ==================== Đọc dữ liệu xã/phường ====================
                                for (int j = 0; j < objectCount; j++)
                                {
                                    //Read commune
                                    BAGCommune commune = new BAGCommune();
                                    //Read 2 bytes (ID)
                                    commune.CommuneID = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                                    //Read 1 byte (Length of VName)
                                    nameLength = (int)reader.ReadByte();
                                    //Read VName
                                    commune.VName = Constants.UnicodeCodePage.GetString(reader.ReadBytes(nameLength));
                                    //if (commune.CommuneID == 1270)
                                    //{
                                    //    LogFile.WriteProcess(string.Format("{0} - {1} -> P.Dư Hàng Kênh", commune.CommuneID, commune.VName));
                                    //    commune.VName = "P.Dư Hàng Kênh";
                                    //}
                                    //else if (commune.CommuneID == 5423)
                                    //{
                                    //    LogFile.WriteProcess(string.Format("{0} - {1} -> P.Kênh Dương", commune.CommuneID, commune.VName));
                                    //    commune.VName = "P.Kênh Dương";
                                    //}
                                    commune.EName = LatinToAscii.Latin2Ascii(commune.VName);
                                    //Read 2 bytes (DistrictID)
                                    commune.DistrictID = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                                    //Read 2 bytes (Length of Points)
                                    int length = (int)BitConverter.ToInt16(reader.ReadBytes(2), 0);
                                    commune.PointList = new List<BAGPoint>();
                                    //Read list Points
                                    for (int k = 0; k < length; k++)
                                        commune.PointList.Add(new BAGPoint(BitConverter.ToDouble(reader.ReadBytes(8), 0), BitConverter.ToDouble(reader.ReadBytes(8), 0)));
                                    // Build RTree
                                    //RunningParams.CommuneData.RTree.Add(commune.GetRectangle(), commune);
                                    // Add object
                                    //if (commune.CommuneID == 1294)
                                    //{
                                    //    LogFile.WriteProcess(string.Format("{0} - {1} -> P.Dư Hàng Kênh", commune.CommuneID, commune.VName));
                                    //    commune.VName = "P.Dư Hàng Kênh";
                                    //    commune.EName = LatinToAscii.Latin2Ascii(commune.VName);
                                    //}
                                    //else if (commune.CommuneID == 5423)
                                    //{
                                    //    LogFile.WriteProcess(string.Format("{0} - {1} -> P.Kênh Dương", commune.CommuneID, commune.VName));
                                    //    commune.VName = "P.Kênh Dương";
                                    //    commune.EName = LatinToAscii.Latin2Ascii(commune.VName);
                                    //}
                                    RunningParams.CommuneData.Objs.Add(commune.CommuneID, commune);
                                }
                                #endregion
                            }
                            else if (i == 3)
                            {
                                #region ==================== Đọc dữ liệu vùng tìm kiếm ====================
                                for (int j = 0; j < objectCount; j++)
                                {
                                    //Read Tile
                                    BAGTile tileInfo = new BAGTile();
                                    //Read 4 bytes (ID)
                                    tileInfo.TileID = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                                    //Read 2 bytes (ID)
                                    tileInfo.CommuneID = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                                    //Read 2 bytes (Length of Points)
                                    int length = (int)BitConverter.ToInt16(reader.ReadBytes(2), 0);
                                    tileInfo.PointList = new List<BAGPoint>();
                                    //Read list Points
                                    for (int k = 0; k < length; k++)
                                        tileInfo.PointList.Add(new BAGPoint(BitConverter.ToDouble(reader.ReadBytes(8), 0), BitConverter.ToDouble(reader.ReadBytes(8), 0)));
                                    // Build RTree
                                    RunningParams.TileData.RTree.Add(tileInfo.GetRectangle(), tileInfo);
                                    // Add object
                                    RunningParams.TileData.Objs.Add(tileInfo.TileID, tileInfo);
                                }
                                #endregion
                            }
                            else if (i == 4)
                            {
                                #region ==================== Đọc dữ liệu quận/huyện ưu tiên thấp khi tìm kiếm ====================
                                short districtID = 0;
                                RunningParams.DistrictPriority = new Hashtable();
                                for (int j = 0; j < objectCount; j++)
                                {
                                    districtID = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                                    RunningParams.DistrictPriority.Add(districtID, districtID);
                                    LogFile.WriteFile("", "DistrictPriority", "", districtID.ToString());
                                }
                                if(RunningParams.DistrictPriorityStr != null && RunningParams.DistrictPriorityStr.Length > 0)
                                {
                                    LogFile.WriteProcess("DistrictPriorityStr: " + RunningParams.DistrictPriorityStr);
                                    string[] temp = RunningParams.DistrictPriorityStr.Split(',');
                                    for (int j = 0; j < temp.Length; j++)
                                    {
                                        districtID = Convert.ToInt16(temp[j]);
                                        if (RunningParams.DistrictPriority.ContainsKey(districtID) == false)
                                        {
                                            RunningParams.DistrictPriority.Add(districtID, districtID);
                                            LogFile.WriteFile("", "DistrictPriority", "", districtID.ToString());
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        reader.Close();
                    }
                    stream.Close();
                }

                // Hủy file tạm
                File.Delete(filePath);

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RegionManager.LoadData, ex: " + ex.ToString());
                return false;
            }
        }

        private static bool CheckMore(short provinceID)
        {
            try
            {
                if (RunningParams.ProvinceRoadByLevel == null)
                    return false;
                else if (RunningParams.ProvinceRoadByLevel.Length == 0)
                    return false;
                else if(HTRoadByLevel.Count == 0)
                {
                    LogFile.WriteProcess("ProvinceRoadByLevel: " + RunningParams.ProvinceRoadByLevel);
                    string[] temp = RunningParams.ProvinceRoadByLevel.Split(',');
                    for (int i = 0; i < temp.Length; i++)
                        HTRoadByLevel.Add(Convert.ToInt16(temp[i]), null);
                }
                return HTRoadByLevel.ContainsKey(provinceID);
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RegionManager.CheckMore, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
