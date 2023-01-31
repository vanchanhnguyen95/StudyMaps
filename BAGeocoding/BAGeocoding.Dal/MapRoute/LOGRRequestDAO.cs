using System;
using System.Collections.Generic;
using System.Data;

//using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.Router;

using BAGeocoding.Utility;

namespace BAGeocoding.Dal.MapRoute
{
    /// <summary>
    /// Quản lý truy xuất thông tin xác thực kết nối dịch vụ
    /// </summary>
    public class LOGRRequestDAO : SQLHelper
    {
        //protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING_MAPROUTE);
        
        /// <summary>
        /// Ghi logs tìm đường
        /// </summary>
        public static void WriteData(List<BARRouteLogs> logsData)
        {
            try
            {
                //SQLHelper.ExecuteBulkInsert(sqlDB, "dbo.[LOGS.RRequestDetail]", DataTableGenerateData(logsData));
            }
            catch (Exception ex)
            {
                LogFile.WriteError("LOGRRequestDAO.WriteData, ex: " + ex.ToString());
            }
        }

        /// <summary>
        /// Khởi tạo bảng để ghi vào CSDL
        /// </summary>
        private static DataTable DataTableGenerateData(List<BARRouteLogs> logsData)
        {
            // 1. Khởi tạo bảng
            DataTable dataTable = new DataTable("NewData");
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Byte"), ColumnName = "MethodID" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Int32"), ColumnName = "RegisterID" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Int64"), ColumnName = "TickTime" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Int32"), ColumnName = "UnixTime" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.String"), ColumnName = "Params" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Byte"), ColumnName = "Amount" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Byte"), ColumnName = "Google" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.String"), ColumnName = "TypeStr" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Int32"), ColumnName = "MonthIndex" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Byte"), ColumnName = "DayIndex" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Byte"), ColumnName = "HourIndex" });

            // 2. Thêm dữ liệu vào bảng
            for (int i = 0; i < logsData.Count; i++)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["MethodID"] = logsData[i].MethodID;
                dataRow["RegisterID"] = logsData[i].RegisterID;
                dataRow["TickTime"] = logsData[i].TickTime;
                dataRow["UnixTime"] = logsData[i].UnixTime;
                dataRow["Params"] = logsData[i].Params;
                dataRow["Amount"] = logsData[i].Amount;
                dataRow["Google"] = logsData[i].Google;
                dataRow["TypeStr"] = logsData[i].TypeStr;
                dataRow["MonthIndex"] = logsData[i].MonthIndex;
                dataRow["DayIndex"] = logsData[i].DayIndex;
                dataRow["HourIndex"] = logsData[i].HourIndex;
                dataTable.Rows.Add(dataRow);
            }

            // 3. Trả về kết quả
            return dataTable;
        }

        /// <summary>
        /// Ghi logs lỗi tìm đường
        /// </summary>
        public static void WriteError(List<BARRouteLogs> logsError)
        {
            try
            {
                //SQLHelper.ExecuteBulkInsert(sqlDB, "dbo.[LOGS.RRequestError]", DataTableGenerateError(logsError));
            }
            catch (Exception ex)
            {
                LogFile.WriteError("LOGRRequestDAO.WriteError, ex: " + ex.ToString());
            }
        }

        /// <summary>
        /// Khởi tạo bảng để ghi vào CSDL
        /// </summary>
        private static DataTable DataTableGenerateError(List<BARRouteLogs> logsError)
        {
            // 1. Khởi tạo bảng
            DataTable dataTable = new DataTable("NewData");
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Byte"), ColumnName = "MethodID" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Int32"), ColumnName = "RegisterID" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Int64"), ColumnName = "TickTime" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Int32"), ColumnName = "UnixTime" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.String"), ColumnName = "Params" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Int32"), ColumnName = "MonthIndex" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Byte"), ColumnName = "DayIndex" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Byte"), ColumnName = "HourIndex" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Byte"), ColumnName = "ErrorCode" });

            // 2. Thêm dữ liệu vào bảng
            for (int i = 0; i < logsError.Count; i++)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["MethodID"] = logsError[i].MethodID;
                dataRow["RegisterID"] = logsError[i].RegisterID;
                dataRow["TickTime"] = logsError[i].TickTime;
                dataRow["UnixTime"] = logsError[i].UnixTime;
                dataRow["Params"] = logsError[i].Params;
                dataRow["MonthIndex"] = logsError[i].MonthIndex;
                dataRow["DayIndex"] = logsError[i].DayIndex;
                dataRow["HourIndex"] = logsError[i].HourIndex;
                dataRow["ErrorCode"] = logsError[i].ErrorCode;
                dataTable.Rows.Add(dataRow);
            }

            // 3. Trả về kết quả
            return dataTable;
        }
    }
}