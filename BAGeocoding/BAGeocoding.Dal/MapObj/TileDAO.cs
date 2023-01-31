using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BAGeocoding.Utility;

//using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapObj;

namespace BAGeocoding.Dal.MapObj
{
    /// <summary>
    /// Quản lý truy xuất thông tin xã/phường
    /// </summary>
    public class TileDAO : SQLHelper
    {
        //protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING);

        public static List<BAGTile> GetAll()
        {
            try
            {
                //DataSet ds = SQLHelper.ExecuteDataset(sqlDB, "[CTL.Tile_GetAll]",
                //                new SqlParameter("@EnumTile", EnumBAGRegionType.Tile));

                //if (ds != null)
                //{
                //    if (ds.Tables.Count > 1)
                //    {
                //        if (ds.Tables[0].Rows.Count > 0)
                //        {
                //            int pointIndex = 0;
                //            List<BAGTile> tileList = new List<BAGTile>();
                //            try
                //            {
                //                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //                {
                //                    BAGTile tileItem = new BAGTile();
                //                    if (tileItem.FromDataRow(ds.Tables[0].Rows[i], ds.Tables[1], ref pointIndex) == false)
                //                        return null;
                //                    tileList.Add(tileItem);
                //                }

                //            }
                //            catch { LogFile.WriteError(string.Format("tileList.Count: {0}", tileList.Count)); }
                //            return tileList;
                //        }
                //    }
                //}
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("TileDAO.GetAll, ex: " + ex.ToString());
                return null;
            }
        }

        public static List<BAGTile> GetByPage()
        {
            int pageNumber = 0;
            int totalCount = 0;

            try
            {
                List<BAGTile> tileList = new List<BAGTile>();
                do
                {
                    List<BAGTile> tileTemp = GetByPage(pageNumber++, ref totalCount);
                    if (tileTemp == null)
                        break;
                    else if (tileTemp.Count == 0)
                        break;
                    tileList.AddRange(tileTemp);
                    
                } while (tileList.Count < totalCount);

                return tileList;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("TileDAO.GetByPage, ex: " + ex.ToString());
                return null;
            }
        }


        private static List<BAGTile> GetByPage(int pageNumber, ref int totalCount)
        {
            try
            {
                //SqlParameter prTotalCount = new SqlParameter("@TotalCount", totalCount);
                //prTotalCount.Direction = ParameterDirection.Output;

                //DataSet ds = SQLHelper.ExecuteDataset(sqlDB, "[CTL.Tile_GetByPage]",
                //                new SqlParameter("@PageNumber", pageNumber),
                //                new SqlParameter("@PageSize", 20000),
                //                new SqlParameter("@EnumTile", EnumBAGRegionType.Tile),
                //                prTotalCount);

                //totalCount = Convert.ToInt32(prTotalCount.Value);

                //if (ds != null)
                //{
                //    if (ds.Tables.Count > 1)
                //    {
                //        if (ds.Tables[0].Rows.Count > 0)
                //        {
                //            int pointIndex = 0;
                //            List<BAGTile> tileList = new List<BAGTile>();
                //            try
                //            {
                //                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //                {
                //                    BAGTile tileItem = new BAGTile();
                //                    if (tileItem.FromDataRow(ds.Tables[0].Rows[i], ds.Tables[1], ref pointIndex) == false)
                //                        return null;
                //                    tileList.Add(tileItem);
                //                }

                //            }
                //            catch { LogFile.WriteError(string.Format("tileList.Count: {0}", tileList.Count)); }
                //            return tileList;
                //        }
                //    }
                //}
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("TileDAO.GetAll, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới thông tin xã/phường
        /// </summary>
        public static bool Add(BAGTile tile)
        {
            try
            {
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.Tile_Add]",
                //            new SqlParameter("@TypeID", EnumBAGRegionType.Tile),
                //            new SqlParameter("@TileID", tile.TileID),
                //            new SqlParameter("@CommuneID", tile.CommuneID),
                //            new SqlParameter("@PointCount", tile.PointList.Count),
                //            new SqlParameter("@LngStr", tile.LngStr ?? string.Empty),
                //            new SqlParameter("@LatStr", tile.LatStr ?? string.Empty));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("CommuneDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Hủy tất cả thông tin xã/phường
        /// </summary>
        public static bool Clear()
        {
            try
            {
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[CTL.Tile_Clear]",
                //            new SqlParameter("@TypeID", EnumBAGRegionType.Tile));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("CommuneDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
