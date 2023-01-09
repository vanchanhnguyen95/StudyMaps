using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using BAGeocoding.Dal.MapRoute;

using BAGeocoding.Entity.Enum.MapObject;
using BAGeocoding.Entity.Router;

using BAGeocoding.Utility;

namespace BAGeocoding.Bll.MapObj
{
    public class RouterManager
    {
        /// <summary>
        /// Tải cấu hình xác thực (Tài khoản + Key)
        /// </summary>
        /// <returns></returns>
        public static bool LoadConfig()
        {
            try
            {
                LogFile.WriteProcess("-------------------- Bắt đầu đọc cấu hình tài khoản --------------------");
                Hashtable trafficHT = new Hashtable();
                RunningParams.RouterData.Authen = DTSAuthenDAO.GetActived(ref trafficHT);
                if (RunningParams.RouterData.Authen == null)
                    RunningParams.RouterData.Authen = new Hashtable();
                RunningParams.RouterData.Traffic = trafficHT;
                LogFile.WriteProcess(string.Format("-------------------- Kết thúc đọc tài khoản ({0:N0}) --------------------", RunningParams.RouterData.Authen.Count));
                LogFile.WriteProcess("-------------------- Bắt đầu đọc cấu hình key ip --------------------");
                RunningParams.RouterData.Register = DTSRegisterDAO.GetActived();
                if (RunningParams.RouterData.Register == null)
                    RunningParams.RouterData.Register = new Hashtable();
                LogFile.WriteProcess(string.Format("-------------------- Kết thúc đọc cấu hình ({0:N0}) --------------------", RunningParams.RouterData.Register.Count));

                return RunningParams.RouterData.Register.Count > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RouterManager.LoadConfig, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Tải dữ liệu tìm đường
        /// </summary>
        public static bool LoadData()
        {
            try
            {
                // Tạo file tạm
                string filePath = string.Empty;
                if (MainProcessing.FileCopy(Constants.DEFAULT_DIRECTORY_DATA, Constants.DEFAULT_FILE_NAME_ROUTE, ref filePath) == false)
                    return false;
                LogFile.WriteProcess("-------------------- Bắt đầu đọc dữ liệu tìm đường --------------------");
                LogFile.WriteProcess(string.Format("File: {0}", filePath));

                // Khai báo biến
                int byteIndex = 0;
                int nameLength = 0;
                int pointCount = 0;
                int pointIndex = 0;
                int segmentCount = 0;

                // Tiến hành đọc dữ liệu
                using (FileStream stream = new FileStream(filePath, FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        segmentCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        LogFile.WriteProcess(string.Format("Segment: {0}", segmentCount));
                        for (int i = 0; i < segmentCount; i++)
                        {
                            #region ==================== Đọc thông tin đường ====================
                            BARSegment segmentInfo = new BARSegment { SegmentID = BitConverter.ToInt32(reader.ReadBytes(4), 0) };
                            byteIndex += 4;
                            // .2.2 Đọc tên
                            nameLength = (int)reader.ReadByte();
                            segmentInfo.VName = Constants.UnicodeCodePage.GetString(reader.ReadBytes(nameLength));
                            segmentInfo.EName = LatinToAscii.Latin2Ascii(segmentInfo.VName);
                            byteIndex += nameLength;
                            // .2.3 Đọc  (ClassFunc)
                            segmentInfo.ClassFunc = reader.ReadByte();
                            // .2.3 Đọc hệ số (Coeff)
                            segmentInfo.Coeff = BitConverter.ToDouble(reader.ReadBytes(8), 0);
                            byteIndex += 8;
                            // .2.4 Đọc  (AllowCar)
                            segmentInfo.AllowCar = new BARDirection(reader.ReadByte());
                            // .2.5 Phà/đò
                            segmentInfo.FerryThese = (reader.ReadByte() == 1);
                            // .2.6 Đọc dữ liệu mở rộng
                            segmentInfo.DataExt = reader.ReadByte();
                            // .2.7 Đọc tọa độ (Length of list Points)
                            pointCount = (int)BitConverter.ToInt16(reader.ReadBytes(2), 0);
                            byteIndex += 2;
                            segmentInfo.PointList = new List<BARPoint>();
                            for (int j = 0; j < pointCount; j++)
                            {
                                // Đọc tọa độ
                                BARPoint pointOriginal = new BARPoint(BitConverter.ToDouble(reader.ReadBytes(8), 0), BitConverter.ToDouble(reader.ReadBytes(8), 0));
                                byteIndex += 16;
                                // Kiểm tra tính khoảng cách từ điểm đó đến điểm bắt đầu
                                if (j > 0)
                                    pointOriginal.D2Start = segmentInfo.PointList[segmentInfo.PointList.Count - 1].D2Start + pointOriginal.Distance(segmentInfo.PointList[segmentInfo.PointList.Count - 1]);
                                // Thêm vào danh sách
                                segmentInfo.PointList.Add(pointOriginal);
                            }
                            segmentInfo.Length = segmentInfo.PointList[segmentInfo.PointList.Count - 1].D2Start;
                            #endregion

                            #region ==================== Thêm vào KDTree để tìm điểm ====================
                            RunningParams.RouterData.KDTree.AddPoint(segmentInfo.PointList[pointIndex].ToArray(), new BARNode(segmentInfo, 0));
                            RunningParams.RouterData.KDTree.AddPoint(segmentInfo.PointList[pointIndex].ToArray(), new BARNode(segmentInfo, segmentInfo.PointList.Count - 1));
                            #endregion

                            #region ==================== Thêm vào RTree để tìm đường ====================
                            // .1 Đường bình thường
                            RunningParams.RouterData.RTree.Add(segmentInfo.GetRectangle(), segmentInfo);
                            #endregion

                            // Lưu đối tượng segment
                            RunningParams.RouterData.Objs.Add(segmentInfo.SegmentID, segmentInfo);

                            if (i % 100000 == 0)
                                LogFile.WriteProcess(string.Format("Index: {0}", i));
                            else if(i < 100000 && i % 10000 == 0)
                                LogFile.WriteProcess(string.Format("Index: {0}", i));
                        }
                    }
                }
                LogFile.WriteProcess("-------------------- Kết thúc đọc dữ liệu tìm đường --------------------");

                // Xóa file tạm
                File.Delete(filePath);

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RouterManager.LoadData, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Backup các file logs vào thư mục riêng
        /// </summary>
        public static void BackupLogs()
        {
            try
            {
                string[] fileList = Directory.GetFiles(Constants.DEFAULT_DIRECTORY_LOGS, "*.txt");
                if (fileList == null || fileList.Length == 0)
                    return;
                string folderName = string.Format("{0:yyyyMMdd-HHmmss}", DateTime.Now);
                if (Directory.Exists(Constants.DEFAULT_DIRECTORY_LOGS + folderName) == false)
                    Directory.CreateDirectory(Constants.DEFAULT_DIRECTORY_LOGS + folderName);
                int indexID = 0;
                for (int i = 0; i < fileList.Length; i++)
                {
                    indexID = fileList[i].LastIndexOf("\\");
                    MoveFile(Constants.DEFAULT_DIRECTORY_LOGS, folderName, fileList[i].Substring(indexID + 1, fileList[i].Length - indexID - 1));
                }
            }
            catch { }
        }

        /// <summary>
        /// Move file sang thư mục khác
        /// </summary>
        private static void MoveFile(string ph, string fd, string fn)
        {
            try
            {                
                if (File.Exists(ph + fn) == true)
                {
                    if (File.Exists(ph + fd + "\\" + fn) == true)
                        File.Delete(ph + fd + "\\" + fn);
                    File.Move(ph + fn, ph + fd + "\\" + fn);
                }
            }
            catch { }
        }
    }
}