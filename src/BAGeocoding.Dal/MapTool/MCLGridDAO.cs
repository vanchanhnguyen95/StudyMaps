using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.MapTool;

using BAGeocoding.Utility;

namespace BAGeocoding.Dal.MapTool
{
    /// <summary>
    /// Quản lý truy xuất thông tin lưới
    /// </summary>
    public class MCLGridDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPTOOL);

        /// <summary>
        /// Lấy danh sách lưới theo tỉnh
        /// </summary>
        public static List<MCLGrid> GetByProvince(byte provinceID)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[CTL.Grid_GetByProvince]",
                            new SqlParameter("@ProvinceID", provinceID));
                if (dt == null)
                    return null;
                List<MCLGrid> gridList = new List<MCLGrid>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MCLGrid gridInfo = new MCLGrid();
                    if (gridInfo.FromDataRow(dt.Rows[i]) == false)
                        return null;
                    gridList.Add(gridInfo);
                }
                return gridList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("MCLGridDAO.GetByProvince, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới thông tin lưới
        /// </summary>
        public static bool Add(MCLGrid gridInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.Grid_Add]",
                            new SqlParameter("@GridID", gridInfo.GridID),
                            new SqlParameter("@DistrictID", gridInfo.DistrictID),
                            new SqlParameter("@Name", gridInfo.Name),
                            new SqlParameter("@CoordsEncrypt", gridInfo.CoordsEncrypt),
                            new SqlParameter("@CoordsOrignal", gridInfo.CoordsOrignal),

                            new SqlParameter("@ProvinceStr", gridInfo.ProvinceStr));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("MCLGridDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Hủy thông tin lưới
        /// </summary>
        public static bool Clear()
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.Grid_Clear]", null);

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("MCLGridDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}