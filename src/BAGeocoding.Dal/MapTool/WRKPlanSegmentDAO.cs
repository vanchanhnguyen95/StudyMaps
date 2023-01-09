using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.Enum.MapTool;
using BAGeocoding.Entity.MapTool;
using BAGeocoding.Entity.MapTool.Plan;

using BAGeocoding.Utility;

namespace BAGeocoding.Dal.MapTool
{
    /// <summary>
    /// Quản lý truy xuất thông tin grid của kế hoạch
    /// </summary>
    public class WRKPlanSegmentDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPTOOL);

        /// <summary>
        /// Lấy danh sách đường của kế hoạch
        /// </summary>
        public static List<WRKPlanSegment> GetByPlan(int planID)
        {
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(sqlDB, "[WRK.PlanSeg_GetByPlan]",
                                new SqlParameter("@PlanID", planID),

                                new SqlParameter("@EnumPolyline", EnumBAGObjectType.Polyline));

                if (ds == null)
                    return null;
                else if (ds.Tables.Count < 2)
                    return null;
                else if (ds.Tables[0].Rows.Count == 0)
                    return null;

                int tableIndex = 0;
                Hashtable htSegment = new Hashtable();
                List<WRKPlanSegment> dataList = new List<WRKPlanSegment>();

                // .1 Lấy danh sách điểm
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    WRKPlanSegment dataInfo = new WRKPlanSegment();
                    if (dataInfo.FromDataRow(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    else if (htSegment.ContainsKey(dataInfo.PLNSegID) == true)
                        continue;
                    htSegment.Add(dataInfo.PLNSegID, dataList.Count);
                    dataInfo.PointGenerate();
                    dataList.Add(dataInfo);
                }
                tableIndex += 1;

                // 2. Cập nhật danh sách ghi chú
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    WRKPlanSegment dataInfo = new WRKPlanSegment();
                    if (dataInfo.FromDataNote(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    else if (htSegment.ContainsKey(dataInfo.PLNSegID) == false)
                        continue;
                    dataList[Convert.ToInt32(htSegment[dataInfo.PLNSegID])].UpdateNote(dataInfo);
                }

                // 3. Trả về kết quả
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanSegmentDAO.GetByPlan, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy dữ liệu để export
        /// </summary>
        public static List<WRKPlanSegment> GetForExport(int planID, WRKPlanOptionExport exportOpt)
        {
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(sqlDB, "[WRK.PlanSeg_GetForExport]",
                                new SqlParameter("@PlanID", planID),
                                new SqlParameter("@GridStr", exportOpt.GridStr),

                                new SqlParameter("@EnumPolyline", EnumBAGObjectType.Polyline));

                if (ds == null)
                    return null;
                else if (ds.Tables.Count < 2)
                    return null;
                else if (ds.Tables[0].Rows.Count == 0)
                    return null;

                int tableIndex = 0;
                Hashtable htSegment = new Hashtable();
                List<WRKPlanSegment> dataList = new List<WRKPlanSegment>();

                // .1 Lấy danh sách điểm
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    WRKPlanSegment dataInfo = new WRKPlanSegment();
                    if (dataInfo.FromDataRow(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    else if (dataInfo.MigrateID > 0)
                    {
                        if (exportOpt.IgnoreBlankData == true && dataInfo.ActionID < 2)
                            continue;
                        else if (exportOpt.IgnoreDeleteByCut == true && dataInfo.ActionGet(EnumMTLObjectAction.DeleteByCut) == true)
                            continue;
                        else if (exportOpt.IgnoreDeleteByJoin == true && dataInfo.ActionGet(EnumMTLObjectAction.DeleteByJoin) == true)
                            continue;
                        else if (htSegment.ContainsKey(dataInfo.PLNSegID) == true)
                            continue;
                    }
                    htSegment.Add(dataInfo.PLNSegID, dataList.Count);
                    dataInfo.PointGenerate();
                    dataList.Add(dataInfo);
                }
                tableIndex += 1;

                // 2. Cập nhật danh sách ghi chú
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    WRKPlanSegment dataInfo = new WRKPlanSegment();
                    if (dataInfo.FromDataNote(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    else if (htSegment.ContainsKey(dataInfo.PLNSegID) == false)
                        continue;
                    dataList[Convert.ToInt32(htSegment[dataInfo.PLNSegID])].UpdateNote(dataInfo);
                }

                // 3. Trả về kết quả
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanSegmentDAO.GetForExport, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới đường
        /// </summary>
        public static bool Generate(WRKPlanSegment segmentInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.PlanSeg_Generate]",
                            new SqlParameter("@PlanID", segmentInfo.PlanID),
                            new SqlParameter("@Name", segmentInfo.Name),
                            new SqlParameter("@Direction", segmentInfo.Direction),
                            new SqlParameter("@ClassFunc", segmentInfo.ClassFunc),
                            new SqlParameter("@LevelID", segmentInfo.LevelID),
                            new SqlParameter("@KindID", segmentInfo.KindID),

                            new SqlParameter("@StartLeft", segmentInfo.StartLeft),
                            new SqlParameter("@EndLeft", segmentInfo.EndLeft),
                            new SqlParameter("@StartRight", segmentInfo.StartRight),
                            new SqlParameter("@EndRight", segmentInfo.EndRight),

                            new SqlParameter("@MinSpeed", segmentInfo.MinSpeed),
                            new SqlParameter("@MaxSpeed", segmentInfo.MaxSpeed),

                            new SqlParameter("@RoadOpts", segmentInfo.RoadOpts),

                            new SqlParameter("@PointCount", segmentInfo.PointCount),
                            new SqlParameter("@Coords", segmentInfo.Coords),
                            new SqlParameter("@RoadLength", segmentInfo.RoadLength),

                            new SqlParameter("@NoteStr", segmentInfo.NoteNew),

                            new SqlParameter("@GridEdit", segmentInfo.GridEdit),
                            new SqlParameter("@GridView", segmentInfo.GridView),

                            new SqlParameter("@StateOpts", segmentInfo.StateOpts),
                            new SqlParameter("@ActionID", segmentInfo.ActionID),
                            new SqlParameter("@ApprovedState", segmentInfo.ApprovedState),

                            new SqlParameter("@EditorID", segmentInfo.EditorID),
                            new SqlParameter("@EditTime", DataUtl.GetUnixTime()),
                            new SqlParameter("@MigrateID", segmentInfo.MigrateID),

                            new SqlParameter("@ObjetTypeID", EnumBAGObjectType.Polyline));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanSegmentDAO.Generate, ex: " + ex.ToString());
                return false;
            }
        }
        
        /// <summary>
        /// Thêm mới đường
        /// </summary>
        public static bool UpdateGridView(WRKPlanSegment segmentInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.PlanSeg_UpdateGridView]",
                            new SqlParameter("@PLNSegID", segmentInfo.PLNSegID),
                            new SqlParameter("@GridView", segmentInfo.GridView));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanSegmentDAO.UpdateGridView, ex: " + ex.ToString());
                return false;
            }
        }


        /// <summary>
        /// Thêm mới đường
        /// </summary>
        public static bool Create(WRKPlanSegment segmentInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.PlanSeg_Create]",
                            new SqlParameter("@PlanID", segmentInfo.PlanID),
                            new SqlParameter("@Name", segmentInfo.Name),
                            new SqlParameter("@Direction", segmentInfo.Direction),
                            new SqlParameter("@ClassFunc", segmentInfo.ClassFunc),
                            new SqlParameter("@LevelID", segmentInfo.LevelID),
                            new SqlParameter("@KindID", segmentInfo.KindID),

                            new SqlParameter("@StartLeft", segmentInfo.StartLeft),
                            new SqlParameter("@EndLeft", segmentInfo.EndLeft),
                            new SqlParameter("@StartRight", segmentInfo.StartRight),
                            new SqlParameter("@EndRight", segmentInfo.EndRight),

                            new SqlParameter("@MinSpeed", segmentInfo.MinSpeed),
                            new SqlParameter("@MaxSpeed", segmentInfo.MaxSpeed),

                            new SqlParameter("@RoadOpts", segmentInfo.RoadOpts),

                            new SqlParameter("@PointCount", segmentInfo.PointCount),
                            new SqlParameter("@Coords", segmentInfo.Coords),
                            new SqlParameter("@RoadLength", segmentInfo.RoadLength),

                            new SqlParameter("@NoteStr", segmentInfo.NoteNew),

                            new SqlParameter("@GridEdit", segmentInfo.GridEdit),
                            new SqlParameter("@GridView", segmentInfo.GridView),

                            new SqlParameter("@StateOpts", segmentInfo.StateOpts),
                            new SqlParameter("@ActionID", segmentInfo.ActionID),

                            new SqlParameter("@EditorID", segmentInfo.EditorID),
                            new SqlParameter("@EditTime", segmentInfo.EditTime),

                            new SqlParameter("@StateGenerate", EnumMTLApprovedState.Generate),
                            new SqlParameter("@ObjetTypeID", EnumBAGObjectType.Polyline),

                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanSegmentDAO.Create, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Cập nhật đường
        /// </summary>
        public static bool Update(WRKPlanSegment segmentInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.PlanSeg_Update]",
                            new SqlParameter("@PLNSegID", segmentInfo.PLNSegID),
                            new SqlParameter("@Name", segmentInfo.Name),
                            new SqlParameter("@Direction", segmentInfo.Direction),
                            new SqlParameter("@ClassFunc", segmentInfo.ClassFunc),
                            new SqlParameter("@LevelID", segmentInfo.LevelID),
                            new SqlParameter("@KindID", segmentInfo.KindID),

                            new SqlParameter("@StartLeft", segmentInfo.StartLeft),
                            new SqlParameter("@EndLeft", segmentInfo.EndLeft),
                            new SqlParameter("@StartRight", segmentInfo.StartRight),
                            new SqlParameter("@EndRight", segmentInfo.EndRight),

                            new SqlParameter("@MinSpeed", segmentInfo.MinSpeed),
                            new SqlParameter("@MaxSpeed", segmentInfo.MaxSpeed),

                            new SqlParameter("@RoadOpts", segmentInfo.RoadOpts),

                            new SqlParameter("@PointCount", segmentInfo.PointCount),
                            new SqlParameter("@Coords", segmentInfo.Coords),
                            new SqlParameter("@RoadLength", segmentInfo.RoadLength),

                            new SqlParameter("@NoteStr", segmentInfo.NoteNew),

                            new SqlParameter("@ActionID", segmentInfo.ActionID),

                            new SqlParameter("@StateOpts", segmentInfo.StateOpts),
                            new SqlParameter("@EditorID", segmentInfo.EditorID),
                            new SqlParameter("@EditTime", segmentInfo.EditTime),

                            new SqlParameter("@ObjetTypeID", EnumBAGObjectType.Polyline),

                            new SqlParameter("@ActionGenerate", EnumActionBase.Generate),
                            new SqlParameter("@ActionUpdate", EnumActionBase.Edit),

                            new SqlParameter("@ACTEditObject", EnumMTLObjectAction.EditObject),
                            new SqlParameter("@ACTEnterNumber", EnumMTLObjectAction.EnterNumber),

                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanSegmentDAO.Update, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Hủy điểm
        /// </summary>
        public static bool Delete(WRKPlanSegment segmentInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.PlanSeg_Delete]",
                            new SqlParameter("@PLNSegID", segmentInfo.PLNSegID),
                            new SqlParameter("@ActionID", segmentInfo.ActionID),
                            new SqlParameter("@NoteStr", segmentInfo.NoteNew),

                            new SqlParameter("@EditorID", segmentInfo.EditorID),
                            new SqlParameter("@EditTime", segmentInfo.EditTime),

                            new SqlParameter("@ObjetTypeID", EnumBAGObjectType.Polyline),

                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanSegmentDAO.Delete, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Duyệt đường
        /// </summary>
        public static bool Approved(WRKPlanSegment segmentInfo, ref byte errorCode)
        {
            try
            {
                SqlParameter prErrorCode = new SqlParameter("@ErrorCode", errorCode);
                prErrorCode.Direction = ParameterDirection.Output;

                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.PlanSeg_Approved]",
                            new SqlParameter("@PLNSegID", segmentInfo.PLNSegID),
                            new SqlParameter("@Name", segmentInfo.Name),
                            new SqlParameter("@Direction", segmentInfo.Direction),
                            new SqlParameter("@ClassFunc", segmentInfo.ClassFunc),
                            new SqlParameter("@LevelID", segmentInfo.LevelID),
                            new SqlParameter("@KindID", segmentInfo.KindID),

                            new SqlParameter("@StartLeft", segmentInfo.StartLeft),
                            new SqlParameter("@EndLeft", segmentInfo.EndLeft),
                            new SqlParameter("@StartRight", segmentInfo.StartRight),
                            new SqlParameter("@EndRight", segmentInfo.EndRight),

                            new SqlParameter("@AppState", segmentInfo.ApprovedState),
                            new SqlParameter("@NoteStr", segmentInfo.NoteNew),

                            new SqlParameter("@PlanDeployment", EnumMTLPlanBehavior.Deployment),
                            new SqlParameter("@PlanEditMode", EnumMTLPlanBehavior.EditMode),

                            new SqlParameter("@GridPending", EnumMTLPlanGridState.Pending),
                            new SqlParameter("@GridDoing", EnumMTLPlanGridState.Doing),

                            new SqlParameter("@StateApproved", EnumMTLApprovedState.Approved),
                            new SqlParameter("@ObjetTypeID", EnumBAGObjectType.Polyline),

                            new SqlParameter("@ActionGenerate", EnumActionBase.Generate),
                            new SqlParameter("@ActionApproved", EnumActionBase.Approved),

                            new SqlParameter("@UserID", segmentInfo.EditorID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()),

                            prErrorCode);

                errorCode = Convert.ToByte(prErrorCode.Value);

                return errorCode == 0 && exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanSegmentDAO.Approved, ex: " + ex.ToString());
                errorCode = 254;
                return false;
            }
        }
    }
}