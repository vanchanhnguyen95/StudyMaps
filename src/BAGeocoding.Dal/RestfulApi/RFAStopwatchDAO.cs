using System;
using System.Collections.Generic;
using System.Data;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Entity.RestfulApi;

using BAGeocoding.Utility;

namespace BAGeocoding.Dal.RestfulApi
{
    public class RFAStopwatchDAO
    {
        protected static SqlDatabase sqlDB = new SqlDatabase(SQLHelper.DBMS_CONNECTION_STRING);

        #region ==================== Ghi dữ liệu theo dõi dịch vụ ====================
        /// <summary>
        /// Ghi logs theo dõi
        /// </summary>
        public static void WStopwatch(List<RFAStopwatch> dataList)
        {
            try
            {
                SQLHelper.ExecuteBulkInsert(sqlDB, "dbo.[DTS.Stopwatch]", WStopwatchTable(dataList));
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RFAStopwatchDAO.WStopwatch, ex: " + ex.ToString());
            }

            //MongoHelper._database.GetCollection<RFAStopwatch>("RFAStopwatch").InsertMany(dataList);
        }

        /// <summary>
        /// Khởi tạo bảng để ghi vào CSDL
        /// </summary>
        private static DataTable WStopwatchTable(List<RFAStopwatch> dataList)
        {
            // 1. Khởi tạo bảng
            DataTable dataTable = new DataTable("NewData");
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Int32"), ColumnName = "MethodID" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Int32"), ColumnName = "RegisterID" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.String"), ColumnName = "IPAddress" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.String"), ColumnName = "ParamsStr" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Int64"), ColumnName = "TickTime" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Int32"), ColumnName = "UnixTime" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Int64"), ColumnName = "ElapMethod" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Int64"), ColumnName = "ElapTotal" });

            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Int16"), ColumnName = "YearIndex" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Byte"), ColumnName = "MonthIndex" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Byte"), ColumnName = "DayIndex" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Byte"), ColumnName = "DateIndex" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Byte"), ColumnName = "HourIndex" });

            // 2. Thêm dữ liệu vào bảng
            for (int i = 0; i < dataList.Count; i++)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["MethodID"] = dataList[i].MethodID;
                dataRow["RegisterID"] = dataList[i].RegisterID;
                dataRow["IPAddress"] = dataList[i].IPAddress;
                dataRow["ParamsStr"] = dataList[i].ParamsStr;
                dataRow["TickTime"] = dataList[i].TickTime;
                dataRow["UnixTime"] = dataList[i].UnixTime;
                dataRow["ElapMethod"] = dataList[i].ElapsedMethod;
                dataRow["ElapTotal"] = dataList[i].ElapsedTotal;

                dataRow["YearIndex"] = dataList[i].YearIndex;
                dataRow["MonthIndex"] = dataList[i].MonthIndex;
                dataRow["DayIndex"] = dataList[i].DayIndex;
                dataRow["DateIndex"] = dataList[i].DateIndex;
                dataRow["HourIndex"] = dataList[i].HourIndex;

                dataTable.Rows.Add(dataRow);
            }

            // 3. Trả về kết quả
            return dataTable;
        }
        #endregion

        #region ==================== Ghi dữ liệu lỗi dịch vụ ====================
        /// <summary>
        /// Ghi logs lỗi
        /// </summary>
        public static void WRequestError(List<RFAStopwatch> dataList)
        {
            try
            {
                SQLHelper.ExecuteBulkInsert(sqlDB, "dbo.[DTS.RequestError]", WRequestErrorTable(dataList));
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RFAStopwatchDAO.WRequestError, ex: " + ex.ToString());
            }
        }

        /// <summary>
        /// Khởi tạo bảng để ghi vào CSDL
        /// </summary>
        private static DataTable WRequestErrorTable(List<RFAStopwatch> dataList)
        {
            // 1. Khởi tạo bảng
            DataTable dataTable = new DataTable("NewData");
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Int32"), ColumnName = "MethodID" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Int32"), ColumnName = "RegisterID" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.String"), ColumnName = "KeyStr" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.String"), ColumnName = "IPAddress" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.String"), ColumnName = "ParamsStr" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Byte"), ColumnName = "ErrorCode" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Int64"), ColumnName = "TickTime" });
            dataTable.Columns.Add(new DataColumn { DataType = Type.GetType("System.Int32"), ColumnName = "UnixTime" });

            // 2. Thêm dữ liệu vào bảng
            for (int i = 0; i < dataList.Count; i++)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["MethodID"] = dataList[i].MethodID;
                dataRow["RegisterID"] = dataList[i].RegisterID;
                dataRow["KeyStr"] = dataList[i].KeyStr;
                dataRow["IPAddress"] = dataList[i].IPAddress;
                dataRow["ParamsStr"] = dataList[i].ParamsStr;
                dataRow["ErrorCode"] = dataList[i].ErrorCode;
                dataRow["TickTime"] = dataList[i].TickTime;
                dataRow["UnixTime"] = dataList[i].UnixTime;
                dataTable.Rows.Add(dataRow);
            }

            // 3. Trả về kết quả
            return dataTable;
        }
        #endregion
    }
}
