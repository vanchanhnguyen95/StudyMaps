using System;
using System.Collections.Generic;
using System.Data;

using RTree.Engine.Entity;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.Enum.MapObject;

using BAGeocoding.Utility;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BAGeocoding.Entity.MapObj
{
    public class BAGSegment : SQLDataUlt
    {
        public int TemplateID { get; set; }
        public int SegmentID { get; set; }
        public string VName { get; set; }
        public string EName { get; set; }
        public short ProvinceID { get; set; }
        public short DistrictID { get; set; }
        public byte Direction { get; set; }
        public byte ClassFunc { get; set; }
        public int DataExt { get; set; }
        public List<BAGPoint> PointList { get; set; }
        
        public short StartLeft { get; set; }
        public short StartRight { get; set; }
        public short EndLeft { get; set; }
        public short EndRight { get; set; }

        public byte LevelID { get; set; }
        public byte KindID { get; set; }
        public byte RegionLev { get; set; }
        public byte Wide { get; set; }
        public byte MinSpeed { get; set; }
        public byte MaxSpeed { get; set; }
        public float SegLength { get; set; }
        public int Fee { get; set; }
        public bool IsNumber { get; set; }
        public bool IsBridge { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsPed { get; set; }
        public bool IsSerial { get { return DataExtGet(EnumMOBSegmentDataExt.BuildingSerial); } set { DataExtSet(EnumMOBSegmentDataExt.BuildingSerial, value); } }
        public bool AllowPed { get; set; }
        public bool AllowWalk { get; set; }
        public bool AllowBicycle { get; set; }
        public bool AllowMoto { get; set; }
        public bool AllowCar { get; set; }
        public byte DirCar { get; set; }
        public bool AllowBus { get; set; }
        public byte DirBus { get; set; }
        public bool AllowTruck { get; set; }
        public byte DirTruck { get; set; }
        public bool AllowTaxi { get; set; }
        public byte DirTaxi { get; set; }
        
        public byte MinSpeed1 { get; set; }
        public byte MaxSpeed1 { get; set; }
        public byte MinSpeed2 { get; set; }
        public byte MaxSpeed2 { get; set; }
        public byte MinSpeed3 { get; set; }
        public byte MaxSpeed3 { get; set; }
        public byte MinSpeed4 { get; set; }
        public byte MaxSpeed4 { get; set; }
        public short LyTrinh { get; set; }

        public float Length { get; set; }
        public string GridStr { get; set; }
        public string LngStr { get; set; }
        public string LatStr { get; set; }

        public bool IsDone { get; set; }

        public BAGSegment()
        {
            PointList = new List<BAGPoint>();
            Length = 0;
            LngStr = string.Empty;
            LatStr = string.Empty;
        }

        #region ==================== Thao tác CSDL ===============================
        public bool FromDataSimple(DataRow dr)
        {
            try
            {
                SegmentID = base.GetDataValue<int>(dr, "SegmentID");
                VName = base.GetDataValue<string>(dr, "VName", string.Empty);

                DataExt = base.GetDataValue<int>(dr, "DataExt");
                StartLeft = base.GetDataValue<short>(dr, "StartLeft");
                EndLeft = base.GetDataValue<short>(dr, "EndLeft");
                StartRight = base.GetDataValue<short>(dr, "StartRight");
                EndRight = base.GetDataValue<short>(dr, "EndRight");


                string[] lngStr = base.GetDataValue<string>(dr, "LngStr", string.Empty).Split('@');
                string[] latStr = base.GetDataValue<string>(dr, "LatStr", string.Empty).Split('@');

                PointList = new List<BAGPoint>();
                for (int i = 0; i < lngStr.Length; i++)
                    PointList.Add(new BAGPoint(lngStr[i], latStr[i]));

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BAGSegment.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataRow(DataRow dr, DataTable dt)
        {
            try
            {
                SegmentID = base.GetDataValue<int>(dr, "SegmentID");
                ProvinceID = base.GetDataValue<short>(dr, "ProvinceID");
                DistrictID = base.GetDataValue<short>(dr, "DistrictID");
                VName = base.GetDataValue<string>(dr, "VName", string.Empty);
                EName = base.GetDataValue<string>(dr, "EName", string.Empty);
                DataExt = base.GetDataValue<int>(dr, "DataExt");

                StartLeft = base.GetDataValue<short>(dr, "StartLeft");
                EndLeft = base.GetDataValue<short>(dr, "EndLeft");
                StartRight = base.GetDataValue<short>(dr, "StartRight");
                EndRight = base.GetDataValue<short>(dr, "EndRight");

                MinSpeed = base.GetDataValue<byte>(dr, "MinSpeed");
                MaxSpeed = base.GetDataValue<byte>(dr, "MaxSpeed");

                PointList = new List<BAGPoint>();
                DataRow[] rowList = dt.Select(string.Format("SegmentID = {0}", SegmentID));
                for (int i = 0; i < rowList.Length; i++)
                {
                    BAGPoint point = new BAGPoint();
                    if (point.FromDataRow(rowList[i]) == false)
                        return false;
                    PointList.Add(point);
                }

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BAGSegment.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }
        #endregion
        
        #region ==================== Dữ liệu mở rộng ===============================
        public bool DataExtGet(EnumMOBSegmentDataExt dataExt)
        {
            return ((DataExt & (int)Math.Pow(2, (int)dataExt)) > 0);
        }

        public void DataExtSet(EnumMOBSegmentDataExt dataExt, bool status)
        {
            // Bít đã được bật
            if (((DataExt >> (int)dataExt) & 1) > 0)
            {
                if (status == false)
                    DataExt = DataExt - (int)Math.Pow(2, (int)dataExt);
            }
            // Bít chưa bật
            else
            {
                if (status == true)
                    DataExt = DataExt + (int)Math.Pow(2, (int)dataExt);
            }
        }
        #endregion

        #region ==================== Xác định đường bao ===============================
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
        }

        public BAGDistance DistanceFrom(BAGPoint point)
        {
            BAGDistance distance = new BAGDistance();
            for (int i = 0; i < PointList.Count - 1; i++)
            {
                if (i == 0)
                {
                    distance = DistancePointLine(PointList[0], PointList[1], point);
                    distance.PointIndex = i + 1;
                }
                else
                {
                    BAGDistance tmp = DistancePointLine(PointList[i], PointList[i + 1], point);
                    if (distance.Distance > tmp.Distance)
                    {
                        distance = new BAGDistance(tmp.Anchor, tmp.Point, tmp.Distance);
                        distance.PointIndex = i + 1;
                    }
                }
            }
            return distance;
        }
        #endregion

        private BAGDistance DistancePointLine(BAGPoint p1, BAGPoint p2, BAGPoint p3)
        {
            double LineMag = Magnitude(p2, p1);
            double U = (((p3.Lng - p1.Lng) * (p2.Lng - p1.Lng)) + ((p3.Lat - p1.Lat) * (p2.Lat - p1.Lat))) / (LineMag * LineMag);

            if (U < 0.0d)
                return new BAGDistance(EnumBAGAnchor.Left, p1, Magnitude(p3, p1));
            else if (U > 1.0d)
                return new BAGDistance(EnumBAGAnchor.Right, p2, Magnitude(p3, p2));
            else
            {
                BAGPoint p0 = new BAGPoint(p1.Lng + U * (p2.Lng - p1.Lng), p1.Lat + U * (p2.Lat - p1.Lat));
                return new BAGDistance(EnumBAGAnchor.Middle, p0, Magnitude(p3, p0));
            }
        }

        private double Magnitude(BAGPoint p1, BAGPoint p2)
        {
            return Math.Sqrt(Math.Pow(p2.Lng - p1.Lng, 2) + Math.Pow(p2.Lat - p1.Lat, 2));
        }

        public bool IsMissing(ref string msg)
        {
            msg = string.Empty;
            if (StartLeft == 0 && EndLeft > 0)
                msg += string.Format(", Thiếu bắt đầu bên trái (ID: {0}, StartLeft: {1}, EndLeft: {2})", TemplateID, StartLeft, EndLeft);
            if (StartLeft > 0 && EndLeft == 0)
                msg += string.Format(", Thiếu kết thúc bên trái (ID: {0}, StartLeft: {1}, EndLeft: {2})", TemplateID, StartLeft, EndLeft);
            if (StartRight == 0 && EndRight > 0)
                msg += string.Format(", Thiếu bắt đầu bên phải (ID: {0}, StartRight: {1}, EndRight: {2})", TemplateID, StartRight, EndRight);
            if (StartRight > 0 && EndRight == 0)
                msg += string.Format(", Thiếu kết thúc bên phải (ID: {0}, StartRight: {1}, EndRight: {2})", TemplateID, StartRight, EndRight);
            if (msg.Length > 0)
                msg = msg.Substring(2);
            return msg.Length > 0;
        }

        public bool IsDuplicate(BAGSegment other, ref string msgStr, ref string geoStr)
        {
            bool errorState = false;
            // Kiểm tra bên phải
            if (StartRight > 0)
            {
                List<short> n1 = HouseNumRight();
                // Trùng bên phải
                if (StartRight % 2 == other.StartRight % 2)
                {
                    List<short> n2 = other.HouseNumRight();
                    if (ListDuplicate(n1, n2) == true)
                    {
                        errorState = true;
                        //msgStr += string.Format(", (ID: {0}, SR: {1}, ER: {2}) - (ID: {3}, SR: {4}, ER: {5})", TemplateID, StartRight, EndRight, other.TemplateID, other.StartRight, other.EndRight);
                        msgStr += string.Format(", (ID: {0}, SR: {1}, ER: {2})", other.TemplateID, other.StartRight, other.EndRight);
                    }
                }
                // Trùng bên trái
                else if (StartRight % 2 == other.StartLeft % 2)
                {
                    List<short> n2 = other.HouseNumLeft();
                    if (ListDuplicate(n1, n2) == true)
                    {
                        errorState = true;
                        //msgStr += string.Format(", (ID: {0}, SR: {1}, ER: {2}) - (ID: {3}, SL: {4}, EL: {5})", TemplateID, StartRight, EndRight, other.TemplateID, other.StartLeft, other.EndLeft);
                        msgStr += string.Format(", (ID: {0}, SL: {1}, EL: {2})", other.TemplateID, other.StartLeft, other.EndLeft);
                    }
                }
            }
            // Kiểm tra bên trái
            if (StartLeft > 0)
            {
                List<short> n1 = HouseNumLeft();
                // Trùng bên phải
                if (StartLeft % 2 == other.StartLeft % 2)
                {
                    List<short> n2 = other.HouseNumLeft();
                    if (ListDuplicate(n1, n2) == true)
                    {
                        errorState = true;
                        //msgStr += string.Format(", (ID: {0}, SL: {1}, EL: {2}) - (ID: {3}, SL: {4}, EL: {5})", TemplateID, StartLeft, EndLeft, other.TemplateID, other.StartLeft, other.EndLeft);
                        msgStr += string.Format(", (ID: {0}, SL: {1}, EL: {2})", other.TemplateID, other.StartLeft, other.EndLeft);
                    }
                }
                // Trùng bên trái
                else if (StartLeft % 2 == other.StartRight % 2)
                {
                    List<short> n2 = other.HouseNumRight();
                    if (ListDuplicate(n1, n2) == true)
                    {
                        errorState = true;
                        //msgStr += string.Format(", (ID: {0}, SL: {1}, EL: {2}) - (ID: {3}, SR: {4}, ER: {5})", TemplateID, StartLeft, EndLeft, other.TemplateID, other.StartRight, other.EndRight);
                        msgStr += string.Format(", (ID: {0}, SR: {1}, ER: {2})", other.TemplateID, other.StartRight, other.EndRight);
                    }
                }
            }
            if (errorState == true)
                geoStr += string.Format(" ({0:N8}, {1:N8})", other.PointList[0].Lat, other.PointList[0].Lng);
            return msgStr.Length > 0;
        }

        private List<short> HouseNumRight()
        {
            if (StartRight > EndRight)
                return new List<short> { StartRight, EndRight };
            else
                return new List<short> { EndRight, StartRight };
        }

        private List<short> HouseNumLeft()
        {
            if (StartLeft > EndLeft)
                return new List<short> { StartLeft, EndLeft };
            else
                return new List<short> { EndLeft, StartLeft };
        }

        /// <summary>
        /// Kiểm tra giao nhau
        /// Dữ liệu so sánh: =====
        /// Dữ liệu gốc:     -----
        /// Phần giao nhau:  +++++
        /// </summary>
        private bool ListDuplicate(List<short> a, List<short> b)
        {
            // Chứa cận dưới [=====[+++++]-----]
            if (b[0] < a[0] && b[1] > a[0])
                return true;
            // Chưa cận trên [-----[+++++]=====]
            else if (b[0] < a[1] && b[1] > a[1])
                return true;
            // Chứa toàn bộ [=====[+++++]=====]
            else if (b[0] < a[0] && b[1] > a[1])
                return true;
            // Là tập con   [-----[+++++]-----]
            else if (b[0] > a[0] && b[1] < a[1])
                return true;
            else
                return false;
        }

        public byte[] ToBinary(bool speed = false)
        {
            List<byte> resultList = new List<byte>();
            resultList.AddRange(BitConverter.GetBytes(SegmentID));

            // Tên đường
            byte[] bffVName = Constants.UnicodeCodePage.GetBytes(VName);
            resultList.Add((byte)bffVName.Length);
            resultList.AddRange(bffVName);

            // Tỉnh
            resultList.AddRange(BitConverter.GetBytes(ProvinceID));

            // Số nhà liên tiếp
            resultList.Add((byte)(IsSerial == true ? 1 : 0));     // ANHPT: Tạm thời chưa có dữ liệu

            // Số nhà đầu đường
            resultList.AddRange(BitConverter.GetBytes(StartLeft));
            resultList.AddRange(BitConverter.GetBytes(StartRight));

            // Số nhà cuối đường
            resultList.AddRange(BitConverter.GetBytes(EndLeft));
            resultList.AddRange(BitConverter.GetBytes(EndRight));

            // Giới hạn tốc độ
            if (speed == true)
            {
                resultList.Add(MinSpeed);
                resultList.Add(MaxSpeed);
            }
            // Dữ liệu mở rộng
            resultList.AddRange(BitConverter.GetBytes(DataExt));

            // Tọa độ
            resultList.AddRange(BitConverter.GetBytes((short)PointList.Count));
            for (int i = 0; i < PointList.Count; i++)
            {
                resultList.AddRange(BitConverter.GetBytes(PointList[i].Lng));
                resultList.AddRange(BitConverter.GetBytes(PointList[i].Lat));
            }

            return resultList.ToArray();
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", SegmentID, VName.Replace(",", "-"), StartLeft, EndLeft, StartRight, EndRight, 1, 1, "");
        }
    }

    public class BAGSegmentV2 : SQLDataUlt
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int TemplateID { get; set; }
        public int SegmentID { get; set; }
        public string VName { get; set; }
        public string EName { get; set; }
        public short ProvinceID { get; set; }
        public short DistrictID { get; set; }
        public byte Direction { get; set; }
        public byte ClassFunc { get; set; }
        public int DataExt { get; set; }
        public List<BAGPointV2> PointList { get; set; }

        public short StartLeft { get; set; }
        public short StartRight { get; set; }
        public short EndLeft { get; set; }
        public short EndRight { get; set; }

        public byte LevelID { get; set; }
        public byte KindID { get; set; }
        public byte RegionLev { get; set; }
        public byte Wide { get; set; }
        public byte MinSpeed { get; set; }
        public byte MaxSpeed { get; set; }
        public float SegLength { get; set; }
        public int Fee { get; set; }
        public bool IsNumber { get; set; }
        public bool IsBridge { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsPed { get; set; }
        public bool IsSerial { get { return DataExtGet(EnumMOBSegmentDataExt.BuildingSerial); } set { DataExtSet(EnumMOBSegmentDataExt.BuildingSerial, value); } }
        public bool AllowPed { get; set; }
        public bool AllowWalk { get; set; }
        public bool AllowBicycle { get; set; }
        public bool AllowMoto { get; set; }
        public bool AllowCar { get; set; }
        public byte DirCar { get; set; }
        public bool AllowBus { get; set; }
        public byte DirBus { get; set; }
        public bool AllowTruck { get; set; }
        public byte DirTruck { get; set; }
        public bool AllowTaxi { get; set; }
        public byte DirTaxi { get; set; }

        public byte MinSpeed1 { get; set; }
        public byte MaxSpeed1 { get; set; }
        public byte MinSpeed2 { get; set; }
        public byte MaxSpeed2 { get; set; }
        public byte MinSpeed3 { get; set; }
        public byte MaxSpeed3 { get; set; }
        public byte MinSpeed4 { get; set; }
        public byte MaxSpeed4 { get; set; }
        public short LyTrinh { get; set; }

        public float Length { get; set; }
        public string GridStr { get; set; }
        public string LngStr { get; set; }
        public string LatStr { get; set; }

        public bool IsDone { get; set; }

        public BAGSegmentV2()
        {
            PointList = new List<BAGPointV2>();
            Length = 0;
            LngStr = string.Empty;
            LatStr = string.Empty;
        }

        #region ==================== Thao tác CSDL ===============================
        public bool FromDataSimple(DataRow dr)
        {
            try
            {
                SegmentID = base.GetDataValue<int>(dr, "SegmentID");
                VName = base.GetDataValue<string>(dr, "VName", string.Empty);

                DataExt = base.GetDataValue<int>(dr, "DataExt");
                StartLeft = base.GetDataValue<short>(dr, "StartLeft");
                EndLeft = base.GetDataValue<short>(dr, "EndLeft");
                StartRight = base.GetDataValue<short>(dr, "StartRight");
                EndRight = base.GetDataValue<short>(dr, "EndRight");


                string[] lngStr = base.GetDataValue<string>(dr, "LngStr", string.Empty).Split('@');
                string[] latStr = base.GetDataValue<string>(dr, "LatStr", string.Empty).Split('@');

                PointList = new List<BAGPointV2>();
                for (int i = 0; i < lngStr.Length; i++)
                    PointList.Add(new BAGPointV2(lngStr[i], latStr[i]));

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BAGSegment.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataRow(DataRow dr, DataTable dt)
        {
            try
            {
                SegmentID = base.GetDataValue<int>(dr, "SegmentID");
                ProvinceID = base.GetDataValue<short>(dr, "ProvinceID");
                DistrictID = base.GetDataValue<short>(dr, "DistrictID");
                VName = base.GetDataValue<string>(dr, "VName", string.Empty);
                EName = base.GetDataValue<string>(dr, "EName", string.Empty);
                DataExt = base.GetDataValue<int>(dr, "DataExt");

                StartLeft = base.GetDataValue<short>(dr, "StartLeft");
                EndLeft = base.GetDataValue<short>(dr, "EndLeft");
                StartRight = base.GetDataValue<short>(dr, "StartRight");
                EndRight = base.GetDataValue<short>(dr, "EndRight");

                MinSpeed = base.GetDataValue<byte>(dr, "MinSpeed");
                MaxSpeed = base.GetDataValue<byte>(dr, "MaxSpeed");

                PointList = new List<BAGPointV2>();
                DataRow[] rowList = dt.Select(string.Format("SegmentID = {0}", SegmentID));
                for (int i = 0; i < rowList.Length; i++)
                {
                    BAGPointV2 point = new BAGPointV2();
                    if (point.FromDataRow(rowList[i]) == false)
                        return false;
                    PointList.Add(point);
                }

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BAGSegment.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }
        #endregion

        #region ==================== Dữ liệu mở rộng ===============================
        public bool DataExtGet(EnumMOBSegmentDataExt dataExt)
        {
            return ((DataExt & (int)Math.Pow(2, (int)dataExt)) > 0);
        }

        public void DataExtSet(EnumMOBSegmentDataExt dataExt, bool status)
        {
            // Bít đã được bật
            if (((DataExt >> (int)dataExt) & 1) > 0)
            {
                if (status == false)
                    DataExt = DataExt - (int)Math.Pow(2, (int)dataExt);
            }
            // Bít chưa bật
            else
            {
                if (status == true)
                    DataExt = DataExt + (int)Math.Pow(2, (int)dataExt);
            }
        }
        #endregion

        #region ==================== Xác định đường bao ===============================
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
        }

        public BAGDistanceV2 DistanceFrom(BAGPointV2 point)
        {
            BAGDistanceV2 distance = new BAGDistanceV2();
            for (int i = 0; i < PointList.Count - 1; i++)
            {
                if (i == 0)
                {
                    distance = DistancePointLine(PointList[0], PointList[1], point);
                    distance.PointIndex = i + 1;
                }
                else
                {
                    BAGDistanceV2 tmp = DistancePointLine(PointList[i], PointList[i + 1], point);
                    if (distance.Distance > tmp.Distance)
                    {
                        distance = new BAGDistanceV2(tmp.Anchor, tmp.Point, tmp.Distance);
                        distance.PointIndex = i + 1;
                    }
                }
            }
            return distance;
        }
        #endregion

        private BAGDistanceV2 DistancePointLine(BAGPointV2 p1, BAGPointV2 p2, BAGPointV2 p3)
        {
            double LineMag = Magnitude(p2, p1);
            double U = (((p3.Lng - p1.Lng) * (p2.Lng - p1.Lng)) + ((p3.Lat - p1.Lat) * (p2.Lat - p1.Lat))) / (LineMag * LineMag);

            if (U < 0.0d)
                return new BAGDistanceV2(EnumBAGAnchor.Left, p1, Magnitude(p3, p1));
            else if (U > 1.0d)
                return new BAGDistanceV2(EnumBAGAnchor.Right, p2, Magnitude(p3, p2));
            else
            {
                BAGPointV2 p0 = new BAGPointV2(p1.Lng + U * (p2.Lng - p1.Lng), p1.Lat + U * (p2.Lat - p1.Lat));
                return new BAGDistanceV2(EnumBAGAnchor.Middle, p0, Magnitude(p3, p0));
            }
        }

        private double Magnitude(BAGPointV2 p1, BAGPointV2 p2)
        {
            return Math.Sqrt(Math.Pow(p2.Lng - p1.Lng, 2) + Math.Pow(p2.Lat - p1.Lat, 2));
        }

        public bool IsMissing(ref string msg)
        {
            msg = string.Empty;
            if (StartLeft == 0 && EndLeft > 0)
                msg += string.Format(", Thiếu bắt đầu bên trái (ID: {0}, StartLeft: {1}, EndLeft: {2})", TemplateID, StartLeft, EndLeft);
            if (StartLeft > 0 && EndLeft == 0)
                msg += string.Format(", Thiếu kết thúc bên trái (ID: {0}, StartLeft: {1}, EndLeft: {2})", TemplateID, StartLeft, EndLeft);
            if (StartRight == 0 && EndRight > 0)
                msg += string.Format(", Thiếu bắt đầu bên phải (ID: {0}, StartRight: {1}, EndRight: {2})", TemplateID, StartRight, EndRight);
            if (StartRight > 0 && EndRight == 0)
                msg += string.Format(", Thiếu kết thúc bên phải (ID: {0}, StartRight: {1}, EndRight: {2})", TemplateID, StartRight, EndRight);
            if (msg.Length > 0)
                msg = msg.Substring(2);
            return msg.Length > 0;
        }

        public bool IsDuplicate(BAGSegmentV2 other, ref string msgStr, ref string geoStr)
        {
            bool errorState = false;
            // Kiểm tra bên phải
            if (StartRight > 0)
            {
                List<short> n1 = HouseNumRight();
                // Trùng bên phải
                if (StartRight % 2 == other.StartRight % 2)
                {
                    List<short> n2 = other.HouseNumRight();
                    if (ListDuplicate(n1, n2) == true)
                    {
                        errorState = true;
                        //msgStr += string.Format(", (ID: {0}, SR: {1}, ER: {2}) - (ID: {3}, SR: {4}, ER: {5})", TemplateID, StartRight, EndRight, other.TemplateID, other.StartRight, other.EndRight);
                        msgStr += string.Format(", (ID: {0}, SR: {1}, ER: {2})", other.TemplateID, other.StartRight, other.EndRight);
                    }
                }
                // Trùng bên trái
                else if (StartRight % 2 == other.StartLeft % 2)
                {
                    List<short> n2 = other.HouseNumLeft();
                    if (ListDuplicate(n1, n2) == true)
                    {
                        errorState = true;
                        //msgStr += string.Format(", (ID: {0}, SR: {1}, ER: {2}) - (ID: {3}, SL: {4}, EL: {5})", TemplateID, StartRight, EndRight, other.TemplateID, other.StartLeft, other.EndLeft);
                        msgStr += string.Format(", (ID: {0}, SL: {1}, EL: {2})", other.TemplateID, other.StartLeft, other.EndLeft);
                    }
                }
            }
            // Kiểm tra bên trái
            if (StartLeft > 0)
            {
                List<short> n1 = HouseNumLeft();
                // Trùng bên phải
                if (StartLeft % 2 == other.StartLeft % 2)
                {
                    List<short> n2 = other.HouseNumLeft();
                    if (ListDuplicate(n1, n2) == true)
                    {
                        errorState = true;
                        //msgStr += string.Format(", (ID: {0}, SL: {1}, EL: {2}) - (ID: {3}, SL: {4}, EL: {5})", TemplateID, StartLeft, EndLeft, other.TemplateID, other.StartLeft, other.EndLeft);
                        msgStr += string.Format(", (ID: {0}, SL: {1}, EL: {2})", other.TemplateID, other.StartLeft, other.EndLeft);
                    }
                }
                // Trùng bên trái
                else if (StartLeft % 2 == other.StartRight % 2)
                {
                    List<short> n2 = other.HouseNumRight();
                    if (ListDuplicate(n1, n2) == true)
                    {
                        errorState = true;
                        //msgStr += string.Format(", (ID: {0}, SL: {1}, EL: {2}) - (ID: {3}, SR: {4}, ER: {5})", TemplateID, StartLeft, EndLeft, other.TemplateID, other.StartRight, other.EndRight);
                        msgStr += string.Format(", (ID: {0}, SR: {1}, ER: {2})", other.TemplateID, other.StartRight, other.EndRight);
                    }
                }
            }
            if (errorState == true)
                geoStr += string.Format(" ({0:N8}, {1:N8})", other.PointList[0].Lat, other.PointList[0].Lng);
            return msgStr.Length > 0;
        }

        private List<short> HouseNumRight()
        {
            if (StartRight > EndRight)
                return new List<short> { StartRight, EndRight };
            else
                return new List<short> { EndRight, StartRight };
        }

        private List<short> HouseNumLeft()
        {
            if (StartLeft > EndLeft)
                return new List<short> { StartLeft, EndLeft };
            else
                return new List<short> { EndLeft, StartLeft };
        }

        /// <summary>
        /// Kiểm tra giao nhau
        /// Dữ liệu so sánh: =====
        /// Dữ liệu gốc:     -----
        /// Phần giao nhau:  +++++
        /// </summary>
        private bool ListDuplicate(List<short> a, List<short> b)
        {
            // Chứa cận dưới [=====[+++++]-----]
            if (b[0] < a[0] && b[1] > a[0])
                return true;
            // Chưa cận trên [-----[+++++]=====]
            else if (b[0] < a[1] && b[1] > a[1])
                return true;
            // Chứa toàn bộ [=====[+++++]=====]
            else if (b[0] < a[0] && b[1] > a[1])
                return true;
            // Là tập con   [-----[+++++]-----]
            else if (b[0] > a[0] && b[1] < a[1])
                return true;
            else
                return false;
        }

        public byte[] ToBinary(bool speed = false)
        {
            List<byte> resultList = new List<byte>();
            resultList.AddRange(BitConverter.GetBytes(SegmentID));

            // Tên đường
            byte[] bffVName = Constants.UnicodeCodePage.GetBytes(VName);
            resultList.Add((byte)bffVName.Length);
            resultList.AddRange(bffVName);

            // Tỉnh
            resultList.AddRange(BitConverter.GetBytes(ProvinceID));

            // Số nhà liên tiếp
            resultList.Add((byte)(IsSerial == true ? 1 : 0));     // ANHPT: Tạm thời chưa có dữ liệu

            // Số nhà đầu đường
            resultList.AddRange(BitConverter.GetBytes(StartLeft));
            resultList.AddRange(BitConverter.GetBytes(StartRight));

            // Số nhà cuối đường
            resultList.AddRange(BitConverter.GetBytes(EndLeft));
            resultList.AddRange(BitConverter.GetBytes(EndRight));

            // Giới hạn tốc độ
            if (speed == true)
            {
                resultList.Add(MinSpeed);
                resultList.Add(MaxSpeed);
            }
            // Dữ liệu mở rộng
            resultList.AddRange(BitConverter.GetBytes(DataExt));

            // Tọa độ
            resultList.AddRange(BitConverter.GetBytes((short)PointList.Count));
            for (int i = 0; i < PointList.Count; i++)
            {
                resultList.AddRange(BitConverter.GetBytes(PointList[i].Lng));
                resultList.AddRange(BitConverter.GetBytes(PointList[i].Lat));
            }

            return resultList.ToArray();
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", SegmentID, VName.Replace(",", "-"), StartLeft, EndLeft, StartRight, EndRight, 1, 1, "");
        }
    }
}