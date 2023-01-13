using System;
using System.Data;

using BAGeocoding.Entity.MapTool.Base;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapTool
{
    public class MCLPGroup : CatalogBase
    {
        public short GroupID { get; set; }
        public short ParentID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DataExt { get; set; }

        public string NameSort { get { return string.Format("{0:D2}.{1}", SortOrder, Name); } }

        public bool FromDataCache(DataRow dr)
        {
            try
            {
                GroupID = base.GetDataValue<short>(dr, "GroupID");
                ParentID = base.GetDataValue<short>(dr, "ParentID", 0);
                Name = base.GetDataValue<string>(dr, "Name", string.Empty);
                Description = base.GetDataValue<string>(dr, "Description", string.Empty);
                DataExt = base.GetDataValue<int>(dr, "DataExt");
                SortOrder = base.GetDataValue<short>(dr, "SortOrder");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("MCLPGroup.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                GroupID = base.GetDataValue<short>(dr, "GroupID");
                ParentID = base.GetDataValue<short>(dr, "ParentID", 0);
                Name = base.GetDataValue<string>(dr, "Name", string.Empty);
                Description = base.GetDataValue<string>(dr, "Description", string.Empty);
                DataExt = base.GetDataValue<int>(dr, "DataExt");

                if (base.FromDataRow(dr) == false)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("MCLPGroup.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }
    }
}