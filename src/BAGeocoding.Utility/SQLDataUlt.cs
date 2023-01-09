using System;
using System.Data;

namespace BAGeocoding.Utility
{
    [Serializable]
    public class SQLDataUlt : ICloneable //345,456456456
    {
        protected virtual T GetDataValue<T>(DataRow dataRow, string columnName)
        {
            T ret = default(T);
            if (dataRow.Table.Columns.Contains(columnName) == true)
            {
                try
                {
                    object value = dataRow[columnName];
                    if (value == null || DBNull.Value.Equals(value))
                    {
                        return default(T);
                    }
                    else if (typeof(T).IsEnum == true)
                    {
                        ret = (T)System.Enum.ToObject(typeof(T), value);

                    }
                    else
                    {
                        Type t = typeof(T);
                        ret = (T)Convert.ChangeType(value, Nullable.GetUnderlyingType(t) ?? t);
                    }
                }
                catch { }
            }
            return ret;
        }

        protected virtual T GetDataValue<T>(DataRow dataRow, int indexColumn)
        {
            T ret = default(T);
            if (dataRow.Table.Columns.Count > indexColumn)
            {
                try
                {
                    object value = dataRow[indexColumn];
                    if (value == null || DBNull.Value.Equals(value))
                    {
                        return default(T);
                    }
                    else if (typeof(T).IsEnum == true)
                    {
                        ret = (T)System.Enum.ToObject(typeof(T), value);
                    }
                    else
                    {
                        Type t = typeof(T);
                        ret = (T)Convert.ChangeType(value, Nullable.GetUnderlyingType(t) ?? t);
                    }
                }
                catch { }
            }
            return ret;
        }

        protected virtual T GetDataValue<T>(DataRow dataRow, string columnName, object defaultValue)
        {
            T ret = default(T);
            Type t = typeof(T);

            if (dataRow.Table.Columns.Contains(columnName) == false)
            {
                if (typeof(T).IsEnum == true)
                    ret = (T)System.Enum.ToObject(typeof(T), defaultValue);
                else
                    ret = (T)Convert.ChangeType(defaultValue, Nullable.GetUnderlyingType(t) ?? t);
            }
            else
            {
                try
                {
                    object value = dataRow[columnName];
                    if (value == null || DBNull.Value.Equals(value))
                    {
                        if (typeof(T).IsEnum == true)
                            ret = (T)System.Enum.ToObject(typeof(T), defaultValue);
                        else if (defaultValue == null)
                            ret = default(T);
                        else
                            ret = (T)Convert.ChangeType(defaultValue, Nullable.GetUnderlyingType(t) ?? t);
                    }
                    else if (typeof(T).IsEnum == true)
                    {
                        ret = (T)System.Enum.ToObject(typeof(T), value);
                    }
                    else
                    {
                        ret = (T)Convert.ChangeType(value, Nullable.GetUnderlyingType(t) ?? t);
                    }
                }
                catch { Console.Write("A"); }
            }
            return ret;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
