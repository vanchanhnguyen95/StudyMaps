using System;
using System.Data.SqlClient;

//using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;

namespace BAGeocoding.Dal.SegmentEdit
{
    /// <summary>
    /// Quản lý truy xuất tọa độ đoạn đường
    /// </summary>
    public class EDTSegmentPointDAO : SQLHelper
    {
        //protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING);

        /// <summary>
        /// Thêm mới tọa độ đối tượng
        /// </summary>
        public static bool Add(int segmentID, BAGPoint point, int indexID)
        {
            try
            {
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.SegmentPoint_Add]",
                //            new SqlParameter("@SegmentID", segmentID),
                //            new SqlParameter("@Lng", point.Lng),
                //            new SqlParameter("@Lat", point.Lat),
                //            new SqlParameter("@IndexID", indexID));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("EDTSegmentPointDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Thêm mới tọa độ đối tượng
        /// </summary>
        public static bool AddStr(BAGSegment segment)
        {
            try
            {
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[EDT.SegmentPoint_AddStr]",
                //            new SqlParameter("@SegmentID", segment.SegmentID),
                //            new SqlParameter("@LngStr", segment.LngStr.Replace('@', ',')),
                //            new SqlParameter("@LatStr", segment.LatStr.Replace('@', ',')));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("EDTSegmentPointDAO.AddStr, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
