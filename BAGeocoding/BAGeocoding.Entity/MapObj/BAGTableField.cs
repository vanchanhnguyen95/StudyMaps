using System;
using System.Data;

using BAGeocoding.Utility;
using System.Collections.Generic;

namespace BAGeocoding.Entity.MapObj
{
    public class BAGTableField : SQLDataUlt
    {
        public int IndexID { get; set; }
        public string RoadName { get; set; }
        public string Description { get; set; }
        
        public bool FromDataRow(DataRow dr)
        {
            try
            {
                IndexID = base.GetDataValue<int>(dr, "IndexID");
                RoadName = base.GetDataValue<string>(dr, "RoadName", string.Empty);
                Description = base.GetDataValue<string>(dr, "Description", string.Empty);

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BAGTableField.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }
    }
}