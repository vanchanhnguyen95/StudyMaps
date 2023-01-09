using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.Enum.MapTool;
using BAGeocoding.Entity.MapTool;
using BAGeocoding.Entity.MapTool.Data;

using BAGeocoding.Utility;
using System.Collections;

namespace BAGeocoding.Dal.MapTool
{
    /// <summary>
    /// Quản lý truy xuất thông tin gán grid cho người dùng
    /// </summary>
    public class WRKUserGridDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPTOOL);

        /// <summary>
        /// Lấy danh sách lưới gán cho người dùng
        /// </summary>
        public static List<WRKUserGrid> GetByUser(WRKUserGrid userGrid, byte actionType, bool editAble)
        {
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(sqlDB, "[WRK.UserGrid_GetByUser]",
                            new SqlParameter("@PlanID", userGrid.PlanID),
                            new SqlParameter("@UserID", userGrid.UserID),

                            new SqlParameter("@EnumEdit", EnumMTLUserGridDataExt.Edit),

                            new SqlParameter("@EditAble", editAble));
                if (ds == null)
                    return null;
                else if (ds.Tables.Count < 2)
                    return null;

                int tableIndex = 0;
                List<WRKUserGrid> dataList = new List<WRKUserGrid>();
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    WRKUserGrid dataInfo = new WRKUserGrid();
                    if (dataInfo.FromDataRow(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    dataList.Add(dataInfo);
                }
                tableIndex += 1;
                if (actionType == 0)
                    return dataList;

                Hashtable htGridExt = new Hashtable();
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    WRKUserGrid dataExt = new WRKUserGrid();
                    if (dataExt.FromDataExt(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    else if (dataExt.UserID == userGrid.UserID)
                        continue;
                    else if (dataExt.DataExtGet((EnumMTLUserGridDataExt)actionType) == false)
                        continue;
                    else if (htGridExt.ContainsKey(dataExt.GridInfo.GridID) == false)
                        htGridExt.Add(dataExt.GridInfo.GridID, null);
                }
                for (int i = dataList.Count - 1; i > -1; i--)
                {
                    if (htGridExt.ContainsKey(dataList[i].GridInfo.GridID) == true)
                        dataList.RemoveAt(i);
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
        /// Lấy dữ liệu gán tài khoản cho grid
        /// </summary>
        public static List<DTSGridUser> GetByGrid(int gridID, int planID, bool viewHistory)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[WRK.UserGrid_GetByGrid]",
                            new SqlParameter("@GridID", gridID),
                            new SqlParameter("@PlanID", planID),

                            new SqlParameter("@GetHistory", viewHistory));
                if (dt == null)
                    return null;
                List<DTSGridUser> dataList = new List<DTSGridUser>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DTSGridUser dataInfo = new DTSGridUser();
                    if (dataInfo.FromDataRow(dt.Rows[i]) == false)
                        return null;
                    dataList.Add(dataInfo);
                }
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanUserDAO.GetByGrid, ex: " + ex.ToString());
                return null;
            }
        }
        
        /// <summary>
        /// Lấy trạng thái grid gán cho người dùng theo kế hoạch
        /// </summary>
        public static List<WRKPlanGrid> GetGridState(int userID, int planID)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[WRK.UserGrid_GetGridState]",
                                new SqlParameter("@UserID", userID),
                                new SqlParameter("@PlanID", planID));
                if (dt == null)
                    return null;
                List<WRKPlanGrid> dataList = new List<WRKPlanGrid>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    WRKPlanGrid dataInfo = new WRKPlanGrid();
                    if (dataInfo.FromDataState(dt.Rows[i]) == false)
                        return null;
                    dataList.Add(dataInfo);
                }
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKUserGridDAO.GetGridState, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gán lưới cho người dùng
        /// </summary>
        public static bool Create(WRKUserGrid userGrid, string gridStr, string dataStr, ref byte errorCode, ref string errorMsg)
        {
            try
            {
                SqlParameter prErrorCode = new SqlParameter("@ErrorCode", errorCode);
                prErrorCode.Direction = ParameterDirection.Output;

                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[WRK.UserGrid_Create]",
                            new SqlParameter("@PlanID", userGrid.PlanID),
                            new SqlParameter("@UserID", userGrid.UserID),
                            new SqlParameter("@GridStr", gridStr),
                            new SqlParameter("@DataStr", dataStr),

                            new SqlParameter("@EnumGenerate", EnumMTLPlanGridState.Generate),
                            new SqlParameter("@EnumApproved", EnumMTLPlanGridState.Approved),

                            new SqlParameter("@ActionEdit", EnumMTLUserGridDataExt.Edit),

                            new SqlParameter("@ActorID", userGrid.AssignerID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()),
                            
                            prErrorCode);

                errorCode = Convert.ToByte(prErrorCode.Value);
                if (errorCode > 0)
                {
                    errorMsg = string.Empty;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        MCLGrid gridInfo = new MCLGrid();
                        if (gridInfo.FromDataBase(dt.Rows[i]) == false)
                            return false;
                        else if (errorMsg.Length > 0)
                            errorMsg += ",";
                        errorMsg += string.Format("{0}", gridInfo.GridID);
                    }
                }

                return errorCode == 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanUserDAO.Create, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Hủy lưới người dùng
        /// </summary>
        public static bool Abort(WRKUserGrid userGrid, string gridStr, ref byte errorCode, ref string errorMsg)
        {
            try
            {
                SqlParameter prErrorCode = new SqlParameter("@ErrorCode", errorCode);
                prErrorCode.Direction = ParameterDirection.Output;

                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[WRK.UserGrid_Abort]",
                            new SqlParameter("@PlanID", userGrid.PlanID),
                            new SqlParameter("@UserID", userGrid.UserID),
                            new SqlParameter("@GridStr", gridStr),

                            new SqlParameter("@EnumGenerate", EnumMTLPlanGridState.Generate),
                            new SqlParameter("@EnumApproved", EnumMTLPlanGridState.Approved),

                            new SqlParameter("@ActionEdit", EnumMTLUserGridDataExt.Edit),

                            new SqlParameter("@ActorID", userGrid.AbrogaterID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()),

                            prErrorCode);

                errorCode = Convert.ToByte(prErrorCode.Value);
                if (errorCode > 0)
                {
                    errorMsg = string.Empty;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        MCLGrid gridInfo = new MCLGrid();
                        if (gridInfo.FromDataBase(dt.Rows[i]) == false)
                            return false;
                        else if (errorMsg.Length > 0)
                            errorMsg += ",";
                        errorMsg += string.Format("{0}", gridInfo.GridID);
                    }
                }

                return errorCode == 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanUserDAO.Abort, ex: " + ex.ToString());
                return false;
            }
        }
        
        /// <summary>
        /// Báo trạng thái grid
        /// </summary>
        public static bool State(WRKUserGrid userGrid, bool isManager, ref byte errorCode)
        {
            try
            {
                SqlParameter prErrorCode = new SqlParameter("@ErrorCode", errorCode);
                prErrorCode.Direction = ParameterDirection.Output;

                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.UserGrid_State]",
                            new SqlParameter("@PlanID", userGrid.PlanID),
                            new SqlParameter("@UserID", userGrid.UserID),
                            new SqlParameter("@GridID", userGrid.GridInfo.GridID),
                            new SqlParameter("@PGState", userGrid.PGState),
                            new SqlParameter("@IsManager", isManager),

                            new SqlParameter("@EnumEdit", EnumMTLUserGridDataExt.Edit),

                            new SqlParameter("@ActorID", userGrid.AssignerID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()),

                            prErrorCode);

                errorCode = Convert.ToByte(prErrorCode.Value);

                return errorCode == 0 && exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanUserDAO.State, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Trả grid được gán quyền thao tác
        /// </summary>
        public static bool Giveback(WRKUserGrid userGrid, bool isManager, ref byte errorCode)
        {
            try
            {
                SqlParameter prErrorCode = new SqlParameter("@ErrorCode", errorCode);
                prErrorCode.Direction = ParameterDirection.Output;

                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.UserGrid_Giveback]",
                            new SqlParameter("@PlanID", userGrid.PlanID),
                            new SqlParameter("@UserID", userGrid.UserID),
                            new SqlParameter("@GridID", userGrid.GridInfo.GridID),
                            new SqlParameter("@Password", StringUlt.EncryptPassword(userGrid.Password)),

                            new SqlParameter("@EnumEdit", EnumMTLUserGridDataExt.Edit),

                            new SqlParameter("@StateGenerate", EnumMTLPlanGridState.Generate),
                            new SqlParameter("@StateProcess", EnumMTLPlanGridState.Processing),

                            new SqlParameter("@ActorID", userGrid.AbrogaterID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()),

                            prErrorCode);

                errorCode = Convert.ToByte(prErrorCode.Value);

                return errorCode == 0 && exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanUserDAO.Giveback, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Tiến hành Duyệt/Trả lại grid
        /// </summary>
        public static bool AppGvb(int planID, string gridStr, string stateStr, int userID, ref byte errorCode)
        {
            try
            {
                SqlParameter prErrorCode = new SqlParameter("@ErrorCode", errorCode);
                prErrorCode.Direction = ParameterDirection.Output;

                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.UserGrid_AppGvb]",
                            new SqlParameter("@PlanID", planID),
                            new SqlParameter("@GridStr", gridStr),
                            new SqlParameter("@StateStr", stateStr),

                            new SqlParameter("@StateDoing", EnumMTLPlanGridState.Doing),

                            new SqlParameter("@UserID", userID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()),

                            prErrorCode);

                errorCode = Convert.ToByte(prErrorCode.Value);

                return errorCode == 0 && exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanUserDAO.AppGvb, ex: " + ex.ToString());
                return false;
            }
        }


        /// <summary>
        /// Tiến hành khởi tạo lại grid
        /// </summary>
        public static bool Reset(int planID, int gridID, byte state, bool resetPOI, int userID, string password, ref byte errorCode)
        {
            try
            {
                SqlParameter prErrorCode = new SqlParameter("@ErrorCode", errorCode);
                prErrorCode.Direction = ParameterDirection.Output;

                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.UserGrid_Reset]",
                            new SqlParameter("@PlanID", planID),
                            new SqlParameter("@GridID", gridID),
                            new SqlParameter("@ResetPOI", resetPOI),
                            new SqlParameter("@State", state),

                            new SqlParameter("@StateGenerate", EnumMTLPlanGridState.Generate),
                            new SqlParameter("@StatePending", EnumMTLPlanGridState.Pending),
                            new SqlParameter("@StateDoing", EnumMTLPlanGridState.Doing),
                            new SqlParameter("@StateGiveback", EnumMTLPlanGridState.Giveback),
                            new SqlParameter("@StateApproved", EnumMTLPlanGridState.Approved),

                            new SqlParameter("@Password", StringUlt.EncryptPassword(password)),
                            new SqlParameter("@UserID", userID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()),

                            prErrorCode);

                errorCode = Convert.ToByte(prErrorCode.Value);

                return errorCode == 0 && exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanUserDAO.Reset, ex: " + ex.ToString());
                return false;
            }
        }
    }
}