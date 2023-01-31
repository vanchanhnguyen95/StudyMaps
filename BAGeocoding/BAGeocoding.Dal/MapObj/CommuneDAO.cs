using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BAGeocoding.Utility;

//using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapObj;

namespace BAGeocoding.Dal.MapObj
{
    /// <summary>
    /// Quản lý truy xuất thông tin xã/phường
    /// </summary>
    public class CommuneDAO : SQLHelper
    {
        //protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING);

        public static List<BAGCommune> GetAll()
        {
            try
            {
                //DataSet ds = SQLHelper.ExecuteDataset(sqlDB, "[CTL.Commune_GetAll]",
                //                new SqlParameter("@EnumCommune", EnumBAGRegionType.Commune));

                //if (ds != null)
                //{
                //    if (ds.Tables.Count > 1)
                //    {
                //        if (ds.Tables[0].Rows.Count > 0)
                //        {
                //            List<BAGCommune> communeList = new List<BAGCommune>();
                //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //            {
                //                BAGCommune communeItem = new BAGCommune();
                //                if (communeItem.FromDataRow(ds.Tables[0].Rows[i], ds.Tables[1]) == false)
                //                    return null;
                //                communeList.Add(communeItem);
                //            }
                //            return communeList;
                //        }
                //    }
                //}
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("CommuneDAO.GetAll, ex: " + ex.ToString());
                return null;
            }
        }

        public static List<BAGCommune> GetForManager()
        {
            try
            {
                //DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[CTL.Commune_GetForManager]",
                //                new SqlParameter("@EnumCommune", EnumBAGRegionType.Commune));

                //if (dt == null)
                //    return null;
                //else if (dt.Rows.Count == 0)
                //    return null;

                //List<BAGCommune> communeList = new List<BAGCommune>();
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    BAGCommune communeItem = new BAGCommune();
                //    if (communeItem.FromDataManager(dt.Rows[i]) == false)
                //        return null;
                //    communeList.Add(communeItem);
                //}
                //return communeList;
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("CommuneDAO.GetForManager, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới thông tin xã/phường
        /// </summary>
        public static bool Add(BAGCommune commune)
        {
            try
            {
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.Commune_Add]",
                //            new SqlParameter("@CommuneID", commune.CommuneID),
                //            new SqlParameter("@DistrictID", commune.DistrictID),
                //            new SqlParameter("@VName", commune.VName),
                //            new SqlParameter("@EName", commune.EName),
                //            new SqlParameter("@Description", commune.Description ?? string.Empty),
                //            new SqlParameter("@PointCount", commune.PointList.Count),
                //            new SqlParameter("@LngStr", commune.LngStr ?? string.Empty),
                //            new SqlParameter("@LatStr", commune.LatStr ?? string.Empty),
                //            new SqlParameter("@GeoStr", commune.GeoStr ?? string.Empty));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("CommuneDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Cập nhật tên xã/phường
        /// </summary>
        public static bool UpdateName(BAGCommune commune)
        {
            try
            {
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.Commune_UpdateName]",
                //            new SqlParameter("@CommuneID", commune.CommuneID),
                //            new SqlParameter("@VName", commune.VName),
                //            new SqlParameter("@EName", commune.EName));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("CommuneDAO.UpdateName, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Hủy tất cả thông tin xã/phường
        /// </summary>
        public static bool Clear()
        {
            try
            {
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.Commune_Clear]",
                //            new SqlParameter("@TypeID", EnumBAGRegionType.Commune));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("CommuneDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
