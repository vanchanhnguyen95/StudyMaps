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
    public class USRSession : SQLDataUlt
    {
        public long SessionID { get; set; }
        public USRUser UserInfo { get; set; }
        public byte TypeID { get; set; }
        public string OSVersion { get; set; }
        public string AppVersion { get; set; }
        public string MachineKey { get; set; }
        public int StartTime { get; set; }
        public DateTime StartTimeGMT { get { return DataUtl.GetTimeUnix(StartTime); } }
        public int EndTime { get; set; }
        public DateTime? EndTimeGMT { get { if (EndTime > 0) return DataUtl.GetTimeUnix(EndTime); else return null; } }

        public byte ErrorCode { get; set; }

        public int DeviceID { get; set; }

        public USRSession()
        {
            UserInfo = new USRUser();
        }
        
        public bool FromDataRow(DataRow dr)
        {
            try
            {
                SessionID = base.GetDataValue<long>(dr, "SessionID");
                TypeID = base.GetDataValue<byte>(dr, "TypeID");
                OSVersion = base.GetDataValue<string>(dr, "OSVersion");
                AppVersion = base.GetDataValue<string>(dr, "AppVersion");
                MachineKey = base.GetDataValue<string>(dr, "MachineKey");

                StartTime = base.GetDataValue<int>(dr, "StartTime");
                EndTime = base.GetDataValue<int>(dr, "EndTime");

                DeviceID = base.GetDataValue<int>(dr, "DeviceID", 0);

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("USRUser.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataError(DataRow dr)
        {
            try
            {
                UserInfo = new USRUser
                {
                    UserName = base.GetDataValue<string>(dr, "UserName")
                };
                TypeID = base.GetDataValue<byte>(dr, "TypeID");
                OSVersion = base.GetDataValue<string>(dr, "OSVersion");
                AppVersion = base.GetDataValue<string>(dr, "AppVersion");
                MachineKey = base.GetDataValue<string>(dr, "MachineKey");

                StartTime = base.GetDataValue<int>(dr, "EditTime");

                ErrorCode = base.GetDataValue<byte>(dr, "ErrorCode");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("USRUser.FromDataError, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataDevice(DataRow dr)
        {
            try
            {
                SessionID = base.GetDataValue<long>(dr, "SessionID");
                if (UserInfo.FromDataSimple(dr) == false)
                    return false;
                OSVersion = base.GetDataValue<string>(dr, "OSVersion");
                AppVersion = base.GetDataValue<string>(dr, "AppVersion");
                MachineKey = base.GetDataValue<string>(dr, "MachineKey");

                StartTime = base.GetDataValue<int>(dr, "StartTime");
                EndTime = base.GetDataValue<int>(dr, "EndTime");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("USRUser.FromDataDevice, ex: {0}", ex.ToString()));
                return false;
            }
        }
    }

    public class USRSessionCondition
    {
        public int DeviceID { get; set; }
        public int UserID { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
    }
}