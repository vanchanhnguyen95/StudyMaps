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
    /// Quản lý quyền truy xuất menu ribbon
    /// </summary>
    public class USRRolePermissionDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPTOOL);
        
        /// <summary>
        /// Lấy danh sách quyền của người dùng
        /// </summary>
        public static List<RolePermissionData> GetForManager(int userID)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[USR.RolePermission_GetForManager]",
                                new SqlParameter("@UserID", userID));

                if (dt == null)
                    return null;
                List<RolePermissionData> resultList = new List<RolePermissionData>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    RolePermissionData gridInfo = new RolePermissionData();
                    if (gridInfo.FromDataRow(dt.Rows[i]) == false)
                        return null;
                    resultList.Add(gridInfo);
                }
                return resultList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("USRRolePermissionDAO.GetForManager, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách quyền của người dùng
        /// </summary>
        public static List<RolePermissionData> GetByUser(int userID)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[USR.RolePermission_GetByUser]",
                                new SqlParameter("@UserID", userID));

                if (dt == null)
                    return null;
                List<RolePermissionData> resultList = new List<RolePermissionData>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    RolePermissionData gridInfo = new RolePermissionData();
                    if (gridInfo.FromDataSimple(dt.Rows[i]) == false)
                        return null;
                    resultList.Add(gridInfo);
                }
                return resultList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("USRRolePermissionDAO.GetByUser, ex: " + ex.ToString());
                return null;
            }
        }

        public static bool Create(int userID, RolePermissionData permissionInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[USR.RolePermission_Create]",
                                new SqlParameter("@UserID", userID),
                                new SqlParameter("@ItemID", permissionInfo.ItemID),
                                new SqlParameter("@PRMData", permissionInfo.PRMData),

                                new SqlParameter("@ActorID", permissionInfo.AssignerID),
                                new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("USRRolePermissionDAO.Create, ex: " + ex.ToString());
                return false;
            }
        }
    }
}