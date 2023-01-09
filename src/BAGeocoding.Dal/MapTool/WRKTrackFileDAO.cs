using System;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.MapTool.Plan;

using BAGeocoding.Utility;

namespace BAGeocoding.Dal.MapTool
{
    /// <summary>
    /// Quản lý truy xuất track logs
    /// </summary>
    public class WRKTrackFileDAO : SQLHelper
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPTOOL);
        
        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        public static bool Create(WRKTrackFile trackFile)
        {
            try
            {
                int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[WRK.TrackFile_Create]",
                            new SqlParameter("@PlanID", trackFile.PlanID),
                            new SqlParameter("@UserID", trackFile.UserID),
                            new SqlParameter("@TypeID", trackFile.TypeID),
                            new SqlParameter("@FilePath", trackFile.FilePath),
                            new SqlParameter("@CurrentTime", DataUtl.GetUnixTime()));

                return exec > 0;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("WRKTrackFileDAO.Create, ex: " + ex.ToString());
                return false;
            }
        }
    }
}