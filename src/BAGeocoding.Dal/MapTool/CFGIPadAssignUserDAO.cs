using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.MapTool;

using BAGeocoding.Utility;
using BAGeocoding.Entity.MapTool.Config;

namespace BAGeocoding.Dal.MapTool
{
    /// <summary>
    /// Quản lý truy xuất thông tin gán thiết bị iPad
    /// </summary>
    public class CFGIPadAssignUserDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPTOOL);

        /// <summary>
        /// Lấy danh sách gán iPad
        /// </summary>
        public static List<CFGIPadAssignUser> GetByIPad(MCLIPad ipadInfo)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[CFG.IPadAssignUser_GetByIPad]",
                                new SqlParameter("@DeviceID", ipadInfo.DeviceID));
                if (dt == null)
                    return null;
                List<CFGIPadAssignUser> dataList = new List<CFGIPadAssignUser>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CFGIPadAssignUser dataInfo = new CFGIPadAssignUser();
                    if (dataInfo.FromDataRow(dt.Rows[i]) == false)
                        return null;
                    dataList.Add(dataInfo);
                }
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("CFGIPadAssignUserDAO.GetByIPad, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Bàn giao iPad
        /// </summary>
        public static bool Assign(CFGIPadAssignUser assignInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CFG.IPadAssignUser_Assign]",
                            new SqlParameter("@DeviceID", assignInfo.IPadInfo.DeviceID),
                            new SqlParameter("@UserID", assignInfo.UserID),
                            new SqlParameter("@Description", assignInfo.AssignerInfo.Description),

                            new SqlParameter("@EditorID", assignInfo.AssignerInfo.EditorID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("CFGIPadAssignUserDAO.Assign, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Trả lại iPad
        /// </summary>
        public static bool Abrogate(CFGIPadAssignUser assignInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CFG.IPadAssignUser_Abrogate]",
                            new SqlParameter("@ActionKeyID", assignInfo.ActionKeyID),
                            new SqlParameter("@Description", assignInfo.AbrogaterInfo.Description),

                            new SqlParameter("@EditorID", assignInfo.AbrogaterInfo.EditorID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("CFGIPadAssignUserDAO.Abrogate, ex: " + ex.ToString());
                return false;
            }
        }
    }
}