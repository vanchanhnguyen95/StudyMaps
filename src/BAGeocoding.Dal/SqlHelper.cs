using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using BAGeocoding.Utility;

namespace BAGeocoding.Dal
{
    public class SQLHelper
    {
        public static string DBMS_CONNECTION_STRING = Constants.DBMS_CONNECTION_STRING_GEOCODING;
        public static string DBMS_CONNECTION_STRING_MAPIMAGE = Constants.DBMS_CONNECTION_STRING_MAPIMAGE;
        public static string DBMS_CONNECTION_STRING_MAPTOOL = Constants.DBMS_CONNECTION_STRING_MAPTOOL;
        public static string DBMS_CONNECTION_STRING_MAPROUTE = Constants.DBMS_CONNECTION_STRING_MAPROUTE;

        private static string m_ParametersList = string.Empty;
        public static string ParametersList
        {
            get { return m_ParametersList; }
            set { m_ParametersList = value; }
        }

        #region ======================================== Functions ========================================
        /// <summary>
        /// This method is used to attach array of SqlParameters to a SqlCommand.
        /// 
        /// This method will assign a value of DbNull to any parameter with a direction of
        /// InputOutput and a value of null.  
        /// 
        /// This behavior will prevent default values from being used, but
        /// this will be the less common case than an intended pure output parameter (derived as InputOutput)
        /// where the user provided no input value.
        /// </summary>
        /// <param name="command">
        /// The command to which the parameters will be added
        /// </param>
        /// <param name="commandParameters">
        /// An array of SqlParameters to be added to command
        /// </param>
        private static void AttachParameters(DbCommand command, SqlParameter[] commandParameters)
        {
            m_ParametersList = string.Empty;
            foreach (SqlParameter p in commandParameters)
            {
                //check for derived output value with no value assigned
                if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }

                command.Parameters.Add(p);

                // Lấy danh sách parramn
                if (m_ParametersList.Length > 0)
                    m_ParametersList += ", ";

                if (p.Value != null)
                    m_ParametersList += string.Format("{0}: {1}", p.ParameterName, p.Value.ToString());
                else if (p.Value != null)
                    m_ParametersList += string.Format("{0}: {1}", p.ParameterName, "Output param");
            }
        }

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against the specified SqlConnection 
        /// using the provided parameters.
        /// </summary>
        /// <example>
        /// int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", 
        ///									new SqlParameter("@prodid", 24));
        /// </example>
        /// <param name="connection">a valid SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(SqlDatabase database, string commandText, params SqlParameter[] commandParameters)
        {
            try
            {
                using (DbCommand proComm = database.GetStoredProcCommand(commandText))
                {
                    proComm.CommandTimeout = 0;
                    if (commandParameters != null)
                        AttachParameters(proComm, commandParameters);
                    return database.ExecuteNonQuery(proComm);
                }
            }
            catch (Exception ex)
            {
                string _error = "";
                if (commandParameters != null)
                {
                    foreach (SqlParameter p in commandParameters)
                    {
                        if (p.Value != null)
                            _error = StringUlt.Join(",", _error, StringUlt.Join(":", p.ParameterName, p.Value.ToString()));
                        else
                            _error = StringUlt.Join(",", _error, StringUlt.Join(":", p.ParameterName, "Output param"));
                    }
                }
                LogFile.WriteError(StringUlt.Join(String.Empty, "SqlHelper, ExecuteNonQuery, Command: ", commandText, "(", _error, "), ex: ", ex.ToString()));
                return 0;
            }
        }
        
