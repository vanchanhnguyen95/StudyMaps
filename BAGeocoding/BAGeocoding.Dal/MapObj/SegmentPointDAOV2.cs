using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace BAGeocoding.Dal.MapObj
{
    /// <summary>
    /// Quản lý truy xuất tọa độ đoạn đường
    /// </summary>
    public class SegmentPointDAOV2 : NoSqlHelper
    {
        private static IMongoCollection<BAGPointV2> _collectionBAGPointV2;
        private static IMongoCollection<BAGSegmentV2> _collectionBAGSegmentV2;

        public SegmentPointDAOV2()
        {
            ConventionRegistry.Register("elementNameConvention", pack, x => true);
            _collectionBAGPointV2 = database.GetCollection<BAGPointV2>(nameof(BAGPointV2));
            _collectionBAGSegmentV2 = database.GetCollection<BAGSegmentV2>(nameof(BAGSegmentV2));
        }

        /// <summary>
        /// Thêm mới tọa độ đối tượng
        /// </summary>
        public static async Task<bool> Add(int segmentID, BAGPointV2 point, int indexID)
        {
            try
            {
                await _collectionBAGPointV2.InsertOneAsync(point);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.SegmentPoint_Add]",
                //            new SqlParameter("@SegmentID", segmentID),
                //            new SqlParameter("@Lng", point.Lng),
                //            new SqlParameter("@Lat", point.Lat),
                //            new SqlParameter("@IndexID", indexID));

                //return exec > 0;
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("SegmentPointDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Thêm mới tọa độ đối tượng
        /// </summary>
        public static async Task<bool> AddStr(BAGSegmentV2 segment)
        {
            try
            {
                await _collectionBAGSegmentV2.InsertOneAsync(segment);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.SegmentPoint_AddStr]",
                //            new SqlParameter("@SegmentID", segment.SegmentID),
                //            new SqlParameter("@LngStr", segment.LngStr),
                //            new SqlParameter("@LatStr", segment.LatStr));

                //return exec > 0;
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("SegmentPointDAO.AddStr, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
