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
    /// Quản lý truy xuất thông POI
    /// </summary>
    public class PointOfInterestDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING);

        /// <summary>
        /// Lấy khu vực theo tỉnh
        /// </summary>
        public static List<BAGPointOfInterest> GetByProvince(int provinceID)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[DTS.PointOfInterest_GetByProvince]", 
                                new SqlParameter("@ProvinceID", provinceID));

                if (dt == null)
                    return null;
                else if (dt.Rows.Count == 0)
                    return null;

                List<BAGPointOfInterest> poiList = new List<BAGPointOfInterest>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    BAGPointOfInterest poiInfo = new BAGPointOfInterest();
                    if (poiInfo.FromDataRow(dt.Rows[i]) == false)
                        return null;
                    poiList.Add(poiInfo);
                }
                return poiList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PointOfInterestDAO.GetByProvince, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới thông tin khu vực
        /// </summary>
        public static bool Add(BAGPointOfInterest poiInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.PointOfInterest_Add]",
                            new SqlParameter("@PoiID", poiInfo.PoiID),
                            new SqlParameter("@ProvinceID", poiInfo.ProvinceID),
                            new SqlParameter("@KindID", poiInfo.KindID),
                            new SqlParameter("@Name", poiInfo.Name),
                            new SqlParameter("@House", poiInfo.House),
                            new SqlParameter("@Road", poiInfo.Road),
                            new SqlParameter("@Address", poiInfo.Address),
                            new SqlParameter("@Tel", poiInfo.Tel),
                            new SqlParameter("@Anchor", poiInfo.Anchor),
                            new SqlParameter("@Info", poiInfo.Info),
                            new SqlParameter("@Note", poiInfo.Note),
                            new SqlParameter("@ShortKey", poiInfo.ShortKey),
                            new SqlParameter("@Lng", poiInfo.Coords.Lng),
                            new SqlParameter("@Lat", poiInfo.Coords.Lat));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PointOfInterestDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }
        
        /// <summary>
        /// Hủy tất cả khu vực theo tỉnh
        /// </summary>
        public static bool Clear(short provinceID)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.PointOfInterest_Clear]",
                            new SqlParameter("@ProvinceID", provinceID));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PointOfInterestDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
