using System;
using System.Collections;
using System.IO;

using BAGeocoding.Entity.DataService;
using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.Utility;

using BAGeocoding.Utility;

namespace BAGeocoding.Bll.MapObj
{
    public class KeySearchManager
    {
        public static bool LoadData()
        {
            try
            {
                // Tạo file tạm
                string filePath = string.Empty;
                if (MainProcessing.FileCopy(Constants.DEFAULT_DIRECTORY_DATA, Constants.DEFAULT_KEY_FILE_NAME, ref filePath) == false)
                    return false;

                // Tiến hành đọc dữ liệu
                using (FileStream stream = new FileStream(filePath, FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        int nFiles = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        for (int i = 0; i < nFiles; i++)
                        {
                            int keyLength = 0;
                            int objectCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                            if (i == 0)
                            {
                                #region ==================== Đọc dữ liệu tìm kiếm tỉnh/thành ====================
                                RunningParams.ProvinceData.Keys = new Hashtable();
                                for (int j = 0; j < objectCount; j++)
                                {
                                    //Read province key
                                    UTLSearchKey province = new UTLSearchKey();
                                    //Read 1 bytes (Length of key)
                                    keyLength = (int)reader.ReadByte();
                                    province.KeyStr = Constants.TCVN3CodePage.GetString(reader.ReadBytes(keyLength));
                                    //Read 2 byte (Length of list province)
                                    int objLength = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                                    province.ObjectID = new Hashtable();
                                    //Read list ProvinceID
                                    for (int k = 0; k < objLength; k++)
                                    {
                                        province.ObjectID.Add(BitConverter.ToInt16(reader.ReadBytes(2), 0), new BAGKeyRate
                                        {
                                            IndexID = reader.ReadByte(),
                                            Percent = reader.ReadByte()
                                        });
                                    }
                                    RunningParams.ProvinceData.Keys.Add(province.KeyStr, province);
                                }
                                #endregion
                            }
                            else if (i == 1)
                            {
                                #region ==================== Đọc dữ liệu tìm kiếm quận/huyện ====================
                                RunningParams.DistrictData.Keys = new Hashtable();
                                for (int j = 0; j < objectCount; j++)
                                {
                                    //Read district key
                                    UTLSearchKey district = new UTLSearchKey();
                                    //Read 1 bytes (Length of key)
                                    keyLength = (int)reader.ReadByte();
                                    district.KeyStr = Constants.TCVN3CodePage.GetString(reader.ReadBytes(keyLength));
                                    //Read 2 byte (Length of list district)
                                    int objLength = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                                    district.ObjectID = new Hashtable();
                                    //Read list DistrictID
                                    for (int k = 0; k < objLength; k++)
                                    {
                                        district.ObjectID.Add(BitConverter.ToInt16(reader.ReadBytes(2), 0), new BAGKeyRate
                                        {
                                            IndexID = reader.ReadByte(),
                                            Percent = reader.ReadByte(),
                                            ReferenceID = BitConverter.ToInt16(reader.ReadBytes(2), 0)
                                        });
                                    }
                                    RunningParams.DistrictData.Keys.Add(district.KeyStr, district);
                                }
                                #endregion
                            }
                            else if (i == 2)
                            {
                                #region ==================== Đọc dữ liệu tìm kiếm xã/phường ====================
                                RunningParams.CommuneData.Keys = new Hashtable();
                                for (int j = 0; j < objectCount; j++)
                                {
                                    //Read commune key
                                    UTLSearchKey commune = new UTLSearchKey();
                                    //Read 1 bytes (Length of key)
                                    keyLength = (int)reader.ReadByte();
                                    commune.KeyStr = Constants.TCVN3CodePage.GetString(reader.ReadBytes(keyLength));
                                    //Read 2 byte (Length of list commune)
                                    int objLength = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                                    commune.ObjectID = new Hashtable();
                                    //Read list CommuneID
                                    for (int k = 0; k < objLength; k++)
                                    {
                                        commune.ObjectID.Add(BitConverter.ToInt16(reader.ReadBytes(2), 0), new BAGKeyRate
                                        {
                                            IndexID = reader.ReadByte(),
                                            Percent = reader.ReadByte()
                                        });
                                    }
                                    RunningParams.CommuneData.Keys.Add(commune.KeyStr, commune);
                                }
                                #endregion
                            }
                            else if (i == 3)
                            {
                                #region ==================== Đọc dữ liệu tìm kiếm tên điểm ====================
                                for (int j = 0; j < objectCount; j++)
                                {
                                    //Read poi key
                                    UTLSearchKey poi = new UTLSearchKey();
                                    //Read 1 bytes (Length of key)
                                    keyLength = (int)reader.ReadByte();
                                    poi.KeyStr = Constants.TCVN3CodePage.GetString(reader.ReadBytes(keyLength));
                                    //Read 2 byte (Length of list commune)
                                    int objLength = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                                    poi.ObjectID = new Hashtable();
                                    //Read list CommuneID
                                    for (int k = 0; k < objLength; k++)
                                        poi.ObjectID.Add(BitConverter.ToInt32(reader.ReadBytes(4), 0), null);
                                    RunningParams.PointData.KeyName.Add(poi.KeyStr, poi);
                                }
                                #endregion
                            }
                            else if (i == 4)
                            {
                                #region ==================== Đọc dữ liệu tìm kiếm thông tin điểm ====================
                                for (int j = 0; j < objectCount; j++)
                                {
                                    //Read poi key
                                    UTLSearchKey poi = new UTLSearchKey();
                                    //Read 1 bytes (Length of key)
                                    keyLength = (int)reader.ReadByte();
                                    poi.KeyStr = Constants.TCVN3CodePage.GetString(reader.ReadBytes(keyLength));
                                    //Read 2 byte (Length of list commune)
                                    int objLength = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                                    poi.ObjectID = new Hashtable();
                                    //Read list CommuneID
                                    for (int k = 0; k < objLength; k++)
                                    {
                                        poi.ObjectID.Add(BitConverter.ToInt32(reader.ReadBytes(4), 0), null);
                                    }
                                    RunningParams.PointData.KeyInfo.Add(poi.KeyStr, poi);
                                }
                                #endregion
                            }
                            else if (i == 5)
                            {
                                #region ==================== Đọc dữ liệu đường phố đặc biệt ====================
                                int roadLength = 0;
                                string roadName = string.Empty; 
                                RunningParams.RoadSpecial = new Hashtable();
                                for (int j = 0; j < objectCount; j++)
                                {
                                    roadLength = (int)reader.ReadByte();
                                    roadName = Constants.TCVN3CodePage.GetString(reader.ReadBytes(roadLength));
                                    RunningParams.RoadSpecial.Add(roadName, null);
                                }
                                #endregion
                            }
                            else
                            {
                                #region ==================== Đọc dữ liệu tìm kiếm đường phố ====================
                                for (int j = 0; j < objectCount; j++)
                                {
                                    //Read ProviceID
                                    short proviceID = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                                    DTSSegment segmentData = new DTSSegment();
                                    if (RunningParams.ProvinceData.Segm.Contains(proviceID) == true)
                                        segmentData = (DTSSegment)RunningParams.ProvinceData.Segm[proviceID];
                                    // Số lượng từ khóa
                                    int keyCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                                    for (int k = 0; k < keyCount; k++)
                                    {
                                        UTLSearchKey segmentKey = new UTLSearchKey();
                                        keyLength = (int)reader.ReadByte();
                                        segmentKey.KeyStr = Constants.TCVN3CodePage.GetString(reader.ReadBytes(keyLength));
                                        // Số lượng segment có từ khóa xuất hiện
                                        int segmentCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                                        segmentKey.ObjectID = new Hashtable();
                                        for (int m = 0; m < segmentCount; m++)
                                        {
                                            int objectID = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                                            BAGKeyRate keyRate = new BAGKeyRate
                                            {
                                                IndexID = reader.ReadByte(),
                                                Percent = reader.ReadByte(),
                                                ReferenceID = BitConverter.ToInt16(reader.ReadBytes(2), 0)
                                            };
                                            if (segmentKey.ObjectID.ContainsKey(objectID) == false)
                                                segmentKey.ObjectID.Add(objectID, keyRate);
                                        }
                                        segmentData.Keys.Add(segmentKey.KeyStr, segmentKey);
                                    }

                                    if (RunningParams.ProvinceData.Segm.Contains(proviceID) == true)
                                        RunningParams.ProvinceData.Segm[proviceID] = segmentData;
                                    else
                                        RunningParams.ProvinceData.Segm.Add(proviceID, segmentData);
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
                LogFile.WriteError("KeySearchManager.LoadData, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
