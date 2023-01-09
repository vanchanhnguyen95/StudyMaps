using System.Collections.Generic;

using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.MapTool;

namespace BAGeocoding.Entity
{
    public class CacheCatalogData
    {
        public List<BAGProvince> ProvinceList { get; set; }
        public List<BAGDistrict> DistrictList { get; set; }
        public List<MCLPGroup> PGroupList { get; set; }
        public List<MCLPKind> PKindList { get; set; }
        public List<USRUser> UserList { get; set; }
        public List<WRKPlan> PlanList { get; set; }

        public CacheCatalogData()
        {
            ProvinceList = new List<BAGProvince>();
            DistrictList = new List<BAGDistrict>();
            PGroupList = new List<MCLPGroup>();
            PKindList = new List<MCLPKind>();
            UserList = new List<USRUser>();
            PlanList = new List<WRKPlan>();
        }

        public USRUser GetByID(int userID)
        {
            int indexID = UserList.FindIndex(item => item.UserID == userID);
            if (indexID > -1)
                return new USRUser(UserList[indexID]);
            else
                return new USRUser { UserID = userID };
        }
    }
}
