using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BAGeocoding.Utility;

//using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapObj;

namespace BAGeocoding.Dal.MapObj
{
    /// <summary>
    /// Quản lý truy xuất thông tin tên đường đặc biệt
    /// </summary>
    public class RoadSpecialDAO : SQLHelper
    {
        //protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING);

        /// <summary>
        /// Lấy tất cả danh sách tên đường đặc biệt
        /// </summary>
        public static List<BAGRoadSpecial> GetAll()
        {
            try
            {
                //DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[EXT.RoadSpecial_GetAll]", null);

                //if (dt != null)
                //{
                //    List<BAGRoadSpecial> roadList = new List<BAGRoadSpecial>();
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        BAGRoadSpecial roadItem = new BAGRoadSpecial();
                //        if (roadItem.FromDataRow(dt.Rows[i]) == false)
                //            return null;
                //        roadList.Add(roadItem);
                //    }
                //    return roadList;
                //}
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RoadSpecialDAO.GetAll, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới thông tin tên đường đặc biệt
        /// </summary>
        public static bool Add(BAGRoadSpecial tile)
        {
            try
            {
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[EXT.RoadSpecial_Add]",
                //            new SqlParameter("@RoadName", tile.RoadName),
                //            new SqlParameter("@Description", tile.Description ?? string.Empty),
                //            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RoadSpecialDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Cập nhật thông tin tên đường đặc biệt
        /// </summary>
        public static bool Update(BAGRoadSpecial tile)
        {
            try
            {
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[EXT.RoadSpecial_Update]",
                //            new SqlParameter("@IndexID", tile.IndexID),
                //            new SqlParameter("@RoadName", tile.RoadName),
                //            new SqlParameter("@Description", tile.Description),
                //            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RoadSpecialDAO.Update, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Hủy tên đường đặc biệt
        /// </summary>
        public static bool Delete(int indexID)
        {
            try
            {
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[EXT.RoadSpecial_Delete]",
                //            new SqlParameter("@IndexID", indexID));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RoadSpecialDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
