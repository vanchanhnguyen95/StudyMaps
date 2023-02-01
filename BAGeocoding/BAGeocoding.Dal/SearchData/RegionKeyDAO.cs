using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

//using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.SearchData;
using BAGeocoding.Entity.Utility;

using BAGeocoding.Utility;

namespace BAGeocoding.Dal.SearchData
{
    /// <summary>
    /// Quản lý truy xuất từ khóa tìm kiếm vùng
    /// </summary>
    public class RegionKeyDAO : SQLHelper
    {
        //protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING);

        public static List<UTLSearchKey> GetByType(EnumBAGRegionType typeID)
        {
            try
            {
                //DataSet ds = SQLHelper.ExecuteDataset(sqlDB, "[CTL.RegionKey_GetByType]",
                //                new SqlParameter("@TypeID", typeID),
                //                new SqlParameter("@TypeDistrict", EnumBAGRegionType.District));

                //if (ds != null)
                //{
                //    if (ds.Tables.Count > 1)
                //    {
                //        if (ds.Tables[0].Rows.Count > 0)
                //        {
                //            List<UTLSearchKey> segmentList = new List<UTLSearchKey>();
                //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //            {
                //                UTLSearchKey segmentItem = new UTLSearchKey();
                //                if (segmentItem.FromDataRow(ds.Tables[0].Rows[i], ds.Tables[1], false, typeID == EnumBAGRegionType.District) == false)
                //                    return null;
                //                segmentList.Add(segmentItem);
                //            }
                //            return segmentList;
                //        }
                //        return null;
                //    }
                //}
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RegionKeyDAO.GetByType, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới thông tin thông tin tìm kiếm vùng
        /// </summary>
        public static bool Add(BAGRegionKey regionKey)
        {
            try
            {
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.RegionKey_Add]",
                //            new SqlParameter("@TypeID", regionKey.TypeID),
                //            new SqlParameter("@KeyStr", regionKey.KeyStr),
                //            new SqlParameter("@ObjectID", regionKey.ObjectID),
                //            new SqlParameter("@IndexID", regionKey.IndexID),
                //            new SqlParameter("@Rate", regionKey.Rate));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RegionKeyDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Hủy thông tin tìm kiếm vùng
        /// </summary>
        public static bool Clear(EnumBAGRegionType typeID)
        {
            try
            {
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.RegionKey_Clear]",
                //            new SqlParameter("@TypeID", typeID));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RegionKeyDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
