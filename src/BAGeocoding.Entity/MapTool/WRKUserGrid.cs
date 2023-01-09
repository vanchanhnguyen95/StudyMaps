using System;
using System.Collections.Generic;
using System.Data;

using BAGeocoding.Entity.Enum.MapTool;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapTool
{
    public class WRKUserGrid : SQLDataUlt
    {
        public int PlanID { get; set; }
        public int UserID { get; set; }
        public MCLGrid GridInfo { get; set; }

        public bool State { get; set; }
        public bool StateOriginal { get; set; }

        public byte DataExt { get; set; }
        public byte DataExtOriginal { get; set; }
        public bool IsEdit { get { return DataExtGet(EnumMTLUserGridDataExt.Edit); } set { DataExtSet(EnumMTLUserGridDataExt.Edit, value); } }
        public bool IsComposePoi { get { return DataExtGet(EnumMTLUserGridDataExt.ComposePoi); } set { DataExtSet(EnumMTLUserGridDataExt.ComposePoi, value); } }
        public bool IsComposeSeg { get { return DataExtGet(EnumMTLUserGridDataExt.ComposeSeg); } set { DataExtSet(EnumMTLUserGridDataExt.ComposeSeg, value); } }

        public int AssignerID { get; set; }
        public int StartTime { get; set; }
        public DateTime? StartTimeGMT { get { if (StartTime > 0) return DataUtl.GetTimeUnix(StartTime); else return null; } }

        public int AbrogaterID { get; set; }
        public int EndTime { get; set; }
        public DateTime EndTimeGMT { get { return DataUtl.GetTimeUnix(EndTime); } }


        public int ProcessorID { get; set; }

        public byte PGState { get; set; }
        public int PGLastEdit { get; set; }
        public string Password { get; set; }

        public bool FromDataExt(DataRow dr)
        {
            try
            {
                PlanID = base.GetDataValue<int>(dr, "PlanID");
                UserID = base.GetDataValue<int>(dr, "UserID");
                GridInfo = new MCLGrid { GridID = base.GetDataValue<int>(dr, "GridID") };
                DataExt = base.GetDataValue<byte>(dr, "DataExt");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKUserGrid.FromDataExt, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataLogin(DataRow dr)
        {
            try
            {
                PlanID = base.GetDataValue<int>(dr, "PlanID");
                UserID = base.GetDataValue<int>(dr, "UserID");
                GridInfo = new MCLGrid
                {
                    GridID = base.GetDataValue<int>(dr, "GridID"),
                    CoordsEncrypt = base.GetDataValue<string>(dr, "CoordsEncrypt")
                };
                DataExt = DataExtOriginal = base.GetDataValue<byte>(dr, "DataExt");

                PGState = base.GetDataValue<byte>(dr, "PGState");
                PGLastEdit = base.GetDataValue<int>(dr, "PGLastEdit");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKUserGrid.FromDataLogin, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                PlanID = base.GetDataValue<int>(dr, "PlanID");
                UserID = base.GetDataValue<int>(dr, "UserID");
                GridInfo = new MCLGrid();
                if (GridInfo.FromDataRow(dr) == false)
                    return false;

                ProcessorID = base.GetDataValue<int>(dr, "ProcessorID", 0);

                DataExt = DataExtOriginal = base.GetDataValue<byte>(dr, "DataExt");
                AssignerID = base.GetDataValue<int>(dr, "AssignerID", 0);
                StartTime = base.GetDataValue<int>(dr, "StartTime", 0);

                State = StateOriginal = (StartTime > 0);

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKUserGrid.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool IsUpdate()
        {
            if (State != StateOriginal)
                return true;
            else if (DataExt != DataExtOriginal)
                return true;
            else
                return false;
        }

        public bool DataExtGet(EnumMTLUserGridDataExt dataExt)
        {
            return ((DataExt & (int)Math.Pow(2, (int)dataExt)) > 0);
        }

        public void DataExtSet(EnumMTLUserGridDataExt dataExt, bool status)
        {
            // Bít đã được bật
            if (((DataExt >> (int)dataExt) & 1) > 0)
            {
                if (status == false)
                    DataExt = (byte)(DataExt - (int)Math.Pow(2, (int)dataExt));
            }
            // Bít chưa bật
            else
            {
                if (status == true)
                    DataExt = (byte)(DataExt + (int)Math.Pow(2, (int)dataExt));
            }
        }


        public bool FromBinaryState(byte[] bff)
        {
            try
            {
                int dataIndex = 0;

                // 1. Lấy tài khoản
                UserID = BitConverter.ToInt32(bff, dataIndex);
                dataIndex += 4;
                // 2. Lấy kế hoạch
                PlanID = BitConverter.ToInt32(bff, dataIndex);
                dataIndex += 4;
                // 3. Lấy grid
                GridInfo = new MCLGrid { GridID = BitConverter.ToInt32(bff, dataIndex) };
                dataIndex += 4;
                // 4. Lấy trạng thái
                PGState = bff[dataIndex];
                dataIndex += 1;

                // Gán người xử lý
                AssignerID = UserID;

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("USRUserInfo.FromBinaryState, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromBinaryGiveback(byte[] bff)
        {
            try
            {
                int dataIndex = 0;

                // 1. Lấy tài khoản
                UserID = BitConverter.ToInt32(bff, dataIndex);
                dataIndex += 4;
                // 2. Lấy kế hoạch
                PlanID = BitConverter.ToInt32(bff, dataIndex);
                dataIndex += 4;
                // 3. Lấy grid
                GridInfo = new MCLGrid { GridID = BitConverter.ToInt32(bff, dataIndex) };
                dataIndex += 4;
                // 4. Mật khẩu
                int lengthPassword = BitConverter.ToInt16(bff, dataIndex);
                dataIndex += 2;
                Password = Constants.UTF8CodePage.GetString(bff, dataIndex, lengthPassword);

                // Gán người xử lý
                AbrogaterID = UserID;

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("USRUserInfo.FromBinaryGiveback, ex: {0}", ex.ToString()));
                return false;
            }
        }
        
        
        public byte[] ToBinaryLogin()
        {
            try
            {
                // 1. Lấy dữ liệu
                List<byte> resultList = new List<byte>();
                // 1.1 Mã kế hoạch
                //resultList.AddRange(BitConverter.GetBytes(PlanID));
                //// 1.2 Mã tài khoản
                //resultList.AddRange(BitConverter.GetBytes(UserID));
                // 1.3 Mã lưới
                resultList.AddRange(BitConverter.GetBytes(GridInfo.GridID));
                // 1.4 Dữ liệu
                resultList.Add(DataExt);
                // 1.5 Trạng thái xử lý grid
                resultList.Add(PGState);
                // 1.6 Thời điểm cập nhật grid
                resultList.AddRange(BitConverter.GetBytes(PGLastEdit));
                // 1.7 Tọa độ của grid
                byte[] coordsBff = Constants.UTF8CodePage.GetBytes(GridInfo.CoordsEncrypt);
                resultList.AddRange(BitConverter.GetBytes((short)coordsBff.Length));
                resultList.AddRange(coordsBff);

                // 2. Trả về kết quả
                return resultList.ToArray();
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKUserGrid.ToBinaryLogin, ex: {0}", ex.ToString()));
                return null;
            }
        }


        public string ToStringState()
        {
            return string.Format("UserID: {1}, PlanID: {0}, GridID: {2}, PGState: {3}", PlanID, UserID, GridInfo.GridID, PGState);
        }

        public string ToStringGive()
        {
            return string.Format("UserID: {1}, PlanID: {0}, GridID: {2}", PlanID, UserID, GridInfo.GridID);
        }
    }
}