using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapObj;
using BAGeocoding.Utility;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace BAGeocoding.Dal.MapObj
{
    /// <summary>
    /// Quản lý truy xuất thông tin khu vực
    /// </summary>
    public class PlaceDAOV2 : NoSqlHelper
    {
        private static IMongoCollection<BAGPlaceV2> _collection;

        public PlaceDAOV2()
        {
            ConventionRegistry.Register("elementNameConvention", pack, x => true);
            _collection = database.GetCollection<BAGPlaceV2>(nameof(BAGPlaceV2));
        }


        /// <summary>
        /// Lấy khu vực theo tỉnh
        /// </summary>
        public static async Task<List<BAGPlaceV2>> GetByProvince(int provinceID)
        {
            try
            {
                var ds = await _collection.Find(x => x.ParentID == provinceID).ToListAsync();
                if (ds != null)
                    return ds;
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PlaceDAO.GetByProvince, ex: " + ex.ToString());
                return null;
            }
        }
        
        /// <summary>
        /// Lấy khu vực theo loại
        /// </summary>
        public static async Task<List<BAGPlaceV2>> GetByType(EnumBAGPlaceType typeID)
        {
            try
            {
                var ds = await _collection.Find(x => x.TypeID == (byte)typeID).ToListAsync();
                if (ds != null)
                    return ds;
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PlaceDAO.GetByType, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới thông tin khu vực
        /// </summary>
        public static bool Add(BAGPlaceV2 placeData, ref int realID)
        {
            try
            {
                _collection.InsertOne(placeData);
                realID = (int)placeData.RealID;
                //SqlParameter prPlaceID = new SqlParameter("@PlaceID", realID);
                //prPlaceID.Direction = ParameterDirection.Output;

                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.Place_Add]",
                //            new SqlParameter("@TypeID", placeData.TypeID),
                //            new SqlParameter("@ParentID", placeData.ParentID),
                //            new SqlParameter("@Name", placeData.Name),
                //            new SqlParameter("@Address", placeData.Address),
                //            new SqlParameter("@Description", placeData.Description),
                //            new SqlParameter("@Lng", placeData.Center.Lng),
                //            new SqlParameter("@Lat", placeData.Center.Lat),
                //            prPlaceID);

                //realID = Convert.ToInt32(prPlaceID.Value);

                //return exec > 0;
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PlaceDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }
        
        /// <summary>
        /// Hủy tất cả khu vực theo tỉnh
        /// </summary>
        public static async Task<bool> Clear(short provinceID)
        {
            try
            {
                var filter = Builders<BAGPlaceV2>.Filter.Eq(x => x.ParentID, provinceID);
                await _collection.DeleteOneAsync(filter);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.Place_Clear]",
                //            new SqlParameter("@ProvinceID", provinceID),

                //            new SqlParameter("@EnumUrban", EnumBAGPlaceType.Urban),
                //            new SqlParameter("@EnumPortion", EnumBAGPlaceType.Portion),
                //            new SqlParameter("@EnumPlot", EnumBAGPlaceType.Plot));

                //return exec > 0;
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PlaceDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
