using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BAGeocoding.Utility;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapImg;

namespace BAGeocoding.Dal.MapImg
{
    /// <summary>
    /// Quản lý danh sách đăng ký sử dụng bản đồ
    /// </summary>
    public class RequestURLDAO : SQLHelper
    {
        //protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPIMAGE);

        public static List<BAGRequestURL> GetAll(bool actived)
        {
            try
            {
                //DataTable dt = SQLHelper.ExecuteDataTable(sqlDB, "[CFG.RequestURL_List]",
                //                new SqlParameter("@Actived", actived));

                //if (dt != null)
                //{
                //    List<BAGRequestURL> requestList = new List<BAGRequestURL>();
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        BAGRequestURL requestItem = new BAGRequestURL();
                //        if (requestItem.FromDataRow(dt.Rows[i]) == false)
                //            return null;
                //        requestList.Add(requestItem);
                //    }
                //    return requestList;
                //}
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RequestURLDAO.GetAll, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Xử lý đăng ký sử dụng bản đồ
        /// </summary>
        public static bool Create(BAGRequestURL request)
        {
            try
            {
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CFG.RequestURL_Create]",
                //            new SqlParameter("@RequestURL", request.RequestURL),
                //            new SqlParameter("@DataExt", request.DataExt),
                //            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RequestURLDAO.Create, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Hủy đăng ký sử dụng bản đồ
        /// </summary>
        public static bool Delete(BAGRequestURL request)
        {
            try
            {
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CFG.RequestURL_Delete]",
                //            new SqlParameter("@IndexID", request.IndexID),
                //            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RequestURLDAO.Delete, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
