using System;
using System.Data;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapTool.Config
{
    public class CFGIPadAssignUser : SQLDataUlt
    {
        public int ActionKeyID { get; set; }
        public MCLIPad IPadInfo { get; set; }
        public int UserID { get; set; }

        public CFGIPadAssignData AssignerInfo { get; set; }
        public CFGIPadAssignData AbrogaterInfo { get; set; }

        public CFGIPadAssignUser()
        {
            IPadInfo = new MCLIPad();
            AssignerInfo = new CFGIPadAssignData();
            AbrogaterInfo = new CFGIPadAssignData();
        }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                ActionKeyID = base.GetDataValue<int>(dr, "ActionKeyID");
                UserID = base.GetDataValue<int>(dr, "UserID");

                if (IPadInfo.FromDataView(dr) == false)
                    return false;
                else if (AssignerInfo.FromDataRow(dr, "ASG") == false)
                    return false;
                else if (AbrogaterInfo.FromDataRow(dr, "ABR") == false)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("CFGIPadAssignData.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }
    }

    public class CFGIPadAssignData : SQLDataUlt
    {
        public string Description { get; set; }
        public int EditorID { get; set; }
        public int EditTime { get; set; }
        public DateTime? EditTimeGMT { get { if (EditTime > 0) return DataUtl.GetTimeUnix(EditTime); else return null; } }

        public bool FromDataRow(DataRow dr, string pr = "")
        {
            try
            {
                Description = base.GetDataValue<string>(dr, pr + "Description");

                EditorID = base.GetDataValue<int>(dr, pr + "EditorID");
                EditTime = base.GetDataValue<int>(dr, pr + "EditTime");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("CFGIPadAssignData.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }
    }
}
