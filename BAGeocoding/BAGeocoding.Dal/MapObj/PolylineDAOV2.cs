using System;
using System.Data.SqlClient;
using BAGeocoding.Utility;

//using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.MapObj;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace BAGeocoding.Dal.MapObj
{
    /// <summary>
    /// Quản lý truy xuất thông tin đoạn đường
    /// </summary>
    public class PolylineDAOV2 : NoSqlHelper
    {
        private static IMongoCollection<BAGPolylineV2> _collection;

        public PolylineDAOV2()
        {
            ConventionRegistry.Register("elementNameConvention", pack, x => true);
            _collection = database.GetCollection<BAGPolylineV2>(nameof(BAGPolylineV2));
        }

        /// <summary>
        /// Thêm mới thông tin đoạn đường
        /// </summary>
        public static async Task<bool> Add(BAGPolylineV2 polyline)
        {
            try
            {
                await _collection.InsertOneAsync(polyline);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.PolylineInfo_Add]",
                //            new SqlParameter("@PolylineID", polyline.PolylineID),
                //            new SqlParameter("@NameStr", polyline.Name),
                //            new SqlParameter("@LngStr", polyline.LngStr),
                //            new SqlParameter("@LatStr", polyline.LatStr));

                //return exec > 0;
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("SegmentDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
