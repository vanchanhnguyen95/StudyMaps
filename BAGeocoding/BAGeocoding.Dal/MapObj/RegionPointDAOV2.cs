using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace BAGeocoding.Dal.MapObj
{
    /// <summary>
    /// Quản lý truy xuất tọa độ đối tượng
    /// </summary>
    public class RegionPointDAOV2 : NoSqlHelper
    {

        private static IMongoCollection<BAGPointV2> _collection;

        public RegionPointDAOV2()
        {
            ConventionRegistry.Register("elementNameConvention", pack, x => true);
            _collection = database.GetCollection<BAGPointV2>(nameof(BAGPointV2));
        }

        /// <summary>
        /// Thêm mới tọa độ đối tượng
        /// </summary>
        public static async Task<bool> Add(EnumBAGRegionType typeID, int objectID, BAGPointV2 point, int sortOrder)
        {
            try
            {
                await _collection.InsertOneAsync(point);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.RegionPoint_Add]",
                //            new SqlParameter("@TypeID", typeID),
                //            new SqlParameter("@ObjectID", objectID),
                //            new SqlParameter("@Lng", point.Lng),
                //            new SqlParameter("@Lat", point.Lat),
                //            new SqlParameter("@SortOrder", sortOrder));

                //return exec > 0;
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("ProvinceDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Thêm mới tọa độ đối tượng
        /// </summary>
        public static async Task<bool> Add(EnumBAGRegionType typeID, int objectID, float longtitude, float latitude, int sortOrder)
        {
            try
            {
                BAGPointV2 point = new BAGPointV2();
                point.Lat = longtitude;
                point.Lat = latitude;
                await _collection.InsertOneAsync(point);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.RegionPoint_Add]",
                //            new SqlParameter("@TypeID", typeID),
                //            new SqlParameter("@ObjectID", objectID),
                //            new SqlParameter("@Lng", longtitude),
                //            new SqlParameter("@Lat", latitude),
                //            new SqlParameter("@SortOrder", sortOrder));

                //return exec > 0;
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("ProvinceDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
