using BAGeocoding.Entity.MapObj;
using BAGeocoding.Utility;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace BAGeocoding.Dal.MapObj
{
    /// <summary>
    /// Quản lý truy xuất thông tin tên đường đặc biệt
    /// </summary>
    public class RoadSpecialDAOV2 : NoSqlHelper
    {
        private static IMongoCollection<BAGRoadSpecialV2> _collection;

        public RoadSpecialDAOV2()
        {
            ConventionRegistry.Register("elementNameConvention", pack, x => true);
            _collection = database.GetCollection<BAGRoadSpecialV2>(nameof(BAGRoadSpecialV2));
        }

        /// <summary>
        /// Lấy tất cả danh sách tên đường đặc biệt
        /// </summary>
        public static async Task<List<BAGRoadSpecialV2>> GetAll()
        {
            try
            {
                var dt = await _collection.Find(new BsonDocument()).ToListAsync();
                if (dt != null)
                    return dt;
                //DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[EXT.RoadSpecial_GetAll]", null);

                //if (dt != null)
                //{
                //    List<BAGRoadSpecial> roadList = new List<BAGRoadSpecial>();
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        BAGRoadSpecial roadItem = new BAGRoadSpecial();
                //        if (roadItem.FromDataRow(dt.Rows[i]) == false)
                //            return null;
                //        roadList.Add(roadItem);
                //    }
                //    return roadList;
                //}
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RoadSpecialDAO.GetAll, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới thông tin tên đường đặc biệt
        /// </summary>
        public static async Task<bool> Add(BAGRoadSpecialV2 tile)
        {
            try
            {
                await _collection.InsertOneAsync(tile);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[EXT.RoadSpecial_Add]",
                //            new SqlParameter("@RoadName", tile.RoadName),
                //            new SqlParameter("@Description", tile.Description ?? string.Empty),
                //            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                //return exec > 0;
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RoadSpecialDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Cập nhật thông tin tên đường đặc biệt
        /// </summary>
        public static async Task<bool> Update(BAGRoadSpecialV2 tile)
        {
            try
            {
                var filter = Builders<BAGRoadSpecialV2>.Filter.Eq(c => c.IndexID, tile.IndexID);
                var update = Builders<BAGRoadSpecialV2>.Update
                    .Set(c => c.RoadName, tile.RoadName)
                    .Set(c => c.Description, tile.Description);
                await _collection.UpdateOneAsync(filter, update);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[EXT.RoadSpecial_Update]",
                //            new SqlParameter("@IndexID", tile.IndexID),
                //            new SqlParameter("@RoadName", tile.RoadName),
                //            new SqlParameter("@Description", tile.Description),
                //            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                //return exec > 0;
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RoadSpecialDAO.Update, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Hủy tên đường đặc biệt
        /// </summary>
        public static async Task<bool> Delete(int indexID)
        {
            try
            {
                var filter = Builders<BAGRoadSpecialV2>.Filter.Eq(c => c.IndexID, indexID);
                await _collection.DeleteOneAsync(filter);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[EXT.RoadSpecial_Delete]",
                //            new SqlParameter("@IndexID", indexID));

                //return exec > 0;
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RoadSpecialDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
