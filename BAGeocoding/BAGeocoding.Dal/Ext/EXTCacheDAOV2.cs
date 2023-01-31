using BAGeocoding.Entity.MapTool;

using BAGeocoding.Utility;
using BAGeocoding.Entity;
using BAGeocoding.Entity.MapObj;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using MongoDB.Driver;
using Amazon.Runtime.Internal.Util;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson;
using System.Reflection.Metadata.Ecma335;

namespace BAGeocoding.Dal.Ext
{
    /// <summary>
    /// Quản lý truy xuất xử liệu cache
    /// </summary>
    public class EXTCacheDAOV2 : SQLHelper
    {
        /// <summary>
        /// Lấy cache catalog
        /// </summary>
        public static CacheCatalogDataV2 Catalog()
        {
            try
            {
                var pack = new ConventionPack { new CamelCaseElementNameConvention() };
                ConventionRegistry.Register("elementNameConvention", pack, x => true);

                var client = new MongoClient("mongodb://localhost:27017");
                var database = client.GetDatabase("geo-db");
                var _cache = database.GetCollection<CacheCatalogDataV2>(nameof(CacheCatalogDataV2));

                // Lấy danh sách CacheCatalogData trong database
                CacheCatalogDataV2 ds = _cache.Find(new BsonDocument()).FirstOrDefault();

                if (ds != null)
                {
                    CacheCatalogDataV2 cacheData = new CacheCatalogDataV2();
                    // 1. Danh sách tỉnh thành
                    for (int i = 0; i < ds.ProvinceList.Count; i++)
                    {
                        BAGProvince provinceInfo = new BAGProvince();
                        cacheData.ProvinceList.Add(provinceInfo);
                    }

                    // 2. Danh sách quận huyện
                    for (int i = 0; i < ds.DistrictList.Count; i++)
                    {
                        BAGDistrict districtInfo = new BAGDistrict();
                        cacheData.DistrictList.Add(districtInfo);
                    }

                    // 3. Danh sách nhóm POI
                    for (int i = 0; i < ds.PGroupList.Count; i++)
                    {
                        MCLPGroup kindInfo = new MCLPGroup();
                        cacheData.PGroupList.Add(kindInfo);
                    }

                    // 4. Danh sách loại POI
                    for (int i = 0; i < ds.PKindList.Count; i++)
                    {
                        MCLPKind kindInfo = new MCLPKind();
                        cacheData.PKindList.Add(kindInfo);
                    }

                    // 5. Danh sách tài khoản
                    for (int i = 0; i < ds.UserList.Count; i++)
                    {
                        USRUser userInfo = new USRUser();
                        cacheData.UserList.Add(userInfo);
                    }

                    // 6. Danh sách kế hoạch
                    for (int i = 0; i < ds.UserList.Count; i++)
                    {
                        WRKPlan planInfo = new WRKPlan();
                        cacheData.PlanList.Add(planInfo);
                    }

                    return cacheData;
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
