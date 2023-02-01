using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BAGeocoding.Utility;


using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.Enum;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace BAGeocoding.Dal.MapObj
{
    /// <summary>
    /// Quản lý truy xuất thông tin đoạn đường
    /// </summary>
    public class SegmentDAOV2 : NoSqlHelper
    {
        private static IMongoCollection<BAGSegmentV2> _collection;

        public SegmentDAOV2()
        {
            ConventionRegistry.Register("elementNameConvention", pack, x => true);
            _collection = database.GetCollection<BAGSegmentV2>(nameof(BAGSegmentV2));
        }

        public static async Task<List<BAGSegmentV2>> GetByProvince(int provinceID)
        {
            try
            {
                var ds = await _collection.Find(x => x.ProvinceID == provinceID).ToListAsync();
                if (ds != null)
                    return ds;
                //DataSet ds = SQLHelper.ExecuteDataset(sqlDB, "[DTS.Segment_GetByProvince]", 
                //                new SqlParameter("@ProvinceID", provinceID));

                //if (ds != null)
                //{
                //    if (ds.Tables.Count > 1)
                //    {
                //        if (ds.Tables[0].Rows.Count > 0)
                //        {
                //            List<BAGSegment> segmentList = new List<BAGSegment>();
                //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //            {
                //                BAGSegment segmentItem = new BAGSegment();
                //                if(segmentItem.FromDataRow(ds.Tables[0].Rows[i], ds.Tables[1]) == false)
                //                    return null;
                //                segmentList.Add(segmentItem);
                //            }
                //            return segmentList;
                //        }
                //        return null;
                //    }
                //}
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("SegmentDAO.GetByProvince, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới thông tin đoạn đường
        /// </summary>
        public static async Task<bool> Add(BAGSegmentV2 segment)
        {
            try
            {
                await _collection.InsertOneAsync(segment);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.Segment_Add]",
                //            new SqlParameter("@SegmentID", segment.SegmentID),
                //            new SqlParameter("@ProvinceID", segment.ProvinceID),
                //            new SqlParameter("@DistrictID", segment.DistrictID),
                //            new SqlParameter("@VName", segment.VName),
                //            new SqlParameter("@EName", segment.EName),
                //            new SqlParameter("@ClassFunc", segment.ClassFunc),
                //            new SqlParameter("@Direction", segment.Direction),
                //            new SqlParameter("@DataExt", segment.DataExt),
                //            new SqlParameter("@PointCount", segment.PointList.Count),
                //            new SqlParameter("@MinSpeed", segment.MinSpeed),
                //            new SqlParameter("@MaxSpeed", segment.MaxSpeed),
                //            new SqlParameter("@StartLeft", segment.StartLeft),
                //            new SqlParameter("@EndLeft", segment.EndLeft),
                //            new SqlParameter("@StartRight", segment.StartRight),
                //            new SqlParameter("@EndRight", segment.EndRight),
                //            new SqlParameter("@SegmentLength", segment.Length));

                //return exec > 0;
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("SegmentDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Hiệu chỉnh số nhà
        /// </summary>
        public static async Task<bool> AdjustBuilding(short provinceID)
        {
            try
            {
                var ds = await _collection.Find(x => x.ProvinceID == provinceID).ToListAsync();
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.Segment_AdjustBuilding]",
                //            new SqlParameter("@ProvinceID", provinceID));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("SegmentDAO.AdjustBuilding, ex: " + ex.ToString());
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
                var filter = Builders<BAGSegmentV2>.Filter.Eq(c => c.ProvinceID, (short)EnumBAGRegionType.Province);
                await _collection.DeleteOneAsync(filter);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.Segment_Clear]" ,
                //            new SqlParameter("@ProvinceID", provinceID));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("SegmentDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
