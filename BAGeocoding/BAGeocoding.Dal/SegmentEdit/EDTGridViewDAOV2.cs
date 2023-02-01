using System;
using System.Data.SqlClient;
using BAGeocoding.Utility;


using BAGeocoding.Entity.MapObj;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using BAGeocoding.Entity.SearchData;

namespace BAGeocoding.Dal.SegmentEdit
{
    /// <summary>
    /// Quản lý truy xuất thông tin đoạn đường
    /// </summary>
    public class EDTGridViewDAOV2: NoSqlHelper
    {
        private static IMongoCollection<BAGGridViewV2> _collection;

        public EDTGridViewDAOV2()
        {
            ConventionRegistry.Register("elementNameConvention", pack, x => true);
            _collection = database.GetCollection<BAGGridViewV2>(nameof(BAGGridViewV2));
        }

        public static async Task<bool> Add(BAGGridViewV2 gridView)
        {
            try
            {
                await _collection.InsertOneAsync(gridView);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[EDT.GridView_Add]",
                //            new SqlParameter("@GridID", gridView.GridID),
                //            new SqlParameter("@ProvinceID", gridView.ProvinceID),
                //            new SqlParameter("@Name", gridView.Name),
                //            new SqlParameter("@LngStr", gridView.LngStr),
                //            new SqlParameter("@LatStr", gridView.LatStr));

                //return exec > 0;
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("EDTGridViewDAO.Add, ex: " + ex.ToString());
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
                var filter = Builders<BAGGridViewV2>.Filter.Eq(c => c.ProvinceID, provinceID);
                await _collection.DeleteOneAsync(filter);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[EDT.GridView_Clear]",
                //            new SqlParameter("@ProvinceID", provinceID));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("EDTGridViewDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
