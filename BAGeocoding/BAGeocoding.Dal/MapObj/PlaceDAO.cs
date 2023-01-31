using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BAGeocoding.Utility;

//using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.Enum;

namespace BAGeocoding.Dal.MapObj
{
    /// <summary>
    /// Quản lý truy xuất thông tin khu vực
    /// </summary>
    public class PlaceDAO : SQLHelper
    {
        //protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING);

        /// <summary>
        /// Lấy khu vực theo tỉnh
        /// </summary>
        public static List<BAGPlace> GetByProvince(int provinceID)
        {
            try
            {
                //DataSet ds = SQLHelper.ExecuteDataset(sqlDB, "[DTS.Place_GetByProvince]", 
                //                new SqlParameter("@ProvinceID", provinceID));

                //if (ds != null)
                //{
                //    if (ds.Tables.Count > 1)
                //    {
                //        if (ds.Tables[0].Rows.Count > 0)
                //        {
                //            List<BAGPlace> placeList = new List<BAGPlace>();
                //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //            {
                //                BAGPlace placeData = new BAGPlace();
                //                if(placeData.FromDataRow(ds.Tables[0].Rows[i], ds.Tables[1]) == false)
                //                    return null;
                //                placeList.Add(placeData);
                //            }
                //            return placeList;
                //        }
                //        return null;
                //    }
                //}
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PlaceDAO.GetByProvince, ex: " + ex.ToString());
                return null;
            }
        }
        
        /// <summary>
        /// Lấy khu vực theo loại
        /// </summary>
        public static List<BAGPlace> GetByType(EnumBAGPlaceType typeID)
        {
            try
            {
                //DataSet ds = SQLHelper.ExecuteDataset(sqlDB, "[DTS.Place_GetByType]",
                //                new SqlParameter("@TypeID", typeID));

                //if (ds != null)
                //{
                //    if (ds.Tables.Count > 1)
                //    {
                //        if (ds.Tables[0].Rows.Count > 0)
                //        {
                //            List<BAGPlace> placeList = new List<BAGPlace>();
                //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //            {
                //                BAGPlace placeData = new BAGPlace();
                //                if (placeData.FromDataRow(ds.Tables[0].Rows[i], ds.Tables[1]) == false)
                //                    return null;
                //                placeList.Add(placeData);
                //            }
                //            return placeList;
                //        }
                //        return null;
                //    }
                //}
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PlaceDAO.GetByType, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thêm mới thông tin khu vực
        /// </summary>
        public static bool Add(BAGPlace placeData, ref int realID)
        {
            try
            {
                //SqlParameter prPlaceID = new SqlParameter("@PlaceID", realID);
                //prPlaceID.Direction = ParameterDirection.Output;

                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.Place_Add]",
                //            new SqlParameter("@TypeID", placeData.TypeID),
                //            new SqlParameter("@ParentID", placeData.ParentID),
                //            new SqlParameter("@Name", placeData.Name),
                //            new SqlParameter("@Address", placeData.Address),
                //            new SqlParameter("@Description", placeData.Description),
                //            new SqlParameter("@Lng", placeData.Center.Lng),
                //            new SqlParameter("@Lat", placeData.Center.Lat),
                //            prPlaceID);

                //realID = Convert.ToInt32(prPlaceID.Value);

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PlaceDAO.Add, ex: " + ex.ToString());
                return false;
            }
        }
        
        /// <summary>
        /// Hủy tất cả khu vực theo tỉnh
        /// </summary>
        public static bool Clear(short provinceID)
        {
            try
            {
                //int exec = SQLHelper.ExecuteNonQuery(sqlDB, "[DTS.Place_Clear]",
                //            new SqlParameter("@ProvinceID", provinceID),

                //            new SqlParameter("@EnumUrban", EnumBAGPlaceType.Urban),
                //            new SqlParameter("@EnumPortion", EnumBAGPlaceType.Portion),
                //            new SqlParameter("@EnumPlot", EnumBAGPlaceType.Plot));

                //return exec > 0;
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PlaceDAO.Clear, ex: " + ex.ToString());
                return false;
            }
        }
    }
}
