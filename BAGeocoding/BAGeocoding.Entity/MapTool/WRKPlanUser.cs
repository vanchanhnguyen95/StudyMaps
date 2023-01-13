using System;
using System.Data;

using BAGeocoding.Entity.Enum.MapTool;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapTool
{
    public class WRKPlanUser : SQLDataUlt
    {
        public int PlanID { get; set; }
        public WRKPlan PlanInfo { get; set; }
        public USRUser UserInfo { get; set; }

        public bool State { get; set; }
        public bool StateOriginal { get; set; }

        public byte DataExt { get; set; }
        public byte DataExtOriginal { get; set; }
        public bool IsMain { get { return DataExtGet(EnumMTLPlanUserDataExt.Main); } set { DataExtSet(EnumMTLPlanUserDataExt.Main, value); } }
        public bool IsWork { get { return DataExtGet(EnumMTLPlanUserDataExt.Work); } set { DataExtSet(EnumMTLPlanUserDataExt.Work, value); } }

        public int AssignerID { get; set; }
        public int StartTime { get; set; }
        public DateTime? StartTimeGMT { get { if (StartTime > 0) return DataUtl.GetTimeUnix(StartTime); else return null; } }

        public int AbrogaterID { get; set; }
        public int EndTime { get; set; }
        public DateTime? EndTimeGMT { get { if (EndTime > 0) return DataUtl.GetTimeUnix(EndTime); else return null; } }

        public WRKPlanUser()
        {
            PlanInfo = new WRKPlan();
            UserInfo = new USRUser();
        }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                PlanID = base.GetDataValue<int>(dr, "PlanID");
                UserInfo = new USRUser();
                if (UserInfo.FromDataRow(dr) == false)
                    return false;
                DataExt = DataExtOriginal = base.GetDataValue<byte>(dr, "DataExt");

                AssignerID = base.GetDataValue<int>(dr, "AssignerID", 0);
                StartTime = base.GetDataValue<int>(dr, "StartTime", 0);

                State = StateOriginal = (StartTime > 0);

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanUser.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataUser(DataRow dr, string pr)
        {
            try
            {
                if (PlanInfo.FromDataRow(dr) == false)
                    return false;

                DataExt = base.GetDataValue<byte>(dr, pr + "DataExt", 0);
                AssignerID = base.GetDataValue<int>(dr, pr + "AssignerID", 0);
                StartTime = base.GetDataValue<int>(dr, pr + "StartTime", 0);

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanUser.FromDataUser, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataHistory(DataRow dr)
        {
            try
            {
                PlanInfo.PlanID = base.GetDataValue<int>(dr, "PlanID");
                if (UserInfo.FromDataRow(dr) == false)
                    return false;
                DataExt = base.GetDataValue<byte>(dr, "DataExt");

                AssignerID = base.GetDataValue<int>(dr, "AssignerID");
                StartTime = base.GetDataValue<int>(dr, "StartTime");

                AbrogaterID = base.GetDataValue<int>(dr, "AbrogaterID", 0);
                EndTime = base.GetDataValue<int>(dr, "EndTime", 0);

                State = StateOriginal = (StartTime > 0);

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanUser.FromDataHistory, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool DataExtGet(EnumMTLPlanUserDataExt dataExt)
        {
            return ((DataExt & (int)Math.Pow(2, (int)dataExt)) > 0);
        }

        public void DataExtSet(EnumMTLPlanUserDataExt dataExt, bool status)
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

        public bool IsUpdate()
        {
            if (State != StateOriginal)
                return true;
            else if (DataExt != DataExtOriginal)
                return true;
            else
                return false;
        }
    }
}