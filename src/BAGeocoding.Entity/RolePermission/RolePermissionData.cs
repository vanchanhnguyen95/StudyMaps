using System;
using System.Data;

using BAGeocoding.Entity.Enum.RolePermission;

using BAGeocoding.Utility;
using BAGeocoding.Entity.MapTool.Base;

namespace BAGeocoding.Entity.RolePermission
{
    public class RolePermissionData : ConfigBase
    {
        public int ItemID { get; set; }
        public byte PageID { get; set; }
        public short GroupID { get; set; }
        public EnumPermissionItem EnumItemID { get { return (EnumPermissionItem)ItemID; } }
        public string Name { get; set; }

        public int PRMData { get; set; }
        public int PRMDataOriginal { get; set; }

        public bool IsView { get { return PRMDataGet(EnumPermissionAction.View); } set { PRMDataSet(EnumPermissionAction.View, value); } }
        public bool IsAdd { get { return PRMDataGet(EnumPermissionAction.Add); } set { PRMDataSet(EnumPermissionAction.Add, value); } }
        public bool IsEdit { get { return PRMDataGet(EnumPermissionAction.Edit); } set { PRMDataSet(EnumPermissionAction.Edit, value); } }
        public bool IsDelete { get { return PRMDataGet(EnumPermissionAction.Delete); } set { PRMDataSet(EnumPermissionAction.Delete, value); } }
        public bool IsExport { get { return PRMDataGet(EnumPermissionAction.Export); } set { PRMDataSet(EnumPermissionAction.Export, value); } }
        public bool IsLogs { get { return PRMDataGet(EnumPermissionAction.Logs); } set { PRMDataSet(EnumPermissionAction.Logs, value); } }
        
        public RolePermissionData() { }

        public RolePermissionData(RolePermissionData other)
        {
            ItemID = other.ItemID;
            PRMData = other.PRMData;
            PRMDataOriginal = other.PRMDataOriginal;
        }

        public bool FromDataSimple(DataRow dr)
        {
            try
            {
                ItemID = base.GetDataValue<int>(dr, "ItemID");
                PRMData = PRMDataOriginal = base.GetDataValue<int>(dr, "PRMData");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("RolePermissionData.FromDataSimple, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                if (base.FromDataBase(dr) == false)
                    return false;
                ItemID = base.GetDataValue<int>(dr, "ItemID");
                PageID = base.GetDataValue<byte>(dr, "PageID");
                GroupID = base.GetDataValue<short>(dr, "GroupID");
                Name = base.GetDataValue<string>(dr, "Name");
                PRMData = PRMDataOriginal = base.GetDataValue<int>(dr, "PRMData");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("RolePermissionData.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool PRMDataGet(EnumPermissionAction opts)
        {
            return ((PRMData & (int)Math.Pow(2, (int)opts)) > 0);
        }

        public void PRMDataSet(EnumPermissionAction opts, bool state)
        {
            // Bít đã được bật
            if (((PRMData >> (int)opts) & 1) > 0)
            {
                if (state == false)
                    PRMData = PRMData - (int)Math.Pow(2, (int)opts);
            }
            // Bít chưa bật
            else
            {
                if (state == true)
                    PRMData = PRMData + (int)Math.Pow(2, (int)opts);
            }
        }

        public bool IsUpdate()
        {
            if (PRMData != PRMDataOriginal)
                return true;
            else
                return false;
        }
    }
}