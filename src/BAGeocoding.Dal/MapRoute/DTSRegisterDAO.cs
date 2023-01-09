using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.Router;

using BAGeocoding.Utility;

namespace BAGeocoding.Dal.MapRoute
{
    /// <summary>
    /// Quản lý truy xuất thông tin key đăng ký sử dụng dịch vụ
    /// </summary>
    public class DTSRegisterDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPROUTE);

        public static Hashtable GetActived()
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[DTS.Register_GetActived]", null);

                if (dt == null)
                    return null;
                else if (dt.Rows.Count < 1)
                    return null;

                Hashtable registerHT = new Hashtable();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    BARRegister registerInfo = new BARRegister();
                    if (registerInfo.FromDataSimple(dt.Rows[i]) == false)
                        return null;
                    LogFile.WriteProcess(string.Format("Register: {0}", registerInfo.ToString()));
                    if (registerHT.ContainsKey(registerInfo.KeyStr) == false)
                        registerHT.Add(registerInfo.KeyStr, registerInfo);
                    else
                        ((BARRegister)registerHT[registerInfo.KeyStr]).UpdateIP(registerInfo);
                }
                return registerHT;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("DTSRegisterDAO.GetActived, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới thông tin xã/phường
        /// </summary>
        public static bool Add(BAGCommune commune)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.Commune_Add]",
                            new SqlParameter("@CommuneID", commune.CommuneID),
                            new SqlParameter("@DistrictID", commune.DistrictID),
                            new SqlParameter("@VName", commune.VName),
                            new SqlParameter("@EName", commune.EName),
                            new SqlParameter("@PointCount", commune.PointList.Count),
                            new SqlParameter("@LngStr", commune.LngStr),
                            new SqlParameter("@LatStr", commune.LatStr),
                            new SqlParameter("@GeoStr", commune.GeoStr));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("DTSRegisterDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Cập nhật tên xã/phường
        /// </summary>
        public static bool Update(BAGCommune commune)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.Commune_UpdateName]",
                            new SqlParameter("@CommuneID", commune.CommuneID),
                            new SqlParameter("@VName", commune.VName),
                            new SqlParameter("@EName", commune.EName));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("DTSRegisterDAO.Update, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
