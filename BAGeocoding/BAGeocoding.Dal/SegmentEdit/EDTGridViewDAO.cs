using System;
using System.Data.SqlClient;
using BAGeocoding.Utility;

//using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.MapObj;

namespace BAGeocoding.Dal.SegmentEdit
{
    /// <summary>
    /// Quản lý truy xuất thông tin đoạn đường
    /// </summary>
    public class EDTGridViewDAO : SQLHelper
    {
        //protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING);

        public static bool Add(BAGGridView gridView)
        {
            try
            {
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[EDT.GridView_Add]",
                //            new SqlParameter("@GridID", gridView.GridID),
                //            new SqlParameter("@ProvinceID", gridView.ProvinceID),
                //            new SqlParameter("@Name", gridView.Name),
                //            new SqlParameter("@LngStr", gridView.LngStr),
                //            new SqlParameter("@LatStr", gridView.LatStr));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("EDTGridViewDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Hủy tất cả thông tin đoạn đường
        /// </summary>
        public static bool Clear(short provinceID)
        {
            try
            {
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[EDT.GridView_Clear]",
                //            new SqlParameter("@ProvinceID", provinceID));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("EDTGridViewDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
