
using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.SearchData;
using BAGeocoding.Entity.Utility;

using BAGeocoding.Utility;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace BAGeocoding.Dal.SearchData
{
    /// <summary>
    /// Quản lý truy xuất từ khóa tìm kiếm đường
    /// </summary>
    public class SegmentKeyDAOV2 : NoSqlHelper
    {
        private static IMongoCollection<UTLSearchKeyV2> _collectionUTLSearchKey;
        private static IMongoCollection<BAGRegionKeyV2> _collectionRegionKeyV2;
        private static IMongoCollection<BAGSegmentKeyV2> _collectionSegmentKeyV2;
        public SegmentKeyDAOV2()
        {
            ConventionRegistry.Register("elementNameConvention", pack, x => true);
            _collectionRegionKeyV2 = database.GetCollection<BAGRegionKeyV2>(nameof(BAGRegionKeyV2));
            _collectionUTLSearchKey = database.GetCollection<UTLSearchKeyV2>(nameof(UTLSearchKeyV2));
            _collectionSegmentKeyV2 = database.GetCollection<BAGSegmentKeyV2>(nameof(BAGSegmentKeyV2));
        }

        public static async Task<List<UTLSearchKeyV2>> GetByProvince(short provinceID)
        {
            try
            {
                var ds = await _collectionUTLSearchKey.Find(new BsonDocument()).ToListAsync();
                if (ds != null)
                    return ds;
                //DataSet ds = SQLHelper.ExecuteDataset(sqlDB, "[DTS.SegmentKey_GetByProvince]",
                //                new SqlParameter("@ProvinceID", provinceID));

                //if (ds != null)
                //{
                //    if (ds.Tables.Count > 1)
                //    {
                //        if (ds.Tables[0].Rows.Count > 0)
                //        {
                //            List<UTLSearchKey> segmentList = new List<UTLSearchKey>();
                //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //            {
                //                UTLSearchKey segmentItem = new UTLSearchKey();
                //                if (segmentItem.FromDataRow(ds.Tables[0].Rows[i], ds.Tables[1], true, true) == false)
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
                LogFile.WriteError("SegmentKeyDAO.GetByProvince, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới thông tin thông tin tìm kiếm đường
        /// </summary>
        public static async Task<bool> Add(BAGSegmentKeyV2 regionKey, int keyShift)
        {
            try
            {
                await _collectionSegmentKeyV2.InsertOneAsync(regionKey);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.SegmentKey_Add]",
                //            new SqlParameter("@ProvinceID", regionKey.ProvinceID),
                //            new SqlParameter("@SegmentID", regionKey.SegmentID),
                //            new SqlParameter("@KeyStr", regionKey.KeyStr),
                //            new SqlParameter("@IndexID", regionKey.IndexID),
                //            new SqlParameter("@Rate", regionKey.Rate),
                //            new SqlParameter("@KeyShift", keyShift));

                //return exec > 0;
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("SegmentKeyDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Hủy thông tin tìm kiếm đường
        /// </summary>
        public static async Task<bool> Clear(EnumBAGRegionType typeID)
        {
            try
            {
                var filter = Builders<BAGRegionKeyV2>.Filter.Eq(c => c.TypeID, (byte)typeID);
                await _collectionRegionKeyV2.DeleteOneAsync(filter);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.RegionKey_Clear]",
                //            new SqlParameter("@TypeID", typeID));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("SegmentKeyDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
