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
    public class PRMVersionAppDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPTOOL);

        /// <summary>
        /// Lấy danh sách tài khoản
        /// </summary>
        public static List<PRMVersionApp> GetAll()
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[PRM.VersionApp_GetAll]", null);

                if (dt == null)
                    return null;
                List<PRMVersionApp> resultList = new List<PRMVersionApp>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PRMVersionApp objectInfo = new PRMVersionApp();
                    if (objectInfo.FromDataSimple(dt.Rows[i]) == false)
                        return null;
                    resultList.Add(objectInfo);
                }
                return resultList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PRMVersionAppDAO.GetAll, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Kiểm tra phiên bản
        /// </summary>
        public static bool Check(EnumMTLAppType typeID, string versionStr)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[PRM.VersionApp_Check]",
                                new SqlParameter("@TypeID", typeID),
                                new SqlParameter("@VersionStr", versionStr));

                if (dt == null)
                    return false;
                else
                    return dt.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PRMVersionAppDAO.Check, ex: " + ex.ToString());
                return false;
            }
        }

        public static bool Create(PRMVersionApp versionInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[PRM.VersionApp_Create]",
                                new SqlParameter("@TypeID", versionInfo.TypeID),
                                new SqlParameter("@VersionStr", versionInfo.VersionStr),
                                new SqlParameter("@Description", versionInfo.Description),

                                new SqlParameter("@UserID", versionInfo.AssignerID),
                                new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PRMVersionAppDAO.Create, ex: " + ex.ToString());
                return false;
            }
        }
    }
}