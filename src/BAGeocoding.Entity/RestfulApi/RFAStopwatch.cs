using System;

using BAGeocoding.Entity.Enum;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.RestfulApi
{
    public class RFAStopwatch
    {
        public int MethodID { get; set; }
        public EnumRestfulApiMethod EnumMethodID { get { return (EnumRestfulApiMethod)MethodID; } set { MethodID = (int)value; } }
        public int RegisterID { get; set; }
        public string KeyStr { get; set; }
        public string IPAddress { get; set; }
        public string ParamsStr { get; set; }
        public byte ErrorCode { get; set; }

        public long TickTime { get; set; }
        public int UnixTime { get; set; }
        public DateTime UnixTimeGMT { get { return DataUtl.GetTimeUnix(UnixTime); } }
        public long ElapsedMethod { get; set; }
        public long ElapsedTotal { get; set; }

        public short YearIndex { get; set; }
        public byte MonthIndex { get; set; }
        public byte DayIndex { get; set; }
        public byte DateIndex { get; set; }
        public byte HourIndex { get; set; }

        public RFAStopwatch()
        {
            UnixTime = DataUtl.GetUnixTime();

            YearIndex = (short)UnixTimeGMT.Year;
            MonthIndex = (byte)UnixTimeGMT.Month;
            DayIndex = (byte)UnixTimeGMT.Day;
            DateIndex = (byte)UnixTimeGMT.DayOfWeek;
            HourIndex = (byte)UnixTimeGMT.Hour;
        }

        public RFAStopwatch(RFAStopwatch other)
        {
            MethodID = other.MethodID;
            RegisterID = other.RegisterID;
            KeyStr = other.KeyStr;
            IPAddress = other.IPAddress;
            ParamsStr = other.ParamsStr;
            ErrorCode = other.ErrorCode;
            TickTime = other.TickTime;
            UnixTime = other.UnixTime;
            ElapsedMethod = other.ElapsedMethod;
            ElapsedTotal = other.ElapsedTotal;

            YearIndex = other.YearIndex;
            MonthIndex = other.MonthIndex;
            DayIndex = other.DayIndex;
            DateIndex = other.DateIndex;
            HourIndex = other.HourIndex;
        }

        public RFAStopwatch(RFARegisterCheck other)
        {
            EnumMethodID = other.MethodID;

            RegisterID = other.RegisterID;
            KeyStr = other.KeyStr;
            IPAddress = other.IPAddress;

            TickTime = DataUtl.GetFullTime().Ticks;
            UnixTime = DataUtl.GetUnixTime();
            YearIndex = (short)UnixTimeGMT.Year;
            MonthIndex = (byte)UnixTimeGMT.Month;
            DayIndex = (byte)UnixTimeGMT.Day;
            DateIndex = (byte)UnixTimeGMT.DayOfWeek;
            HourIndex = (byte)UnixTimeGMT.Hour;
        }
    }
}
