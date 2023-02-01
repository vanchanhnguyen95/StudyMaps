using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace BAGeocoding.Dal.MapObj
{
    /// <summary>
    /// Quản lý truy xuất thông POI
    /// </summary>
    public class PointOfInterestDAOV2 : NoSqlHelper
    {
        private static IMongoCollection<BAGPointOfInterestV2> _collection;

        public PointOfInterestDAOV2()
        {
            ConventionRegistry.Register("elementNameConvention", pack, x => true);
            _collection = database.GetCollection<BAGPointOfInterestV2>(nameof(BAGPointOfInterestV2));
        }

        /// <summary>
        /// Lấy khu vực theo tỉnh
        /// </summary>
        public static async Task<List<BAGPointOfInterestV2>> GetByProvince(int provinceID)
        {
            try
            {
                var ds = await _collection.Find(x => x.ProvinceID == provinceID).ToListAsync();
                if (ds != null)
                {
                    return ds;
                }
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PointOfInterestDAO.GetByProvince, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới thông tin khu vực
        /// </summary>
        public static async Task<bool> Add(BAGPointOfInterestV2 poiInfo)
        {
            try
            {
                await _collection.InsertOneAsync(poiInfo);
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PointOfInterestDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }
        
        /// <summary>
        /// Hủy tất cả khu vực theo tỉnh
        /// </summary>
        public static async Task<bool> Clear(short provinceID)
        {
            try
            {
                var filter = Builders<BAGPointOfInterestV2>.Filter.Eq(x => x.ProvinceID, provinceID);
                await _collection.DeleteOneAsync(filter);
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PointOfInterestDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
