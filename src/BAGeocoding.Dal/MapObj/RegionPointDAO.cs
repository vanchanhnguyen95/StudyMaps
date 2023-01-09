using System;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;

namespace BAGeocoding.Dal.MapObj
{
    /// <summary>
    /// Quản lý truy xuất tọa độ đối tượng
    /// </summary>
    public class RegionPointDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING);

        /// <summary>
        /// Thêm mới tọa độ đối tượng
        /// </summary>
        public static bool Add(EnumBAGRegionType typeID, int objectID, BAGPoint point, int sortOrder)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.RegionPoint_Add]",
                            new SqlParameter("@TypeID", typeID),
                            new SqlParameter("@ObjectID", objectID),
                            new SqlParameter("@Lng", point.Lng),
                            new SqlParameter("@Lat", point.Lat),
                            new SqlParameter("@SortOrder", sortOrder));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("ProvinceDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Thêm mới tọa độ đối tượng
        /// </summary>
        public static bool Add(EnumBAGRegionType typeID, int objectID, float longtitude, float latitude, int sortOrder)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.RegionPoint_Add]",
                            new SqlParameter("@TypeID", typeID),
                            new SqlParameter("@ObjectID", objectID),
                            new SqlParameter("@Lng", longtitude),
                            new SqlParameter("@Lat", latitude),
                            new SqlParameter("@SortOrder", sortOrder));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("ProvinceDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
