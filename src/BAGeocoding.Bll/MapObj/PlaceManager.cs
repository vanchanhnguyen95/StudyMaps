using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using BAGeocoding.Entity.Enum.MapObject;
using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;

namespace BAGeocoding.Bll.MapObj
{
    public class PlaceManager
    {
        public static bool LoadData(bool state = true)
        {
            try
            {
                if (state == false)
                    return true;

                // Tạo file tạm
                string filePath = string.Empty;
                if (MainProcessing.FileCopy(Constants.DEFAULT_DIRECTORY_DATA, Constants.DEFAULT_PLACE_FILE_NAME, ref filePath) == false)
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
                            int addressLength = 0;
                            int objectCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                            if (i == 0)
                            {
                                #region ==================== Đọc dữ liệu khu đô thị ====================
                                for (int j = 0; j < objectCount; j++)
                                {
                                    BAGPlace urban = new BAGPlace();
                                    urban.PlaceID = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                                    // Read Name
                                    nameLength = (int)reader.ReadByte();
                                    urban.Name = Constants.UnicodeCodePage.GetString(reader.ReadBytes(nameLength));
                                    // Read Address
                                    addressLength = (int)BitConverter.ToInt16(reader.ReadBytes(2), 0);
                                    urban.Address = Constants.UnicodeCodePage.GetString(reader.ReadBytes(addressLength));
                                    //Add object
                                    RunningParams.PlaceData.Urban.Add(urban.PlaceID, urban);
                                }
                                #endregion
                            }
                            else if (i == 1)
                            {
                                #region ==================== Đọc dữ liệu lô đất ====================
                                for (int j = 0; j < objectCount; j++)
                                {
                                    BAGPlace portion = new BAGPlace();
                                    portion.PlaceID = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                                    portion.ParentID = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                                    // Read Name
                                    nameLength = (int)reader.ReadByte();
                                    portion.Name = Constants.UnicodeCodePage.GetString(reader.ReadBytes(nameLength));
                                    //Add object
                                    RunningParams.PlaceData.Portion.Add(portion.PlaceID, portion);
                                }
                                #endregion
                            }
                            else if(i == 2)
                            {
                                #region ==================== Đọc dữ liệu ô đất ====================
                                for (int j = 0; j < objectCount; j++)
                                {
                                    BAGPlace plot = new BAGPlace();
                                    plot.PlaceID = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                                    plot.ParentID = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                                    // Read Name
                                    nameLength = (int)reader.ReadByte();
                                    plot.Name = Constants.UnicodeCodePage.GetString(reader.ReadBytes(nameLength));
                                    // Read center
                                    plot.Center = new BAGPoint(BitConverter.ToDouble(reader.ReadBytes(8), 0), BitConverter.ToDouble(reader.ReadBytes(8), 0));
                                    //Read 2 bytes (Length of list Points)
                                    int length = (int)BitConverter.ToInt16(reader.ReadBytes(2), 0);
                                    plot.PointList = new List<BAGPoint>();
                                    //Read list Points
                                    for (int k = 0; k < length; k++)
                                        plot.PointList.Add(new BAGPoint(BitConverter.ToDouble(reader.ReadBytes(8), 0), BitConverter.ToDouble(reader.ReadBytes(8), 0)));
                                    // Build RTree
                                    RunningParams.PlaceData.RTree.Add(plot.GetRectangle(), plot);
                                    //Add object
                                    RunningParams.PlaceData.Plot.Add(plot.PlaceID, plot);
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
                LogFile.WriteError("PlaceManager.LoadData, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
