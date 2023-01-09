using System;

using BAGeocoding.Entity.Enum.Route;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Router
{
    [Serializable]
    public class BARRouteLogs
    {
        public byte MethodID { get; set; }
        public EnumBARMethod EnumMethod { get { return (EnumBARMethod)MethodID; } set { MethodID = (byte)value; } }
        public int RegisterID { get; set; }
        public long TickTime { get; set; }
        public int UnixTime { get; set; }
        public string Params { get; set; }
        public byte Amount { get; set; }
        public byte Google { get; set; }
        public string TypeStr { get; set; }
        public int MonthIndex { get; set; }
        public byte DayIndex { get; set; }
        public byte HourIndex { get; set; }
        public byte ErrorCode { get; set; }
                
        public BARRouteLogs() { }

        public BARRouteLogs(BARRouteLogs other)
        {
            MethodID = other.MethodID;
            RegisterID = other.RegisterID;
            TickTime = other.TickTime;
            UnixTime = other.UnixTime;
            Params = other.Params;
            Amount = other.Amount;
            Google = other.Google;
            TypeStr = other.TypeStr;
            MonthIndex = other.MonthIndex;
            DayIndex = other.DayIndex;
            HourIndex = other.HourIndex;
            ErrorCode = other.ErrorCode;
        }

        public void InitTime(DateTime dt)
        {
            TickTime = dt.Ticks;
            UnixTime = DataUtl.GetUnixTime(dt);
            MonthIndex = DataUtl.GetMonthIndex(dt);
            DayIndex = (byte)dt.Day;
            HourIndex = (byte)dt.Hour;
        }
    }
}