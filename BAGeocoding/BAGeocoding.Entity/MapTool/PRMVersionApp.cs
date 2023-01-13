using System;
using System.Collections.Generic;
using System.Data;

using BAGeocoding.Entity.Enum.MapTool;
using BAGeocoding.Entity.Enum.RolePermission;
using BAGeocoding.Entity.MapTool.Base;
using BAGeocoding.Entity.RolePermission;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapTool
{
    public class PRMVersionApp : ConfigBase
    {
        public int VersionID { get; set; }
        public byte TypeID { get; set; }
        public EnumMTLAppType EnumTypeID { get { return (EnumMTLAppType)TypeID; } set { TypeID = (byte)value; } }
        public string VersionStr { get; set; }
        public string Description { get; set; }
        
        public PRMVersionApp() : base() { }

        public bool FromDataSimple(DataRow dr)
        {
            try
            {
                VersionID = base.GetDataValue<int>(dr, "VersionID");
                TypeID = base.GetDataValue<byte>(dr, "TypeID");
                VersionStr = base.GetDataValue<string>(dr, "VersionStr");
                Description = base.GetDataValue<string>(dr, "Description");
                StartTime = base.GetDataValue<int>(dr, "StartTime");
                EndTime = base.GetDataValue<int>(dr, "EndTime");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("PRMVersionApp.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                if (base.FromDataRow(dr) == false)
                    return false;

                VersionID = base.GetDataValue<int>(dr, "VersionID");
                TypeID = base.GetDataValue<byte>(dr, "TypeID");
                VersionStr = base.GetDataValue<string>(dr, "VersionStr");
                Description = base.GetDataValue<string>(dr, "Description");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("USRUser.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }
    }
}