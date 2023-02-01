using BAGeocoding.Entity.MapImg;
using BAGeocoding.Utility;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace BAGeocoding.Dal.MapImg
{
    /// <summary>
    /// Quản lý danh sách đăng ký sử dụng bản đồ
    /// </summary>
    public class RequestURLDAOV2 : SQLHelper
    {
        private static IMongoCollection<BAGRequestURLV2> _collection;
        
        public RequestURLDAOV2()
        {
            var pack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("elementNameConvention", pack, x => true);
            var client = new MongoClient(Constants.MONGO_CONNECTION_STRING);
            var database = client.GetDatabase(Constants.MONGO_GEO_DB);
            _collection = database.GetCollection<BAGRequestURLV2>(nameof(BAGRequestURLV2));
        }

        public static async Task<List<BAGRequestURLV2>> GetAll(bool actived)
        {
            try
            {
                var dt =  await _collection.Find(new BsonDocument()).ToListAsync();
                if (dt != null)
                    return dt;
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RequestURLDAO.GetAll, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Xử lý đăng ký sử dụng bản đồ
        /// </summary>
        public static async Task<bool> Create(BAGRequestURLV2 request)
        {
            try
            {
                await _collection.InsertOneAsync(request);
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RequestURLDAO.Create, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Hủy đăng ký sử dụng bản đồ
        /// </summary>
        public static async Task<bool> Delete(BAGRequestURLV2 request)
        {
            try
            {

                var filter = Builders<BAGRequestURLV2>.Filter.Eq(c => c.IndexID, request.IndexID);
                //var update = Builders<BAGRequestURLV2>.Update
                //    .Set(c => c.IndexID, request.IndexID);
                //var result = await _collection.UpdateOneAsync(filter, update);
                await _collection.DeleteOneAsync(filter);
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RequestURLDAO.Delete, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
