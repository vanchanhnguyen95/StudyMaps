using System;
using System.Data;

using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapTool.Base
{
    public class GpsTrackBase : SQLDataUlt
    {
        public BAGPoint Coord { get; set; }
        public decimal Speed { get; set; }
        public int TimeStamp { get; set; }
        public DateTime TimeStampGMT { get { return DataUtl.GetTimeUnix(TimeStamp); } }
        
        public bool FromDataRow(DataRow dr)
        {
            try
            {
                Coord = new BAGPoint
                {
                    Lng = base.GetDataValue<double>(dr, "Lng"),
                    Lat = base.GetDataValue<double>(dr, "Lat")
                };
                Speed = base.GetDataValue<decimal>(dr, "Speed");
                TimeStamp = base.GetDataValue<int>(dr, "TimeStamp");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("GpsTrackBase.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromBinary(byte[] bff, ref int dataIndex)
        {
            try
            {
                // 1. Tọa độ
                Coord = new BAGPoint
                {
                    Lng = BitConverter.ToDouble(bff, dataIndex),
                    Lat = BitConverter.ToDouble(bff, dataIndex + 8)
                };
                dataIndex += 16;
                // 2. Tốc độ di chuyển
                Speed = (decimal)BitConverter.ToSingle(bff, dataIndex);
                dataIndex += 4;
                // 3. Thời gian
                TimeStamp = BitConverter.ToInt32(bff, dataIndex);
                dataIndex += 4;

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("GpsTrackBase.FromBinary, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool IsValid()
        {
            if (Coord.Lng < 101.953125)
                return false;
            else if (Coord.Lng > 109.7314453125)
                return false;
            else if (Coord.Lat < 8.428904092875392)
                return false;
            else if (Coord.Lat > 23.624394569716923)
                return false;
            else
                return true;
        }
    }
}
