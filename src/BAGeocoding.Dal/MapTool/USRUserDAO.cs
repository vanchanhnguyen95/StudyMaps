using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.Enum.MapTool;
using BAGeocoding.Entity.MapTool;

using BAGeocoding.Utility;

namespace BAGeocoding.Dal.MapTool
{
    /// <summary>
    /// Quản lý truy xuất thông tài khoản
    /// </summary>
    public class USRUserDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPTOOL);

        /// <summary>
        /// Lấy danh sách tài khoản
        /// </summary>
        public static List<USRUser> GetAll()
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[USR.User_GetAll]", null);

                if (dt == null)
                    return null;
                List<USRUser> gridList = new List<USRUser>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    USRUser gridInfo = new USRUser();
                    if (gridInfo.FromDataRow(dt.Rows[i]) == false)
                        return null;
                    gridList.Add(gridInfo);
                }
                return gridList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("USRUserDAO.GetAll, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Kiểm tra quyền đăng nhập
        /// </summary>
        public static bool CheckLogin(int userID, string password)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[USR.User_CheckLogin]",
                                new SqlParameter("@UserID", userID),
                                new SqlParameter("@Password", password),

                                new SqlParameter("@DataExt", Math.Pow(2, (int)EnumMCLUserDataExt.LoginWin)));

                if (dt == null)
                    return false;
                else
                    return dt.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("USRUserDAO.CheckLogin, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Đăng nhập trên windows
        /// </summary>
        public static USRUser LoginWindow(USRUserInfo loginInfo, ref byte errorCode)
        {
            try
            {
                SqlParameter prErrorCode = new SqlParameter("@ErrorCode", errorCode);
                prErrorCode.Direction = ParameterDirection.Output;

                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[USR.User_LoginWindow]",
                                new SqlParameter("@UserName", loginInfo.UserName),
                                new SqlParameter("@Password", StringUlt.EncryptPassword(loginInfo.Password)),
                                new SqlParameter("@VersionStr", Constants.WINAPP_VERSION),
                                new SqlParameter("@OSVersion", loginInfo.OSVersion),
                                new SqlParameter("@AppVersion", loginInfo.AppVersion),
                                new SqlParameter("@MachineKey", loginInfo.MachineKey),

                                new SqlParameter("@EnumWindow", EnumMTLAppType.Window),
                                new SqlParameter("@EnumLoginWin", EnumMCLUserDataExt.LoginWin),

                                new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()),

                                prErrorCode);

                errorCode = Convert.ToByte(prErrorCode.Value);

                if (errorCode > 0)
                    return null;
                else if (dt == null)
                    return null;
                else if (dt.Rows.Count < 1)
                    return null;
                USRUser userInfo = new USRUser();
                if (userInfo.FromDataRow(dt.Rows[0]) == true)
                    return userInfo;
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("USRUserDAO.LoginWin, ex: " + ex.ToString());
                errorCode = 252;
                return null;
            }
        }
        
        /// <summary>
        /// Đăng nhập trên mobile
        /// </summary>
        public static USRUserApp LoginMobile(USRUserInfo loginInfo, ref byte errorCode)
        {
            try
            {
                SqlParameter prErrorCode = new SqlParameter("@ErrorCode", errorCode) { Direction = ParameterDirection.Output };

                if (loginInfo.VersionStr == null || loginInfo.VersionStr.Length == 0)
                    loginInfo.VersionStr = "1.0.0";

                DataSet ds = SQLHelper.ExecuteDataset(sqlDB, "[USR.User_LoginMobile]",
                                new SqlParameter("@UserName", loginInfo.UserName),
                                new SqlParameter("@Password", StringUlt.EncryptPassword(loginInfo.Password)),
                                new SqlParameter("@VersionStr", loginInfo.VersionStr),
                                new SqlParameter("@OSVersion", loginInfo.OSVersion),
                                new SqlParameter("@AppVersion", loginInfo.AppVersion),
                                new SqlParameter("@MachineKey", loginInfo.MachineKey),

                                new SqlParameter("@EnumMobile", EnumMTLAppType.Mobile),
                                new SqlParameter("@EnumLoginMbl", EnumMCLUserDataExt.LoginMbl),

                                new SqlParameter("@EnumWorking", EnumMTLPlanUserDataExt.Work),
                                new SqlParameter("@BehaviorDeploy", EnumMTLPlanBehavior.Deployment),

                                new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()),

                                prErrorCode);

                errorCode = Convert.ToByte(prErrorCode.Value);
                LogFile.WriteResponse(string.Format("LoginSystem, ErrorCode => ({0})", errorCode));
                if (errorCode > 0)
                    errorCode = 1;

                if (errorCode > 0)
                    return null;
                else if (ds == null)
                    return null;
                else if (ds.Tables.Count < 3)
                    return null;
                else if (ds.Tables[0].Rows.Count < 1)
                    return null;

                int tableIndex = 0;
                USRUserApp userInfo = new USRUserApp();
                // 1. Thông tin tài khoản
                if (userInfo.UserInfo.FromDataRow(ds.Tables[tableIndex].Rows[0]) == false)
                    return null;
                tableIndex += 1;
                // 2. Thông tin kế hoạch
                if (ds.Tables[tableIndex].Rows.Count > 0 && userInfo.PlanInfo.FromDataLogin(ds.Tables[tableIndex].Rows[0]) == false)
                    return null;
                tableIndex += 1;
                // 3. Danh sách grid
                if (userInfo.PlanInfo.PlanID > 0)
                {
                    for (int i = 0; i < ds.Tables[tableIndex].Rows.Count; i++)
                    {
                        WRKUserGrid gridInfo = new WRKUserGrid();
                        if (gridInfo.FromDataLogin(ds.Tables[tableIndex].Rows[i]) == false)
                            return null;
                        userInfo.GridList.Add(gridInfo);
                    }
                    tableIndex += 1;
                }

                return userInfo;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("USRUserDAO.LoginApp, ex: " + ex.ToString());
                return null;
            }
        }
        
        /// <summary>
        /// Đăng nhập trên website
        /// </summary>
        public static USRUser LoginWebsite(USRUserInfo loginInfo, ref byte errorCode)
        {
            try
            {
                SqlParameter prErrorCode = new SqlParameter("@ErrorCode", errorCode);
                prErrorCode.Direction = ParameterDirection.Output;

                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[USR.User_LoginWebsite]",
                                new SqlParameter("@UserName", loginInfo.UserName),
                                new SqlParameter("@Password", StringUlt.EncryptPassword(loginInfo.Password)),
                                new SqlParameter("@VersionStr", Constants.WEBSITE_VERSION),
                                new SqlParameter("@OSVersion", loginInfo.OSVersion),
                                new SqlParameter("@AppVersion", loginInfo.AppVersion),
                                new SqlParameter("@MachineKey", loginInfo.MachineKey),

                                new SqlParameter("@EnumWebsite", EnumMTLAppType.Website),
                                new SqlParameter("@EnumLoginWeb", EnumMCLUserDataExt.LoginWeb),

                                new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()),

                                prErrorCode);

                errorCode = Convert.ToByte(prErrorCode.Value);

                if (errorCode > 0)
                    return null;
                else if (dt == null)
                    return null;
                else if (dt.Rows.Count < 1)
                    return null;
                USRUser userInfo = new USRUser();
                if (userInfo.FromDataRow(dt.Rows[0]) == true)
                    return userInfo;
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("USRUserDAO.LoginWebsite, ex: " + ex.ToString());
                errorCode = 252;
                return null;
            }
        }

        /// <summary>
        /// Đăng xuất
        /// </summary>
        public static void Logout(long sessionID)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[USR.User_Logout]",
                                new SqlParameter("@SessionID", sessionID),

                                new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));
            }
            catch (Exception ex)
            {
                LogFile.WriteError("USRUserDAO.Logout, ex: " + ex.ToString());
            }
        }


        public static bool Create(USRUser userInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[USR.User_Create]",
                                new SqlParameter("@FullName", userInfo.FullName),
                                new SqlParameter("@UserName", userInfo.UserName),
                                new SqlParameter("@Password", StringUlt.EncryptPassword(userInfo.Password)),
                                new SqlParameter("@Mobile", userInfo.Mobile),
                                new SqlParameter("@Description", userInfo.Description),
                                new SqlParameter("@DataExt", userInfo.DataExt),

                                new SqlParameter("@CatalogID", EnumCatalogType.User),

                                new SqlParameter("@ActorID", userInfo.EditorID),
                                new SqlParameter("@SessionID", userInfo.SessionID),
                                new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("USRUserDAO.Create, ex: " + ex.ToString());
                return false;
            }
        }

        public static bool Update(USRUser userInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[USR.User_Update]",
                                new SqlParameter("@UserID", userInfo.UserID),
                                new SqlParameter("@FullName", userInfo.FullName),
                                new SqlParameter("@Mobile", userInfo.Mobile),
                                new SqlParameter("@Description", userInfo.Description),
                                new SqlParameter("@DataExt", userInfo.DataExt),

                                new SqlParameter("@CatalogID", EnumCatalogType.User),

                                new SqlParameter("@ActorID", userInfo.EditorID),
                                new SqlParameter("@SessionID", userInfo.SessionID),
                                new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("USRUserDAO.Update, ex: " + ex.ToString());
                return false;
            }
        }

        public static bool UpdateQuick(USRUser userInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[USR.User_UpdateQuick]",
                                new SqlParameter("@UserID", userInfo.UserID),
                                new SqlParameter("@State", userInfo.Actived),

                                new SqlParameter("@CatalogID", EnumCatalogType.User),

                                new SqlParameter("@ActorID", userInfo.EditorID),
                                new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("USRUserDAO.UpdateDataExt, ex: " + ex.ToString());
                return false;
            }
        }

        public static bool UpdateDataExt(USRUser userInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[USR.User_UpdateDataExt]",
                                new SqlParameter("@UserID", userInfo.UserID),
                                new SqlParameter("@DataExt", userInfo.DataExt),

                                new SqlParameter("@CatalogID", EnumCatalogType.User),

                                new SqlParameter("@ActorID", userInfo.EditorID),
                                new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("USRUserDAO.UpdateDataExt, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Reset mật khẩu tài khoản
        /// </summary>
        public static bool ResetPassword(USRUser userInfo, USRUser actorInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[USR.User_ResetPassword]",
                                new SqlParameter("@UserID", userInfo.UserID),
                                new SqlParameter("@Password", StringUlt.EncryptPassword(userInfo.Password)),

                                new SqlParameter("@ActorID", userInfo.EditorID),
                                new SqlParameter("@SessionID", actorInfo.SessionID),
                                new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;                
            }
            catch (Exception ex)
            {
                LogFile.WriteError("USRUserDAO.ResetPassword, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Tạo mật khẩu mới cho tài khoản
        /// </summary>
        public static bool PasswordRenew(USRUser userInfo, USRUser actorInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[USR.User_PasswordRenew]",
                                new SqlParameter("@UserID", userInfo.UserID),
                                new SqlParameter("@Password", StringUlt.EncryptPassword(userInfo.Password)),

                                new SqlParameter("@ActorID", userInfo.EditorID),
                                new SqlParameter("@SessionID", actorInfo.SessionID),
                                new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("USRUserDAO.PasswordRenew, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Reset mật khẩu tài khoản
        /// </summary>
        public static bool PasswordReset(USRUser userInfo, USRUser actorInfo, ref byte errorCode)
        {
            try
            {
                SqlParameter prErrorCode = new SqlParameter("@ErrorCode", errorCode);
                prErrorCode.Direction = ParameterDirection.Output;

                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[USR.User_PasswordReset]",
                                new SqlParameter("@UserID", userInfo.UserID),
                                new SqlParameter("@PasswordOld", StringUlt.EncryptPassword(userInfo.PasswordOld)),
                                new SqlParameter("@PasswordNew", StringUlt.EncryptPassword(userInfo.PasswordNew)),

                                new SqlParameter("@SessionID", actorInfo.SessionID),
                                new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()),

                                prErrorCode);

                errorCode = Convert.ToByte(prErrorCode.Value);

                return errorCode == 0 && exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("USRUserDAO.PasswordReset, ex: " + ex.ToString());
                return false;
            }
        }

        public static bool Delete(USRUser userInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[USR.User_Delete]",
                                new SqlParameter("@UserID", userInfo.UserID),
                                
                                new SqlParameter("@CatalogID", EnumCatalogType.User),

                                new SqlParameter("@ActorID", userInfo.EditorID),
                                new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("USRUserDAO.Delete, ex: " + ex.ToString());
                return false;
            }
        }


        /// <summary>
        /// Gán quyền từ người dùng khác
        /// </summary>
        public static bool AssignData(USRAssignData condition)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[USR.User_AssignData]",
                                new SqlParameter("@SourceID", condition.SourceID),
                                new SqlParameter("@UserID", condition.UserID),

                                new SqlParameter("@AssignPerms", condition.AssignPerms),
                                new SqlParameter("@AssignMenu", condition.AssignMenu),

                                new SqlParameter("@EquivalentFlag", condition.EquivalentFlag),

                                new SqlParameter("@ActorID", condition.ActorID),
                                new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("USRUserDAO.AssignData, ex: " + ex.ToString());
                return false;
            }
        }
    }
}