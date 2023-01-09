using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.MapTool;
using BAGeocoding.Entity.Utility;

using BAGeocoding.Utility;
using BAGeocoding.Entity.Enum.MapTool;
using BAGeocoding.Entity.MapTool.Logs;
using BAGeocoding.Entity.Enum;

namespace BAGeocoding.Dal.MapTool
{
    /// <summary>
    /// Quản lý truy xuất thông tin grid của kế hoạch
    /// </summary>
    public class WRKPlanGridDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPTOOL);

        /// <summary>
        /// Lấy danh sách lưới của kế hoạch
        /// </summary>
        public static List<WRKPlanGrid> GetByPlan(int planID, bool editAble)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[WRK.PlanGrid_GetByPlan]",
                            new SqlParameter("@PlanID", planID),
                            new SqlParameter("@EditAble", editAble));
                if (dt == null)
                    return null;
                List<WRKPlanGrid> dataList = new List<WRKPlanGrid>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    WRKPlanGrid dataInfo = new WRKPlanGrid();
                    if (dataInfo.FromDataRow(dt.Rows[i]) == false)
                        return null;
                    dataList.Add(dataInfo);
                }
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanGridDAO.GetByPlan, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách lưới của kế hoạch
        /// </summary>
        public static List<WRKPlanGrid> GetForExport(int planID, byte gridState)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[WRK.PlanGrid_GetForExport]",
                                new SqlParameter("@PlanID", planID),
                                new SqlParameter("@GridState", gridState),

                                new SqlParameter("@EnumPoint", EnumBAGObjectType.Point),
                                new SqlParameter("@EnumPolyline", EnumBAGObjectType.Polyline),

                                new SqlParameter("@ActionEdit", EnumMTLUserGridDataExt.Edit),

                                new SqlParameter("@StateProcess", EnumMTLPlanGridState.Processing),
                                new SqlParameter("@StatePending", EnumMTLPlanGridState.Pending));
                if (dt == null)
                    return null;
                List<WRKPlanGrid> dataList = new List<WRKPlanGrid>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    WRKPlanGrid dataInfo = new WRKPlanGrid();
                    if (dataInfo.FromDataExport(dt.Rows[i]) == false)
                        return null;
                    dataList.Add(dataInfo);
                }
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanGridDAO.GetForExport, ex: " + ex.ToString());
                return null;
            }
        }

        public static bool LogForExport(int planID, EnumBAGObjectType objectType, string gridStr, int userID)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.PlanGrid_LogForExport]",
                            new SqlParameter("@PlanID", planID),
                            new SqlParameter("@GridStr", gridStr),

                            new SqlParameter("@ObjetTypeID", objectType),

                            new SqlParameter("@UserID", userID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanGridDAO.LogForExport, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Lấy danh sách lưới của kế hoạch
        /// </summary>
        public static List<WRKPlanGrid> GetForDoing(int planID)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[WRK.PlanGrid_GetForDoing]",
                                new SqlParameter("@PlanID", planID),

                                new SqlParameter("@ActionEdit", EnumMTLUserGridDataExt.Edit),
                                new SqlParameter("@StatePending", EnumMTLPlanGridState.Pending));
                if (dt == null)
                    return null;
                List<WRKPlanGrid> dataList = new List<WRKPlanGrid>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    WRKPlanGrid dataInfo = new WRKPlanGrid();
                    if (dataInfo.FromDataExt(dt.Rows[i]) == false)
                        return null;
                    dataList.Add(dataInfo);
                }
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanGridDAO.GetForDoing, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách lưới của kế hoạch
        /// </summary>
        public static List<WRKPlanGrid> GetGridState(int planID)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[WRK.PlanGrid_GetGridState]",
                                new SqlParameter("@PlanID", planID),

                                new SqlParameter("@ActionEdit", EnumMTLUserGridDataExt.Edit));
                if (dt == null)
                    return null;
                List<WRKPlanGrid> dataList = new List<WRKPlanGrid>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    WRKPlanGrid dataInfo = new WRKPlanGrid();
                    if (dataInfo.FromDataExt(dt.Rows[i]) == false)
                        return null;
                    dataList.Add(dataInfo);
                }
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanGridDAO.GetGridState, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Dữ liệu của grid
        /// </summary>
        public static List<WRKPlanGrid> GetGridData(int planID, byte viewType)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[WRK.PlanGrid_GetGridData]",
                                new SqlParameter("@PlanID", planID),

                                new SqlParameter("@StateGenerate", EnumMTLPlanGridState.Generate),
                                new SqlParameter("@StateProcessing", EnumMTLPlanGridState.Processing),
                                new SqlParameter("@StatePending", EnumMTLPlanGridState.Pending),
                                new SqlParameter("@StateApproved", EnumMTLPlanGridState.Approved),

                                new SqlParameter("@ActionEdit", EnumMTLUserGridDataExt.Edit));
                if (dt == null)
                    return null;
                List<WRKPlanGrid> dataList = new List<WRKPlanGrid>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    WRKPlanGrid dataInfo = new WRKPlanGrid();
                    if (dataInfo.FromDataData(dt.Rows[i]) == false)
                        return null;
                    else if (viewType == 1 && dataInfo.DataError() == true)
                        continue;
                    else if (viewType == 2 && dataInfo.DataError() == false)
                        continue;
                    dataList.Add(dataInfo);
                }
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanGridDAO.GetGridState, ex: " + ex.ToString());
                return null;
            }
        }
        
        /// <summary>
        /// Lấy dữ liệu của grid để Duyệt - Trả lại
        /// </summary>
        public static List<WRKPlanGrid> GetGridAppGvb(int planID)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[WRK.PlanGrid_GetGridAppGvb]",
                                new SqlParameter("@PlanID", planID),

                                new SqlParameter("@StateDoing", EnumMTLPlanGridState.Doing),

                                new SqlParameter("@ApvNeedInfo", EnumMTLApprovedState.NeedInfo),
                                new SqlParameter("@ApvGiveback", EnumMTLApprovedState.Giveback),

                                new SqlParameter("@ActionEdit", EnumMTLUserGridDataExt.Edit));
                if (dt == null)
                    return null;
                List<WRKPlanGrid> dataList = new List<WRKPlanGrid>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    WRKPlanGrid dataInfo = new WRKPlanGrid();
                    if (dataInfo.FromDataAppGvb(dt.Rows[i]) == false)
                        return null;
                    dataList.Add(dataInfo);
                }
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanGridDAO.GetGridAppGvb, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy lịch sử thay đổi trạng thái grid của kế hoạch
        /// </summary>
        public static List<WRKPlanGridState> StateHistory(WRKPlanGrid planGrid)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[WRK.PlanGrid_StateHistory]",
                                new SqlParameter("@PlanID", planGrid.PlanID),
                                new SqlParameter("@GridID", planGrid.GridInfo.GridID));

                if (dt == null)
                    return null;
                List<WRKPlanGridState> dataList = new List<WRKPlanGridState>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    WRKPlanGridState dataInfo = new WRKPlanGridState();
                    if (dataInfo.FromDataRow(dt.Rows[i]) == false)
                        return null;
                    dataList.Add(dataInfo);
                }
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanGridDAO.StateHistory, ex: " + ex.ToString());
                return null;
            }
        }


        /// <summary>
        /// Kiểm tra commit dữ liệu
        /// </summary>
        public static bool CheckCommit(WRKPlanGridSync planInfo, ref byte errorCode)
        {
            try
            {
                LogFile.WriteProcess(string.Format("CheckCommit(UserID: {0}, PlanID: {1}, GridID: {2})", planInfo.UserID, planInfo.PlanID, planInfo.GridID));
                SqlParameter prErrorCode = new SqlParameter("@ErrorCode", errorCode);
                prErrorCode.Direction = ParameterDirection.Output;

                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.PlanGrid_CheckCommit]",
                            new SqlParameter("@UserID", planInfo.UserID),
                            new SqlParameter("@PlanID", planInfo.PlanID),
                            new SqlParameter("@GridID", planInfo.GridID),

                            new SqlParameter("@Deployment", EnumMTLPlanBehavior.Deployment),
                            new SqlParameter("@Processing", EnumMTLPlanGridState.Processing),

                            prErrorCode);

                errorCode = Convert.ToByte(prErrorCode.Value);

                return errorCode == 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanGridDAO.CheckCommit, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Gán lưới cho kế hoạch
        /// </summary>
        public static bool Create(WRKPlanGrid planInfo, string gridStr, ref byte errorCode)
        {
            try
            {
                SqlParameter prErrorCode = new SqlParameter("@ErrorCode", errorCode);
                prErrorCode.Direction = ParameterDirection.Output;

                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.PlanGrid_Create]",
                            new SqlParameter("@PlanID", planInfo.PlanID),
                            new SqlParameter("@GridStr", gridStr),

                            new SqlParameter("@UserID", planInfo.AssignerID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()),

                            prErrorCode);

                errorCode = Convert.ToByte(prErrorCode.Value);

                return errorCode == 0 && exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanGridDAO.Create, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Hủy lưới kế hoạch
        /// </summary>
        public static bool Abort(WRKPlanGrid planInfo, string gridStr, ref byte errorCode)
        {
            try
            {
                SqlParameter prErrorCode = new SqlParameter("@ErrorCode", errorCode);
                prErrorCode.Direction = ParameterDirection.Output;

                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.PlanGrid_Abort]",
                            new SqlParameter("@PlanID", planInfo.PlanID),
                            new SqlParameter("@GridStr", gridStr),

                            new SqlParameter("@StateGenerate", EnumMTLPlanGridState.Generate),

                            new SqlParameter("@UserID", planInfo.AbrogaterID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()),

                            prErrorCode);

                errorCode = Convert.ToByte(prErrorCode.Value);

                return errorCode == 0 && exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanGridDAO.Abort, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Chuyển trạng thái tiến hành duyệt
        /// </summary>
        public static bool Doing(WRKPlan planInfo, string gridStr, int userID)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.PlanGrid_Doing]",
                            new SqlParameter("@PlanID", planInfo.PlanID),
                            new SqlParameter("@GridStr", gridStr),

                            new SqlParameter("@StateGenerate", EnumMTLPlanGridState.Generate),
                            new SqlParameter("@StateDoing", EnumMTLPlanGridState.Doing),

                            new SqlParameter("@UserID", userID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanGridDAO.Doing, ex: " + ex.ToString());
                return false;
            }
        }
    }
}