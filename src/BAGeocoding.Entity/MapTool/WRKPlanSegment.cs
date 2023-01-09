using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using BAGeocoding.Entity.Enum.MapTool;
using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapTool
{
    public class WRKPlanSegment : WRKPlanObjBase
    {
        public long PLNSegID { get; set; }
        public byte Direction { get; set; }
        public EnumMTLRoadDirection EnumDirection { get { return (EnumMTLRoadDirection)Direction; } set { Direction = (byte)value; } }
        public byte ClassFunc { get; set; }
        public EnumMTLRoadClassFunc EnumClassFunc { get { return (EnumMTLRoadClassFunc)ClassFunc; } set { ClassFunc = (byte)value; } }
        public short LevelID { get; set; }
        public EnumMTLRoadLevel EnumLevelID { get { return (EnumMTLRoadLevel)LevelID; } set { LevelID = (byte)value; } }
        public byte KindID { get; set; }
        public EnumMTLRoadKind EnumKindID { get { return (EnumMTLRoadKind)KindID; } set { KindID = (byte)value; } }

        public short StartLeft { get; set; }
        public short EndLeft { get; set; }
        public short StartRight { get; set; }
        public short EndRight { get; set; }

        public short MinSpeed { get; set; }
        public short MaxSpeed { get; set; }

        public int RoadOpts { get; set; }

        public short PointCount { get; set; }
        public string Coords { get; set; }
        public decimal RoadLength { get; set; }
        public List<BAGPoint> PointList { get; set; }


        public byte PrcApv { get; set; }
        public EnumMTLApvDataState EnumPrcApv { get { return (EnumMTLApvDataState)PrcApv; } set { PrcApv = (byte)value; } }

        public WRKPlanSegment() : base()
        {
            Coords = string.Empty;
            PointList = new List<BAGPoint>();
        }

        public WRKPlanSegment(WRKPlanSegment other) : base(other)
        {
            PLNSegID = other.PLNSegID;
            Direction = other.Direction;
            ClassFunc = other.ClassFunc;
            LevelID = other.LevelID;
            KindID = other.KindID;
            StartLeft = other.StartLeft;
            EndLeft = other.EndLeft;
            StartRight = other.StartRight;
            EndRight = other.EndRight;
            MinSpeed = other.MinSpeed;
            MaxSpeed = other.MaxSpeed;
            RoadOpts = other.RoadOpts;
            PointCount = other.PointCount;
            Coords = other.Coords;
            RoadLength = other.RoadLength;
            PointList = new List<BAGPoint>();
            for (int i = 0; i < other.PointList.Count; i++)
                PointList.Add(new BAGPoint(other.PointList[i]));
        }
        
        public bool FromDataNote(DataRow dr)
        {
            try
            {
                PLNSegID = base.GetDataValue<long>(dr, "PLNObjID");
                if (base.FromDataNote(dr) == false)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanPoint.FromDataNote, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                PLNSegID = base.GetDataValue<long>(dr, "PLNSegID");
                if (base.FromDataRow(dr) == false)
                    return false;

                Direction = base.GetDataValue<byte>(dr, "Direction");
                ClassFunc = base.GetDataValue<byte>(dr, "ClassFunc");
                LevelID = base.GetDataValue<short>(dr, "LevelID");
                KindID = base.GetDataValue<byte>(dr, "KindID");

                StartLeft = base.GetDataValue<short>(dr, "StartLeft");
                EndLeft = base.GetDataValue<short>(dr, "EndLeft");
                StartRight = base.GetDataValue<short>(dr, "StartRight");
                EndRight = base.GetDataValue<short>(dr, "EndRight");

                MinSpeed = base.GetDataValue<short>(dr, "MinSpeed");
                MaxSpeed = base.GetDataValue<short>(dr, "MaxSpeed");

                RoadOpts = base.GetDataValue<int>(dr, "RoadOpts");

                PointCount = base.GetDataValue<short>(dr, "PointCount");
                Coords = base.GetDataValue<string>(dr, "Coords");
                RoadLength = base.GetDataValue<decimal>(dr, "RoadLength");
                
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanSegment.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        #region ==================== Trạng thái của điểm ====================
        public bool RoadOptsGet(EnumMTLRoadOption opts)
        {
            return ((RoadOpts & (int)Math.Pow(2, (int)opts)) > 0);
        }

        public void RoadOptsSet(EnumMTLRoadOption opts, bool state)
        {
            // Bít đã được bật
            if (((RoadOpts >> (int)opts) & 1) > 0)
            {
                if (state == false)
                    RoadOpts = (StateOpts - (int)Math.Pow(2, (int)opts));
            }
            // Bít chưa bật
            else
            {
                if (state == true)
                    RoadOpts = (StateOpts + (int)Math.Pow(2, (int)opts));
            }
        }
        #endregion

        public bool IsHaveNumber()
        {
            if (StartLeft > 0)
                return true;
            else if (EndLeft > 0)
                return true;
            else if (StartRight > 0)
                return true;
            else if (EndRight > 0)
                return true;
            else
                return false;
        }

        public void PointGenerate()
        {
            PointList = MapHelper.PolylineAlgorithmDecode(Coords).ToList();
        }

        public BAGPoint Center()
        {
            if (PointList.Count == 0)
                return new BAGPoint(0, 0);
            int indexID = Convert.ToInt32(PointList.Count / 2);
            return new BAGPoint(PointList[indexID]);
        }
        
        public byte[] Tobinary()
        {
            try
            {
                // Khởi tạo dữ liệu
                List<byte> resultList = new List<byte>();
                byte[] namBff = Constants.UTF8CodePage.GetBytes(Name);
                byte[] noteBff = Constants.UTF8CodePage.GetBytes(NoteOld);
                byte[] gridBff = Constants.UTF8CodePage.GetBytes(GridView);
                byte[] coordsBff = Constants.UTF8CodePage.GetBytes(Coords);

                // Thông tin cơ bản
                resultList.AddRange(BitConverter.GetBytes(PLNSegID));   // Key của server
                resultList.AddRange(BitConverter.GetBytes((short)namBff.Length));
                if (namBff.Length > 0)
                    resultList.AddRange(namBff);
                resultList.Add(Direction);
                resultList.Add(ClassFunc);
                resultList.AddRange(BitConverter.GetBytes(LevelID));
                resultList.Add(KindID);
                
                // Số nhà
                resultList.AddRange(BitConverter.GetBytes(StartLeft));
                resultList.AddRange(BitConverter.GetBytes(EndLeft));
                resultList.AddRange(BitConverter.GetBytes(StartRight));
                resultList.AddRange(BitConverter.GetBytes(EndRight));

                // Tốc độ
                resultList.AddRange(BitConverter.GetBytes(MinSpeed));
                resultList.AddRange(BitConverter.GetBytes(MaxSpeed));

                // Cấu hình tùy chỉnh
                resultList.AddRange(BitConverter.GetBytes(RoadOpts));
                resultList.AddRange(BitConverter.GetBytes(StateOpts));

                // Grid
                resultList.AddRange(BitConverter.GetBytes(GridEdit));
                resultList.AddRange(BitConverter.GetBytes((short)gridBff.Length));
                if (gridBff.Length > 0)
                    resultList.AddRange(gridBff);

                // Ghi chú
                resultList.AddRange(BitConverter.GetBytes((short)noteBff.Length));
                if (noteBff.Length > 0)
                    resultList.AddRange(noteBff);

                // Thông tin thao tác
                resultList.AddRange(BitConverter.GetBytes(ActionID));
                resultList.Add(ApprovedState);

                // Thông tin về tọa đồ
                resultList.AddRange(BitConverter.GetBytes((short)coordsBff.Length));
                if (coordsBff.Length > 0)
                    resultList.AddRange(coordsBff);

                // Trả về kết quả
                return resultList.ToArray();
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanSegment.Tobinary, ex: {0}", ex.ToString()));
                return null;
            }
        }

        public bool FromBinaryCommitData(byte[] bff, ref int idx)
        {
            try
            {
                // Mã điểm của hệ thống server
                PLNSegID = BitConverter.ToInt64(bff, idx);
                idx += 8;
                // Mã thao tác
                ActionID = BitConverter.ToInt32(bff, idx);
                idx += 4;
                // Kiểm tra để lấy thông tin POI
                int textLength = 0;
                if (true)    //if (IsDelete() == false) // ANHPT: Version hiện tại của app đang lỗi, luôn gửi full dữ liệu
                {
                    // Tên đường
                    textLength = BitConverter.ToInt16(bff, idx);
                    idx += 2;
                    Name = Constants.UTF8CodePage.GetString(bff, idx, textLength);
                    idx += textLength;
                    if (Name.Equals("Ngõ 36 Dịch Vọng Hậu") == true)
                        Console.Write("A");
                    // Chiều
                    Direction = bff[idx];
                    idx += 1;
                    // Loại đường
                    ClassFunc = bff[idx];
                    idx += 1;
                    // Cấp đường
                    LevelID = BitConverter.ToInt16(bff, idx);
                    idx += 2;
                    // Kiểu đường
                    KindID = bff[idx];
                    idx += 1;
                    // Số nhà bắt đầu bên trái
                    StartLeft = BitConverter.ToInt16(bff, idx);
                    idx += 2;
                    // Số nhà kết thúc bên trái
                    EndLeft = BitConverter.ToInt16(bff, idx);
                    idx += 2;
                    // Số nhà bắt đầu bên phải
                    StartRight = BitConverter.ToInt16(bff, idx);
                    idx += 2;
                    // Số nhà kết thúc bên phải
                    EndRight = BitConverter.ToInt16(bff, idx);
                    idx += 2;
                    // Tốc độ tối thiểu
                    MinSpeed = BitConverter.ToInt16(bff, idx);
                    idx += 2;
                    // Tốc độ tối đa
                    MaxSpeed = BitConverter.ToInt16(bff, idx);
                    idx += 2;
                    // Road option
                    RoadOpts = BitConverter.ToInt32(bff, idx);
                    idx += 4;
                    // State option
                    StateOpts = BitConverter.ToInt16(bff, idx);
                    idx += 2;
                    // Grid edit
                    GridEdit = BitConverter.ToInt32(bff, idx);
                    idx += 4;
                    // Grid view
                    textLength = BitConverter.ToInt16(bff, idx);
                    idx += 2;
                    GridView = Constants.UTF8CodePage.GetString(bff, idx, textLength);
                    idx += textLength;
                    // Ghi chú
                    textLength = BitConverter.ToInt16(bff, idx);
                    idx += 2;
                    NoteNew = Constants.UTF8CodePage.GetString(bff, idx, textLength);
                    idx += textLength;
                    // Tọa độ
                    textLength = BitConverter.ToInt16(bff, idx);
                    idx += 2;
                    Coords = Constants.UTF8CodePage.GetString(bff, idx, textLength);
                    idx += textLength;
                }
                // Thông tin thao tác
                EditorID = BitConverter.ToInt32(bff, idx);
                idx += 4;
                EditTime = BitConverter.ToInt32(bff, idx);
                idx += 4;

                // Kiểm tra hiệu chỉnh thao tác
                if (IsCreate() == true)
                {
                    // Bổ sung thao tác cập nhật thông tin đường
                    if (Name.Length > 0 && ActionGet(EnumMTLObjectAction.EditObject) == false)
                        ActionSet(EnumMTLObjectAction.EditObject, true);

                    // Bổ sung thao tác cập nhật số nhà
                    if (IsHaveNumber() == true && ActionGet(EnumMTLObjectAction.EnterNumber) == false)
                        ActionSet(EnumMTLObjectAction.EnterNumber, true);
                }

                // Trả về trạng thái
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanSegment.FromBinaryCommitData, ex: {0}", ex.ToString()));
                return false;
            }
        }
    }
}