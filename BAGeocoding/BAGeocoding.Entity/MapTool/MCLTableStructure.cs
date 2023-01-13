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
    /// Thông tin cấu trúc một bảng
    /// </summary>
    public class MCLTableStructure : CatalogBase
    {
        #region ==================== Contructor Init ===============================
        public int IndexID { get; set; }
        public short ObjectID { get; set; }
        public EnumMOBObjectTable EnumObject { get { return (EnumMOBObjectTable)ObjectID; } set { ObjectID = (byte)value; } }
        public string FieldName { get; set; }
        public byte DataType { get; set; }
        public EnumDBDataType EnumDataType { get { return (EnumDBDataType)DataType; } set { DataType = (byte)value; } }
        public byte FieldType { get; set; }
        public EnumDBFieldType EnumFieldType { get { return (EnumDBFieldType)FieldType; } set { FieldType = (byte)value; } }
        public string FieldTypeStr 
        {
            get
            {
                if (EnumDataType == EnumDBDataType.Number)
                    return string.Format("{0}", StringUlt.ConvertEnumToString(EnumFieldType));
                else
                    return string.Format("{0} ({1})", StringUlt.ConvertEnumToString(EnumFieldType), FieldLength);
            }
        }
        public int FieldLength { get; set; }
        public bool AllowNull { get; set; }
        public string Description { get; set; }
        public int DataExt { get; set; }
        #endregion

        #region ==================== Get From DataRow ==============================
        public bool FromDataRow(DataRow dr)
        {
            try
            {
                IndexID = base.GetDataValue<int>(dr, "IndexID");
                ObjectID = base.GetDataValue<short>(dr, "ObjectID");
                FieldName = base.GetDataValue<string>(dr, "FieldName");
                DataType = base.GetDataValue<byte>(dr, "DataType");
                FieldType = base.GetDataValue<byte>(dr, "FieldType");
                FieldLength = base.GetDataValue<int>(dr, "FieldLength");
                AllowNull = base.GetDataValue<bool>(dr, "AllowNull");
                DataExt = base.GetDataValue<int>(dr, "DataExt");
                Description = base.GetDataValue<string>(dr, "Description", string.Empty);

                if (base.FromDataRow(dr) == false)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("MCLTableStructure.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataRowHistory(DataRow dr)
        {
            try
            {
                IndexID = base.GetDataValue<int>(dr, "IndexID");
                ObjectID = base.GetDataValue<short>(dr, "ObjectID");
                FieldName = base.GetDataValue<string>(dr, "FieldName");
                DataType = base.GetDataValue<byte>(dr, "DataType");
                FieldType = base.GetDataValue<byte>(dr, "FieldType");
                FieldLength = base.GetDataValue<int>(dr, "FieldLength");
                AllowNull = base.GetDataValue<bool>(dr, "AllowNull");
                Description = base.GetDataValue<string>(dr, "Description", string.Empty);

                if (base.FromDataRowHistory(dr) == false)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("MCLTableStructure.FromDataRowHistory, ex: {0}", ex.ToString()));
                return false;
            }
        }
        #endregion

        #region ==================== Process Data Object ===========================
        /// <summary>
        /// Kiểm tra có thay đổi dữ liệu
        /// </summary>
        public bool Equal(MCLTableStructure other)
        {
            if (FieldName != other.FieldName)
                return false;
            else if (DataType != other.DataType)
                return false;
            else if (FieldType != other.FieldType)
                return false;
            else if (FieldLength != other.FieldLength)
                return false;
            else if (AllowNull != other.AllowNull)
                return false;
            else if (Description != other.Description)
                return false;

            return true;
        }

        /// <summary>
        /// Kiểm tra có thay đổi dữ liệu mở rộng
        /// </summary>
        public bool EqualDataExt(MCLTableStructure other)
        {
            if (DataExt != other.DataExt)
                return false;

            return true;
        }
        #endregion

        #region ==================== Dữ liệu mở rộng ===============================
        public List<MCLTableStructureDataExt> DataExtList(bool state)
        {
            List<MCLTableStructureDataExt> dataExtList = new List<MCLTableStructureDataExt>();
            List<EnumItemAttribute> atributeList = StringUlt.GetListEnumAttribute(EnumMCLTableStructureDataExt.Default);
            foreach (EnumItemAttribute item in atributeList)
            {
                dataExtList.Add(new MCLTableStructureDataExt
                {
                    DataExt = (EnumMCLTableStructureDataExt)item.Value,
                    Name = item.Name,
                    State = state && DataExtGet((EnumMCLTableStructureDataExt)item.Value)
                });
            }
            return dataExtList;
        }

        public bool DataExtGet(EnumMCLTableStructureDataExt dataExt)
        {
            return ((DataExt & (long)Math.Pow(2, (int)dataExt)) > 0);
        }

        public void DataExtSet(EnumMCLTableStructureDataExt dataExt, bool status)
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
    public class MCLTableStructureDataExt
    {
        public EnumMCLTableStructureDataExt DataExt { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }
    }
}