using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity;
using BAGeocoding.Entity.MapTool.Base;
using BAGeocoding.Entity.MapTool.Plan;

using BAGeocoding.Utility;
using BAGeocoding.Entity.MapTool;

namespace BAGeocoding.Dal.MapTool
{
    /// <summary>
    /// Quản lý truy xuất thông tin track logs di chuyển của người dùng
    /// </summary>
    public class WRKTrackMoveDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPTOOL);

        /// <summary>
        /// Lấy danh sách track logs di chuyển của người dùng
        /// </summary>
        public static List<WRKTrackMove> GetByPlan(int planID)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[WRK.TrackMove_GetByPlan]",
                            new SqlParameter("@PlanID", planID));
                if (dt == null)
                    return null;
                List<WRKTrackMove> dataList = new List<WRKTrackMove>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    WRKTrackMove dataInfo = new WRKTrackMove();
                    if (dataInfo.FromDataRow(dt.Rows[i]) == false)
                        return null;
                    dataList.Add(dataInfo);
                }
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKTrackMoveDAO.GetByUser, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách track logs di chuyển của người dùng
        /// </summary>
        public static List<WRKTrackMove> GetByUser(ConditionBase condition)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[WRK.TrackMove_GetByUser]",
                            new SqlParameter("@UserID", condition.UserID),
                            new SqlParameter("@StartTime", condition.StartTime),
                            new SqlParameter("@EndTime", condition.EndTime));
                if (dt == null)
                    return null;
                List<WRKTrackMove> dataList = new List<WRKTrackMove>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    WRKTrackMove dataInfo = new WRKTrackMove();
                    if (dataInfo.FromDataRow(dt.Rows[i]) == false)
                        return null;
                    dataInfo.PointGenerate();
                    dataList.Add(dataInfo);
                }
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKTrackMoveDAO.GetByUser, ex: " + ex.ToString());
                return null;
            }
        }


        /// <summary>
        /// Lấy danh sách người dùng có lộ trình di chuyển
        /// </summary>
        public static List<USRUser> GetUserByPlan(int planID)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[WRK.TrackMove_GetUserByPlan]",
                            new SqlParameter("@PlanID", planID));
                if (dt == null)
                    return null;
                List<USRUser> dataList = new List<USRUser>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    USRUser dataInfo = new USRUser();
                    if (dataInfo.FromDataRow(dt.Rows[i]) == false)
                        return null;
                    dataList.Add(dataInfo);
                }
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKTrackMoveDAO.GetUserByPlan, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy lộ trình di chuyển của người dùng theo kế hoạch
        /// </summary>
        public static List<WRKTrackMove> GetByPlanUser(int planID, int userID)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[WRK.TrackMove_GetByPlanUser]",
                            new SqlParameter("@PlanID", planID),
                            new SqlParameter("@UserID", userID));
                if (dt == null)
                    return null;
                List<WRKTrackMove> dataList = new List<WRKTrackMove>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    WRKTrackMove dataInfo = new WRKTrackMove();
                    if (dataInfo.FromDataRow(dt.Rows[i]) == false)
                        return null;
                    dataInfo.PointGenerate();
                    dataList.Add(dataInfo);
                }
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKTrackMoveDAO.GetByPlanUser, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        public static bool Create(WRKTrackMove trackMove)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.TrackMove_Create]",
                            new SqlParameter("@PlanID", trackMove.PlanID),
                            new SqlParameter("@UserID", trackMove.UserID),
                            new SqlParameter("@DayIndex", trackMove.DayIndex),
                            new SqlParameter("@StartTime", trackMove.StartTime),
                            new SqlParameter("@EndTime", trackMove.EndTime),
                            new SqlParameter("@Distance", trackMove.Distance),
                            new SqlParameter("@CoordsStr", MapHelper.PolylineAlgorithmEncode(trackMove.PointList)));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKTrackMoveDAO.Create, ex: " + ex.ToString());
                return false;
            }
        }
    }
}