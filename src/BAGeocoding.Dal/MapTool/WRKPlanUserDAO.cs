using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.MapTool;
using BAGeocoding.Entity.Utility;

using BAGeocoding.Utility;

namespace BAGeocoding.Dal.MapTool
{
    /// <summary>
    /// Quản lý truy xuất thông tin grid của kế hoạch
    /// </summary>
    public class WRKPlanUserDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPTOOL);

        /// <summary>
        /// Lấy danh sách lưới của kế hoạch
        /// </summary>
        public static List<WRKPlanUser> GetByPlan(int planID, bool editAble)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[WRK.PlanUser_GetByPlan]",
                            new SqlParameter("@PlanID", planID),
                            new SqlParameter("@EditAble", editAble));
                if (dt == null)
                    return null;
                List<WRKPlanUser> dataList = new List<WRKPlanUser>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    WRKPlanUser dataInfo = new WRKPlanUser();
                    if (dataInfo.FromDataRow(dt.Rows[i]) == false)
                        return null;
                    dataList.Add(dataInfo);
                }
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanUserDAO.GetByPlan, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách kế hoạch người dùng từng tham gia
        /// </summary>
        public static List<WRKPlanUser> GetByUser(int userID)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[WRK.PlanUser_GetByUser]",
                            new SqlParameter("@UserID", userID));
                if (dt == null)
                    return null;
                List<WRKPlanUser> dataList = new List<WRKPlanUser>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    WRKPlanUser dataInfo = new WRKPlanUser();
                    if (dataInfo.FromDataUser(dt.Rows[i], "WPU") == false)
                        return null;
                    dataInfo.UserInfo.UserID = userID;
                    dataList.Add(dataInfo);
                }
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanUserDAO.GetByUser, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy lịch sử gán người dùng cho kế hoạch
        /// </summary>
        public static List<WRKPlanUser> GetHistory(WRKPlanUser planUser)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[WRK.PlanUser_GetHistory]",
                            new SqlParameter("@PlanID", planUser.PlanInfo.PlanID),
                            new SqlParameter("@UserID", planUser.UserInfo.UserID));
                if (dt == null)
                    return null;
                List<WRKPlanUser> dataList = new List<WRKPlanUser>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    WRKPlanUser dataInfo = new WRKPlanUser();
                    if (dataInfo.FromDataHistory(dt.Rows[i]) == false)
                        return null;
                    dataList.Add(dataInfo);
                }
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanUserDAO.GetHistory, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gán lưới cho kế hoạch
        /// </summary>
        public static bool Create(WRKPlanUser planInfo, string userStr, string dataStr)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.PlanUser_Create]",
                            new SqlParameter("@PlanID", planInfo.PlanID),
                            new SqlParameter("@UserStr", userStr),
                            new SqlParameter("@DataStr", dataStr),

                            new SqlParameter("@UserID", planInfo.AssignerID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanUserDAO.Create, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Hủy lưới kế hoạch
        /// </summary>
        public static bool Abort(WRKPlanUser planInfo, string userStr, ref byte errorCode)
        {
            try
            {
                SqlParameter prErrorCode = new SqlParameter("@ErrorCode", errorCode);
                prErrorCode.Direction = ParameterDirection.Output;

                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.PlanUser_Abort]",
                            new SqlParameter("@PlanID", planInfo.PlanID),
                            new SqlParameter("@UserStr", userStr),

                            new SqlParameter("@UserID", planInfo.AbrogaterID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()),

                            prErrorCode);

                errorCode = Convert.ToByte(prErrorCode.Value);

                return errorCode == 0 && exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanUserDAO.Abort, ex: " + ex.ToString());
                return false;
            }
        }
    }
}