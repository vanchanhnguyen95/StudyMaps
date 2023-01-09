using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.MapTool;
using BAGeocoding.Entity.RolePermission;

using BAGeocoding.Utility;

namespace BAGeocoding.Dal.MapTool
{
    /// <summary>
    /// Quản lý quyền truy xuất menu chuột phải
    /// </summary>
    public class USRRoleContextMenuDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPTOOL);
        
        /// <summary>
        /// Lấy danh sách quyền của người dùng
        /// </summary>
        public static List<RoleContextMenuData> GetForManager(int userID)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[USR.RoleContextMenu_GetForManager]",
                                new SqlParameter("@UserID", userID));

                if (dt == null)
                    return null;
                List<RoleContextMenuData> resultList = new List<RoleContextMenuData>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    RoleContextMenuData gridInfo = new RoleContextMenuData();
                    if (gridInfo.FromDataRow(dt.Rows[i]) == false)
                        return null;
                    resultList.Add(gridInfo);
                }
                return resultList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("USRRoleContextMenuDAO.GetForManager, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách quyền của người dùng
        /// </summary>
        public static List<RoleContextMenuData> GetByUser(int userID)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[USR.RoleContextMenu_GetByUser]",
                                new SqlParameter("@UserID", userID));

                if (dt == null)
                    return null;
                List<RoleContextMenuData> resultList = new List<RoleContextMenuData>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    RoleContextMenuData gridInfo = new RoleContextMenuData();
                    if (gridInfo.FromDataSimple(dt.Rows[i]) == false)
                        return null;
                    resultList.Add(gridInfo);
                }
                return resultList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("USRRoleContextMenuDAO.GetByUser, ex: " + ex.ToString());
                return null;
            }
        }

        public static bool Create(int userID, RoleContextMenuData menuInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[USR.RoleContextMenu_Create]",
                                new SqlParameter("@UserID", userID),
                                new SqlParameter("@MenuID", menuInfo.MenuID),
                                new SqlParameter("@CMNData", menuInfo.CMNData),

                                new SqlParameter("@ActorID", menuInfo.AssignerID),
                                new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("USRRoleContextMenuDAO.Create, ex: " + ex.ToString());
                return false;
            }
        }
    }
}