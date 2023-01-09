using System;
using System.Data;

using BAGeocoding.Entity.MapTool.Base;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapTool.Data
{
    public class DTSGridUser : ConfigBase
    {
        public WRKPlan PlanInfo { get; set; }
        public USRUser UserInfo { get; set; }
        public WRKUserGrid UserGrid { get; set; }


        public DTSGridUser()
        {
            PlanInfo = new WRKPlan();
            UserInfo = new USRUser();
            UserGrid = new WRKUserGrid();
        }


        public bool FromDataRow(DataRow dr)
        {
            try
            {
                if (base.FromDataRow(dr) == false)
                    return false;

                if (PlanInfo.FromDataSimple(dr, "PLN") == false)
                    return false;
                else if (UserInfo.FromDataSimple(dr) == false)
                    return false;
                UserGrid.DataExt = base.GetDataValue<byte>(dr, "DataExt");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("DTSGridUser.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }
    }
}
