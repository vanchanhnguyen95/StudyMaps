using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.MapTool;

using BAGeocoding.Utility;

namespace BAGeocoding.Dal.MapTool
{
    /// <summary>
    /// Quản lý truy xuất thông tin thiết bị iPad
    /// </summary>
    public class MCLIPadDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPTOOL);

        /// <summary>
        /// Lấy danh sách loại điểm
        /// </summary>
        public static List<MCLIPad> GetAll()
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[CTL.DeviceIPad_GetAll]", null);
                if (dt == null)
                    return null;
                List<MCLIPad> dataList = new List<MCLIPad>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MCLIPad dataInfo = new MCLIPad();
                    if (dataInfo.FromDataRow(dt.Rows[i]) == false)
                        return null;
                    dataList.Add(dataInfo);
                }
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("MCLIPadDAO.GetAll, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới loại điểm
        /// </summary>
        public static bool Add(MCLIPad ipadInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.DeviceIPad_Add]",
                            new SqlParameter("@IMEINumber", ipadInfo.IMEINumber),
                            new SqlParameter("@MachineKey", ipadInfo.MachineKey),
                            new SqlParameter("@SIMNumber", ipadInfo.SIMNumber),
                            new SqlParameter("@Description", ipadInfo.Description),
                            new SqlParameter("@DataExt", ipadInfo.DataExt),

                            new SqlParameter("@CatalogID", EnumCatalogType.IPad),

                            new SqlParameter("@UserID", ipadInfo.CreatorID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("MCLIPadDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Cập nhật loại điểm
        /// </summary>
        public static bool Update(MCLIPad ipadInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.DeviceIPad_Update]",
                            new SqlParameter("@DeviceID", ipadInfo.DeviceID),
                            new SqlParameter("@MachineKey", ipadInfo.MachineKey),
                            new SqlParameter("@SIMNumber", ipadInfo.SIMNumber),
                            new SqlParameter("@Description", ipadInfo.Description),
                            new SqlParameter("@DataExt", ipadInfo.DataExt),

                            new SqlParameter("@CatalogID", EnumCatalogType.IPad),

                            new SqlParameter("@UserID", ipadInfo.EditorID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("MCLIPadDAO.Update, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Cập nhật nhanh
        /// </summary>
        public static bool UpdateQuick(MCLIPad ipadInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.DeviceIPad_UpdateQuick]",
                            new SqlParameter("@DeviceID", ipadInfo.DeviceID),
                            new SqlParameter("@SortOrder", ipadInfo.SortOrder),
                            new SqlParameter("@State", ipadInfo.Actived),

                            new SqlParameter("@CatalogID", EnumCatalogType.PKind),

                            new SqlParameter("@UserID", ipadInfo.EditorID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("MCLIPadDAO.Update, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Cập nhật dữ liệu mở rộng
        /// </summary>
        public static bool UpdateDataExt(MCLIPad ipadInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.DeviceIPad_UpdateDataExt]",
                            new SqlParameter("@DeviceID", ipadInfo.DeviceID),
                            new SqlParameter("@DataExt", ipadInfo.DataExt),

                            new SqlParameter("@CatalogID", EnumCatalogType.IPad),

                            new SqlParameter("@UserID", ipadInfo.EditorID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("MCLIPadDAO.Update, ex: " + ex.ToString());
                return false;
            }
        }
    }
}