        /// <summary>
        /// Thực thi ghi bảng dữ liệu
        /// </summary>
        public static void ExecuteBulkInsert(SqlDatabase database, string tableName, DataTable dataTable)
        {
            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(database.ConnectionString))
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.WriteToServer(dataTable);
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteError(StringUlt.Join(String.Empty, "SqlHelper, ExecuteBulkInsert, ex: ", ex.ToString()));
            }
        }

        /// <summary>
        /// Thuc thi phuong thuc lay nhanh 1 tham so
        /// </summary>
        /// <param name="database"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(SqlDatabase database, string commandText, params SqlParameter[] commandParameters)
        {
            try
            {
                using (DbCommand proComm = database.GetStoredProcCommand(commandText))
                {
                    proComm.CommandTimeout = 0;
                    if (commandParameters != null)
                        AttachParameters(proComm, commandParameters);
                    return database.ExecuteScalar(proComm);
                }
            }
            catch (Exception ex)
            {
                string _error = "";
                if (commandParameters != null)
                {
                    foreach (SqlParameter p in commandParameters)
                    {
                        if (p.Value != null)
                            _error = StringUlt.Join(",", _error, StringUlt.Join(":", p.ParameterName, p.Value.ToString()));
                        else
                            _error = StringUlt.Join(",", _error, StringUlt.Join(":", p.ParameterName, "Output param"));
                    }
                }
                LogFile.WriteError(StringUlt.Join(String.Empty, "SqlHelper, ExecuteNonQuery, Command: ", commandText, "(", _error, "), ex: ", ex.ToString()));
                return null;
            }
        }

        /// <summary>
        /// Truy xuất CSDL có phiên làm việc
        /// </summary>
        public static int ExecuteNonQuery(DbTransaction transaction, SqlDatabase database, string commandText, params SqlParameter[] commandParameters)
        {
            try
            {
                using (DbCommand proComm = database.GetStoredProcCommand(commandText))
                {
                    proComm.CommandTimeout = 0;
                    if (commandParameters != null)
                        AttachParameters(proComm, commandParameters);
                    return database.ExecuteNonQuery(proComm, transaction);
                }
            }
            catch (Exception ex)
            {
                string _error = "";
                if (commandParameters != null)
                {
                    foreach (SqlParameter p in commandParameters)
                    {
                        if (p.Value != null)
                            _error = StringUlt.Join(",", _error, StringUlt.Join(":", p.ParameterName, p.Value.ToString()));
                        else
                            _error = StringUlt.Join(",", _error, StringUlt.Join(":", p.ParameterName, "Output param"));
                    }
                }
                LogFile.WriteError(StringUlt.Join(String.Empty, "SqlHelper, ExecuteNonQuery, Command: ", commandText, "(", _error, "), ex: ", ex.ToString()));
                return 0;
            }
        }

        /// <summary>
        /// Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
        /// using the provided parameters.
        /// </summary>
        /// <example>
        /// DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </example>
        /// <param name="connection"> a valid SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        public static DataSet ExecuteDataset(SqlDatabase database, string commandText, params SqlParameter[] commandParameters)
        {
            try
            {
                using (DbCommand proComm = database.GetStoredProcCommand(commandText))
                {
                    proComm.CommandTimeout = 0;
                    if (commandParameters != null)
                        AttachParameters(proComm, commandParameters);
                    return database.ExecuteDataSet(proComm);
                }
            }
            catch (Exception ex)
            {
                string _error = "";
                if (commandParameters != null)
                {
                    foreach (SqlParameter p in commandParameters)
                    {
                        if (p.Value != null)
                            _error = StringUlt.Join(",", _error, StringUlt.Join(":", p.ParameterName, p.Value.ToString()));
                        else
                            _error = StringUlt.Join(",", _error, StringUlt.Join(":", p.ParameterName, "Output param"));
                    }
                }
                LogFile.WriteError(StringUlt.Join(String.Empty, "SqlHelper, ExecuteDataset, Command: ", commandText, "(", _error, "), ex: ", ex.ToString()));
                return null;
            }
        }

        /// <summary>
        /// Truy xuất CSDL trả về bảng
        /// </summary>
        public static DataTable ExecuteDataTable(SqlDatabase database, string commandText, params SqlParameter[] commandParameters)
        {
            DataSet ds = ExecuteDataset(database, commandText, commandParameters);

            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    return ds.Tables[0];
                else
                    return null;
            }
            else
                return null;
        }
        #endregion
    }
}
