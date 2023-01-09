using System;
using System.Data;

using BAGeocoding.Entity.Enum.RolePermission;
using BAGeocoding.Entity.MapTool.Base;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.RolePermission
{
    public class RoleContextMenuData : ConfigBase
    {
        public int MenuID { get; set; }
        public short ObjectID { get; set; }
        public EnumContextMenuItem EnumMenuID { get { return (EnumContextMenuItem)MenuID; } }
        public string Name { get; set; }

        public int CMNData { get; set; }
        public int CMNDataOriginal { get; set; }

        public bool IsView { get { return CMNDataGet(EnumPermissionAction.View); } set { CMNDataSet(EnumPermissionAction.View, value); } }
        public bool IsAdd { get { return CMNDataGet(EnumPermissionAction.Add); } set { CMNDataSet(EnumPermissionAction.Add, value); } }
        public bool IsEdit { get { return CMNDataGet(EnumPermissionAction.Edit); } set { CMNDataSet(EnumPermissionAction.Edit, value); } }
        public bool IsDelete { get { return CMNDataGet(EnumPermissionAction.Delete); } set { CMNDataSet(EnumPermissionAction.Delete, value); } }
        public bool IsExport { get { return CMNDataGet(EnumPermissionAction.Export); } set { CMNDataSet(EnumPermissionAction.Export, value); } }
        public bool IsLogs { get { return CMNDataGet(EnumPermissionAction.Logs); } set { CMNDataSet(EnumPermissionAction.Logs, value); } }


        public RoleContextMenuData() { }

        public RoleContextMenuData(RoleContextMenuData other)
        {
            MenuID = other.MenuID;
            CMNData = other.CMNData;
        }

        public bool FromDataSimple(DataRow dr)
        {
            try
            {
                MenuID = base.GetDataValue<int>(dr, "MenuID");
                CMNData = CMNDataOriginal = base.GetDataValue<int>(dr, "CMNData");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("RoleContextMenuData.FromDataSimple, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                if (base.FromDataBase(dr) == false)
                    return false;
                MenuID = base.GetDataValue<int>(dr, "MenuID");
                ObjectID = base.GetDataValue<short>(dr, "ObjectID");
                Name = base.GetDataValue<string>(dr, "Name");
                CMNData = CMNDataOriginal = base.GetDataValue<int>(dr, "CMNData");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("RoleContextMenuData.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool CMNDataGet(EnumPermissionAction opts)
        {
            return ((CMNData & (int)Math.Pow(2, (int)opts)) > 0);
        }

        public void CMNDataSet(EnumPermissionAction opts, bool state)
        {
            // Bít đã được bật
            if (((CMNData >> (int)opts) & 1) > 0)
            {
                if (state == false)
                    CMNData = CMNData - (int)Math.Pow(2, (int)opts);
            }
            // Bít chưa bật
            else
            {
                if (state == true)
                    CMNData = CMNData + (int)Math.Pow(2, (int)opts);
            }
        }

        public bool IsUpdate()
        {
            if (CMNData != CMNDataOriginal)
                return true;
            else
                return false;
        }
    }
}