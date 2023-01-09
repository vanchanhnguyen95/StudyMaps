using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.Enum.MapTool;
using BAGeocoding.Entity.MapTool;

using BAGeocoding.Utility;

namespace BAGeocoding.Dal.MapTool
{
    /// <summary>
    /// Quản lý truy xuất thông tài khoản
    /// </summary>
    public class USRSessionDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPTOOL);

        /// <summary>
        /// Lấy danh sách các lần đăng nhập
        /// </summary>
        public static List<USRSession> GetByUser(USRSessionCondition condition)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[USR.Session_GetByUser]",
                                new SqlParameter("@UserID", condition.UserID),
                                new SqlParameter("@StartTime", condition.StartTime),
                                new SqlParameter("@EndTime", condition.EndTime));

                if (dt == null)
                    return null;
                List<USRSession> resultList = new List<USRSession>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    USRSession itemInfo = new USRSession();
                    if (itemInfo.FromDataRow(dt.Rows[i]) == false)
                        return null;
                    resultList.Add(itemInfo);
                }
                return resultList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("USRUserDAO.GetByUser, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách các lần đăng nhập
        /// </summary>
        public static List<USRSession> LoginError(USRSessionCondition condition)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[USR.Session_LoginError]",
                                new SqlParameter("@UserID", condition.UserID),
                                new SqlParameter("@StartTime", condition.StartTime),
                                new SqlParameter("@EndTime", condition.EndTime));

                if (dt == null)
                    return null;
                List<USRSession> resultList = new List<USRSession>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    USRSession itemInfo = new USRSession();
                    if (itemInfo.FromDataError(dt.Rows[i]) == false)
                        return null;
                    resultList.Add(itemInfo);
                }
                return resultList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("USRUserDAO.GetByUser, ex: " + ex.ToString());
                return null;
            }
        }
        
        /// <summary>
        /// Lấy danh sách các lần đăng nhập
        /// </summary>
        public static List<USRSession> GetByDevice(USRSessionCondition condition)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[USR.Session_GetByDevice]",
                                new SqlParameter("@DeviceID", condition.DeviceID),
                                new SqlParameter("@StartTime", condition.StartTime),
                                new SqlParameter("@EndTime", condition.EndTime));

                if (dt == null)
                    return null;
                List<USRSession> resultList = new List<USRSession>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    USRSession itemInfo = new USRSession();
                    if (itemInfo.FromDataDevice(dt.Rows[i]) == false)
                        return null;
                    resultList.Add(itemInfo);
                }
                return resultList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("USRUserDAO.GetByDevice, ex: " + ex.ToString());
                return null;
            }
        }
    }
}