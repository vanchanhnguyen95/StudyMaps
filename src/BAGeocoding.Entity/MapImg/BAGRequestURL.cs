using System;
using System.Collections.Generic;
using System.Data;

using BAGeocoding.Entity.Enum;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapImg
{
    public class BAGRequestURL : SQLDataUlt
    {
        public int IndexID { get; set; }
        public string RequestURL { get; set; }
        public int DataExt { get; set; }
        public bool IsBA { get { return DataExtGet(EnumBAGMapImgType.BinhAnh); } }
        public bool IsHB { get { return DataExtGet(EnumBAGMapImgType.Hybird); } }
        public bool IsGT { get { return DataExtGet(EnumBAGMapImgType.GiaoThong); } }
        public bool DeleteAllow { get; set; }

        public int StartTime { get; set; }
        public DateTime StartTimeGMT { get { return DataUtl.GetTimeUnix(StartTime); } }
        public int EndTime { get; set; }
        public DateTime? EndTimeGMT { get { if (EndTime > 0) return DataUtl.GetTimeUnix(EndTime); else return null; } }
        
        public bool FromDataRow(DataRow dr, bool fl = false)
        {
            try
            {
                IndexID = base.GetDataValue<int>(dr, "IndexID");
                RequestURL = base.GetDataValue<string>(dr, "RequestURL");
                DataExt = base.GetDataValue<int>(dr, "DataExt", 0);
                StartTime = base.GetDataValue<int>(dr, "StartTime");
                EndTime = base.GetDataValue<int>(dr, "EndTime", 0);

                DeleteAllow = (EndTime == 0);

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BAGRequestURL.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        #region ==================== Dữ liệu mở rộng ===============================
        public List<BAGRequestURLDataExt> DataExtList(bool state = true)
        {
            List<BAGRequestURLDataExt> dataExtList = new List<BAGRequestURLDataExt>();
            return dataExtList;
        }

        public bool DataExtGet(EnumBAGMapImgType dataExt)
        {
            return ((DataExt & (int)Math.Pow(2, (int)dataExt)) > 0);
        }

        public void DataExtSet(EnumBAGMapImgType dataExt, bool status)
        {
            // Bít đã được bật
            if (((DataExt >> (int)dataExt) & 1) > 0)
            {
                if (status == false)
                    DataExt = DataExt - (int)Math.Pow(2, (int)dataExt);
            }
            // Bít chưa bật
            else
            {
                if (status == true)
                    DataExt = DataExt + (int)Math.Pow(2, (int)dataExt);
            }
        }
        #endregion
    }

    public class BAGRequestURLDataExt
    {
        public EnumBAGMapImgType DataExt { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }
    }
}