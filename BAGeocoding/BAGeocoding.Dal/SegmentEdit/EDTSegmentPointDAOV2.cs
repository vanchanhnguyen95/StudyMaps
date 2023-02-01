using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace BAGeocoding.Dal.SegmentEdit
{
    /// <summary>
    /// Quản lý truy xuất tọa độ đoạn đường
    /// </summary>
    public class EDTSegmentPointDAOV2 : NoSqlHelper
    {

        private static IMongoCollection<BAGPointV2> _collectionPoint;
        private static IMongoCollection<BAGSegmentV2> _collectionSegment;

        public EDTSegmentPointDAOV2()
        {
            ConventionRegistry.Register("elementNameConvention", pack, x => true);
            _collectionPoint = database.GetCollection<BAGPointV2>(nameof(BAGPointV2));
            _collectionSegment = database.GetCollection<BAGSegmentV2>(nameof(BAGSegmentV2));
        }

        /// <summary>
        /// Thêm mới tọa độ đối tượng
        /// </summary>
        public static async Task<bool> Add(int segmentID, BAGPointV2 point, int indexID)
        {
            try
            {
                await _collectionPoint.InsertOneAsync(point);
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
                LogFile.WriteError("EDTSegmentPointDAO.Add, ex: " + ex.ToString());
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
                await _collectionSegment.InsertOneAsync(segment);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[EDT.SegmentPoint_AddStr]",
                //            new SqlParameter("@SegmentID", segment.SegmentID),
                //            new SqlParameter("@LngStr", segment.LngStr.Replace('@', ',')),
                //            new SqlParameter("@LatStr", segment.LatStr.Replace('@', ',')));

                //return exec > 0;
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("EDTSegmentPointDAO.AddStr, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
