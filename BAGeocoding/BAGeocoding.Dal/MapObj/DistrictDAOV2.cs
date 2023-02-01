using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.Enum.MapObject;
using BAGeocoding.Entity.MapObj;
using BAGeocoding.Utility;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System.Data;

namespace BAGeocoding.Dal.MapObj
{
    /// <summary>
    /// Quản lý truy xuất thông tin quận/huyện
    /// </summary>
    public class DistrictDAOV2 : NoSqlHelper
    {
        private static IMongoCollection<BAGDistrictV2> _collection;

        public DistrictDAOV2()
        {
            ConventionRegistry.Register("elementNameConvention", pack, x => true);
            _collection = database.GetCollection<BAGDistrictV2>(nameof(BAGDistrictV2));
        }

        /// <summary>
        /// Lấy toàn bộ quận/huyện
        /// </summary>
        public static async Task<List<BAGDistrictV2>> GetAll()
        {
            try
            {
                var dt = await _collection.Find(x => x.DistrictID == (short)EnumBAGRegionType.District).ToListAsync();

                if (dt != null)
                    return dt;

                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("DistrictDAO.GetAll, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách quận/huyện theo dữ liệu mở rộng
        /// </summary>
        public static async Task<List<BAGDistrictV2>> GetByDataExt(params EnumMOBDistrictDataExt[] dataExtArray)
        {
            BAGDistrictV2 provinceItem = new BAGDistrictV2();
            for (int i = 0; i < dataExtArray.Length; i++)
                provinceItem.DataExtSet(dataExtArray[i], true);
            return await GetByDataExt(provinceItem.DataExt);
        }

        /// <summary>
        /// Lấy danh sách quận/huyện theo dữ liệu mở rộng
        /// </summary>
        public static async Task<List<BAGDistrictV2>> GetByDataExt(int dataExt)
        {
            try
            {
                var dt = await _collection.Find(x => x.DistrictID == (short)EnumBAGRegionType.District && x.DataExt == dataExt).ToListAsync();
                if (dt != null)
                    return dt;

                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("DistrictDAO.GetByDataExt, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách quận/huyện ưu tiên thấp khi tìm kiếm
        /// </summary>
        public static async Task<List<BAGDistrictV2>> GetPriorityLow()
        {
            try
            {
                BAGDistrictV2 districtTemp = new BAGDistrictV2();
                districtTemp.DataExtSet(EnumMOBDistrictDataExt.Special, true);

                var dt = await _collection.Find(x => x.DistrictID == (short)EnumBAGRegionType.District && x.DataExt == districtTemp.DataExt).ToListAsync();
                if (dt != null)
                    return dt;

                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("DistrictDAO.GetPriorityLow, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới thông tin quận/huyện
        /// </summary>
        public static async Task<bool> Add(BAGDistrictV2 district)
        {
            try
            {
                district.Description = district.Description ?? string.Empty;
                district.LngStr = district.LngStr ?? string.Empty;
                district.LatStr = district.LatStr ?? string.Empty;
                district.GeoStr = district.GeoStr ?? string.Empty;
                await _collection.InsertOneAsync(district);
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("DistrictDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Cập nhật thông tin mở rộng
        /// </summary>
        public static async Task<bool> UpdateDataExt(BAGDistrictV2 district)
        {
            try
            {
                var filter = Builders<BAGDistrictV2>.Filter.Eq(c => c.DistrictID, district.DistrictID);
                var update = Builders<BAGDistrictV2>.Update
                    .Set(c => c.DataExt, district.DataExt)
                    .Set(c => c.SortOrder, district.SortOrder);
                await _collection.UpdateOneAsync(filter, update);
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("DistrictDAO.UpdateDataExt, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Hủy tất cả thông tin quận/huyện
        /// </summary>
        public static async Task<bool> Clear()
        {
            try
            {
                var filter = Builders<BAGDistrictV2>.Filter.Eq(x => x.DistrictID, (short)EnumBAGRegionType.District);
                await _collection.DeleteOneAsync(filter);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.District_Clear]", null);
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("DistrictDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
