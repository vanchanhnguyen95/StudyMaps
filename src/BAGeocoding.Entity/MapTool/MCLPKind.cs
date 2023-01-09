using System;
using System.Data;

using BAGeocoding.Entity.MapTool.Base;

using BAGeocoding.Utility;
using BAGeocoding.Entity.Enum.MapTool;
using System.Collections.Generic;

namespace BAGeocoding.Entity.MapTool
{
    public class MCLPKind : CatalogBase
    {
        public short KindID { get; set; }
        public short ParentID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DataExt { get; set; }

        public bool FromDataCache(DataRow dr)
        {
            try
            {
                KindID = base.GetDataValue<short>(dr, "KindID");
                ParentID = base.GetDataValue<short>(dr, "ParentID", 0);
                Name = base.GetDataValue<string>(dr, "Name", string.Empty);
                Description = base.GetDataValue<string>(dr, "Description", string.Empty);
                DataExt = base.GetDataValue<int>(dr, "DataExt");
                SortOrder = base.GetDataValue<short>(dr, "SortOrder");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("MCLPKind.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                KindID = base.GetDataValue<short>(dr, "KindID");
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
                LogFile.WriteError(string.Format("MCLPKind.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool Equal(MCLPKind other)
        {
            if (ParentID != other.ParentID)
                return false;
            else if (Name != other.Name)
                return false;
            else if (Description != other.Description)
                return false;
            else
                return true;
        }

        public bool EqualDataExt(MCLPKind other)
        {
            if (DataExt != other.DataExt)
                return false;
            else
                return true;
        }

        #region ==================== Dữ liệu mở rộng ===============================
        public List<MCLPKindDataExt> DataExtList(bool state)
        {
            List<MCLPKindDataExt> dataExtList = new List<MCLPKindDataExt>();
            List<EnumItemAttribute> atributeList = StringUlt.GetListEnumAttribute(EnumMCLPKindDataExt.Single);
            foreach (EnumItemAttribute item in atributeList)
            {
                dataExtList.Add(new MCLPKindDataExt
                {
                    DataExt = (EnumMCLPKindDataExt)item.Value,
                    Name = item.Name,
                    State = state && DataExtGet((EnumMCLPKindDataExt)item.Value)
                });
            }
            return dataExtList;
        }

        public bool DataExtGet(EnumMCLPKindDataExt dataExt)
        {
            return ((DataExt & (long)Math.Pow(2, (int)dataExt)) > 0);
        }

        public void DataExtSet(EnumMCLPKindDataExt dataExt, bool status)
        {
            // Bít đã được bật
            if (((DataExt >> (int)dataExt) & 1) > 0)
            {
                if (status == false)
                    DataExt = DataExt - (int)Math.Pow(2, (int)dataExt);
            }
            // Bít chưa bật
            else
            {
                if (status == true)
                    DataExt = DataExt + (int)Math.Pow(2, (int)dataExt);
            }
        }
        #endregion
    }

    public class MCLPKindDataExt
    {
        public EnumMCLPKindDataExt DataExt { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }
    }
}