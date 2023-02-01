using System;
using System.Data.SqlClient;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace BAGeocoding.Dal.MapObj
{
    /// <summary>
    /// Quản lý truy xuất tọa độ đoạn đường
    /// </summary>
    public class PlacePointDAOV2 : NoSqlHelper
    {
        private static IMongoCollection<BAGPointV2> _collectionBAGPointV2;
        private static IMongoCollection<BAGPlaceV2> _collectionBAGPlaceV2;

        public PlacePointDAOV2()
        {
            ConventionRegistry.Register("elementNameConvention", pack, x => true);
            _collectionBAGPointV2 = database.GetCollection<BAGPointV2>(nameof(BAGPointV2));
            _collectionBAGPlaceV2 = database.GetCollection<BAGPlaceV2>(nameof(BAGPlaceV2));
        }

        /// <summary>
        /// Thêm mới tọa độ đối tượng
        /// </summary>
        public static async Task<bool> Add(int placeID, BAGPointV2 point, int indexID)
        {
            try
            {
                await _collectionBAGPointV2.InsertOneAsync(point);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.PlacePoint_Add]",
                //            new SqlParameter("@PlaceID", placeID),
                //            new SqlParameter("@Lng", point.Lng),
                //            new SqlParameter("@Lat", point.Lat),
                //            new SqlParameter("@IndexID", indexID));

                //return exec > 0;
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PlacePointDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Thêm mới tọa độ đối tượng
        /// </summary>
        public static async Task<bool> AddStr(BAGPlaceV2 placeData)
        {
            try
            {;
                await _collectionBAGPlaceV2.InsertOneAsync(placeData);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.PlacePoint_AddStr]",
                //            new SqlParameter("@PlaceID", placeData.PlaceID),
                //            new SqlParameter("@LngStr", placeData.LngStr),
                //            new SqlParameter("@LatStr", placeData.LatStr));

                //return exec > 0;
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PlacePointDAO.AddStr, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Thêm mới tọa độ đối tượng
        /// </summary>
        public static async Task<bool> AddStr(int placeID, BAGPlaceV2 placeData)
        {
            try
            {
                placeData.PlaceID= placeID;
                await _collectionBAGPlaceV2.InsertOneAsync(placeData);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.PlacePoint_AddStr]",
                //            new SqlParameter("@PlaceID", placeID),
                //            new SqlParameter("@LngStr", placeData.LngStr),
                //            new SqlParameter("@LatStr", placeData.LatStr));

                //return exec > 0;
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PlacePointDAO.AddStr, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
