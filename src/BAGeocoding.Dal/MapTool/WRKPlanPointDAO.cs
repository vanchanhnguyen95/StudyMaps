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
    public class WRKPlanPointDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPTOOL);

        /// <summary>
        /// Lấy danh sách điểm của kế hoạch
        /// </summary>
        public static List<WRKPlanPoint> GetByPlan(int planID)
        {
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(sqlDB, "[WRK.PlanPoi_GetByPlan]",
                                new SqlParameter("@PlanID", planID),

                                new SqlParameter("@EnumPoint", EnumBAGObjectType.Point));

                if (ds == null)
                    return null;
                else if (ds.Tables.Count < 2)
                    return null;
                else if (ds.Tables[0].Rows.Count == 0)
                    return null;

                int tableIndex = 0;
                Hashtable htPOI = new Hashtable();
                List<WRKPlanPoint> dataList = new List<WRKPlanPoint>();
                // .1 Lấy danh sách điểm
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    WRKPlanPoint dataInfo = new WRKPlanPoint();
                    if (dataInfo.FromDataRow(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    htPOI.Add(dataInfo.PLNPoiID, dataList.Count);
                    dataList.Add(dataInfo);
                }
                tableIndex += 1;
                // 2. Cập nhật danh sách ghi chú
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    WRKPlanPoint dataInfo = new WRKPlanPoint();
                    if (dataInfo.FromDataNote(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    else if (htPOI.ContainsKey(dataInfo.PLNPoiID) == false)
                        continue;
                    dataList[Convert.ToInt32(htPOI[dataInfo.PLNPoiID])].UpdateNote(dataInfo);
                }

                // 3. Trả về kết quả
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanPointDAO.GetByPlan, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách điểm của kế hoạch
        /// </summary>
        public static List<WRKPlanPoint> GetForExport(int planID, WRKPlanOptionExport exportOpt)
        {
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(sqlDB, "[WRK.PlanPoi_GetForExport]",
                                new SqlParameter("@PlanID", planID),
                                new SqlParameter("@GridStr", exportOpt.GridStr),
                                new SqlParameter("@PKind", exportOpt.PKind),

                                new SqlParameter("@EnumPoint", EnumBAGObjectType.Point));

                if (ds == null)
                    return null;
                else if (ds.Tables.Count < 2)
                    return null;
                else if (ds.Tables[0].Rows.Count == 0)
                    return null;

                int tableIndex = 0;
                Hashtable htPOI = new Hashtable();
                List<WRKPlanPoint> dataList = new List<WRKPlanPoint>();
                // .1 Lấy danh sách điểm
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    WRKPlanPoint dataInfo = new WRKPlanPoint();
                    if (dataInfo.FromDataRow(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    else if (exportOpt.IgnoreBlankData == true && dataInfo.MigrateID > 0)
                        continue;
                    htPOI.Add(dataInfo.PLNPoiID, dataList.Count);
                    dataList.Add(dataInfo);
                }
                tableIndex += 1;
                // 2. Cập nhật danh sách ghi chú
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    WRKPlanPoint dataInfo = new WRKPlanPoint();
                    if (dataInfo.FromDataNote(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    else if (htPOI.ContainsKey(dataInfo.PLNPoiID) == false)
                        continue;
                    dataList[Convert.ToInt32(htPOI[dataInfo.PLNPoiID])].UpdateNote(dataInfo);
                }

                // 3. Trả về kết quả
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanPointDAO.GetForExport, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách điểm của kế hoạch
        /// </summary>
        public static List<WRKPlanPoint> GetExportNone(int planID, WRKPlanOptionExport exportOpt)
        {
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(sqlDB, "[WRK.PlanPoi_GetExportNone]",
                                new SqlParameter("@PlanID", planID),
                                new SqlParameter("@GridStr", exportOpt.GridStr),
                                new SqlParameter("@PKind", exportOpt.PKind),

                                new SqlParameter("@StateGen", EnumMTLApprovedState.Generate),

                                new SqlParameter("@EnumPoint", EnumBAGObjectType.Point));

                if (ds == null)
                    return null;
                else if (ds.Tables.Count < 2)
                    return null;
                else if (ds.Tables[0].Rows.Count == 0)
                    return null;

                int tableIndex = 0;
                Hashtable htPOI = new Hashtable();
                List<WRKPlanPoint> dataList = new List<WRKPlanPoint>();
                // .1 Lấy danh sách điểm
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    WRKPlanPoint dataInfo = new WRKPlanPoint();
                    if (dataInfo.FromDataRow(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    else if (exportOpt.IgnoreBlankData == true && dataInfo.MigrateID > 0)
                        continue;
                    htPOI.Add(dataInfo.PLNPoiID, dataList.Count);
                    dataList.Add(dataInfo);
                }
                tableIndex += 1;
                // 2. Cập nhật danh sách ghi chú
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    WRKPlanPoint dataInfo = new WRKPlanPoint();
                    if (dataInfo.FromDataNote(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    else if (htPOI.ContainsKey(dataInfo.PLNPoiID) == false)
                        continue;
                    dataList[Convert.ToInt32(htPOI[dataInfo.PLNPoiID])].UpdateNote(dataInfo);
                }

                // 3. Trả về kết quả
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanPointDAO.GetExportNone, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới điểm
        /// </summary>
        public static bool Generate(WRKPlanPoint pointInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.PlanPoi_Generate]",
                            new SqlParameter("@PlanID", pointInfo.PlanID),
                            new SqlParameter("@KindID", pointInfo.KindID),
                            new SqlParameter("@Name", pointInfo.Name),
                            new SqlParameter("@Lng", pointInfo.Coords.Lng),
                            new SqlParameter("@Lat", pointInfo.Coords.Lat),
                            new SqlParameter("@ImageSrc", pointInfo.ImageSrc),

                            new SqlParameter("@NoteStr", pointInfo.NoteNew),

                            new SqlParameter("@GridEdit", pointInfo.GridEdit),
                            new SqlParameter("@GridView", pointInfo.GridView),

                            new SqlParameter("@ActionID", pointInfo.ActionID),
                            new SqlParameter("@ApprovedState", pointInfo.ApprovedState),

                            new SqlParameter("@EditorID", pointInfo.EditorID),
                            new SqlParameter("@EditTime", DataUtl.GetUnixTime()),
                            new SqlParameter("@SyncTokenID", pointInfo.SyncTokenID),
                            new SqlParameter("@MigrateID", pointInfo.MigrateID),

                            new SqlParameter("@ObjetTypeID", EnumBAGObjectType.Point));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanPointDAO.Generate, ex: " + ex.ToString());
                return false;
            }
        }



        /// <summary>
        /// Thêm mới điểm
        /// </summary>
        public static bool Create(WRKPlanPoint pointInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.PlanPoi_Create]",
                            new SqlParameter("@PlanID", pointInfo.PlanID),
                            new SqlParameter("@KindID", pointInfo.KindID),
                            new SqlParameter("@Name", pointInfo.Name),
                            new SqlParameter("@Lng", pointInfo.Coords.Lng),
                            new SqlParameter("@Lat", pointInfo.Coords.Lat),
                            new SqlParameter("@NoteStr", pointInfo.NoteNew),

                            new SqlParameter("@GridEdit", pointInfo.GridEdit),
                            new SqlParameter("@GridView", pointInfo.GridView),

                            new SqlParameter("@ActionID", pointInfo.ActionID),
                            new SqlParameter("@EditorID", pointInfo.EditorID),
                            new SqlParameter("@EditTime", pointInfo.EditTime),
                            new SqlParameter("@SyncTokenID", pointInfo.SyncTokenID),

                            new SqlParameter("@StateGenerate", EnumMTLApprovedState.Generate),
                            new SqlParameter("@ObjetTypeID", EnumBAGObjectType.Point),

                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanPointDAO.Create, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Cập nhật điểm
        /// </summary>
        public static bool Update(WRKPlanPoint pointInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.PlanPoi_Update]",
                            new SqlParameter("@PLNPoiID", pointInfo.PLNPoiID),
                            new SqlParameter("@KindID", pointInfo.KindID),
                            new SqlParameter("@Name", pointInfo.Name),
                            new SqlParameter("@Lng", pointInfo.Coords.Lng),
                            new SqlParameter("@Lat", pointInfo.Coords.Lat),
                            new SqlParameter("@NoteStr", pointInfo.NoteNew),

                            new SqlParameter("@ActionID", pointInfo.ActionID),
                            new SqlParameter("@SyncTokenID", pointInfo.SyncTokenID),

                            new SqlParameter("@EditorID", pointInfo.EditorID),
                            new SqlParameter("@EditTime", pointInfo.EditTime),

                            new SqlParameter("@ObjetTypeID", EnumBAGObjectType.Point),

                            new SqlParameter("@ActionGenerate", EnumActionBase.Generate),
                            new SqlParameter("@ActionApproved", EnumActionBase.Approved),

                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanPointDAO.Update, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Cập nhật đường dẫn ảnh
        /// </summary>
        public static bool Image(WRKPlanGridSync dataInfo, WRKPlanPoint pointInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.PlanPoi_Image]",
                            new SqlParameter("@PlanID", dataInfo.PlanID),
                            new SqlParameter("@GridID", dataInfo.GridID),
                            new SqlParameter("@SyncTokenID", pointInfo.SyncTokenID),
                            new SqlParameter("@ImageSrc", pointInfo.ImageSrc),

                            new SqlParameter("@UserID", dataInfo.UserID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanPointDAO.Update, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Hủy điểm
        /// </summary>
        public static bool Delete(WRKPlanPoint pointInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.PlanPoi_Delete]",
                            new SqlParameter("@PLNPoiID", pointInfo.PLNPoiID),
                            new SqlParameter("@ActionID", pointInfo.ActionID),

                            new SqlParameter("@EditorID", pointInfo.EditorID),
                            new SqlParameter("@EditTime", pointInfo.EditTime),

                            new SqlParameter("@ObjetTypeID", EnumBAGObjectType.Point),

                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanPointDAO.Update, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Duyệt điểm
        /// </summary>
        public static bool Approved(WRKPlanPoint pointInfo, ref byte errorCode)
        {
            try
            {
                SqlParameter prErrorCode = new SqlParameter("@ErrorCode", errorCode);
                prErrorCode.Direction = ParameterDirection.Output;

                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.PlanPoi_Approved]",
                            new SqlParameter("@PLNPoiID", pointInfo.PLNPoiID),
                            new SqlParameter("@KindID", pointInfo.KindID),
                            new SqlParameter("@Name", pointInfo.Name),
                            new SqlParameter("@Lng", pointInfo.Coords.Lng),
                            new SqlParameter("@Lat", pointInfo.Coords.Lat),
                            new SqlParameter("@AppState", pointInfo.ApprovedState),
                            new SqlParameter("@NoteStr", pointInfo.NoteNew),

                            new SqlParameter("@PlanDeployment", EnumMTLPlanBehavior.Deployment),
                            new SqlParameter("@PlanEditMode", EnumMTLPlanBehavior.EditMode),

                            new SqlParameter("@GridPending", EnumMTLPlanGridState.Pending),
                            new SqlParameter("@GridDoing", EnumMTLPlanGridState.Doing),

                            new SqlParameter("@StateApproved", EnumMTLApprovedState.Approved),
                            new SqlParameter("@ObjetTypeID", EnumBAGObjectType.Point),

                            new SqlParameter("@ActionGenerate", EnumActionBase.Generate),
                            new SqlParameter("@ActionApproved", EnumActionBase.Approved),

                            new SqlParameter("@UserID", pointInfo.EditorID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()),

                            prErrorCode);

                errorCode = Convert.ToByte(prErrorCode.Value);

                return errorCode == 0 && exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanPointDAO.Approved, ex: " + ex.ToString());
                errorCode = 254;
                return false;
            }
        }
    }
}