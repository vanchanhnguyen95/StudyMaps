using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BAGeocoding.Utility;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.Enum.MapObject;
using System.Collections;

namespace BAGeocoding.Dal.MapObj
{
    /// <summary>
    /// Quản lý truy xuất thông tin tỉnh/thành
    /// </summary>
    public class ProvinceDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING);
        
        /// <summary>
        /// Lấy toàn bộ tỉnh/thành
        /// </summary>
        public static List<BAGProvince> GetAll()
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[CTL.Province_GetAll]",
                                new SqlParameter("@EnumProvince", EnumBAGRegionType.Province));

                if (dt != null)
                {
                    if (dt.Rows.Count > 1)
                    {
                        List<BAGProvince> provinceList = new List<BAGProvince>();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            BAGProvince provinceItem = new BAGProvince();
                            if (provinceItem.FromDataRow(dt.Rows[i]) == false)
                                return null;
                            provinceList.Add(provinceItem);
                        }
                        return provinceList;
                    }
                }
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
        public static Hashtable GetForCheck()
        {
            try
            {
                List<BAGProvince> list = GetAll();
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
        public static List<BAGProvince> GetByDataExt(params EnumMOBProvinceDataExt[] dataExtArray)
        {
            BAGProvince provinceItem = new BAGProvince();
            for (int i = 0; i < dataExtArray.Length; i++)
                provinceItem.DataExtSet(dataExtArray[i], true);
            return GetByDataExt(provinceItem.DataExt);
        }

        /// <summary>
        /// Lấy danh sách tỉnh/thành theo dữ liệu mở rộng
        /// </summary>
        private static List<BAGProvince> GetByDataExt(int dataExt)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[CTL.Province_GetByDataExt]",
                                new SqlParameter("@DataExt", dataExt));

                if (dt != null)
                {
                    if (dt.Rows.Count > 1)
                    {
                        List<BAGProvince> provinceList = new List<BAGProvince>();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            BAGProvince provinceItem = new BAGProvince();
                            if (provinceItem.FromDataRow(dt.Rows[i]) == false)
                                return null;
                            provinceList.Add(provinceItem);
                        }
                        return provinceList;
                    }
                }
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
        public static bool Add(BAGProvince province)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.Province_Add]",
                            new SqlParameter("@ProvinceID", province.ProvinceID),
                            new SqlParameter("@VName", province.VName),
                            new SqlParameter("@EName", province.EName),
                            new SqlParameter("@Description", province.Description ?? string.Empty),
                            new SqlParameter("@PointCount", province.PointList.Count),
                            new SqlParameter("@LngStr", province.LngStr ?? string.Empty),
                            new SqlParameter("@LatStr", province.LatStr ?? string.Empty),
                            new SqlParameter("@GeoStr", province.GeoStr ?? string.Empty),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
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
        public static bool UpdateDataExt(BAGProvince province)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.Province_DataExt]",
                            new SqlParameter("@ProvinceID", province.ProvinceID),
                            new SqlParameter("@DataExt", province.DataExt),
                            new SqlParameter("@SortOrder", province.SortOrder),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
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
        public static bool Clear()
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.Province_Clear]", null);

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("ProvinceDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
