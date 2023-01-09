using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;

namespace BAGeocoding.Dal.MapObj
{
    /// <summary>
    /// Quản lý truy xuất tên đường
    /// </summary>
    public class RoadNameDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING);

        /// <summary>
        /// Lấy khu vực theo tỉnh
        /// </summary>
        public static List<BAGRoadName> GetForCreate(int provinceID)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[DTS.RoadName_GetForCreate]", 
                                new SqlParameter("@ProvinceID", provinceID));

                if (dt == null)
                    return null;
                else if (dt.Rows.Count == 0)
                    return null;

                List<BAGRoadName> roadList = new List<BAGRoadName>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    BAGRoadName roadInfo = new BAGRoadName();
                    if (roadInfo.FromDataCreate(dt.Rows[i]) == false)
                        return null;
                    roadList.Add(roadInfo);
                }
                return roadList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RoadNameDAO.GetForCreate, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy khu vực theo tỉnh
        /// </summary>
        public static List<BAGRoadName> GetByProvince(int provinceID)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[DTS.RoadName_GetByProvince]",
                                new SqlParameter("@ProvinceID", provinceID));

                if (dt == null)
                    return null;
                else if (dt.Rows.Count == 0)
                    return null;

                List<BAGRoadName> roadList = new List<BAGRoadName>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    BAGRoadName roadInfo = new BAGRoadName();
                    if (roadInfo.FromDataRow(dt.Rows[i]) == false)
                        return null;
                    roadList.Add(roadInfo);
                }
                return roadList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RoadNameDAO.GetByProvince, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới thông tin
        /// </summary>
        public static bool Add(BAGRoadName poiInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.RoadName_Create]",
                            new SqlParameter("@ProvinceID", poiInfo.ProvinceID),
                            new SqlParameter("@RoadName", poiInfo.RoadName),
                            new SqlParameter("@NameExt", poiInfo.NameExt),
                            new SqlParameter("@Lng", poiInfo.Coords.Lng),
                            new SqlParameter("@Lat", poiInfo.Coords.Lat));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RoadNameDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }
        
        /// <summary>
        /// Hủy tất cả theo tỉnh
        /// </summary>
        public static bool Clear(short provinceID)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.RoadName_Clear]",
                            new SqlParameter("@ProvinceID", provinceID));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RoadNameDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
