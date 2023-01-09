using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.Enum.MapObject;
using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;
using BAGeocoding.Entity.MapTool;

namespace BAGeocoding.Dal.MapTool
{
    /// <summary>
    /// Quản lý truy xuất thông tin cấu trúc một bảng
    /// </summary>
    public class CTLTableStructureDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPTOOL);

        /// <summary>
        /// Lấy tất cả danh sách
        /// </summary>
        public static List<MCLTableStructure> GetAll(short objectID)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[CTL.TableStructure_GetAll]",
                            new SqlParameter("@ObjectID", objectID));

                // 1. Kiểm tra dữ liệu
                if (dt == null)
                    return null;
                else if (dt.Rows.Count < 1)
                    return null;

                // 2.Lấy dữ liệu
                List<MCLTableStructure> result = new List<MCLTableStructure>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MCLTableStructure dataItem = new MCLTableStructure();
                    if (dataItem.FromDataRow(dt.Rows[i]) == false)
                        return null;
                    result.Add(dataItem);
                }

                return result;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("CTLTableStructureDAO.GetAll, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy lịch sử danh mục chức danh
        /// </summary>
        public static List<MCLTableStructure> GetHistory(int indexID)
        {
            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[CTL.TableStructure_GetHistory]",
                            new SqlParameter("@IndexID", indexID));

                // 1. Kiểm tra dữ liệu
                if (dt == null)
                    return null;
                else if (dt.Rows.Count < 1)
                    return null;

                // 2.Lấy dữ liệu
                List<MCLTableStructure> result = new List<MCLTableStructure>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MCLTableStructure dataItem = new MCLTableStructure();
                    if (dataItem.FromDataRowHistory(dt.Rows[i]) == false)
                        return null;
                    result.Add(dataItem);
                }

                return result;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("CTLTableStructureDAO.GetHistory, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        public static bool Add(MCLTableStructure entity, ref byte errorCode)
        {
            try
            {
                SqlParameter prErrorCodeID = new SqlParameter("@ErrorCode", 0);
                prErrorCodeID.Direction = ParameterDirection.Output;

                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.TableStructure_Add]",
                            new SqlParameter("@ObjectID", entity.ObjectID),
                            new SqlParameter("@FieldName", entity.FieldName),
                            new SqlParameter("@DataType", entity.DataType),
                            new SqlParameter("@FieldType", entity.FieldType),
                            new SqlParameter("@FieldLength", entity.FieldLength),
                            new SqlParameter("@AllowNull", entity.AllowNull),
                            new SqlParameter("@Description", entity.Description),
                            new SqlParameter("@DataExt", entity.DataExt),

                            new SqlParameter("@CatalogID", EnumCatalogType.Field),

                            new SqlParameter("@UserID", entity.CreatorID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()),

                            prErrorCodeID);

                errorCode = Convert.ToByte(prErrorCodeID.Value);

                return exec > 0 && errorCode == 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("CTLTableStructureDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Cập nhật
        /// </summary>
        public static bool Update(MCLTableStructure entity, ref byte errorCode)
        {
            try
            {
                SqlParameter prErrorCodeID = new SqlParameter("@ErrorCode", 0);
                prErrorCodeID.Direction = ParameterDirection.Output;

                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.TableStructure_Update]",
                            new SqlParameter("@IndexID", entity.IndexID),
                            new SqlParameter("@FieldName", entity.FieldName),
                            new SqlParameter("@DataType", entity.DataType),
                            new SqlParameter("@FieldType", entity.FieldType),
                            new SqlParameter("@FieldLength", entity.FieldLength),
                            new SqlParameter("@AllowNull", entity.AllowNull),
                            new SqlParameter("@Description", entity.Description),
                            new SqlParameter("@DataExt", entity.DataExt),

                            new SqlParameter("@CatalogID", EnumCatalogType.Field),

                            new SqlParameter("@UserID", entity.EditorID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()),

                            prErrorCodeID);

                errorCode = Convert.ToByte(prErrorCodeID.Value);

                return exec > 0 && errorCode == 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("CTLTableStructureDAO.Update, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Cập nhật dữ liệu mở rộng
        /// </summary>
        public static bool UpdateDataExt(MCLTableStructure entity)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.TableStructure_UpdateDataExt]",
                            new SqlParameter("@IndexID", entity.IndexID),
                            new SqlParameter("@DataExt", entity.DataExt),

                            new SqlParameter("@CatalogID", EnumCatalogType.Field),

                            new SqlParameter("@UserID", entity.EditorID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("CTLTableStructureDAO.UpdateDataExt, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Cập nhật nhanh thứ tự, trạng thái
        /// </summary>
        public static bool UpdateQuick(MCLTableStructure entity)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.TableStructure_UpdateQuick]",
                            new SqlParameter("@IndexID", entity.IndexID),
                            new SqlParameter("@SortOrder", entity.SortOrder),
                            new SqlParameter("@State", entity.Actived),

                            new SqlParameter("@CatalogID", EnumCatalogType.Field),

                            new SqlParameter("@UserID", entity.EditorID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("CTLTableStructureDAO.UpdateQuick, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Hủy 
        /// </summary>
        public static bool Delete(int indexID, int userID)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.TableStructure_Delete]",
                            new SqlParameter("@IndexID", indexID),
                            new SqlParameter("@UserID", userID),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("CTLTableStructureDAO.Delete, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
