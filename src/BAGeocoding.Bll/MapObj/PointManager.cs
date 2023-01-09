using System;
using System.Collections.Generic;
using System.IO;

using BAGeocoding.Entity.DataService;
using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;

namespace BAGeocoding.Bll.MapObj
{
    public class PointManager
    {
        public static bool LoadData()
        {
            try
            {
                // Tạo file tạm
                string filePath = string.Empty;
                if (MainProcessing.FileCopy(Constants.DEFAULT_DIRECTORY_DATA, Constants.DEFAULT_POI_FILE_NAME, ref filePath) == false)
                    return false;

                // Tiến hành đọc dữ liệu
                using (FileStream stream = new FileStream(filePath, FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        int nameLength = 0;
                        int objectCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        for (int j = 0; j < objectCount; j++)
                        {
                            //Read segment
                            BAGPoi poiInfo = new BAGPoi();
                            //Read 4 bytes (ID)
                            poiInfo.PoiID = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                            //Coords
                            poiInfo.Coords = new BAGPoint(BitConverter.ToSingle(reader.ReadBytes(4), 0), BitConverter.ToSingle(reader.ReadBytes(4), 0));
                            //Read 1 byte (Length of Name)
                            nameLength = (int)reader.ReadByte();
                            //Read Name
                            poiInfo.VName = Constants.UnicodeCodePage.GetString(reader.ReadBytes(nameLength));
                            poiInfo.EName = LatinToAscii.Latin2Ascii(poiInfo.VName);
                            //Read 2 byte (Length of Info)
                            int InfoLength = (int)BitConverter.ToInt16(reader.ReadBytes(2), 0);
                            //Read Info
                            poiInfo.VInfo = Constants.UnicodeCodePage.GetString(reader.ReadBytes(InfoLength));
                            poiInfo.EInfo = LatinToAscii.Latin2Ascii(poiInfo.VInfo);

                            RunningParams.PointData.Objs.Add(poiInfo.PoiID, poiInfo);
                            RunningParams.PointData.RTree.Add(poiInfo.GetRectangle(), poiInfo.PoiID);
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
                LogFile.WriteError("PointManager.LoadData, ex: " + ex.ToString());
                return false;
            }
        }
    }
}