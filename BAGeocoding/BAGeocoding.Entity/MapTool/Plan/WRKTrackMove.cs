using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.MapTool.Base;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapTool.Plan
{
    public class WRKTrackMove : SQLDataUlt
    {
        public int PlanID { get; set; }
        public int UserID { get; set; }
        public int DayIndex { get; set; }
        public DateTime DayIndexGMT { get { return DataUtl.GetTimeUnix(DayIndex); } }
        public int StartTime { get; set; }
        public DateTime StartTimeGMT { get { return DataUtl.GetTimeUnix(StartTime); } }
        public int EndTime { get; set; }
        public DateTime EndTimeGMT { get { return DataUtl.GetTimeUnix(EndTime); } }
        public decimal Distance { get; set; }
        public string Coords { get; set; }

        public List<BAGPoint> PointList { get; set; }

        public WRKTrackMove() 
        {
            Distance = 0;
            PointList = new List<BAGPoint>();
        }

        public WRKTrackMove(GpsTrackBase gpsData)
        {
            DayIndex = DataUtl.GetUnixTime(DataUtl.GetStartDay(gpsData.TimeStampGMT));
            StartTime = gpsData.TimeStamp;
            EndTime = gpsData.TimeStamp;
            Distance = 0;
            PointList = new List<BAGPoint>();
            PointList.Add(new BAGPoint(gpsData.Coord));
        }

        public WRKTrackMove(WRKTrackMove other)
        {
            PlanID = other.PlanID;
            UserID = other.UserID;
            DayIndex = other.DayIndex;
            StartTime = other.StartTime;
            EndTime = other.EndTime;
            Distance = other.Distance;
            PointList = new List<BAGPoint>();
            for (int i = 0; i < other.PointList.Count; i++)
                PointList.Add(new BAGPoint(other.PointList[i]));
        }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                PlanID = base.GetDataValue<int>(dr, "PlanID");
                UserID = base.GetDataValue<int>(dr, "UserID");
                DayIndex = base.GetDataValue<int>(dr, "DayIndex");
                StartTime = base.GetDataValue<int>(dr, "StartTime");
                EndTime = base.GetDataValue<int>(dr, "EndTime");
                Distance = base.GetDataValue<decimal>(dr, "Distance");
                Coords = base.GetDataValue<string>(dr, "CoordsStr");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKTrackMove.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
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

        public void AddGps(GpsTrackBase gpsData)
        {
            StartTime = Math.Min(StartTime, gpsData.TimeStamp);
            EndTime = Math.Max(EndTime, gpsData.TimeStamp);
            if (PointList.Count > 0)
                Distance += (decimal)gpsData.Coord.Distance(PointList[PointList.Count - 1]);
            PointList.Add(new BAGPoint(gpsData.Coord));
        }

        public bool FromBinary(byte[] bff)
        {
            try
            {
                int dataIndex = 0;
                // 1. Lấy kế hoạch
                PlanID = BitConverter.ToInt32(bff, dataIndex);
                dataIndex += 4;
                // 2. Lấy tài khoản
                UserID = BitConverter.ToInt32(bff, dataIndex);
                dataIndex += 4;
                // 3. Lấy grid

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKTrackMove.FromBinaryGiveback, ex: {0}", ex.ToString()));
                return false;
            }
        }
    }
}