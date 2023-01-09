﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.SearchData;
using BAGeocoding.Entity.Utility;

using BAGeocoding.Utility;

namespace BAGeocoding.Dal.SearchData
{
    /// <summary>
    /// Quản lý truy xuất từ khóa tìm kiếm đường
    /// </summary>
    public class SegmentKeyDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING);

        public static List<UTLSearchKey> GetByProvince(short provinceID)
        {
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(sqlDB, "[DTS.SegmentKey_GetByProvince]",
                                new SqlParameter("@ProvinceID", provinceID));

                if (ds != null)
                {
                    if (ds.Tables.Count > 1)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            List<UTLSearchKey> segmentList = new List<UTLSearchKey>();
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                UTLSearchKey segmentItem = new UTLSearchKey();
                                if (segmentItem.FromDataRow(ds.Tables[0].Rows[i], ds.Tables[1], true, true) == false)
                                    return null;
                                segmentList.Add(segmentItem);
                            }
                            return segmentList;
                        }
                        return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("SegmentKeyDAO.GetByProvince, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới thông tin thông tin tìm kiếm đường
        /// </summary>
        public static bool Add(BAGSegmentKey regionKey, int keyShift)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.SegmentKey_Add]",
                            new SqlParameter("@ProvinceID", regionKey.ProvinceID),
                            new SqlParameter("@SegmentID", regionKey.SegmentID),
                            new SqlParameter("@KeyStr", regionKey.KeyStr),
                            new SqlParameter("@IndexID", regionKey.IndexID),
                            new SqlParameter("@Rate", regionKey.Rate),
                            new SqlParameter("@KeyShift", keyShift));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("SegmentKeyDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Hủy thông tin tìm kiếm đường
        /// </summary>
        public static bool Clear(EnumBAGRegionType typeID)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.RegionKey_Clear]",
                            new SqlParameter("@TypeID", typeID));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("SegmentKeyDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
