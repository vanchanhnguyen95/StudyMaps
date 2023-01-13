using System;
using System.Collections.Generic;

using BAGeocoding.Entity.Enum;

using BAGeocoding.Utility;
using RTree.Engine.Entity;
using BAGeocoding.Entity.Enum.MapObject;

namespace BAGeocoding.Entity.Router
{
    public class BARSegment
    {
        public int SegmentID { get; set; }
        public string VName { get; set; }
        public string EName { get; set; }
        public byte ClassFunc { get; set; }
        public double Coeff { get; set; }
        public bool FerryThese { get; set; }
        public byte DataExt { get; set; }
        public BARDirection AllowCar { get; set; }
        public List<BARPoint> PointList { get; set; }
        public double Length { get; set; }
        
        public BARSegment()
        {
            VName = string.Empty;
            EName = string.Empty;
            AllowCar = new BARDirection();
            PointList = new List<BARPoint>();
        }

        public BARSegment(BARSegment other)
        {
            SegmentID = other.SegmentID;
            VName = other.VName;
            EName = other.EName;
            ClassFunc = other.ClassFunc;
            Coeff = 0;
            FerryThese = other.FerryThese;
            AllowCar = new BARDirection(other.AllowCar);
            PointList = new List<BARPoint>();
            other.PointList.ForEach(item => PointList.Add(new BARPoint(item)));
            Length = other.Length;
        }

        public bool DataExtGet(EnumMOBSegmentDataExt dataExt)
        {
            return ((DataExt & (byte)Math.Pow(2, (byte)dataExt)) > 0);
        }

        public void DataExtSet(EnumMOBSegmentDataExt dataExt, bool status)
        {
            // Bít đã được bật
            if (((DataExt >> (int)dataExt) & 1) > 0)
            {
                if (status == false)
                    DataExt = (byte)(DataExt - (byte)Math.Pow(2, (byte)dataExt));
            }
            // Bít chưa bật
            else
            {
                if (status == true)
                    DataExt = (byte)(DataExt + (byte)Math.Pow(2, (byte)dataExt));
            }
        }
        
        public RTRectangle GetRectangle()
        {
            double minlng = PointList[0].Lng;
            double minlat = PointList[0].Lat;
            double maxlng = PointList[0].Lng;
            double maxlat = PointList[0].Lat;
            for (int i = 1; i < PointList.Count; i++)
            {
                minlng = Math.Min(minlng, PointList[i].Lng);
                minlat = Math.Min(minlat, PointList[i].Lat);
                maxlng = Math.Max(maxlng, PointList[i].Lng);
                maxlat = Math.Max(maxlat, PointList[i].Lat);
            }
            return new RTRectangle(minlng, minlat, maxlng, maxlat);
            //return new RTRectangle(minlng, maxlat, maxlng, minlat);
        }

        public BARDetech DistanceFrom(bool state, BARPoint point)
        {
            // 1. Xử lý kết quả với cặp điểm đầu tiên
            BARDetech result = DistancePointLine(PointList[0], PointList[1], point);
            result.PointIndex = 1;
            
            // 2. Lặp với các cặp điểm tiếp theo
            for (int i = 1; i < PointList.Count - 1; i++)
            {
                BARDetech temp = DistancePointLine(PointList[i], PointList[i + 1], point);
                if (temp.Distance < result.Distance)
                {
                    result.Update(temp);
                    result.PointIndex = i + 1;
                }
            }
            
            // 3. Bổ sung lại kết quả và trả về
            result.NodeInfo.SegmentID = SegmentID;
            result.NodeInfo.Coeff = Coeff;
            result.NodeInfo.D2End = (Length - result.NodeInfo.D2Start);
            result.D2Start = result.NodeInfo.D2Start;
            result.D2End = result.NodeInfo.D2End;
            if (AllowCar.Reverse == false)
            {
                if (state == true)
                    result.NodeInfo.D2Start = -1;
                else
                    result.NodeInfo.D2End = -1;
            }
            else if (AllowCar.Forward == false)
            {
                if (state == true)
                    result.NodeInfo.D2End = -1;
                else
                    result.NodeInfo.D2Start = -1;
            }
            return result;
        }

        private BARDetech DistancePointLine(BARPoint p1, BARPoint p2, BARPoint p3)
        {
            double lineMag = Magnitude(p2, p1);
            double coefficient = (((p3.Lng - p1.Lng) * (p2.Lng - p1.Lng)) + ((p3.Lat - p1.Lat) * (p2.Lat - p1.Lat))) / (lineMag * lineMag);

            if (coefficient < 0.0d)
                return new BARDetech { Anchor = EnumBAGAnchor.Left, Point = new BARPoint(p1), Distance = p3.Distance(p1), NodeInfo = new BARNode { D2Start = p1.D2Start } };
            else if (coefficient > 1.0d)
                return new BARDetech { Anchor = EnumBAGAnchor.Right, Point = new BARPoint(p2), Distance = p3.Distance(p2), NodeInfo = new BARNode { D2Start = p2.D2Start } };
            else
            {
                BARPoint p0 = new BARPoint(p1.Lng + coefficient * (p2.Lng - p1.Lng), p1.Lat + coefficient * (p2.Lat - p1.Lat));
                return new BARDetech { Anchor = EnumBAGAnchor.Middle, Point = p0, Distance = p3.Distance(p0), NodeInfo = new BARNode { D2Start = p1.D2Start + p1.Distance(p0) } };
            }
        }

        private double Magnitude(BARPoint p1, BARPoint p2)
        {
            double lng = p2.Lng - p1.Lng;
            double lat = p2.Lat - p1.Lat;
            return Math.Sqrt(lng * lng + lat * lat);
        }

        public byte[] ToBinary()
        {
            List<byte> resultList = new List<byte>();
            resultList.AddRange(BitConverter.GetBytes(SegmentID));

            // Tên đường
            byte[] bffVName = Constants.UnicodeCodePage.GetBytes(VName);
            resultList.Add((byte)bffVName.Length);
            resultList.AddRange(bffVName);

            // Loại đường
            resultList.Add(ClassFunc);
            // Hệ số
            resultList.AddRange(BitConverter.GetBytes(Coeff));

            // Chiều đường
            resultList.Add(AllowCar.GetByte());

            // Phà/đò
            resultList.Add((byte)(FerryThese == true ? 1 : 0));

            // Dữ liệu mở rộng
            resultList.Add(DataExt);

            // Tọa độ
            resultList.AddRange(BitConverter.GetBytes((short)PointList.Count));
            for (int i = 0; i < PointList.Count; i++)
                resultList.AddRange(PointList[i].ToBinary());

            return resultList.ToArray();
        }
    }
}