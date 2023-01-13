using System;
using System.Data;

using BAGeocoding.Entity.Enum.MapTool;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapTool
{
    public class WRKPlanBehavior : SQLDataUlt
    {
        public int PlanID { get; set; }
        public byte BehaviorID { get; set; }
        public EnumMTLPlanBehavior EnumBehaviorID { get { return (EnumMTLPlanBehavior)BehaviorID; } set { BehaviorID = (byte)value; } }
        public string Description { get; set; }

        public int EditorID { get; set; }
        public int EditTime { get; set; }
        public DateTime EditTimeGMT { get { return DataUtl.GetTimeUnix(EditTime); } set { EditTime = DataUtl.GetUnixTime(value); } }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                PlanID = base.GetDataValue<int>(dr, "PlanID");
                BehaviorID = base.GetDataValue<byte>(dr, "BehaviorID");
                Description = base.GetDataValue<string>(dr, "Description");

                EditorID = base.GetDataValue<int>(dr, "EditorID");
                EditTime = base.GetDataValue<int>(dr, "EditTime");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanBehavior.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }
    }
}