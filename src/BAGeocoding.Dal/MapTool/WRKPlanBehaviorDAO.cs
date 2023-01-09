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
    /// Quản lý truy xuất trạng thái kế hoạch
    /// </summary>
    public class WRKPlanBehaviorDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPTOOL);

        /// <summary>
        /// Lấy danh sách trạng thái của kế hoạch
        /// </summary>
        public static List<WRKPlanBehavior> GetByPlan(int planID)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[WRK.PlanBehavior_GetByPlan]",
                            new SqlParameter("@PlanID", planID));
                if (dt == null)
                    return null;
                List<WRKPlanBehavior> dataList = new List<WRKPlanBehavior>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    WRKPlanBehavior dataInfo = new WRKPlanBehavior();
                    if (dataInfo.FromDataRow(dt.Rows[i]) == false)
                        return null;
                    dataList.Add(dataInfo);
                }
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanBehaviorDAO.GetByPlan, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Cập nhật trạng thái cho kế hoạch
        /// </summary>
        public static bool Create(WRKPlanBehavior planBehavior)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.PlanBehavior_Create]",
                            new SqlParameter("@PlanID", planBehavior.PlanID),
                            new SqlParameter("@BehaviorID", planBehavior.BehaviorID),
                            new SqlParameter("@Description", planBehavior.Description),

                            new SqlParameter("@UserID", planBehavior.EditorID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanBehaviorDAO.Create, ex: " + ex.ToString());
                return false;
            }
        }
    }
}