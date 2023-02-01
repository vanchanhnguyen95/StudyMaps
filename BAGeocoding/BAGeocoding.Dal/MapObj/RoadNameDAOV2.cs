using BAGeocoding.Entity.MapObj;
using BAGeocoding.Utility;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace BAGeocoding.Dal.MapObj
{
    /// <summary>
    /// Quản lý truy xuất tên đường
    /// </summary>
    public class RoadNameDAOV2 : NoSqlHelper
    {
        private static IMongoCollection<BAGRoadNameV2> _collection;

        public RoadNameDAOV2()
        {
            ConventionRegistry.Register("elementNameConvention", pack, x => true);
            _collection = database.GetCollection<BAGRoadNameV2>(nameof(BAGRoadNameV2));
        }

        /// <summary>
        /// Lấy khu vực theo tỉnh
        /// </summary>
        public static async Task<List<BAGRoadNameV2>> GetForCreate(int provinceID)
        {
            try
            {
                var dt = await _collection.Find( x => x.ProvinceID == provinceID).ToListAsync();

                if(dt != null)
                    return dt;
                //DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[DTS.RoadName_GetForCreate]", 
                //                new SqlParameter("@ProvinceID", provinceID));

                //if (dt == null)
                //    return null;
                //else if (dt.Rows.Count == 0)
                //    return null;

                //List<BAGRoadName> roadList = new List<BAGRoadName>();
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    BAGRoadName roadInfo = new BAGRoadName();
                //    if (roadInfo.FromDataCreate(dt.Rows[i]) == false)
                //        return null;
                //    roadList.Add(roadInfo);
                //}
                //return roadList;
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RoadNameDAO.GetForCreate, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy khu vực theo tỉnh
        /// </summary>
        public static async Task<List<BAGRoadNameV2>> GetByProvince(int provinceID)
        {
            try
            {
                var dt = await _collection.Find(x => x.ProvinceID == provinceID).ToListAsync();

                if (dt != null)
                    return dt;
                //DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[DTS.RoadName_GetByProvince]",
                //                new SqlParameter("@ProvinceID", provinceID));

                //if (dt == null)
                //    return null;
                //else if (dt.Rows.Count == 0)
                //    return null;

                //List<BAGRoadName> roadList = new List<BAGRoadName>();
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    BAGRoadName roadInfo = new BAGRoadName();
                //    if (roadInfo.FromDataRow(dt.Rows[i]) == false)
                //        return null;
                //    roadList.Add(roadInfo);
                //}
                //return roadList;
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RoadNameDAO.GetByProvince, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới thông tin
        /// </summary>
        public static async Task<bool> Add(BAGRoadNameV2 poiInfo)
        {
            try
            {
                await _collection.InsertOneAsync(poiInfo);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.RoadName_Create]",
                //            new SqlParameter("@ProvinceID", poiInfo.ProvinceID),
                //            new SqlParameter("@RoadName", poiInfo.RoadName),
                //            new SqlParameter("@NameExt", poiInfo.NameExt),
                //            new SqlParameter("@Lng", poiInfo.Coords.Lng),
                //            new SqlParameter("@Lat", poiInfo.Coords.Lat));

                //return exec > 0;
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RoadNameDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }
        
        /// <summary>
        /// Hủy tất cả theo tỉnh
        /// </summary>
        public static async Task<bool> Clear(short provinceID)
        {
            try
            {
                var filter = Builders<BAGRoadNameV2>.Filter.Eq(c => c.ProvinceID, provinceID);
                await _collection.DeleteOneAsync(filter);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.RoadName_Clear]",
                //            new SqlParameter("@ProvinceID", provinceID));

                //return exec > 0;
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RoadNameDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
