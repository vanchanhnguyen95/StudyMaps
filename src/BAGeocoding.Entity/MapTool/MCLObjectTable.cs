using System;
using System.Collections.Generic;
using System.Data;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.Enum.MapObject;
using BAGeocoding.Entity.Enum.MapTool;
using BAGeocoding.Entity.MapTool.Base;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapTool
{
    /// <summary>
    /// Thông tin một bảng
    /// </summary>
    public class MCLObjectTable : CatalogBase
    {
        #region ==================== Contructor Init ===============================
        public short ObjectID { get; set; }
        public EnumMOBObjectTable EnumObject { get { return (EnumMOBObjectTable)ObjectID; } set { ObjectID = (byte)value; } }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DataExt { get; set; }
        #endregion

        public MCLObjectTable() : base()
        {
        }

        public MCLObjectTable(MCLObjectTable other) : base(other)
        {
            ObjectID = other.ObjectID;
            Name = other.Name;
            Description = other.Description;
            DataExt = other.DataExt;
        }

        #region ==================== Get From DataRow ==============================
        public bool FromDataRow(DataRow dr)
        {
            try
            {
                ObjectID = base.GetDataValue<short>(dr, "ObjectID");
                Name = base.GetDataValue<string>(dr, "Name");
                Description = base.GetDataValue<string>(dr, "Description", string.Empty);
                DataExt = base.GetDataValue<int>(dr, "DataExt");
                
                if (base.FromDataRow(dr) == false)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("MCLObjectTable.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataRowHistory(DataRow dr)
        {
            try
            {
                ObjectID = base.GetDataValue<short>(dr, "ObjectID");
                Name = base.GetDataValue<string>(dr, "Name");
                Description = base.GetDataValue<string>(dr, "Description", string.Empty);

                if (base.FromDataRowHistory(dr) == false)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("MCLObjectTable.FromDataRowHistory, ex: {0}", ex.ToString()));
                return false;
            }
        }
        #endregion

        #region ==================== Process Data Object ===========================
        /// <summary>
        /// Kiểm tra có thay đổi dữ liệu
        /// </summary>
        public bool Equal(MCLObjectTable other)
        {
            if (Name != other.Name)
                return false;
            else if (Description != other.Description)
                return false;
            else
                return true;
        }
        #endregion

        #region ==================== Dữ liệu mở rộng ===============================
        public List<MCLObjectTableDataExt> DataExtList(bool state)
        {
            List<MCLObjectTableDataExt> dataExtList = new List<MCLObjectTableDataExt>();
            List<EnumItemAttribute> atributeList = StringUlt.GetListEnumAttribute(EnumMCLObjectTableDataExt.Default);
            foreach (EnumItemAttribute item in atributeList)
            {
                dataExtList.Add(new MCLObjectTableDataExt
                {
                    DataExt = (EnumMCLObjectTableDataExt)item.Value,
                    Name = item.Name,
                    State = state && DataExtGet((EnumMCLObjectTableDataExt)item.Value)
                });
            }
            return dataExtList;
        }

        public bool DataExtGet(EnumMCLObjectTableDataExt dataExt)
        {
            return ((DataExt & (long)Math.Pow(2, (int)dataExt)) > 0);
        }

        public void DataExtSet(EnumMCLObjectTableDataExt dataExt, bool status)
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

    /// <summary>
    /// Dữ liệu mở rộng
    /// </summary>
    public class MCLObjectTableDataExt
    {
        public EnumMCLObjectTableDataExt DataExt { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }
    }
}