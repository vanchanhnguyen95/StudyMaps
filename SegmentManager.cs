using System.Text;
using BA.Geocoding.Shared.Extension;
using Serilog;

namespace BA.Geocoding.Shared.BinhAnh;

public class SegmentManager
{
    private const string FileName = "__sm.ba";

    public static void Init()
    {
        LoadData();
    }

    private static void LoadData()
    {
        try
        {
            var filePath = $"{RunningParams.RootPath}/{FileName}";

            // Tiến hành đọc dữ liệu
            using var stream = new FileStream(filePath, FileMode.Open);
            using (var reader = new BinaryReader(stream))
            {
                var nCount = reader.ReadByte();
                for (var i = 0; i < nCount; i++)
                {
                    var segmentData = new DTSSegment();
                    var provinceId = reader.ReadByte();
                    if (RunningParams.ProvinceData.Segm.ContainsKey(provinceId))
                        segmentData = RunningParams.ProvinceData.Segm[provinceId];
                    var objLength = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                    for (var j = 0; j < objLength; j++)
                    {
                        //Read segment
                        var segmentInfo = new BAGSegment();
                        //Read 4 bytes (ID)
                        segmentInfo.SegmentID = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        //Read 1 byte (Length of VName)
                        int nameLength = reader.ReadByte();
                        //Read VName
                        segmentInfo.VName = Encoding.Unicode.GetString(reader.ReadBytes(nameLength));
                        segmentInfo.EName = LatinToAscii.Latin2Ascii(segmentInfo.VName);
                        //Read 2 bytes (ProvinceID)
                        segmentInfo.ProvinceID = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                        //Read 1 byte trạng thái đường có số nhà liên tiếp
                        segmentInfo.IsSerial = reader.ReadByte() == 1;    // Tạm thời chưa có dữ liệu
                        //Read 2 bytes (StartLeft)
                        segmentInfo.StartLeft = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                        //Read 2 bytes (StartRight)
                        segmentInfo.StartRight = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                        //Read 2 bytes (EndLeft)
                        segmentInfo.EndLeft = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                        //Read 2 bytes (EndRight)
                        segmentInfo.EndRight = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                        //Read 2 bytes cho giới hạn tốc độ
                        //TODO
                        segmentInfo.MinSpeed = reader.ReadByte();
                        segmentInfo.MaxSpeed = reader.ReadByte();

                        //if (segmentInfo.MaxSpeed > 0)
                        //{
                        //    Console.WriteLine($"{segmentInfo.VName} {segmentInfo.MaxSpeed}");
                        //}

                        // Dữ liệu mở rộng
                        segmentInfo.DataExt = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        //Read 2 bytes (Length of list Points)
                        var length = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                        segmentInfo.PointList = new List<BAGPoint>();
                        //Read list Points
                        for (var k = 0; k < length; k++)
                        {
                            // Khởi tạo thông tin cặp tọa độ điểm của segment
                            var pointOriginal = new BAGPoint(BitConverter.ToDouble(reader.ReadBytes(8), 0), BitConverter.ToDouble(reader.ReadBytes(8), 0));
                            // Kiểm tra tính khoảng cách từ điểm đó đến điểm bắt đầu
                            if (k > 0)
                                pointOriginal.D2Start = segmentInfo.PointList[segmentInfo.PointList.Count - 1].D2Start + pointOriginal.Distance(segmentInfo.PointList[segmentInfo.PointList.Count - 1]);
                            // Thêm vào danh sách
                            segmentInfo.PointList.Add(pointOriginal);
                        }
                        segmentInfo.SegLength = (float)segmentInfo.PointList[segmentInfo.PointList.Count - 1].D2Start;

                        if (segmentData.Objs.ContainsKey(segmentInfo.SegmentID))
                            continue;
                        // Build KDTree
                        var pointIndex = 0;
                        segmentData.KDTree.AddPoint(segmentInfo.PointList[pointIndex].ToArray(), new BAGPoint(segmentInfo.PointList[pointIndex]));
                        pointIndex = segmentInfo.PointList.Count - 1;
                        segmentData.KDTree.AddPoint(segmentInfo.PointList[pointIndex].ToArray(), new BAGPoint(segmentInfo.PointList[pointIndex]));
                        // Build RTree
                        segmentData.RTree.Add(segmentInfo.GetRectangle(), segmentInfo);
                        // Save object
                        segmentData.Objs.TryAdd(segmentInfo.SegmentID, segmentInfo);
                    }
                    //Add segment
                    if (RunningParams.ProvinceData.Segm.ContainsKey(provinceId))
                        RunningParams.ProvinceData.Segm[provinceId] = segmentData;
                    else
                        RunningParams.ProvinceData.Segm.TryAdd(provinceId, segmentData);
                }
                reader.Close();
            }
            stream.Close();
        }
        catch (Exception ex)
        {
            Log.Error("Load segment data failed {@Exception}", ex.ExtractInfo());
        }
    }
}