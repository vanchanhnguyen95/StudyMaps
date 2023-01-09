using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.Enum.MapTool;
using BAGeocoding.Entity.MapTool;
using BAGeocoding.Entity.Utility;

using BAGeocoding.Utility;
using BAGeocoding.Entity.MapTool.Data;

namespace BAGeocoding.Dal.MapTool
{
    /// <summary>
    /// Quản lý truy xuất thông tin kế hoạch
    /// </summary>
    public class WRKPlanDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPTOOL);

        /// <summary>
        /// Lấy danh sách kế hoạch
        /// </summary>
        public static List<WRKPlan> GetByCondition(UTLConditionBase condition)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[WRK.Plan_GetByCondition]",
                            new SqlParameter("@StartTime", condition.StartTime),
                            new SqlParameter("@EndTime", condition.EndTime),

                            new SqlParameter("@IncludeAbort", condition.IncludeAbort),
                            new SqlParameter("@BehaviorAbort", EnumMTLPlanBehavior.Abort));
                if (dt == null)
                    return null;
                List<WRKPlan> planList = new List<WRKPlan>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    WRKPlan planInfo = new WRKPlan();
                    if (planInfo.FromDataRow(dt.Rows[i]) == false)
                        return null;
                    planList.Add(planInfo);
                }
                return planList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanDAO.GetByCondition, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy thông tin kế hoạch theo mã
        /// </summary>
        public static WRKPlan GetByID(int planID)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[WRK.Plan_GetByID]",
                            new SqlParameter("@PlanID", planID));
                if (dt == null)
                    return null;
                else if (dt.Rows.Count < 1)
                    return null;
                WRKPlan planInfo = new WRKPlan();
                if (planInfo.FromDataRow(dt.Rows[0]) == true)
                    return planInfo;
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanDAO.GetByID, ex: " + ex.ToString());
                return null;
            }
        }


        /// <summary>
        /// Thêm mới thông tin kế hoạch
        /// </summary>
        public static bool Add(WRKPlan planInfo, ref int planID)
        {
            try
            {
                SqlParameter prPlanID = new SqlParameter("@PlanID", planID);
                prPlanID.Direction = ParameterDirection.Output;

                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.Plan_Add]",
                            new SqlParameter("@ProvinceID", planInfo.ProvinceID),
                            new SqlParameter("@Name", planInfo.Name),
                            new SqlParameter("@StartTime", planInfo.StartTime),
                            new SqlParameter("@EndTime", planInfo.EndTime),
                            new SqlParameter("@RootURL", planInfo.RootURL),
                            new SqlParameter("@ImagePath", planInfo.ImagePath),
                            new SqlParameter("@LogsPath", planInfo.LogsPath),
                            new SqlParameter("@Description", planInfo.Description),
                            new SqlParameter("@DataExt", planInfo.DataExt),
                            new SqlParameter("@Password", StringUlt.EncryptPassword(planInfo.PasswordUser)),

                            new SqlParameter("@BehaviorID", EnumMTLPlanBehavior.Generate),

                            new SqlParameter("@UserID", planInfo.CreatorID),
                            new SqlParameter("@CurrentTime", planInfo.CreateTime),

                            prPlanID);

                planID = Convert.ToInt32(prPlanID.Value);

                return exec > 0 && planID > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Cập nhật thông tin kế hoạch
        /// </summary>
        public static bool Update(WRKPlan planInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.Plan_Update]",
                            new SqlParameter("@PlanID", planInfo.PlanID),
                            new SqlParameter("@Name", planInfo.Name),
                            new SqlParameter("@StartTime", planInfo.StartTime),
                            new SqlParameter("@EndTime", planInfo.EndTime),
                            new SqlParameter("@RootURL", planInfo.RootURL),
                            new SqlParameter("@Description", planInfo.Description),
                            new SqlParameter("@DataExt", planInfo.DataExt),

                            new SqlParameter("@UserID", planInfo.EditorID),
                            new SqlParameter("@CurrentTime", planInfo.EditTime));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanDAO.Update, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Cập nhật tọa độ trung tâm
        /// </summary>
        public static bool UpdateCenter(WRKPlan planInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.Plan_UpdateCenter]",
                            new SqlParameter("@PlanID", planInfo.PlanID),
                            new SqlParameter("@CLng", planInfo.Center.Lng),
                            new SqlParameter("@CLat", planInfo.Center.Lat));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanDAO.UpdateCenter, ex: " + ex.ToString());
                return false;
            }
        }


        /// <summary>
        /// Lấy dữ liệu đồng bộ
        /// </summary>
        public static List<WRKPlanGridSync> SyncData(USRUserApp dataInfo)
        {
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(sqlDB, "[WRK.Plan_SyncData]",
                            new SqlParameter("@PlanID", dataInfo.PlanInfo.PlanID),
                            new SqlParameter("@UserID", dataInfo.UserInfo.UserID),
                            new SqlParameter("@GridStr", dataInfo.GridStr()),

                            new SqlParameter("@BehaviorDeploy", EnumMTLPlanBehavior.Deployment),
                            new SqlParameter("@StateProcessing", EnumMTLPlanGridState.Processing),
                            new SqlParameter("@UserGridEdit", EnumMTLUserGridDataExt.Edit),

                            new SqlParameter("@TypePoint", EnumBAGObjectType.Point),
                            new SqlParameter("@TypeSegment", EnumBAGObjectType.Polyline));

                if (ds == null)
                    return null;
                else if (ds.Tables.Count < 3)
                    return null;
                else if (ds.Tables[0].Rows.Count < 1)
                    return null;

                int tableIndex = 0;
                Hashtable htObjectIndex = new Hashtable();

                // 1. Lấy danh sách grid
                List<WRKPlanGridSync> resultList = new List<WRKPlanGridSync>();
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    WRKPlanGridSync gridInfo = new WRKPlanGridSync();
                    if (gridInfo.FromDataSimple(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    resultList.Add(gridInfo);
                }
                tableIndex += 1;


                // 2. Thông tin điểm
                htObjectIndex = new Hashtable();
                List<WRKPlanPoint> pointList = new List<WRKPlanPoint>();
                // 2.1 Danh sách điểm
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    WRKPlanPoint pointInfo = new WRKPlanPoint();
                    if (pointInfo.FromDataRow(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    else if (htObjectIndex.ContainsKey(pointInfo.PLNPoiID) == true)
                        continue;
                    htObjectIndex.Add(pointInfo.PLNPoiID, pointList.Count);
                    pointList.Add(pointInfo);
                }
                tableIndex += 1;
                // 2.2 Bổ sung thông tin ghi chú
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    WRKPlanPoint objectNote = new WRKPlanPoint();
                    if (objectNote.FromDataNote(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    else if (htObjectIndex.ContainsKey(objectNote.PLNPoiID) == false)
                        continue;
                    pointList[Convert.ToInt32(htObjectIndex[objectNote.PLNPoiID])].UpdateNote(objectNote);
                }
                tableIndex += 1;

                // 3. Thông tin đường
                htObjectIndex = new Hashtable();
                List<WRKPlanSegment> segmentList = new List<WRKPlanSegment>();
                Hashtable htSegmentCheck = new Hashtable();
                // 3.1 Danh sách đường
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    WRKPlanSegment segmentInfo = new WRKPlanSegment();
                    if (segmentInfo.FromDataRow(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    else if (htObjectIndex.ContainsKey(segmentInfo.PLNSegID) == true)
                        continue;
                    htObjectIndex.Add(segmentInfo.PLNSegID, segmentList.Count);
                    segmentList.Add(segmentInfo);
                }
                tableIndex += 1;
                // 3.2 Bổ sung thông tin ghi chú
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    WRKPlanSegment objectNote = new WRKPlanSegment();
                    if (objectNote.FromDataNote(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    else if (htObjectIndex.ContainsKey(objectNote.PLNSegID) == false)
                        continue;
                    segmentList[Convert.ToInt32(htObjectIndex[objectNote.PLNSegID])].UpdateNote(objectNote);
                }
                tableIndex += 1;

                // 4. Phân bổ lại đối tượng
                for (int i = 0; i < resultList.Count; i++)
                {
                    if (resultList[i].EnumState != EnumMTLGridSyncState.Normal)
                        continue;
                    resultList[i].SegmentList = segmentList.FindAll(item => item.GridEdit == resultList[i].GridID);
                    resultList[i].PointList = pointList.FindAll(item => item.GridEdit == resultList[i].GridID);
                }

                // 5. Trả về kết quả
                return resultList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanDAO.UpdateCenter, ex: " + ex.ToString());
                return null;
            }
        }


        public static List<DTSUserMark> MarkCalc(int planID)
        {
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(sqlDB, "[WRK.Plan_MarkCalc]",
                            new SqlParameter("@PlanID", planID),

                            new SqlParameter("@ActionGenerate", EnumActionBase.Generate),
                            new SqlParameter("@ActionUpdate", EnumActionBase.Edit),

                            new SqlParameter("@DeleteByAction", EnumMTLObjectAction.DeleteByAction),

                            new SqlParameter("@StateApproved", EnumMTLApprovedState.Approved),

                            new SqlParameter("@ObjectPoint", EnumBAGObjectType.Point),
                            new SqlParameter("@ObjectSegment", EnumBAGObjectType.Polyline));

                if (ds == null)
                    return null;
                else if (ds.Tables.Count < 3)
                    return null;
                else if (ds.Tables[0].Rows.Count < 1)
                    return null;

                int tableIndex = 0;
                Hashtable htUserIndex = new Hashtable();

                // 1. Lấy danh sách tài khoản
                List<DTSUserMark> resultList = new List<DTSUserMark>();
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    DTSUserMark userInfo = new DTSUserMark();
                    if (userInfo.FromDataUser(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    htUserIndex.Add(userInfo.UserInfo.UserID, resultList.Count);
                    resultList.Add(userInfo);
                }
                tableIndex += 1;


                // A. Thông tin Point
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    DTSUserMark pointInfo = new DTSUserMark();
                    if (pointInfo.FromDataCount(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    else if (htUserIndex.ContainsKey(pointInfo.UserInfo.UserID) == false)
                        continue;
                    resultList[Convert.ToInt32(htUserIndex[pointInfo.UserInfo.UserID])].POICountUpdate(pointInfo);
                }
                tableIndex += 1;

                // B. Thông tin Segment
                // B.1 Đường mới
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    DTSUserMark pointInfo = new DTSUserMark();
                    if (pointInfo.FromDataCount(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    else if (htUserIndex.ContainsKey(pointInfo.UserInfo.UserID) == false)
                        continue;
                    resultList[Convert.ToInt32(htUserIndex[pointInfo.UserInfo.UserID])].SEGNewCountUpdate(pointInfo);
                }
                tableIndex += 1;

                // B.2 Đường mới có số nhà
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    DTSUserMark pointInfo = new DTSUserMark();
                    if (pointInfo.FromDataNumber(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    else if (htUserIndex.ContainsKey(pointInfo.UserInfo.UserID) == false)
                        continue;
                    resultList[Convert.ToInt32(htUserIndex[pointInfo.UserInfo.UserID])].SEGNewNumberUpdate(pointInfo);
                }
                tableIndex += 1;

                // B.3 Đường cũ cập nhật số nhà
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    DTSUserMark pointInfo = new DTSUserMark();
                    if (pointInfo.FromDataNumber(ds.Tables[tableIndex].Rows[i], "OLD", "NEW") == false)
                        return null;
                    else if (htUserIndex.ContainsKey(pointInfo.UserInfo.UserID) == false)
                        continue;
                    resultList[Convert.ToInt32(htUserIndex[pointInfo.UserInfo.UserID])].SEGOldNumberUpdate(pointInfo);
                }
                tableIndex += 1;

                // B.4 Đường cũ cập nhật tên
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    DTSUserMark pointInfo = new DTSUserMark();
                    if (pointInfo.FromDataCount(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    else if (htUserIndex.ContainsKey(pointInfo.UserInfo.UserID) == false)
                        continue;
                    resultList[Convert.ToInt32(htUserIndex[pointInfo.UserInfo.UserID])].SEGOldNameUpdate(pointInfo);
                }
                tableIndex += 1;

                // B.5 Bổ sung thông tin
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    DTSUserMark pointInfo = new DTSUserMark();
                    if (pointInfo.FromDataCount(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    else if (htUserIndex.ContainsKey(pointInfo.UserInfo.UserID) == false)
                        continue;
                    resultList[Convert.ToInt32(htUserIndex[pointInfo.UserInfo.UserID])].SEGOldDeleteUpdate(pointInfo);
                }
                tableIndex += 1;

                // B.6 Đường cũ bị xóa
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    DTSUserMark pointInfo = new DTSUserMark();
                    if (pointInfo.FromDataCount(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    else if (htUserIndex.ContainsKey(pointInfo.UserInfo.UserID) == false)
                        continue;
                    resultList[Convert.ToInt32(htUserIndex[pointInfo.UserInfo.UserID])].SEGOldDeleteUpdate(pointInfo);
                }
                tableIndex += 1;

                // B.7 Lấy quảng đường di chuyển
                for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                {
                    DTSUserMark pointInfo = new DTSUserMark();
                    if (pointInfo.FromDataCount(ds.Tables[tableIndex].Rows[i]) == false)
                        return null;
                    else if (htUserIndex.ContainsKey(pointInfo.UserInfo.UserID) == false)
                        continue;
                    resultList[Convert.ToInt32(htUserIndex[pointInfo.UserInfo.UserID])].DistanceUpdate(pointInfo);
                }
                tableIndex += 1;


                // 5. Trả về kết quả
                for (int i = resultList.Count - 1; i > -1; i--)
                {
                    if (resultList[i].IsData() == false)
                        resultList.RemoveAt(i);
                }
                return resultList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKPlanDAO.MarkCalc, ex: " + ex.ToString());
                return null;
            }
        }
    }
}