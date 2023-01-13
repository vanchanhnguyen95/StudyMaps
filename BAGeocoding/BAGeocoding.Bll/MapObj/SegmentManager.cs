using System;
using System.Collections.Generic;
using System.IO;

using BAGeocoding.Entity.DataService;
using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;

namespace BAGeocoding.Bll.MapObj
{
    public class SegmentManager
    {
        public static bool LoadData()
        {
            try
            {
                // Tạo file tạm
                string filePath = string.Empty;
                if (MainProcessing.FileCopy(Constants.DEFAULT_DIRECTORY_DATA, Constants.DEFAULT_SEGMENT_FILE_NAME, ref filePath) == false)
                    return false;

                // Tiến hành đọc dữ liệu
                using (FileStream stream = new FileStream(filePath, FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        int nameLength = 0;
                        int pointIndex = 0;
                        short nCount = (short)reader.ReadByte();
                        for (short i = 0; i < nCount; i++)
                        {
                            DTSSegment segmentData = new DTSSegment();
                            short proviceID = (short)reader.ReadByte();
                            if (RunningParams.ProvinceData.Segm.ContainsKey(proviceID))
                                segmentData = (DTSSegment)RunningParams.ProvinceData.Segm[proviceID];
                            int objLength = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                            for (int j = 0; j < objLength; j++)
                            {
                                //Read segment
                                BAGSegment segmentInfo = new BAGSegment();
                                //Read 4 bytes (ID)
                                segmentInfo.SegmentID = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                                //Read 1 byte (Length of VName)
                                nameLength = (int)reader.ReadByte();
                                //Read VName
                                segmentInfo.VName = Constants.UnicodeCodePage.GetString(reader.ReadBytes(nameLength));
                                segmentInfo.EName = LatinToAscii.Latin2Ascii(segmentInfo.VName);
                                //Read 2 bytes (ProvinceID)
                                segmentInfo.ProvinceID = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                                //Read 1 byte trạng thái đường có số nhà liên tiếp
                                segmentInfo.IsSerial = (reader.ReadByte() == 1);    // ANHPT: Tạm thời chưa có dữ liệu
                                //Read 2 bytes (StartLeft)
                                segmentInfo.StartLeft = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                                //Read 2 bytes (StartRight)
                                segmentInfo.StartRight = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                                //Read 2 bytes (EndLeft)
                                segmentInfo.EndLeft = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                                //Read 2 bytes (EndRight)
                                segmentInfo.EndRight = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                                //Read 2 bytes cho giới hạn tốc độ
                                if(RunningParams.DataSpeed == true)
                                {
                                    segmentInfo.MinSpeed = reader.ReadByte();
                                    segmentInfo.MaxSpeed = reader.ReadByte();
                                }
                                else if(RunningParams.TestSpeed == true)
                                {
                                    segmentInfo.MinSpeed = DataUlt.Random(0, 60);
                                    segmentInfo.MaxSpeed = DataUlt.Random(segmentInfo.MinSpeed, 120);
                                }
                                // Dữ liệu mở rộng
                                segmentInfo.DataExt = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                                //Read 2 bytes (Length of list Points)
                                int length = (int)BitConverter.ToInt16(reader.ReadBytes(2), 0);
                                segmentInfo.PointList = new List<BAGPoint>();
                                //Read list Points
                                for (int k = 0; k < length; k++)
                                {
                                    // Khởi tạo thông tin cặp tọa độ điểm của segment
                                    BAGPoint pointOriginal = new BAGPoint(BitConverter.ToDouble(reader.ReadBytes(8), 0), BitConverter.ToDouble(reader.ReadBytes(8), 0));
                                    // Kiểm tra tính khoảng cách từ điểm đó đến điểm bắt đầu
                                    if (k > 0)
                                        pointOriginal.D2Start = segmentInfo.PointList[segmentInfo.PointList.Count - 1].D2Start + pointOriginal.Distance(segmentInfo.PointList[segmentInfo.PointList.Count - 1]);
                                    // Thêm vào danh sách
                                    segmentInfo.PointList.Add(pointOriginal);
                                }
                                segmentInfo.SegLength = (float)segmentInfo.PointList[segmentInfo.PointList.Count - 1].D2Start;

                                if (segmentData.Objs.ContainsKey(segmentInfo.SegmentID) == true)
                                    continue;
                                // Build KDTree
                                pointIndex = 0;
                                segmentData.KDTree.AddPoint(segmentInfo.PointList[pointIndex].ToArray(), new BAGPoint(segmentInfo.PointList[pointIndex]));
                                pointIndex = segmentInfo.PointList.Count - 1;
                                segmentData.KDTree.AddPoint(segmentInfo.PointList[pointIndex].ToArray(), new BAGPoint(segmentInfo.PointList[pointIndex]));
                                // Build RTree
                                segmentData.RTree.Add(segmentInfo.GetRectangle(), segmentInfo);
                                // Save object
                                segmentData.Objs.Add(segmentInfo.SegmentID, segmentInfo);
                            }
                            //Add segment
                            if (RunningParams.ProvinceData.Segm.ContainsKey(proviceID))
                                RunningParams.ProvinceData.Segm[proviceID] = segmentData;
                            else
                                RunningParams.ProvinceData.Segm.Add(proviceID, segmentData);
                        }
                        reader.Close();
                    }
                    stream.Close();
                }

                // Xóa file tạm
                File.Delete(filePath);

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("SegmentManager.LoadData, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
