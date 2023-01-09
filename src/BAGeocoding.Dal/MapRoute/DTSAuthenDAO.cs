using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.DataService;
using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.Utility;

using BAGeocoding.Utility;

namespace BAGeocoding.Dal.MapRoute
{
    /// <summary>
    /// Quản lý truy xuất thông tin xác thực kết nối dịch vụ
    /// </summary>
    public class DTSAuthenDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPROUTE);

        public static Hashtable GetActived(ref Hashtable trafficHT)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[DTS.Authen_GetActived]", null);

                if (dt == null)
                    return null;
                else if (dt.Rows.Count < 1)
                    return null;

                trafficHT = new Hashtable();
                Hashtable authenHT = new Hashtable();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    UTLAuthen authenInfo = new UTLAuthen();
                    if (authenInfo.FromDataSimple(dt.Rows[i]) == false)
                        return null;
                    LogFile.WriteProcess(string.Format("Authen: {0}", authenInfo.ToString()));
                    if (authenHT.ContainsKey(authenInfo.Username) == false)
                        authenHT.Add(authenInfo.Username, authenInfo);
                    if (trafficHT.ContainsKey(authenInfo.AuthenID) == false)
                        trafficHT.Add(authenInfo.AuthenID, new DTSTraffic(authenInfo.AuthenID));
                }
                return authenHT;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("DTSAuthenDAO.GetActived, ex: " + ex.ToString());
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
                LogFile.WriteError("DTSAuthenDAO.Add, ex: " + ex.ToString());
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
                LogFile.WriteError("DTSAuthenDAO.Update, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
