using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapObj;
using BAGeocoding.Utility;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace BAGeocoding.Dal.MapObj
{
    /// <summary>
    /// Quản lý truy xuất thông tin xã/phường
    /// </summary>
    public class CommuneDAOV2 : NoSqlHelper
    {
        private static IMongoCollection<BAGCommuneV2> _collection;

        public CommuneDAOV2()
        {
            ConventionRegistry.Register("elementNameConvention", pack, x => true);
            _collection = database.GetCollection<BAGCommuneV2>(nameof(BAGCommuneV2));
        }

        public static async Task<List<BAGCommuneV2>> GetAll()
        {
            try
            {
                var ds = await _collection.Find(new BsonDocument()).ToListAsync();
                if (ds != null)
                    return ds;
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("CommuneDAO.GetAll, ex: " + ex.ToString());
                return null;
            }
        }

        public static async Task<List<BAGCommuneV2>> GetForManager()
        {
            try
            {
                //var dt = _collection.Find($"{{ EnumCommune: {EnumBAGRegionType.Commune}) }}").ToListAsync();
                var dt = await _collection.Find(x => x.CommuneID == (short)EnumBAGRegionType.Commune).ToListAsync();
                if (dt != null)
                    return dt;
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("CommuneDAO.GetForManager, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới thông tin xã/phường
        /// </summary>
        public static async Task<bool> Add(BAGCommuneV2 commune)
        {
            try
            {
                commune.Description = commune.Description?? string.Empty;
                commune.LngStr = commune.LngStr ?? string.Empty;
                commune.LatStr = commune.LatStr ?? string.Empty;
                commune.GeoStr = commune.GeoStr ?? string.Empty;
                await _collection.InsertOneAsync(commune);
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("CommuneDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Cập nhật tên xã/phường
        /// </summary>
        public static async Task<bool> UpdateName(BAGCommuneV2 commune)
        {
            try
            {
                var filter = Builders<BAGCommuneV2>.Filter.Eq(c => c.CommuneID, commune.CommuneID);
                var update = Builders<BAGCommuneV2>.Update
                    .Set(c => c.VName, commune.VName)
                    .Set(c => c.EName, commune.EName);
                await _collection.UpdateOneAsync(filter, update);
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("CommuneDAO.UpdateName, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Hủy tất cả thông tin xã/phường
        /// </summary>
        public static async Task<bool> Clear()
        {
            try
            {
                var filter = Builders<BAGCommuneV2>.Filter.Eq(c => c.CommuneID, (short)EnumBAGRegionType.Commune);
                await _collection.DeleteOneAsync(filter);
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("CommuneDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
