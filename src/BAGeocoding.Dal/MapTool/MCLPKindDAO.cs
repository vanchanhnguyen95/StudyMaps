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
    /// Quản lý truy xuất thông tin loại điểm
    /// </summary>
    public class MCLPKindDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPTOOL);

        /// <summary>
        /// Lấy danh sách loại điểm
        /// </summary>
        public static List<MCLPKind> GetAll()
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[CTL.PKind_GetAll]", null);
                if (dt == null)
                    return null;
                List<MCLPKind> dataList = new List<MCLPKind>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MCLPKind dataInfo = new MCLPKind();
                    if (dataInfo.FromDataRow(dt.Rows[i]) == false)
                        return null;
                    dataList.Add(dataInfo);
                }
                return dataList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("MCLPKindDAO.GetAll, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới loại điểm
        /// </summary>
        public static bool Add(MCLPKind kindInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.PKind_Add]",
                            new SqlParameter("@ParentID", kindInfo.ParentID),
                            new SqlParameter("@Name", kindInfo.Name),
                            new SqlParameter("@Description", kindInfo.Description),
                            new SqlParameter("@DataExt", kindInfo.DataExt),

                            new SqlParameter("@CatalogID", EnumCatalogType.PKind),

                            new SqlParameter("@UserID", kindInfo.CreatorID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("MCLPKindDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Cập nhật loại điểm
        /// </summary>
        public static bool Update(MCLPKind kindInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.PKind_Update]",
                            new SqlParameter("@KindID", kindInfo.KindID),
                            new SqlParameter("@ParentID", kindInfo.ParentID),
                            new SqlParameter("@Name", kindInfo.Name),
                            new SqlParameter("@Description", kindInfo.Description),
                            new SqlParameter("@DataExt", kindInfo.DataExt),

                            new SqlParameter("@CatalogID", EnumCatalogType.PKind),

                            new SqlParameter("@UserID", kindInfo.EditorID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("MCLPKindDAO.Update, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Cập nhật nhanh
        /// </summary>
        public static bool UpdateQuick(MCLPKind kindInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.PKind_UpdateQuick]",
                            new SqlParameter("@KindID", kindInfo.KindID),
                            new SqlParameter("@SortOrder", kindInfo.SortOrder),
                            new SqlParameter("@State", kindInfo.Actived),

                            new SqlParameter("@CatalogID", EnumCatalogType.PKind),

                            new SqlParameter("@UserID", kindInfo.EditorID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("MCLPKindDAO.Update, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Cập nhật dữ liệu mở rộng
        /// </summary>
        public static bool UpdateDataExt(MCLPKind kindInfo)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.PKind_UpdateDataExt]",
                            new SqlParameter("@KindID", kindInfo.KindID),
                            new SqlParameter("@DataExt", kindInfo.DataExt),

                            new SqlParameter("@CatalogID", EnumCatalogType.PKind),

                            new SqlParameter("@UserID", kindInfo.EditorID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("MCLPKindDAO.Update, ex: " + ex.ToString());
                return false;
            }
        }
    }
}