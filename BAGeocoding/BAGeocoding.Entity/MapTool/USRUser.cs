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
    public class USRUser : CatalogBase
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string Description { get; set; }
        public bool IsAdmin { get; set; }
        public int DataExt { get; set; }

        public long SessionID { get; set; }
        public bool ViewID { get; set; }

        public string PasswordOld { get; set; }
        public string PasswordNew { get; set; }
        
        public int PlanID { get; set; }

        public USRUser() : base()
        {
        }

        public USRUser(USRUser other) : base(other)
        {
            UserID = other.UserID;
            FullName = other.FullName;
            UserName = other.UserName;
            Mobile = other.Mobile;
            Description = other.Description;
            IsAdmin = other.IsAdmin;
            DataExt = other.DataExt;
        }

        public bool FromDataSimple(DataRow dr)
        {
            try
            {
                UserID = base.GetDataValue<int>(dr, "UserID");
                UserName = base.GetDataValue<string>(dr, "UserName");
                FullName = base.GetDataValue<string>(dr, "FullName");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("USRUser.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                if (base.FromDataRow(dr) == false)
                    return false;

                FullName = base.GetDataValue<string>(dr, "FullName");
                UserID = base.GetDataValue<int>(dr, "UserID");
                UserName = base.GetDataValue<string>(dr, "UserName");
                Mobile = base.GetDataValue<string>(dr, "Mobile");
                Description = base.GetDataValue<string>(dr, "Description");
                IsAdmin = base.GetDataValue<bool>(dr, "IsAdmin");
                DataExt = base.GetDataValue<int>(dr, "DataExt");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("USRUser.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataLogin(DataRow dr)
        {
            try
            {
                UserID = base.GetDataValue<int>(dr, "UserID");
                FullName = base.GetDataValue<string>(dr, "FullName");
                UserName = base.GetDataValue<string>(dr, "UserName");
                Mobile = base.GetDataValue<string>(dr, "Mobile");
                Description = base.GetDataValue<string>(dr, "Description");
                DataExt = base.GetDataValue<int>(dr, "DataExt");
                SessionID = base.GetDataValue<long>(dr, "SessionID");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("USRUser.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public byte[] ToBinary()
        {
            try
            {
                // 1. Lấy dữ liệu
                List<byte> resultList = new List<byte>();
                // 1.1 Mã kế hoạch
                resultList.AddRange(BitConverter.GetBytes(UserID));
                if (UserID > 0)
                {
                    // 1.2 Tài khoản
                    byte[] userNameBff = Constants.UTF8CodePage.GetBytes(UserName);
                    //resultList.AddRange(BitConverter.GetBytes((byte)userNameBff.Length));
                    resultList.AddRange(BitConverter.GetBytes((long)userNameBff.Length));
                    resultList.AddRange(userNameBff);
                    // 1.3 Họ và tên
                    byte[] fullNameBff = Constants.UTF8CodePage.GetBytes(FullName);
                    //resultList.AddRange(BitConverter.GetBytes((byte)fullNameBff.Length));
                    resultList.AddRange(BitConverter.GetBytes((long)fullNameBff.Length));
                    resultList.AddRange(fullNameBff);
                }

                // 2. Trả về kết quả
                return resultList.ToArray();
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("USRUser.ToBinary, ex: {0}", ex.ToString()));
                return null;
            }
        }

        public bool Equal(USRUser other)
        {
            if (FullName != other.FullName)
                return false;
            else if (Mobile != other.Mobile)
                return false;
            else if (Description != other.Description)
                return false;
            else
                return true;
        }

        public bool EqualDataExt(USRUser other)
        {
            return DataExt == other.DataExt;
        }

        #region ==================== Trạng thái của đối tượng ====================
        public List<USRUserDataExt> DataExtList(bool state)
        {
            List<USRUserDataExt> dataExtList = new List<USRUserDataExt>();
            List<EnumItemAttribute> atributeList = StringUlt.GetListEnumAttribute(EnumMCLUserDataExt.LoginWin);
            foreach (EnumItemAttribute item in atributeList)
            {
                dataExtList.Add(new USRUserDataExt
                {
                    DataExt = (EnumMCLUserDataExt)item.Value,
                    Name = item.Name,
                    State = state && DataExtGet((EnumMCLUserDataExt)item.Value)
                });
            }
            return dataExtList;
        }

        public bool DataExtGet(EnumMCLUserDataExt opts)
        {
            return ((DataExt & (int)Math.Pow(2, (int)opts)) > 0);
        }

        public void DataExtSet(EnumMCLUserDataExt opts, bool state)
        {
            // Bít đã được bật
            if (((DataExt >> (int)opts) & 1) > 0)
            {
                if (state == false)
                    DataExt = DataExt - (int)Math.Pow(2, (int)opts);
            }
            // Bít chưa bật
            else
            {
                if (state == true)
                    DataExt = DataExt + (int)Math.Pow(2, (int)opts);
            }
        }
        #endregion
    }

    public class USRUserInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VersionStr { get; set; }
        public string OSVersion { get; set; }
        public string AppVersion { get; set; }
        public string MachineKey { get; set; }

        public bool FromBinary(byte[] bff)
        {
            try
            {
                int dataIndex = 0;

                // 1. Lấy tài khoản
                int dataLength = BitConverter.ToInt16(bff, dataIndex);
                dataIndex += 2;
                UserName = Constants.UTF8CodePage.GetString(bff, dataIndex, dataLength);
                dataIndex += dataLength;
                // 2. Lấy mật khẩu
                dataLength = BitConverter.ToInt16(bff, dataIndex);
                dataIndex += 2;
                Password = Constants.UTF8CodePage.GetString(bff, dataIndex, dataLength);
                dataIndex += dataLength;
                // 3. Phiên bản HĐH
                dataLength = BitConverter.ToInt16(bff, dataIndex);
                dataIndex += 2;
                OSVersion = Constants.UTF8CodePage.GetString(bff, dataIndex, dataLength);
                dataIndex += dataLength;
                // 4. Lấy phiên bản App
                dataLength = BitConverter.ToInt16(bff, dataIndex);
                dataIndex += 2;
                AppVersion = Constants.UTF8CodePage.GetString(bff, dataIndex, dataLength);
                dataIndex += dataLength;
                // 5. Lấy số IMEI thiết bị
                dataLength = BitConverter.ToInt16(bff, dataIndex);
                dataIndex += 2;
                MachineKey = Constants.UTF8CodePage.GetString(bff, dataIndex, dataLength);
                dataIndex += dataLength;

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("USRUserInfo.FromBinary, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public string ToStringLogin()
        {
            return string.Format("UserName: {0}, Password: {1}, OSVersion: {2}, AppVersion: {3}, MachineKey: {4}", UserName, Password, OSVersion, AppVersion, MachineKey);
        }
    }

    public class USRUserApp
    {
        public byte ErrorCode { get; set; }
        public EnumMTLErrorCodeApp EnumErrorCode { get { return (EnumMTLErrorCodeApp)ErrorCode; } set { ErrorCode = (byte)value; } }

        public WRKPlan PlanInfo { get; set; }
        public USRUser UserInfo { get; set; }
        public List<WRKUserGrid> GridList { get; set; }

        public USRUserApp()
        {
            UserInfo = new USRUser();
            PlanInfo = new WRKPlan();
            GridList = new List<WRKUserGrid>();
        }

        public string GridStr()
        {
            string gridStr = string.Empty;
            if (GridList == null || GridList.Count == 0)
                return gridStr;
            for (int i = 0; i < GridList.Count; i++)
            {
                if (gridStr.Length > 0)
                    gridStr += ",";
                gridStr += string.Format("{0}", GridList[i].GridInfo.GridID);
            }
            return gridStr;
        }

        public bool FromBinarySyncData(byte[] bff)
        {
            try
            {
                int dataIndex = 0;
                // 1. Mã tài khoản
                UserInfo = new USRUser { UserID = BitConverter.ToInt32(bff, dataIndex) };
                dataIndex += 4;
                // 2. Mã kế hoạch
                PlanInfo = new WRKPlan { PlanID = BitConverter.ToInt32(bff, dataIndex) };
                dataIndex += 4;
                // 3. Dữ liệu grid
                // 3.1 Số lượng grid
                int gridCount = BitConverter.ToInt16(bff, dataIndex);
                dataIndex += 2;
                // 3.2 Danh sách mã grid
                for (int i = 0; i < gridCount; i++)
                {
                    GridList.Add(new WRKUserGrid { GridInfo = new MCLGrid { GridID = BitConverter.ToInt32(bff, dataIndex) } });
                    dataIndex += 4;
                }

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("USRUserApp.FromBinarySyncData, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public byte[] ToBinary()
        {
            try
            {
                // 1. Lấy dữ liệu
                List<byte> resultList = new List<byte>();
                // 1.1 Trạng thái đăng nhập
                resultList.Add(ErrorCode);
                // 1.1 Thông tin tài khoản
                resultList.AddRange(UserInfo.ToBinary());
                // 1.2 Thông tin kế hoạch
                resultList.AddRange(PlanInfo.ToBinaryLogin());
                // 1.3 Thông tin grid
                resultList.AddRange(BitConverter.GetBytes((short)GridList.Count));
                for (int i = 0; i < GridList.Count; i++)
                    resultList.AddRange(GridList[i].ToBinaryLogin());

                // 2. Trả về kết quả
                return resultList.ToArray();
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("USRUserApp.ToBinary, ex: {0}", ex.ToString()));
                return null;
            }
        }

        public void InitDataTest()
        {
            PlanInfo = new WRKPlan { PlanID = 2 };
            UserInfo = new USRUser { UserID = 236 };
            GridList = new List<WRKUserGrid>();
            GridList.Add(new WRKUserGrid { GridInfo = new MCLGrid { GridID = 230340 } });
            GridList.Add(new WRKUserGrid { GridInfo = new MCLGrid { GridID = 230341 } });
            GridList.Add(new WRKUserGrid { GridInfo = new MCLGrid { GridID = 230342 } });
        }

        public string ToStringLogin()
        {
            return string.Format("ErrorCode: {0}, UserID: {1}, PlanID: {2}, GridList: {3}", ErrorCode, UserInfo.UserID, PlanInfo.PlanID, GridStr());
        }
        public string ToStringSync()
        {
            return string.Format("UserID: {0}, PlanID: {1}, GridList: {2}", UserInfo.UserID, PlanInfo.PlanID, GridStr());
        }
    }

    public class USRUserDataExt
    {
        public EnumMCLUserDataExt DataExt { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }
    }


    public class USRRolePermission
    {
        public List<RolePermissionData> PRMList { get; set; }
        public List<RoleContextMenuData> CMNList { get; set; }

        public USRRolePermission()
        {
            PRMList = new List<RolePermissionData>();
            CMNList = new List<RoleContextMenuData>();
        }

        public void DataAdjust()
        {
            if (PRMList == null)
                PRMList = new List<RolePermissionData>();
            if (CMNList == null)
                CMNList = new List<RoleContextMenuData>();
        }

        public RolePermissionData GetByItem(EnumPermissionItem enumItem)
        {
            if (PRMList == null || PRMList.Count == 0)
                return new RolePermissionData();
            int indexID = PRMList.FindIndex(item => item.EnumItemID == enumItem);
            if (indexID > -1)
                return new RolePermissionData(PRMList[indexID]);
            else
                return new RolePermissionData();
        }

        public RoleContextMenuData GetByMenu(EnumContextMenuItem menuItem)
        {
            if (CMNList == null || CMNList.Count == 0)
                return new RoleContextMenuData();
            int indexID = CMNList.FindIndex(item => item.EnumMenuID == menuItem);
            if (indexID > -1)
                return new RoleContextMenuData(CMNList[indexID]);
            else
                return new RoleContextMenuData();
        }
    }

    public class USRAssignData
    {
        public int SourceID { get; set; }
        public int UserID { get; set; }

        public bool AssignPerms { get; set; }
        public bool AssignMenu { get; set; }

        public bool EquivalentFlag { get; set; }

        public int ActorID { get; set; }

        public bool Duplicate()
        {
            return UserID == SourceID;
        }

        public bool HaveData()
        {
            if (AssignPerms == true)
                return true;
            else if (AssignMenu == true)
                return true;
            else
                return false;
        }
    }
}