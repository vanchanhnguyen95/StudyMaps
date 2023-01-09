using System;
using System.Data.SqlClient;
using BAGeocoding.Utility;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.MapObj;

namespace BAGeocoding.Dal.MapObj
{
    /// <summary>
    /// Quản lý truy xuất thông tin đoạn đường
    /// </summary>
    public class PolylineDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING);

        /// <summary>
        /// Thêm mới thông tin đoạn đường
        /// </summary>
        public static bool Add(BAGPolyline polyline)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.PolylineInfo_Add]",
                            new SqlParameter("@PolylineID", polyline.PolylineID),
                            new SqlParameter("@NameStr", polyline.Name),
                            new SqlParameter("@LngStr", polyline.LngStr),
                            new SqlParameter("@LatStr", polyline.LatStr));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("SegmentDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
