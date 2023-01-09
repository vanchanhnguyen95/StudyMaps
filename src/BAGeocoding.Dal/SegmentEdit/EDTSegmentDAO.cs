using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BAGeocoding.Utility;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.Enum;

namespace BAGeocoding.Dal.SegmentEdit
{
    /// <summary>
    /// Quản lý truy xuất thông tin đoạn đường
    /// </summary>
    public class EDTSegmentDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING);

        public static List<BAGSegment> GetByProvince(short provinceID)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[EDT.Segment_GetByProvince]",
                            new SqlParameter("@ProvinceID", provinceID));

                if (dt != null)
                {
                    List<BAGSegment> segmentList = new List<BAGSegment>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        BAGSegment segmentItem = new BAGSegment();
                        if (segmentItem.FromDataSimple(dt.Rows[i]) == false)
                            return null;
                        segmentList.Add(segmentItem);
                    }
                    return segmentList;
                }

                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("EDTSegmentDAO.GetByProvince, ex: " + ex.ToString());
                return null;
            }
        }
        
        public static bool Add(BAGSegment segment)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[EDT.Segment_Add]",
                            new SqlParameter("@SegmentID", segment.SegmentID),
                            new SqlParameter("@ProvinceID", segment.ProvinceID),
                            new SqlParameter("@Name", segment.VName),
                            new SqlParameter("@Direction", segment.Direction),
                            new SqlParameter("@ClassFunc", segment.ClassFunc),
                            new SqlParameter("@LevelID", segment.LevelID),
                            new SqlParameter("@KindID", segment.KindID),
                            new SqlParameter("@RegionLev", segment.RegionLev),
                            new SqlParameter("@StartLeft", segment.StartLeft),
                            new SqlParameter("@EndLeft", segment.EndLeft),
                            new SqlParameter("@StartRight", segment.StartRight),
                            new SqlParameter("@EndRight", segment.EndRight),
                            new SqlParameter("@Wide", segment.Wide),
                            new SqlParameter("@MinSpeed", segment.MinSpeed),
                            new SqlParameter("@MaxSpeed", segment.MaxSpeed),
                            new SqlParameter("@SegLength", segment.SegLength),
                            new SqlParameter("@Fee", segment.Fee),
                            new SqlParameter("@IsNumber", segment.IsNumber),
                            new SqlParameter("@IsBridge", segment.IsBridge),
                            new SqlParameter("@IsPrivate", segment.IsPrivate),
                            new SqlParameter("@IsPed", segment.IsPed),
                            new SqlParameter("@AllowPed", segment.AllowPed),
                            new SqlParameter("@AllowWalk", segment.AllowWalk),
                            new SqlParameter("@AllowBicycle", segment.AllowBicycle),
                            new SqlParameter("@AllowMoto", segment.AllowMoto),
                            new SqlParameter("@AllowCar", segment.AllowCar),
                            new SqlParameter("@DirCar", segment.DirCar),
                            new SqlParameter("@AllowBus", segment.AllowBus),
                            new SqlParameter("@DirBus", segment.DirBus),
                            new SqlParameter("@AllowTruck", segment.AllowTruck),
                            new SqlParameter("@DirTruck", segment.DirTruck),
                            new SqlParameter("@AllowTaxi", segment.AllowTaxi),
                            new SqlParameter("@DirTaxi", segment.DirTaxi),
                            new SqlParameter("@PointCount", segment.PointList.Count),
                            new SqlParameter("@GridStr", segment.GridStr),
                            new SqlParameter("@LngStr", segment.LngStr),
                            new SqlParameter("@LatStr", segment.LatStr),
                            new SqlParameter("@IsDone", segment.IsDone));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("EDTSegmentDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Hiệu chỉnh số nhà
        /// </summary>
        public static bool AdjustBuilding(short provinceID)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[EDT.Segment_AdjustBuilding]",
                            new SqlParameter("@ProvinceID", provinceID));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("EDTSegmentDAO.AdjustBuilding, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Hủy tất cả thông tin đoạn đường
        /// </summary>
        public static bool Clear(short provinceID)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[EDT.Segment_Clear]",
                            new SqlParameter("@ProvinceID", provinceID));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("SegmentDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
