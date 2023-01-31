using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BAGeocoding.Utility;

//using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.Enum.MapObject;

namespace BAGeocoding.Dal.MapObj
{
    /// <summary>
    /// Quản lý truy xuất thông tin quận/huyện
    /// </summary>
    public class DistrictDAO : SQLHelper
    {
        //protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING);

        /// <summary>
        /// Lấy toàn bộ quận/huyện
        /// </summary>
        public static List<BAGDistrict> GetAll()
        {
            try
            {
                //DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[CTL.District_GetAll]",
                //                new SqlParameter("@EnumDistrict", EnumBAGRegionType.District));

                //if (dt != null)
                //{
                //    if (dt.Rows.Count > 1)
                //    {
                //        List<BAGDistrict> districtList = new List<BAGDistrict>();
                //        for (int i = 0; i < dt.Rows.Count; i++)
                //        {
                //            BAGDistrict districtItem = new BAGDistrict();
                //            if (districtItem.FromDataRow(dt.Rows[i]) == false)
                //                return null;
                //            districtList.Add(districtItem);
                //        }
                //        return districtList;
                //    }
                //}
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("DistrictDAO.GetAll, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách quận/huyện theo dữ liệu mở rộng
        /// </summary>
        public static List<BAGDistrict> GetByDataExt(params EnumMOBDistrictDataExt[] dataExtArray)
        {
            BAGDistrict provinceItem = new BAGDistrict();
            for (int i = 0; i < dataExtArray.Length; i++)
                provinceItem.DataExtSet(dataExtArray[i], true);
            return GetByDataExt(provinceItem.DataExt);
        }

        /// <summary>
        /// Lấy danh sách quận/huyện theo dữ liệu mở rộng
        /// </summary>
        public static List<BAGDistrict> GetByDataExt(int dataExt)
        {
            try
            {
                //DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[CTL.District_GetByDataExt]",
                //                new SqlParameter("@DataExt", dataExt));

                //if (dt != null)
                //{
                //    if (dt.Rows.Count > 1)
                //    {
                //        List<BAGDistrict> provinceList = new List<BAGDistrict>();
                //        for (int i = 0; i < dt.Rows.Count; i++)
                //        {
                //            BAGDistrict provinceItem = new BAGDistrict();
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
                LogFile.WriteError("DistrictDAO.GetByDataExt, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách quận/huyện ưu tiên thấp khi tìm kiếm
        /// </summary>
        public static List<BAGDistrict> GetPriorityLow()
        {
            try
            {
                BAGDistrict districtTemp = new BAGDistrict();
                districtTemp.DataExtSet(EnumMOBDistrictDataExt.Special, true);

                //DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[CTL.District_GetPriorityLow]",
                //                new SqlParameter("@DataExt", districtTemp.DataExt));

                //if (dt != null)
                //{
                //    if (dt.Rows.Count > 1)
                //    {
                //        List<BAGDistrict> provinceList = new List<BAGDistrict>();
                //        for (int i = 0; i < dt.Rows.Count; i++)
                //        {
                //            BAGDistrict provinceItem = new BAGDistrict();
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
                LogFile.WriteError("DistrictDAO.GetPriorityLow, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới thông tin quận/huyện
        /// </summary>
        public static bool Add(BAGDistrict district)
        {
            try
            {
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.District_Add]",
                //            new SqlParameter("@DistrictID", district.DistrictID),
                //            new SqlParameter("@ProvinceID", district.ProvinceID),
                //            new SqlParameter("@VName", district.VName),
                //            new SqlParameter("@EName", district.EName),
                //            new SqlParameter("@Description", district.Description ?? string.Empty),
                //            new SqlParameter("@PointCount", district.PointList.Count),
                //            new SqlParameter("@LngStr", district.LngStr ?? string.Empty),
                //            new SqlParameter("@LatStr", district.LatStr ?? string.Empty),
                //            new SqlParameter("@GeoStr", district.GeoStr ?? string.Empty));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("DistrictDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Cập nhật thông tin mở rộng
        /// </summary>
        public static bool UpdateDataExt(BAGDistrict district)
        {
            try
            {
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.District_DataExt]",
                //            new SqlParameter("@DistrictID", district.DistrictID),
                //            new SqlParameter("@DataExt", district.DataExt),
                //            new SqlParameter("@SortOrder", district.SortOrder));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("DistrictDAO.UpdateDataExt, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Hủy tất cả thông tin quận/huyện
        /// </summary>
        public static bool Clear()
        {
            try
            {
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.District_Clear]", null);

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("DistrictDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
