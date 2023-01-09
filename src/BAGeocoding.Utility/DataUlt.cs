using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace BAGeocoding.Utility
{
    public class DataUlt
    {
        public static Random RANDOM = new Random();
        public static TimeSpan TIMESPAN_DIFFERENCE = new TimeSpan(0, 0, 0);

        public static DateTime GetDateTimeFull()
        {
            if (TIMESPAN_DIFFERENCE.TotalSeconds == 0)
                return DateTime.Now;
            else
                return DateTime.Now.Add(TIMESPAN_DIFFERENCE);
        }

        public static byte Random(byte min, byte max)
        {
            lock (RANDOM)
            {
                return (byte)(min + RANDOM.Next(max - min));
            }
        }

        /// <summary>
        /// Chuyển đổi Object -> Chuỗi Json (Lưu ý: .Net 4.0 chỉ dùng được Newtonsoft.Json 7.0)
        /// </summary>
        public static string Object2Json<T>(T obj)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                ser.WriteObject(ms, obj);
                byte[] json = ms.ToArray();
                ms.Close();
                return Constants.UTF8CodePage.GetString(json, 0, json.Length);

                //return JsonConvert.SerializeObject(obj, Formatting.Indented);
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("DataUlt.Object2Json, ex: ", ex.ToString()));
                return string.Empty;
            }
        }

        /// <summary>
        /// Chuyển đổi chuỗi Json -> Object (Lưu ý: .Net 4.0 chỉ dùng được Newtonsoft.Json 7.0)
        /// </summary>
        public static T Json2Object<T>(string str)
        {
            try
            {
                MemoryStream ms = new MemoryStream(Constants.UTF8CodePage.GetBytes(str));
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                return (T)ser.ReadObject(ms);

                //return JsonConvert.DeserializeObject<T>(str);
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("DataUlt.Json2Object, ex: ", ex.ToString()));
                return default(T);
            }
        }
    }
}
