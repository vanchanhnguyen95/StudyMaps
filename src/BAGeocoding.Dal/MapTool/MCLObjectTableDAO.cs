using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.Enum.MapObject;
using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;
using BAGeocoding.Entity.MapTool;

namespace BAGeocoding.Dal.MapTool
{
    /// <summary>
    /// Quản lý truy xuất thông tin bảng
    /// </summary>
    public class MCLObjectTableDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPTOOL);

        /// <summary>
        /// Lấy tất cả danh sách
        /// </summary>
        public static List<MCLObjectTable> GetAll()
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[CTL.ObjectTable_GetAll]", null);

                // 1. Kiểm tra dữ liệu
                if (dt == null)
                    return null;
                else if (dt.Rows.Count < 1)
                    return null;

                // 2.Lấy dữ liệu
                List<MCLObjectTable> result = new List<MCLObjectTable>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MCLObjectTable dataItem = new MCLObjectTable();
                    if (dataItem.FromDataRow(dt.Rows[i]) == false)
                        return null;
                    result.Add(dataItem);
                }
                return result;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("MCLObjectTableDAO.GetAll, ex: " + ex.ToString());
                return null;
            }
        }
    }
}
