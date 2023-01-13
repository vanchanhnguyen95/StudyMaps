using System;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.DataService
{
    public class DTSTraffic
    {
        public int AuthenID { get; set; }
        public DateTime DateIndex { get; set; }
        public int TrafficBA { get; set; }
        public int TrafficGG { get; set; }

        public DTSTraffic()
        {
            AuthenID = 0;
            DateIndex = DataUtl.GetCurrDate();
            TrafficBA = 0;
            TrafficGG = 0;
        }

        public DTSTraffic(int authenID)
        {
            AuthenID = authenID;
            DateIndex = DataUtl.GetCurrDate();
            TrafficBA = 0;
            TrafficGG = 0;
        }

        public DTSTraffic(DTSTraffic other)
        {
            AuthenID = other.AuthenID;
            TrafficBA = other.TrafficBA;
            TrafficGG = other.TrafficGG;
        }

        public bool IsCurrent()
        {
            return (DateIndex - DataUtl.GetCurrDate()).Days == 0;
        }
    }
}