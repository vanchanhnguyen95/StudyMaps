using System;
using System.Data;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapTool.Base
{
    public class ConditionBase
    {
        public int PlanID { get; set; }
        public int UserID { get; set; }

        public int StartTime { get; set; }
        public DateTime StartTimeGMT { get { return DataUtl.GetTimeUnix(StartTime); } }
        public int EndTime { get; set; }
        public DateTime EndTimeGMT { get { return DataUtl.GetTimeUnix(EndTime); } }
    }
}
