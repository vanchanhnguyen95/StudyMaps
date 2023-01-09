using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.MapTool;

using BAGeocoding.Utility;
using BAGeocoding.Entity;
using BAGeocoding.Entity.MapObj;

namespace BAGeocoding.Dal.Ext
{
    /// <summary>
    /// Quản lý truy xuất xử liệu cache
    /// </summary>
    public class EXTCacheDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPTOOL);

        /// <summary>
        /// Lấy cache catalog
        /// </summary>
        public static CacheCatalogData Catalog()
        {
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(sqlDB, "[EXT.Cache_Catalog]", null);
                if (ds != null)
                {
                    if (ds.Tables.Count > 2)
                    {
                        int tableIndex = 0;
                        CacheCatalogData cacheData = new CacheCatalogData();

                        // 1. Danh sách tỉnh thành
                        for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                        {
                            BAGProvince provinceInfo = new BAGProvince();
                            if (provinceInfo.FromDataRow(ds.Tables[tableIndex].Rows[i]) == false)
                                return null;
                            cacheData.ProvinceList.Add(provinceInfo);
                        }
                        tableIndex += 1;

                        // 2. Danh sách quận huyện
                        for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                        {
                            BAGDistrict districtInfo = new BAGDistrict();
                            if (districtInfo.FromDataRow(ds.Tables[tableIndex].Rows[i]) == false)
                                return null;
                            cacheData.DistrictList.Add(districtInfo);
                        }
                        tableIndex += 1;

                        // 3. Danh sách nhóm POI
                        for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                        {
                            MCLPGroup kindInfo = new MCLPGroup();
                            if (kindInfo.FromDataCache(ds.Tables[tableIndex].Rows[i]) == false)
                                return null;
                            cacheData.PGroupList.Add(kindInfo);
                        }
                        tableIndex += 1;

                        // 4. Danh sách loại POI
                        for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                        {
                            MCLPKind kindInfo = new MCLPKind();
                            if (kindInfo.FromDataCache(ds.Tables[tableIndex].Rows[i]) == false)
                                return null;
                            cacheData.PKindList.Add(kindInfo);
                        }
                        tableIndex += 1;

                        // 5. Danh sách tài khoản
                        for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                        {
                            USRUser userInfo = new USRUser();
                            if (userInfo.FromDataSimple(ds.Tables[tableIndex].Rows[i]) == false)
                                return null;
                            cacheData.UserList.Add(userInfo);
                        }
                        tableIndex += 1;

                        // 6. Danh sách kế hoạch
                        for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                        {
                            WRKPlan planInfo = new WRKPlan();
                            if (planInfo.FromDataSimple(ds.Tables[tableIndex].Rows[i]) == false)
                                return null;
                            cacheData.PlanList.Add(planInfo);
                        }
                        tableIndex += 1;

                        return cacheData;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("EXTCacheDAO.Catalog, ex: " + ex.ToString());
                return null;
            }
        }
    }
}