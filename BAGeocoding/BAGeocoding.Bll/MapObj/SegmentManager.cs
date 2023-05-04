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
                            DTSSegmentV2 segmentDataV2 = new DTSSegmentV2();

                            DTSSegment segmentData = new DTSSegment();
                            short proviceID = (short)reader.ReadByte();

                            //if (RunningParams.ProvinceDataV2.Segm.ContainsKey(proviceID))
                            //    segmentDataV2 = (DTSSegmentV2)RunningParams.ProvinceDataV2.Segm[proviceID];

                            //if (RunningParams.ProvinceData.Segm.ContainsKey(proviceID))
                            //    segmentData = (DTSSegment)RunningParams.ProvinceData.Segm[proviceID];
                            if (RunningParams.ProvinceData.Segm.ContainsKey(proviceID))
                            {
                                segmentData = (DTSSegment)RunningParams.ProvinceData.Segm[proviceID];
                                segmentDataV2 = (DTSSegmentV2)RunningParams.ProvinceDataV2.Segm[proviceID];
                            }    
                                

                            int objLength = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                            for (int j = 0; j < objLength; j++)
                            {
                                BAGSegmentV2 segmentInfoV2 = new BAGSegmentV2();

                                //Read segment
                                BAGSegment segmentInfo = new BAGSegment();
                                //Read 4 bytes (ID)
                                segmentInfo.SegmentID = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                                segmentInfoV2.SegmentID = segmentInfo.SegmentID;

                                //Read 1 byte (Length of VName)
                                nameLength = (int)reader.ReadByte();
                                //Read VName
                                segmentInfo.VName = Constants.UnicodeCodePage.GetString(reader.ReadBytes(nameLength));
                                segmentInfo.EName = LatinToAscii.Latin2Ascii(segmentInfo.VName);

                                segmentInfoV2.VName = segmentInfo.VName;
                                segmentInfoV2.EName = segmentInfo.EName;

                                //Read 2 bytes (ProvinceID)
                                segmentInfo.ProvinceID = BitConverter.ToInt16(reader.ReadBytes(2), 0);

                                segmentInfoV2.ProvinceID = segmentInfo.ProvinceID;

                                //Read 1 byte trạng thái đường có số nhà liên tiếp
                                segmentInfo.IsSerial = (reader.ReadByte() == 1);    // ANHPT: Tạm thời chưa có dữ liệu
                                segmentInfoV2.IsSerial = segmentInfo.IsSerial;

                                //Read 2 bytes (StartLeft)
                                segmentInfo.StartLeft = BitConverter.ToInt16(reader.ReadBytes(2), 0);

                                segmentInfoV2.StartLeft = segmentInfo.StartLeft;

                                //Read 2 bytes (StartRight)
                                segmentInfo.StartRight = BitConverter.ToInt16(reader.ReadBytes(2), 0);

                                segmentInfoV2.StartRight = segmentInfo.StartRight;

                                //Read 2 bytes (EndLeft)
                                segmentInfo.EndLeft = BitConverter.ToInt16(reader.ReadBytes(2), 0);

                                segmentInfoV2.EndLeft = segmentInfo.EndLeft;

                                //Read 2 bytes (EndRight)
                                segmentInfo.EndRight = BitConverter.ToInt16(reader.ReadBytes(2), 0);


                                segmentInfoV2.EndRight = segmentInfo.EndRight;

                                //Read 2 bytes cho giới hạn tốc độ
                                //if(RunningParams.DataSpeed == true)
                                //{
                                //    segmentInfo.MinSpeed = reader.ReadByte();
                                //    segmentInfo.MaxSpeed = reader.ReadByte();
                                //}
                                //else if(RunningParams.TestSpeed == true)
                                //{
                                //    segmentInfo.MinSpeed = DataUlt.Random(0, 60);
                                //    segmentInfo.MaxSpeed = DataUlt.Random(segmentInfo.MinSpeed, 120);
                                //}


                                //TODO
                                segmentInfo.MinSpeed = reader.ReadByte();
                                segmentInfo.MaxSpeed = reader.ReadByte();

                                segmentInfoV2.MinSpeed = segmentInfo.MinSpeed;
                                segmentInfoV2.MaxSpeed = segmentInfo.MinSpeed;


                                // Dữ liệu mở rộng
                                segmentInfo.DataExt = BitConverter.ToInt32(reader.ReadBytes(4), 0);

                                segmentInfoV2.DataExt = segmentInfo.DataExt;

                                //Read 2 bytes (Length of list Points)
                                int length = (int)BitConverter.ToInt16(reader.ReadBytes(2), 0);
                                segmentInfo.PointList = new List<BAGPoint>();
                                //Read list Points
                                for (int k = 0; k < length; k++)
                                {
                                    // Khởi tạo thông tin cặp tọa độ điểm của segment
                                    BAGPoint pointOriginal = new BAGPoint(BitConverter.ToDouble(reader.ReadBytes(8), 0), BitConverter.ToDouble(reader.ReadBytes(8), 0));

                                    BAGPointV2 pointOriginalV2 = new BAGPointV2(pointOriginal.Lng, pointOriginal.Lat);



                                    // Kiểm tra tính khoảng cách từ điểm đó đến điểm bắt đầu
                                    //if (k > 0)
                                    //    pointOriginal.D2Start = segmentInfo.PointList[segmentInfo.PointList.Count - 1].D2Start + pointOriginal.Distance(segmentInfo.PointList[segmentInfo.PointList.Count - 1]);
                                    if (k > 0)
                                    { 
                                        pointOriginal.D2Start = segmentInfo.PointList[segmentInfo.PointList.Count - 1].D2Start + pointOriginal.Distance(segmentInfo.PointList[segmentInfo.PointList.Count - 1]);
                                        pointOriginalV2.D2Start = pointOriginal.D2Start;
                                    }
                                    // Thêm vào danh sách
                                    segmentInfo.PointList.Add(pointOriginal);
                                    segmentInfoV2.PointList.Add(pointOriginalV2);
                                }
                                segmentInfo.SegLength = (float)segmentInfo.PointList[segmentInfo.PointList.Count - 1].D2Start;
                                segmentInfoV2.SegLength = segmentInfo.SegLength;


                                if (segmentData.Objs.ContainsKey(segmentInfo.SegmentID) == true)
                                    continue;
                                // Build KDTree
                                pointIndex = 0;
                                segmentData.KDTree.AddPoint(segmentInfo.PointList[pointIndex].ToArray(), new BAGPoint(segmentInfo.PointList[pointIndex]));

                                segmentDataV2.KDTree.AddPoint(segmentInfoV2.PointList[pointIndex].ToArray(), new BAGPointV2(segmentInfoV2.PointList[pointIndex]));

                                pointIndex = segmentInfo.PointList.Count - 1;
                                segmentData.KDTree.AddPoint(segmentInfo.PointList[pointIndex].ToArray(), new BAGPoint(segmentInfo.PointList[pointIndex]));

                                segmentDataV2.KDTree.AddPoint(segmentInfoV2.PointList[pointIndex].ToArray(), new BAGPointV2(segmentInfoV2.PointList[pointIndex]));

                                // Build RTree
                                segmentData.RTree.Add(segmentInfo.GetRectangle(), segmentInfo);

                                segmentDataV2.RTree.Add(segmentInfoV2.GetRectangle(), segmentInfoV2);

                                // Save object
                                segmentData.Objs.Add(segmentInfo.SegmentID, segmentInfo);

                                segmentDataV2.Objs.Add(segmentInfoV2.SegmentID, segmentInfoV2);
                            }
                            //Add segment
                            //if (RunningParams.ProvinceData.Segm.ContainsKey(proviceID))
                            //    RunningParams.ProvinceData.Segm[proviceID] = segmentData;
                            //else
                            //    RunningParams.ProvinceData.Segm.Add(proviceID, segmentData);

                            if (RunningParams.ProvinceData.Segm.ContainsKey(proviceID))
                            {
                                RunningParams.ProvinceData.Segm[proviceID] = segmentData;
                                RunningParams.ProvinceDataV2.Segm[proviceID] = segmentDataV2;
                            }
                            else
                            {
                                RunningParams.ProvinceData.Segm.Add(proviceID, segmentData);
                                RunningParams.ProvinceDataV2.Segm.Add(proviceID, segmentDataV2);
                            }

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
