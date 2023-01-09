using System;
using System.Data;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapTool.Base
{
    public class ConfigBase : SQLDataUlt
    {
        public int AssignerID { get; set; }
        public int StartTime { get; set; }
        public DateTime? StartTimeGMT { get { if (StartTime > 0) return DataUtl.GetTimeUnix(StartTime); else return null; } }

        public int AbrogaterID { get; set; }
        public int EndTime { get; set; }
        public DateTime? EndTimeGMT { get { if (EndTime > 0) return DataUtl.GetTimeUnix(EndTime); else return null; } }

        public bool FromDataBase(DataRow dr)
        {
            try
            {
                AssignerID = base.GetDataValue<int>(dr, "AssignerID");
                StartTime = base.GetDataValue<int>(dr, "StartTime");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("ConfigBase.FromDataBase, ex: {0}", ex.ToString()));
                return false;
            }
        }
        
        public bool FromDataRow(DataRow dr)
        {
            try
            {
                AssignerID = base.GetDataValue<int>(dr, "AssignerID");
                StartTime = base.GetDataValue<int>(dr, "StartTime");

                AbrogaterID = base.GetDataValue<int>(dr, "AbrogaterID");
                EndTime = base.GetDataValue<int>(dr, "EndTime");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("ConfigBase.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }
    }
}
