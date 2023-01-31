using System;
using System.Data.SqlClient;

//using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;

namespace BAGeocoding.Dal.MapObj
{
    /// <summary>
    /// Quản lý truy xuất tọa độ đoạn đường
    /// </summary>
    public class PlacePointDAO : SQLHelper
    {
        //protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING);

        /// <summary>
        /// Thêm mới tọa độ đối tượng
        /// </summary>
        public static bool Add(int placeID, BAGPoint point, int indexID)
        {
            try
            {
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.PlacePoint_Add]",
                //            new SqlParameter("@PlaceID", placeID),
                //            new SqlParameter("@Lng", point.Lng),
                //            new SqlParameter("@Lat", point.Lat),
                //            new SqlParameter("@IndexID", indexID));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PlacePointDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Thêm mới tọa độ đối tượng
        /// </summary>
        public static bool AddStr(BAGPlace placeData)
        {
            try
            {
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.PlacePoint_AddStr]",
                //            new SqlParameter("@PlaceID", placeData.PlaceID),
                //            new SqlParameter("@LngStr", placeData.LngStr),
                //            new SqlParameter("@LatStr", placeData.LatStr));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PlacePointDAO.AddStr, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Thêm mới tọa độ đối tượng
        /// </summary>
        public static bool AddStr(int placeID, BAGPlace placeData)
        {
            try
            {
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.PlacePoint_AddStr]",
                //            new SqlParameter("@PlaceID", placeID),
                //            new SqlParameter("@LngStr", placeData.LngStr),
                //            new SqlParameter("@LatStr", placeData.LatStr));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PlacePointDAO.AddStr, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
