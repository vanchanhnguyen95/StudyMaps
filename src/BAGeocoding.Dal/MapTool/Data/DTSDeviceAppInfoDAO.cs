using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.Enum.MapTool;
using BAGeocoding.Entity.MapTool;

using BAGeocoding.Utility;
using BAGeocoding.Entity.MapTool.Data;

namespace BAGeocoding.Dal.MapTool.Data
{
    public class DTSDeviceAppInfoDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPTOOL);
                
        /// <summary>
        /// Lấy lịch sử cập nhật app
        /// </summary>
        public static List<DTSDeviceAppInfo> GetByDevice(int deviceID)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[DTS.DeviceAppInfo_GetByDevice]",
                                new SqlParameter("@DeviceID", deviceID));

                if (dt == null)
                    return null;
                List<DTSDeviceAppInfo> resultList = new List<DTSDeviceAppInfo>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DTSDeviceAppInfo itemInfo = new DTSDeviceAppInfo();
                    if (itemInfo.FromDataRow(dt.Rows[i]) == false)
                        return null;
                    resultList.Add(itemInfo);
                }
                return resultList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("DTSDeviceAppInfoDAO.GetByDevice, ex: " + ex.ToString());
                return null;
            }
        }
    }
}