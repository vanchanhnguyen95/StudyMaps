using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.Enum.MapObject;
using BAGeocoding.Entity.MapObj;
using BAGeocoding.Utility;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System.Collections;

namespace BAGeocoding.Dal.MapObj
{
    /// <summary>
    /// Quản lý truy xuất thông tin tỉnh/thành
    /// </summary>
    public class ProvinceDAOV2 : NoSqlHelper
    {
        private static IMongoCollection<BAGProvinceV2> _collection;

        public ProvinceDAOV2()
        {
            ConventionRegistry.Register("elementNameConvention", pack, x => true);
            _collection = database.GetCollection<BAGProvinceV2>(nameof(BAGProvinceV2));
        }

        /// <summary>
        /// Lấy toàn bộ tỉnh/thành
        /// </summary>
        public static async Task<List<BAGProvinceV2>> GetAll()
        {
            try
            {
                var ds = await _collection.Find(x => x.ProvinceID == (short)EnumBAGRegionType.Province).ToListAsync();
                if (ds != null)
                    return ds;

                 return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("ProvinceDAO.GetAll, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy toàn bộ tỉnh/thành để ktra
        /// </summary>
        public static async Task<Hashtable> GetForCheck()
        {
            try
            {
                List<BAGProvinceV2> list = await GetAll();
                if (list == null || list.Count == 0)
                    return null;
                Hashtable ht = new Hashtable();
                for (int i = 0; i < list.Count; i++)
                {
                    if (ht.ContainsKey(list[i].ProvinceID) == true)
                        return null;
                    ht.Add(list[i].ProvinceID, list[i]);
                }
                return ht;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("ProvinceDAO.GetForCheck, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách tỉnh/thành theo dữ liệu mở rộng
        /// </summary>
        public static async Task<List<BAGProvinceV2>> GetByDataExt(params EnumMOBProvinceDataExt[] dataExtArray)
        {
            BAGProvinceV2 provinceItem = new BAGProvinceV2();
            for (int i = 0; i < dataExtArray.Length; i++)
                provinceItem.DataExtSet(dataExtArray[i], true);
            return await GetByDataExt(provinceItem.DataExt);
        }

        /// <summary>
        /// Lấy danh sách tỉnh/thành theo dữ liệu mở rộng
        /// </summary>
        private static async Task<List<BAGProvinceV2>> GetByDataExt(int dataExt)
        {
            try
            {
                var dt = await _collection.Find(x => x.ProvinceID == (short)EnumBAGRegionType.Province && x.DataExt == dataExt).ToListAsync();
                if (dt != null)
                    return dt;
                //DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[CTL.Province_GetByDataExt]",
                //                new SqlParameter("@DataExt", dataExt));

                //if (dt != null)
                //{
                //    if (dt.Rows.Count > 1)
                //    {
                //        List<BAGProvince> provinceList = new List<BAGProvince>();
                //        for (int i = 0; i < dt.Rows.Count; i++)
                //        {
                //            BAGProvince provinceItem = new BAGProvince();
                //            if (provinceItem.FromDataRow(dt.Rows[i]) == false)
                //                return null;
                //            provinceList.Add(provinceItem);
                //        }
                //        return provinceList;
                //    }
                //}
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("ProvinceDAO.GetByDataExt, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới thông tin tỉnh/thành
        /// </summary>
        public static async Task<bool> Add(BAGProvinceV2 province)
        {
            try
            {
                province.Description = province.Description ?? string.Empty;
                province.LngStr = province.LngStr ?? string.Empty;
                province.LatStr = province.LatStr ?? string.Empty;
                province.GeoStr = province.GeoStr ?? string.Empty;
                //province.CurrentTime = province.GeoStr ?? string.Empty;
                await _collection.InsertOneAsync(province);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.Province_Add]",
                //            new SqlParameter("@ProvinceID", province.ProvinceID),
                //            new SqlParameter("@VName", province.VName),
                //            new SqlParameter("@EName", province.EName),
                //            new SqlParameter("@Description", province.Description ?? string.Empty),
                //            new SqlParameter("@PointCount", province.PointList.Count),
                //            new SqlParameter("@LngStr", province.LngStr ?? string.Empty),
                //            new SqlParameter("@LatStr", province.LatStr ?? string.Empty),
                //            new SqlParameter("@GeoStr", province.GeoStr ?? string.Empty),
                //            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("ProvinceDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Cập nhật thông tin mở rộng
        /// </summary>
        public static async Task<bool> UpdateDataExt(BAGProvinceV2 province)
        {
            try
            {
                var filter = Builders<BAGProvinceV2>.Filter.Eq(c => c.ProvinceID, province.ProvinceID);
                var update = Builders<BAGProvinceV2>.Update
                    .Set(c => c.DataExt, province.DataExt)
                    .Set(c => c.SortOrder, province.SortOrder);
                await _collection.UpdateOneAsync(filter, update);

                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.Province_DataExt]",
                //            new SqlParameter("@ProvinceID", province.ProvinceID),
                //            new SqlParameter("@DataExt", province.DataExt),
                //            new SqlParameter("@SortOrder", province.SortOrder),
                //            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                //return exec > 0;
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("ProvinceDAO.UpdateDataExt, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Xóa tất cả thông tin tỉnh/thành
        /// </summary>
        public static async Task<bool> Clear()
        {
            try
            {
                var filter = Builders<BAGProvinceV2>.Filter.Eq(c => c.ProvinceID, (short)EnumBAGRegionType.Province);
                await _collection.DeleteOneAsync(filter);
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.Province_Clear]", null);

                //return exec > 0;
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("ProvinceDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
