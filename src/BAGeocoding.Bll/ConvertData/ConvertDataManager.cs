using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using BAGeocoding.Entity.ConvertData;
using BAGeocoding.Entity.Enum;

using BAGeocoding.Utility;

namespace BAGeocoding.Bll.WriteData
{
    public class ConvertDataManager
    {
        /// <summary>
        /// Chuyển đổi dữ liệu Text -> Mif
        /// </summary>
        public static bool Text2MifPoint(CVRCondition condition)
        {
            try
            {
                #region ==================== 1. Đọc dữ liệu từ file txt ====================
                List<CVRPoint> objectList = new List<CVRPoint>();
                using (StreamReader streamReader = new StreamReader(condition.InputStr, Encoding.UTF8))
                {
                    while (streamReader.EndOfStream == false)
                    {
                        CVRPoint objectItem = new CVRPoint();
                        if (objectItem.FromString(condition.KindID, streamReader.ReadLine().Trim()) == false)
                        {
                            if (condition.KindID == EnumCVRText2MifKind.LogFileTaxi)
                                continue;
                            return false;
                        }
                        objectItem.ObjectID = objectList.Count + 1;
                        objectList.Add(objectItem);
                    }
                    streamReader.Close();
                }
                if (objectList.Count == 0)
                    return false;
                #endregion

                #region ==================== 2. Tiến hành ghi dữ liệu ====================
                #region ==================== 2.1 File mở rộng .mif ====================
                string fileResultMif = condition.OutputStr;
                if (File.Exists(fileResultMif))
                    File.Delete(fileResultMif);
                using (StreamWriter streamWrite = new StreamWriter(fileResultMif, false))
                {
                    streamWrite.WriteLine("Version 300");
                    streamWrite.WriteLine("CHARSET \"Neutral\"");
                    streamWrite.WriteLine("Delimiter \", \"");
                    streamWrite.WriteLine("CoordSys Earth Projection 1, 104");
                    if (condition.KindID == EnumCVRText2MifKind.CaptureData)
                    {
                        streamWrite.WriteLine("Columns 3");
                        streamWrite.WriteLine("  ObjectID Decimal(10, 0)");
                        streamWrite.WriteLine("  Name Char(200)");
                        streamWrite.WriteLine("  Info Char(200)");
                        streamWrite.WriteLine("Data");
                    }
                    else if (condition.KindID == EnumCVRText2MifKind.MapToolPOI)
                    {
                        streamWrite.WriteLine("Columns 5");
                        streamWrite.WriteLine("  ObjectID Decimal(10, 0)");
                        streamWrite.WriteLine("  ImageSrc Char(200)");
                        streamWrite.WriteLine("  Name Char(200)");
                        streamWrite.WriteLine("  KindID Decimal(10, 0)");
                        streamWrite.WriteLine("  Info Char(200)");
                        streamWrite.WriteLine("Data");
                    }
                    else if (condition.KindID == EnumCVRText2MifKind.LogFileTaxi)
                    {
                        streamWrite.WriteLine("Columns 5");
                        streamWrite.WriteLine("  ObjectID Decimal(10, 0)");
                        streamWrite.WriteLine("  XNCode Decimal(10, 0)");
                        streamWrite.WriteLine("  Plate Char(200)");
                        streamWrite.WriteLine("  State Decimal(10, 0)");
                        streamWrite.WriteLine("  TS Char(200)");
                        streamWrite.WriteLine("Data");
                    }
                    streamWrite.WriteLine("");

                    if (objectList != null)
                    {
                        for (int i = 0; i < objectList.Count; i++)
                        {
                            streamWrite.WriteLine(string.Format("Point {0} {1}", Math.Round(objectList[i].Point.Lng, 8), Math.Round(objectList[i].Point.Lat, 8)));
                            streamWrite.WriteLine("    Symbol (34, 0, 12)");
                        }
                    }
                }
                #endregion

                #region ==================== 2.2 File mở rộng .mid ====================
                string fileResultMid = condition.OutputStr.ToLower().Replace(".mif", ".mid");
                if (File.Exists(fileResultMid))
                    File.Delete(fileResultMid);
                using (StreamWriter streamWrite = new StreamWriter(fileResultMid, false, Encoding.UTF8))
                {
                    if (objectList != null)
                    {
                        for (int i = 0; i < objectList.Count; i++)
                        {
                            if (condition.KindID == EnumCVRText2MifKind.CaptureData)
                                streamWrite.WriteLine(string.Format("{0},\"{1}\",\"{2}\"", objectList[i].ObjectID, objectList[i].Name, objectList[i].Info));
                            else if (condition.KindID == EnumCVRText2MifKind.MapToolPOI)
                                streamWrite.WriteLine(string.Format("{0},\"{1}\",\"{2}\",{3},\"{4}\"", objectList[i].ObjectID, objectList[i].ImageSrc, objectList[i].Name, objectList[i].TypeID, objectList[i].Info));
                            else if (condition.KindID == EnumCVRText2MifKind.LogFileTaxi)
                                streamWrite.WriteLine(string.Format("{0},{1},\"{2}\",{3},\"{4:yyyy-MM-dd HH:mm:ss}\"", objectList[i].ObjectID, objectList[i].XNCode, objectList[i].Plate, objectList[i].State, objectList[i].TS));
                        }
                    }
                }
                #endregion
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("ConvertDataManager.Text2MifPoint, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Chuyển đổi dữ liệu Text -> Mif
        /// </summary>
        public static bool Text2MifPointExt(CVRCondition condition)
        {
            try
            {
                condition.OutputStr = condition.OutputStr.Substring(0, condition.OutputStr.Length - 4);
                string fileResultMif = string.Empty;
                string fileResultMid = string.Empty;
                StreamWriter streamWriteMif = null;
                StreamWriter streamWriteMid = null;
                using (StreamReader streamReader = new StreamReader(condition.InputStr, Encoding.UTF8))
                {
                    int objectID = 1;
                    int fileCount = 1;
                    int objectMax = 4194304;    // 2^22
                    while (streamReader.EndOfStream == false)
                    {
                        #region ==================== 1. Kiểm tra tạo file ====================
                        if (objectID == 1)
                        {
                            #region ==================== 1.1 File mở rộng .mif ====================
                            fileResultMif = string.Format("{0}-{1}.mif", condition.OutputStr, fileCount);
                            if (File.Exists(fileResultMif))
                                File.Delete(fileResultMif);

                            streamWriteMif = new StreamWriter(fileResultMif, false);
                            streamWriteMif.WriteLine("Version 300");
                            streamWriteMif.WriteLine("CHARSET \"Neutral\"");
                            streamWriteMif.WriteLine("Delimiter \", \"");
                            streamWriteMif.WriteLine("CoordSys Earth Projection 1, 104");
                            if (condition.KindID == EnumCVRText2MifKind.CaptureData)
                            {
                                streamWriteMif.WriteLine("Columns 3");
                                streamWriteMif.WriteLine("  ObjectID Decimal(10, 0)");
                                streamWriteMif.WriteLine("  Name Char(200)");
                                streamWriteMif.WriteLine("  Info Char(200)");
                                streamWriteMif.WriteLine("Data");
                            }
                            else if (condition.KindID == EnumCVRText2MifKind.MapToolPOI)
                            {
                                streamWriteMif.WriteLine("Columns 5");
                                streamWriteMif.WriteLine("  ObjectID Decimal(10, 0)");
                                streamWriteMif.WriteLine("  ImageSrc Char(200)");
                                streamWriteMif.WriteLine("  Name Char(200)");
                                streamWriteMif.WriteLine("  KindID Decimal(10, 0)");
                                streamWriteMif.WriteLine("  Info Char(200)");
                                streamWriteMif.WriteLine("Data");
                            }
                            else if (condition.KindID == EnumCVRText2MifKind.LogFileTaxi)
                            {
                                streamWriteMif.WriteLine("Columns 3");
                                streamWriteMif.WriteLine("  ObjectID Decimal(10, 0)");
                                streamWriteMif.WriteLine("  XNCode Decimal(10, 0)");
                                streamWriteMif.WriteLine("  Plate Char(200)");
                                streamWriteMif.WriteLine("Data");
                            }
                            streamWriteMif.WriteLine("");
                            #endregion

                            #region ==================== 1.2 File mở rộng .mid ====================
                            fileResultMid = string.Format("{0}-{1}.mid", condition.OutputStr, fileCount);
                            if (File.Exists(fileResultMid))
                                File.Delete(fileResultMid);
                            streamWriteMid = new StreamWriter(fileResultMid, false, Encoding.UTF8);
                            #endregion
                        }
                        #endregion

                        #region ==================== 2. Đọc và ghi dữ liệu ====================
                        // 2.1 Đọc dữ liệu
                        CVRPoint objectItem = new CVRPoint();
                        if (objectItem.FromString(condition.KindID, streamReader.ReadLine().Trim()) == false)
                            continue;
                        objectItem.ObjectID = objectID++;
                        // 2.2 Ghi dữ liệu file MIF
                        streamWriteMif.WriteLine(string.Format("Point {0} {1}", Math.Round(objectItem.Point.Lng, 8), Math.Round(objectItem.Point.Lat, 8)));
                        streamWriteMif.WriteLine("    Symbol (34, 0, 12)");
                        // 2.3 Ghi dữ liệu file MID
                        if (condition.KindID == EnumCVRText2MifKind.CaptureData)
                            streamWriteMid.WriteLine(string.Format("{0},\"{1}\",\"{2}\"", objectItem.ObjectID, objectItem.Name, objectItem.Info));
                        else if (condition.KindID == EnumCVRText2MifKind.MapToolPOI)
                            streamWriteMid.WriteLine(string.Format("{0},\"{1}\",\"{2}\",{3},\"{4}\"", objectItem.ObjectID, objectItem.ImageSrc, objectItem.Name, objectItem.TypeID, objectItem.Info));
                        else if (condition.KindID == EnumCVRText2MifKind.LogFileTaxi)
                            streamWriteMid.WriteLine(string.Format("{0},{1},\"{2}\"", objectItem.ObjectID, objectItem.XNCode, objectItem.Plate));
                        #endregion

                        #region ==================== 3. Kiểm tra tạo kết thúc để tạo file mới ====================
                        if (objectID > objectMax)
                        {
                            objectID = 1;
                            fileCount++;
                            streamWriteMif.Close();
                            streamWriteMid.Close();
                        }
                        #endregion
                    }
                    streamReader.Close();
                    if (objectID > 1)
                    {
                        streamWriteMif.Close();
                        streamWriteMid.Close();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("ConvertDataManager.Text2MifPoint, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Chuyển đổi dữ liệu đường từ Text -> Mif
        /// 
        /// 1. File MIF, MID chuẩn UTF-8
        /// 2. Dùng Notepad chuyển sang ASCCI
        /// 3. Convert bằng MapInfo
        /// </summary>
        public static bool Text2MifPolyline(CVRCondition condition)
        {
            try
            {
                #region ==================== 1. Đọc dữ liệu từ file txt ====================
                List<CVRPolyline> objectList = new List<CVRPolyline>();
                using (StreamReader streamReader = new StreamReader(condition.InputStr, Encoding.UTF8))
                {
                    while (streamReader.EndOfStream == false)
                    {
                        CVRPolyline objectItem = new CVRPolyline();
                        if (objectItem.FromString(condition.KindID, streamReader.ReadLine().Trim()) == false)
                            return false;
                        else if (objectItem.SegmentID > 0)
                            objectList.Add(objectItem);
                    }
                    streamReader.Close();
                }
                if (objectList.Count == 0)
                    return false;

                //List<BAGSegment> segmentList = EDTSegmentDAO.GetByProvince(16);
                //if (segmentList == null || segmentList.Count == 0)
                //    return false;
                //objectList = new List<CVRPolyline>();
                //for (int i = 0; i < segmentList.Count; i++)
                //    objectList.Add(new CVRPolyline(segmentList[i]));
                #endregion

                #region ==================== 2. Tiến hành ghi dữ liệu ====================
                #region ==================== 2.1 File mở rộng .mif ====================
                string fileResultMif = condition.OutputStr;
                if (File.Exists(fileResultMif))
                    File.Delete(fileResultMif);
                using (StreamWriter streamWrite = new StreamWriter(fileResultMif, false, Encoding.ASCII))
                {
                    streamWrite.WriteLine("Version 300");
                    streamWrite.WriteLine("CHARSET \"Neutral\"");
                    streamWrite.WriteLine("Delimiter \", \"");
                    streamWrite.WriteLine("CoordSys Earth Projection 1, 104");
                    if (condition.KindID == EnumCVRText2MifKind.MapToolPOI)
                    {
                        streamWrite.WriteLine("Columns 9");
                        streamWrite.WriteLine("  SegmentID Decimal(10, 0)");
                        streamWrite.WriteLine("  Name Char(200)");
                        streamWrite.WriteLine("  StartLeft Decimal(10, 0)");
                        streamWrite.WriteLine("  EndLeft Decimal(10, 0)");
                        streamWrite.WriteLine("  StartRight Decimal(10, 0)");
                        streamWrite.WriteLine("  EndRight Decimal(10, 0)");
                        streamWrite.WriteLine("  Visible Decimal(10, 0)");
                        streamWrite.WriteLine("  State Decimal(10, 0)");
                        streamWrite.WriteLine("  Note Char(200)");
                        streamWrite.WriteLine("Data");
                    }
                    streamWrite.WriteLine("");

                    if (objectList != null)
                    {
                        for (int i = 0; i < objectList.Count; i++)
                        {
                            streamWrite.WriteLine(string.Format("Pline {0}", objectList[i].PointList.Count));
                            for (int j = 0; j < objectList[i].PointList.Count; j++)
                                streamWrite.WriteLine(string.Format("{0:N8} {1:N8}", objectList[i].PointList[j].Lng, objectList[i].PointList[j].Lat));
                            streamWrite.WriteLine("    Pen (1, 2, 0)");
                        }
                    }
                }
                #endregion

                #region ==================== 2.2 File mở rộng .mif ====================
                string fileResultMid = condition.OutputStr.ToLower().Replace(".mif", ".mid");
                if (File.Exists(fileResultMid))
                    File.Delete(fileResultMid);
                using (StreamWriter streamWrite = new StreamWriter(fileResultMid, false, Encoding.UTF8))
                {
                    if (objectList != null)
                    {
                        for (int i = 0; i < objectList.Count; i++)
                        {
                            if (condition.KindID == EnumCVRText2MifKind.MapToolPOI)
                                streamWrite.WriteLine(objectList[i].ToString());
                        }
                    }
                }
                #endregion
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("ConvertDataManager.Text2MifPolyline, ex: " + ex.ToString());
                return false;
            }
        }


        /// <summary>
        /// Chuyển đổi dữ liệu Text -> Mif
        /// </summary>
        public static bool Text2MifPointGps(CVRCondition condition)
        {
            try
            {
                int fileIndexID = 0;
                int objectMax = 2097152;    // 2^21
                string fileResultMif = string.Empty;
                string fileResultMid = string.Empty;
                string fileNameOutput = string.Empty;
                List<CVRPoint> objectList = new List<CVRPoint>();
                while (fileIndexID < condition.FileList.Count)
                {
                    #region ==================== 1. Đọc dữ liệu từ file text ====================
                    objectList = new List<CVRPoint>();
                    while (true)
                    {
                        using (StreamReader streamReader = new StreamReader(condition.FileList[fileIndexID].NameFull, Encoding.UTF8))
                        {
                            while (streamReader.EndOfStream == false)
                            {
                                CVRPoint objectItem = new CVRPoint();
                                if (objectItem.FromString(condition.KindID, streamReader.ReadLine().Trim()) == false)
                                    continue;
                                objectItem.ObjectID = objectList.Count + 1;
                                objectList.Add(objectItem);
                            }
                            streamReader.Close();

                            if (condition.MultiFile == false)
                                break;
                            else if (objectList.Count > objectMax)
                                break;
                            else if (fileIndexID == condition.FileList.Count - 1)
                                break;
                            else
                                fileIndexID++;
                        }
                    }
                    if (objectList.Count == 0)
                        continue;
                    #endregion

                    #region ==================== 2. Tạo file bản đồ ====================
                    if (condition.MultiFile == false)
                        fileNameOutput = condition.FileList[fileIndexID].Name.Substring(0, condition.FileList[fileIndexID].Name.Length - 4);
                    else
                        fileNameOutput = string.Format("OUT.{0:yyyyMMddHHmmss}_{1}", DateTime.Now, DateTime.Now.Millisecond);

                    #region ==================== 2.1 File mở rộng .mif ====================
                    fileResultMif = string.Format("{0}\\{1}.mif", condition.FileList[fileIndexID].Path, fileNameOutput);
                    if (File.Exists(fileResultMif))
                        File.Delete(fileResultMif);
                    using (StreamWriter streamWrite = new StreamWriter(fileResultMif, false, Encoding.ASCII))
                    {
                        streamWrite.WriteLine("Version 300");
                        streamWrite.WriteLine("CHARSET \"Neutral\"");
                        streamWrite.WriteLine("Delimiter \", \"");
                        streamWrite.WriteLine("CoordSys Earth Projection 1, 104");
                        streamWrite.WriteLine("Columns 3");
                        streamWrite.WriteLine("  ObjectID Decimal(10, 0)");
                        streamWrite.WriteLine("  XNCode Decimal(10, 0)");
                        streamWrite.WriteLine("  Plate Char(200)");
                        streamWrite.WriteLine("Data");
                        streamWrite.WriteLine("");

                        for (int i = 0; i < objectList.Count; i++)
                        {
                            if (condition.TypeID == EnumBAGObjectType.Point)
                            {
                                streamWrite.WriteLine(string.Format("Point {0} {1}", Math.Round(objectList[i].Point.Lng, 8), Math.Round(objectList[i].Point.Lat, 8)));
                                streamWrite.WriteLine("    Symbol (34, 0, 12)");
                            }
                            else if (condition.TypeID == EnumBAGObjectType.Polyline)
                            {
                                if (i == objectList.Count - 1)
                                    break;
                                streamWrite.WriteLine(string.Format("Pline {0}", 2));
                                streamWrite.WriteLine(string.Format("{0:N8} {1:N8}", objectList[i].Point.Lng, objectList[i].Point.Lat));
                                streamWrite.WriteLine(string.Format("{0:N8} {1:N8}", objectList[i + 1].Point.Lng, objectList[i + 1].Point.Lat));
                                streamWrite.WriteLine("    Pen (1, 2, 0)");
                            }
                        }
                    }
                    #endregion

                    #region ==================== 2.2 File mở rộng .mid ====================
                    fileResultMid = string.Format("{0}\\{1}.mid", condition.FileList[fileIndexID].Path, fileNameOutput);
                    if (File.Exists(fileResultMid))
                        File.Delete(fileResultMid);
                    using (StreamWriter streamWrite = new StreamWriter(fileResultMid, false, Encoding.ASCII))
                    {
                        for (int i = 0; i < objectList.Count; i++)
                            streamWrite.WriteLine(objectList[i].ToString());
                    }
                    #endregion
                    #endregion

                    fileIndexID++;
                }

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("ConvertDataManager.Text2MifPointGps, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Chuyển đổi dữ liệu Text -> Mif
        /// </summary>
        public static bool Json2MifPoint(CVRCondition condition)
        {
            try
            {
                #region ==================== 1. Đọc dữ liệu từ file txt ====================
                List<JSNPoint01> objectList = new List<JSNPoint01>();
                using (StreamReader streamReader = new StreamReader(condition.InputStr, Encoding.UTF8))
                {
                    objectList = StringUlt.Json2Object<List<JSNPoint01>>(streamReader.ReadToEnd().Trim());
                    streamReader.Close();
                }
                if (objectList.Count == 0)
                    return false;
                #endregion

                #region ==================== 2. Tiến hành ghi dữ liệu ====================
                #region ==================== 2.1 File mở rộng .mif ====================
                string fileResultMif = condition.OutputStr;
                if (File.Exists(fileResultMif))
                    File.Delete(fileResultMif);

                using (StreamWriter streamWrite = new StreamWriter(fileResultMif, false, Encoding.ASCII))
                {
                    streamWrite.WriteLine("Version 300");
                    streamWrite.WriteLine("CHARSET \"Neutral\"");
                    streamWrite.WriteLine("Delimiter \", \"");
                    streamWrite.WriteLine("CoordSys Earth Projection 1, 104");
                    streamWrite.WriteLine("Columns 5");
                    streamWrite.WriteLine("  IndexID Integer");
                    streamWrite.WriteLine("  IDStr Char(256)");
                    streamWrite.WriteLine("  TypeStr Char(256)");
                    streamWrite.WriteLine("  NameStr Char(256)");
                    streamWrite.WriteLine("  SpeedStr Char(256)");
                    streamWrite.WriteLine("Data");
                    streamWrite.WriteLine("");

                    if (objectList != null)
                    {
                        for (int i = 0; i < objectList.Count; i++)
                        {
                            streamWrite.WriteLine(string.Format("Point {0} {1}", Math.Round(objectList[i].longitude, 8), Math.Round(objectList[i].latitude, 8)));
                            streamWrite.WriteLine("    Symbol (34, 0, 12)");
                        }
                    }
                }
                #endregion

                #region ==================== 2.2 File mở rộng .mid ====================
                string fileResultMid = condition.OutputStr.ToLower().Replace(".mif", ".mid");
                if (File.Exists(fileResultMid))
                    File.Delete(fileResultMid);
                using (StreamWriter streamWrite = new StreamWriter(fileResultMid, false, Encoding.Unicode))
                {
                    if (objectList != null)
                    {
                        for (int i = 0; i < objectList.Count; i++)
                            streamWrite.WriteLine(string.Format("{0},{1},{2},{3},{4}", (i + 1), objectList[i].id, objectList[i].type, objectList[i].name, objectList[i].speedlimit));
                    }
                }
                #endregion
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("ConvertDataManager.Json2MifPoint, ex: " + ex.ToString());
                return false;
            }
        }
    }
}