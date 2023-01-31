using System;
using System.Data.SqlClient;

//using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.DataService;

using BAGeocoding.Utility;

namespace BAGeocoding.Dal.MapRoute
{
    /// <summary>
    /// Quản lý truy xuất thông tin lưu lượng sử dụng
    /// </summary>
    public class DTSTrafficDAO : SQLHelper
    {
        //protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPROUTE);

        /// <summary>
        /// Thêm thông tin lưu lượng
        /// </summary>
        public static bool Add(DTSTraffic trafficInfo)
        {
            try
            {
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.Traffic_Add]",
                //            new SqlParameter("@AuthenID", trafficInfo.AuthenID),
                //            new SqlParameter("@UnixTime", DataUtl.GetUnixTime(trafficInfo.DateIndex)),
                //            new SqlParameter("@YearIndex", trafficInfo.DateIndex.Year),
                //            new SqlParameter("@MonthIndex", DataUtl.GetMonthIndex(trafficInfo.DateIndex)),
                //            new SqlParameter("@DayIndex", trafficInfo.DateIndex.Day),
                //            new SqlParameter("@TrafficBA", trafficInfo.TrafficBA),
                //            new SqlParameter("@TrafficGG", trafficInfo.TrafficGG));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("DTSTrafficDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
