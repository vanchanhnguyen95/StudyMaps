using System;
using System.Collections.Generic;
using System.Data;

using BAGeocoding.Entity.Enum.MapTool;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapTool
{
    public class WRKPlanGrid : SQLDataUlt
    {
        public int PlanID { get; set; }
        public MCLGrid GridInfo { get; set; }

        public bool State { get; set; }
        public bool StateOriginal { get; set; }

        public int AssignerID { get; set; }
        public int StartTime { get; set; }
        public DateTime? StartTimeGMT { get { if (StartTime > 0) return DataUtl.GetTimeUnix(StartTime); else return null; } }

        public int AbrogaterID { get; set; }
        public int EndTime { get; set; }
        public DateTime EndTimeGMT { get { return DataUtl.GetTimeUnix(EndTime); } }
        
        public byte EXTGridState { get; set; }
        public int StateTime { get; set; }
        public DateTime? StateTimeGMT { get { if (StateTime > 0) return DataUtl.GetTimeUnix(StateTime); else return null; } }
        public USRUser EXTUserInfo { get; set; }
        public int RoadCreate { get; set; }
        public int RoadData { get; set; }
        public int RoadGiveback { get; set; }

        public int PoiCreate { get; set; }
        public int PoiImage { get; set; }
        public int PoiGiveback { get; set; }

        public int TimeExportSeg { get; set; }
        public DateTime? TimeExportSegGMT { get { if (TimeExportSeg > 0) return DataUtl.GetTimeUnix(TimeExportSeg); else return null; } }
        public int TimeExportPoi { get; set; }
        public DateTime? TimeExportPoiGMT { get { if (TimeExportPoi > 0) return DataUtl.GetTimeUnix(TimeExportPoi); else return null; } }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                PlanID = base.GetDataValue<int>(dr, "PlanID");
                GridInfo = new MCLGrid();
                if (GridInfo.FromDataRow(dr) == false)
                    return false;

                AssignerID = base.GetDataValue<int>(dr, "AssignerID", 0);
                StartTime = base.GetDataValue<int>(dr, "StartTime", 0);

                State = StateOriginal = (StartTime > 0);

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanGrid.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataExt(DataRow dr)
        {
            try
            {
                PlanID = base.GetDataValue<int>(dr, "PlanID");
                GridInfo = new MCLGrid();
                if (GridInfo.FromDataRow(dr) == false)
                    return false;

                EXTGridState = base.GetDataValue<byte>(dr, "EXTGridState");
                EXTUserInfo = new USRUser
                {
                    UserID = base.GetDataValue<int>(dr, "USRUserID"),
                    UserName = base.GetDataValue<string>(dr, "USRUserName")
                };

                StateTime = base.GetDataValue<int>(dr, "EditTime");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanGrid.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataExport(DataRow dr)
        {
            try
            {
                PlanID = base.GetDataValue<int>(dr, "PlanID");
                GridInfo = new MCLGrid();
                if (GridInfo.FromDataRow(dr) == false)
                    return false;

                EXTGridState = base.GetDataValue<byte>(dr, "EXTGridState");
                EXTUserInfo = new USRUser
                {
                    UserID = base.GetDataValue<int>(dr, "USRUserID"),
                    UserName = base.GetDataValue<string>(dr, "USRUserName")
                };

                TimeExportSeg = base.GetDataValue<int>(dr, "TimeExportSeg");
                TimeExportPoi = base.GetDataValue<int>(dr, "TimeExportPoi");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanGrid.FromDataExport, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataData(DataRow dr)
        {
            try
            {
                PlanID = base.GetDataValue<int>(dr, "PlanID");
                GridInfo = new MCLGrid();
                if (GridInfo.FromDataRow(dr) == false)
                    return false;

                EXTGridState = base.GetDataValue<byte>(dr, "EXTGridState");
                EXTUserInfo = new USRUser
                {
                    UserID = base.GetDataValue<int>(dr, "USRUserID"),
                    UserName = base.GetDataValue<string>(dr, "USRUserName")
                };

                RoadCreate = base.GetDataValue<int>(dr, "RoadCreate");
                RoadData = base.GetDataValue<int>(dr, "RoadData");
                PoiCreate = base.GetDataValue<int>(dr, "PoiCreate");
                PoiImage = base.GetDataValue<int>(dr, "PoiImage");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanGrid.FromDataData, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataState(DataRow dr)
        {
            try
            {
                PlanID = base.GetDataValue<int>(dr, "PlanID");
                GridInfo = new MCLGrid();
                if (GridInfo.FromDataRow(dr) == false)
                    return false;

                EXTGridState = base.GetDataValue<byte>(dr, "EXTGridState");
                EXTUserInfo = new USRUser
                {
                    UserID = base.GetDataValue<int>(dr, "USRUserID"),
                    UserName = base.GetDataValue<string>(dr, "USRUserName")
                };
                StateTime = base.GetDataValue<int>(dr, "StateTime");

                AssignerID = base.GetDataValue<int>(dr, "AssignerID");
                StartTime = base.GetDataValue<int>(dr, "StartTime");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanGrid.FromDataState, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataAppGvb(DataRow dr)
        {
            try
            {
                PlanID = base.GetDataValue<int>(dr, "PlanID");
                GridInfo = new MCLGrid();
                if (GridInfo.FromDataRow(dr) == false)
                    return false;

                EXTUserInfo = new USRUser
                {
                    UserID = base.GetDataValue<int>(dr, "USRUserID"),
                    UserName = base.GetDataValue<string>(dr, "USRUserName")
                };

                RoadCreate = base.GetDataValue<int>(dr, "RoadCreate");
                RoadData = base.GetDataValue<int>(dr, "RoadData");
                RoadGiveback = base.GetDataValue<int>(dr, "RoadGiveback");

                PoiCreate = base.GetDataValue<int>(dr, "PoiCreate");
                PoiImage = base.GetDataValue<int>(dr, "PoiImage");
                PoiGiveback = base.GetDataValue<int>(dr, "PoiGiveback");

                if (RoadGiveback > 0 || PoiGiveback > 0)
                    EXTGridState = (byte)EnumMTLPlanGridState.Generate;
                else
                    EXTGridState = (byte)EnumMTLPlanGridState.Approved;

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanGrid.FromDataAppGvb, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool DataError()
        {
            if (PoiImage != PoiCreate)
                return true;
            else
                return false;
        }
    }

    public class WRKPlanGridSync : SQLDataUlt
    {
        public int UserID { get; set; }
        public int PlanID { get; set; }
        public int GridID { get; set; }
        public byte State { get; set; }
        public EnumMTLGridSyncState EnumState { get { return (EnumMTLGridSyncState)State; } set { State = (byte)value; } }
        public List<WRKPlanPoint> PointList { get; set; }
        public List<WRKPlanSegment> SegmentList { get; set; }

        public WRKPlanGridSync()
        {
            PointList = new List<WRKPlanPoint>();
            SegmentList = new List<WRKPlanSegment>();
        }

        public bool FromDataSimple(DataRow dr)
        {
            try
            {
                GridID = base.GetDataValue<int>(dr, "GridID");
                EnumState = base.GetDataValue<int>(dr, "UserID") > 0 ?  EnumMTLGridSyncState.Normal : EnumMTLGridSyncState.Blank;
                if (State == 1)
                {
                    WRKUserGrid userGrid = new WRKUserGrid { DataExt = base.GetDataValue<byte>(dr, "DataExt") };
                    if (userGrid.DataExtGet(EnumMTLUserGridDataExt.Edit) == true && base.GetDataValue<byte>(dr, "GState") == (byte)EnumMTLPlanGridState.Processing)
                        EnumState = EnumMTLGridSyncState.Denied;
                }

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanGridSync.FromDataSimple, ex: {0}", ex.ToString()));
                return false;
            }
        }
        
        public byte[] ToBinary()
        {
            try
            {
                // 1. Lấy dữ liệu
                List<byte> resultList = new List<byte>();
                // 1.1 Thông tin cơ bản
                resultList.AddRange(BitConverter.GetBytes(GridID));
                resultList.Add(State);

                if (State == 1)
                {
                    // 1.2 Danh sách điểm
                    resultList.AddRange(BitConverter.GetBytes(PointList.Count));
                    for (int i = 0; i < PointList.Count; i++)
                        resultList.AddRange(PointList[i].Tobinary());
                    // 1.3 Danh sách đường
                    resultList.AddRange(BitConverter.GetBytes(SegmentList.Count));
                    for (int i = 0; i < SegmentList.Count; i++)
                        resultList.AddRange(SegmentList[i].Tobinary());
                }

                return resultList.ToArray();
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanGridSync.ToBinary, ex: {0}", ex.ToString()));
                return null;
            }
        }

        public bool FromBinaryCommitData(byte[] bff)
        {
            try
            {
                int dataIndex = 0;
                // 1. Mã tài khoản
                UserID = BitConverter.ToInt32(bff, dataIndex);
                dataIndex += 4;
                // 2. Mã kế hoạch
                PlanID = BitConverter.ToInt32(bff, dataIndex);
                dataIndex += 4;
                // 3. Mã grid
                GridID = BitConverter.ToInt32(bff, dataIndex);
                dataIndex += 4;
                // 4. Dữ liệu đối tượng
                // 4.1 Dữ liệu điểm
                int pointCount = BitConverter.ToInt32(bff, dataIndex);
                dataIndex += 4;
                for (int i = 0; i < pointCount; i++)
                {
                    WRKPlanPoint pointInfo = new WRKPlanPoint();
                    if (pointInfo.FromBinaryCommitData(bff, ref dataIndex) == false)
                        return false;
                    pointInfo.PlanID = PlanID;
                    PointList.Add(pointInfo);
                }
                // 4.2 Dữ liệu đường
                int segmentCount = BitConverter.ToInt32(bff, dataIndex);
                dataIndex += 4;
                for (int i = 0; i < segmentCount; i++)
                {
                    WRKPlanSegment segmentInfo = new WRKPlanSegment();
                    if (segmentInfo.FromBinaryCommitData(bff, ref dataIndex) == false)
                        return false;
                    segmentInfo.PlanID = PlanID;
                    SegmentList.Add(segmentInfo);
                }

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanGridSync.FromBinaryCommitData, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromBinaryUploadImage(byte[] bff)
        {
            try
            {
                int dataIndex = 0;
                // 1. Mã tài khoản
                UserID = BitConverter.ToInt32(bff, dataIndex);
                dataIndex += 4;
                // 2. Mã kế hoạch
                PlanID = BitConverter.ToInt32(bff, dataIndex);
                dataIndex += 4;
                // 3. Mã grid
                GridID = BitConverter.ToInt32(bff, dataIndex);
                dataIndex += 4;
                // 4. Dữ liệu điểm
                int pointCount = BitConverter.ToInt32(bff, dataIndex);
                dataIndex += 4;
                for (int i = 0; i < pointCount; i++)
                {
                    WRKPlanPoint pointInfo = new WRKPlanPoint();
                    if (pointInfo.FromBinaryUploadImage(bff, ref dataIndex) == false)
                        return false;
                    pointInfo.PlanID = PlanID;
                    PointList.Add(pointInfo);
                }

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanGridSync.FromBinaryUploadImage, ex: {0}", ex.ToString()));
                return false;
            }
        }
    }
}