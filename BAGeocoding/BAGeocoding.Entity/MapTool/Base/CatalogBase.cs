using System;
using System.Data;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapTool.Base
{
    public class CatalogBase : SQLDataUlt
    {
        public byte State { get; set; }
        public byte StateOriginal { get; set; }
        public int SortOrder { get; set; }
        public int SortOrderOriginal { get; set; }

        public bool Actived { get; set; }
        public bool ActivedOriginal { get; set; }

        public int CreatorID { get; set; }
        public int CreateTime { get; set; }
        public DateTime CreateTimeGMT { get { return DataUtl.GetTimeUnix(CreateTime); } }

        public int EditorID { get; set; }
        public int EditTime { get; set; }
        public DateTime EditTimeGMT { get { return DataUtl.GetTimeUnix(EditTime); } }

        public CatalogBase() { }

        public CatalogBase(CatalogBase other)
        {

        }
        
        public bool FromDataRow(DataRow dr)
        {
            try
            {
                State = StateOriginal = base.GetDataValue<byte>(dr, "State");
                SortOrder = SortOrderOriginal = base.GetDataValue<int>(dr, "SortOrder");

                Actived = ActivedOriginal = (State == 1);

                CreatorID = base.GetDataValue<int>(dr, "CreatorID");
                CreateTime = base.GetDataValue<int>(dr, "CreateTime");

                EditorID = base.GetDataValue<int>(dr, "EditorID");
                EditTime = base.GetDataValue<int>(dr, "EditTime");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("CatalogBase.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataRowHistory(DataRow dr)
        {
            try
            {
                EditorID = base.GetDataValue<int>(dr, "EditorID");
                EditTime = base.GetDataValue<int>(dr, "EditTime");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("CatalogBase.FromDataRowHistory, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool EqualCatalogBase()
        {
            if (Actived != ActivedOriginal)
                return false;
            else if (State != StateOriginal)
                return false;
            else if (SortOrder != SortOrderOriginal)
                return false;
            else
                return true;
        }
    }
}
