using System;
using System.Data;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapTool.Data
{
    public class DTSDeviceAppInfo : SQLDataUlt
    {
        public string OSVersion { get; set; }
        public string APVersion { get; set; }
        public int StartTime { get; set; }
        public DateTime StartTimeGMT { get { return DataUtl.GetTimeUnix(StartTime); } }
        public int EndTime { get; set; }
        public DateTime? EndTimeGMT { get { if (EndTime > 0) return DataUtl.GetTimeUnix(EndTime); else return null; } }
        
        public bool FromDataRow(DataRow dr)
        {
            try
            {
                OSVersion = base.GetDataValue<string>(dr, "OSVersion");
                APVersion = base.GetDataValue<string>(dr, "APVersion");
                
                StartTime = base.GetDataValue<int>(dr, "StartTime");
                EndTime = base.GetDataValue<int>(dr, "EndTime");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("DTSDeviceAppInfo.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }
    }
}
