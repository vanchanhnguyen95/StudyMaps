using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.SearchData;
using BAGeocoding.Entity.Utility;

using BAGeocoding.Utility;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace BAGeocoding.Dal.SearchData
{
    /// <summary>
    /// Quản lý truy xuất từ khóa tìm kiếm vùng
    /// </summary>
    public class RegionKeyDAOV2 : NoSqlHelper
    {
        private static IMongoCollection<UTLSearchKeyV2> _collectionUTLSearchKey;
        private static IMongoCollection<BAGRegionKeyV2> _collectionRegionKey;
        public RegionKeyDAOV2()
        {
            ConventionRegistry.Register("elementNameConvention", pack, x => true);
            _collectionUTLSearchKey = database.GetCollection<UTLSearchKeyV2>(nameof(UTLSearchKeyV2));
            _collectionRegionKey = database.GetCollection<BAGRegionKeyV2>(nameof(BAGRegionKeyV2));
        }
        public static async Task<List<UTLSearchKeyV2>> GetByType(EnumBAGRegionType typeID)
        {
            try
            {
                var ds = _collectionUTLSearchKey.Find(new BsonDocument()).ToListAsync();
                //DataSet ds = SQLHelper.ExecuteDataset(sqlDB, "[CTL.RegionKey_GetByType]",
                //                new SqlParameter("@TypeID", typeID),
                //                new SqlParameter("@TypeDistrict", EnumBAGRegionType.District));

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
                //                if (segmentItem.FromDataRow(ds.Tables[0].Rows[i], ds.Tables[1], false, typeID == EnumBAGRegionType.District) == false)
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
                LogFile.WriteError("RegionKeyDAO.GetByType, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới thông tin thông tin tìm kiếm vùng
        /// </summary>
        public static async Task<bool> Add(BAGRegionKeyV2 regionKey)
        {
            try
            {
                await _collectionRegionKey.InsertOneAsync(regionKey);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.RegionKey_Add]",
                //            new SqlParameter("@TypeID", regionKey.TypeID),
                //            new SqlParameter("@KeyStr", regionKey.KeyStr),
                //            new SqlParameter("@ObjectID", regionKey.ObjectID),
                //            new SqlParameter("@IndexID", regionKey.IndexID),
                //            new SqlParameter("@Rate", regionKey.Rate));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RegionKeyDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Hủy thông tin tìm kiếm vùng
        /// </summary>
        public static async Task<bool> Clear(EnumBAGRegionType typeID)
        {
            try
            {
                var filter = Builders<BAGRegionKeyV2>.Filter.Eq(c => c.EnumTypeID, typeID);
                await _collectionRegionKey.DeleteOneAsync(filter);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.RegionKey_Clear]",
                //            new SqlParameter("@TypeID", typeID));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RegionKeyDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
