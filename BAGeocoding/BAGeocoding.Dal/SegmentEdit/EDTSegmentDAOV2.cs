using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BAGeocoding.Utility;

//using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.Enum;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace BAGeocoding.Dal.SegmentEdit
{
    /// <summary>
    /// Quản lý truy xuất thông tin đoạn đường
    /// </summary>
    public class EDTSegmentDAOV2 : NoSqlHelper
    {
        private static IMongoCollection<BAGSegmentV2> _collection;

        public EDTSegmentDAOV2()
        {
            ConventionRegistry.Register("elementNameConvention", pack, x => true);
            _collection = database.GetCollection<BAGSegmentV2>(nameof(BAGSegmentV2));
        }

        public static async Task<List<BAGSegmentV2>> GetByProvince(short provinceID)
        {
            try
            {
                var dt = await _collection.Find(x => x.ProvinceID == provinceID).ToListAsync();
                if (dt != null)
                    return dt;
                //DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[EDT.Segment_GetByProvince]",
                //            new SqlParameter("@ProvinceID", provinceID));

                //if (dt != null)
                //{
                //    List<BAGSegment> segmentList = new List<BAGSegment>();
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        BAGSegment segmentItem = new BAGSegment();
                //        if (segmentItem.FromDataSimple(dt.Rows[i]) == false)
                //            return null;
                //        segmentList.Add(segmentItem);
                //    }
                //    return segmentList;
                //}

                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("EDTSegmentDAO.GetByProvince, ex: " + ex.ToString());
                return null;
            }
        }
        
        public static async Task<bool> Add(BAGSegmentV2 segment)
        {
            try
            {
                await _collection.InsertOneAsync(segment);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[EDT.Segment_Add]",
                //            new SqlParameter("@SegmentID", segment.SegmentID),
                //            new SqlParameter("@ProvinceID", segment.ProvinceID),
                //            new SqlParameter("@Name", segment.VName),
                //            new SqlParameter("@Direction", segment.Direction),
                //            new SqlParameter("@ClassFunc", segment.ClassFunc),
                //            new SqlParameter("@LevelID", segment.LevelID),
                //            new SqlParameter("@KindID", segment.KindID),
                //            new SqlParameter("@RegionLev", segment.RegionLev),
                //            new SqlParameter("@StartLeft", segment.StartLeft),
                //            new SqlParameter("@EndLeft", segment.EndLeft),
                //            new SqlParameter("@StartRight", segment.StartRight),
                //            new SqlParameter("@EndRight", segment.EndRight),
                //            new SqlParameter("@Wide", segment.Wide),
                //            new SqlParameter("@MinSpeed", segment.MinSpeed),
                //            new SqlParameter("@MaxSpeed", segment.MaxSpeed),
                //            new SqlParameter("@SegLength", segment.SegLength),
                //            new SqlParameter("@Fee", segment.Fee),
                //            new SqlParameter("@IsNumber", segment.IsNumber),
                //            new SqlParameter("@IsBridge", segment.IsBridge),
                //            new SqlParameter("@IsPrivate", segment.IsPrivate),
                //            new SqlParameter("@IsPed", segment.IsPed),
                //            new SqlParameter("@AllowPed", segment.AllowPed),
                //            new SqlParameter("@AllowWalk", segment.AllowWalk),
                //            new SqlParameter("@AllowBicycle", segment.AllowBicycle),
                //            new SqlParameter("@AllowMoto", segment.AllowMoto),
                //            new SqlParameter("@AllowCar", segment.AllowCar),
                //            new SqlParameter("@DirCar", segment.DirCar),
                //            new SqlParameter("@AllowBus", segment.AllowBus),
                //            new SqlParameter("@DirBus", segment.DirBus),
                //            new SqlParameter("@AllowTruck", segment.AllowTruck),
                //            new SqlParameter("@DirTruck", segment.DirTruck),
                //            new SqlParameter("@AllowTaxi", segment.AllowTaxi),
                //            new SqlParameter("@DirTaxi", segment.DirTaxi),
                //            new SqlParameter("@PointCount", segment.PointList.Count),
                //            new SqlParameter("@GridStr", segment.GridStr),
                //            new SqlParameter("@LngStr", segment.LngStr),
                //            new SqlParameter("@LatStr", segment.LatStr),
                //            new SqlParameter("@IsDone", segment.IsDone));

                //return exec > 0;
                return true;
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
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[EDT.Segment_AdjustBuilding]",
                //            new SqlParameter("@ProvinceID", provinceID));

                //return exec > 0;
                return false;
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
        public static async Task<bool> Clear(short provinceID)
        {
            try
            {
                var filter = Builders<BAGSegmentV2>.Filter.Eq(c => c.ProvinceID, provinceID);
                await _collection.DeleteOneAsync(filter);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[EDT.Segment_Clear]",
                //            new SqlParameter("@ProvinceID", provinceID));

                //return exec > 0;
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("SegmentDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
