using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BAGeocoding.Utility
{
    public class DataUtl
    {
        private static string IPADDRESS_INTERNER = string.Empty;
        private static string IPADDRESS_INTERNET = string.Empty;

        public static DateTime START_UNIX_TIME = new DateTime(1970, 1, 1);
        
        public static DateTime GetFullTime()
        {
            return DateTime.Now;
        }

        public static DateTime GetCurrDate()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        }

        public static DateTime GetCurrDateTime()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }

        public static DateTime GetStartDay(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day);
        }

        public static DateTime GetEndDay(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
        }
        
        public static DateTime GetStartMonth()
        {
            return GetStartMonth(DateTime.Now);
        }

        public static DateTime GetStartMonth(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        public static DateTime GetEndMonth()
        {
            return GetEndMonth(DateTime.Now);
        }

        public static DateTime GetEndMonth(DateTime dt)
        {
            return GetStartMonth(dt).AddMonths(1).AddSeconds(-1);
        }

        public static int GetUnixTime()
        {
            return GetUnixTime(DateTime.Now);
        }

        public static int GetUnixTime(DateTime dateTime)
        {
            if (dateTime.Year < START_UNIX_TIME.Year)
                return 0;
            else
                return Convert.ToInt32((dateTime - START_UNIX_TIME).TotalSeconds);
        }

        public static DateTime GetTimeUnix(int second = 0)
        {
            return START_UNIX_TIME.AddSeconds(second);
        }

        public static int GetMonthIndex()
        {
            return GetMonthIndex(GetCurrDate());
        }

        public static int GetMonthIndex(DateTime dt)
        {
            return dt.Year * 12 + dt.Month;
        }

        public static double ConvertMeterToLngLat(int distance)
        {
            return (double)((distance * 360) / (2 * Math.PI * Constants.EARTH_RADIUS));
        }

        public static void ConvertDistanceIntersect(string data)
        {
            try
            {
                if (data.Length == 0)
                    return;
                string[] temp = data.Split(',');
                Constants.DISTANCE_INTERSECT_LIST = new List<double>();
                for (int i = 0; i < temp.Length; i++)
                    Constants.DISTANCE_INTERSECT_LIST.Add(ConvertMeterToLngLat(Convert.ToInt32(temp[i])));
            }
            catch { }

            if (Constants.DISTANCE_INTERSECT_LIST.Count == 0)
                Constants.DISTANCE_INTERSECT_LIST.Add(Constants.DISTANCE_INTERSECT_ROAD);
        }
        
        public static List<string> ProcessKeyList(string keyStr)
        {
            return keyStr.Split(' ').ToList();
        }

        public static bool CheckBuilding(bool serial, short build, short start, short end)
        {
            if (build == 0)
                return true;
            else if (serial == false && build % 2 != start % 2)
                return false;
            else if (build < Math.Min(start, end))
                return false;
            else if (build > Math.Max(start, end))
                return false;
            else
                return true;
        }

        public static short DetechBuilding(short build, short start, short end, int leng, ref short delta, ref int index)
        {
            index = 0;
            if (build < start)
            {
                delta = (short)Math.Abs(start - build);
                return start;
            }
            else
            {
                index = leng - 1;
                delta = (short)Math.Abs(end - build);
                return end;
            }
        }


        public static bool IsNumberic(string key)
        {
            for (int i = 0; i < key.Length; i++)
            {
                if (!char.IsNumber(key, i))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Lấy giá trị số
        /// </summary>
        public static bool GetNumber(string data, ref short result)
        {
            try
            {
                int nStart = -1;
                int nEnd = data.Length;
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] > '0' && data[i] <= '9')
                    {
                        nStart = i;
                        break;
                    }
                }
                if (nStart < 0)
                {
                    result = 0;
                    return false;
                }
                for (int i = nStart + 1; i < data.Length; i++)
                {
                    if (data[i] < '0' || data[i] > '9')
                    {
                        nEnd = i;
                        break;
                    }
                }
                if (nStart < nEnd)
                {
                    result = Convert.ToInt16(data.Substring(nStart, nEnd - nStart));
                    return true;
                }
                else
                {
                    result = 0;
                    return false;
                }
            }
            catch
            {
                result = 0;
                return false;
            }
        }

        /// <summary>
        /// Lấy giá trị số nhà
        /// </summary>
        public static bool GetNumber(string data, int shift, ref short result)
        {
            try
            {
                if (data.ToLower().IndexOf("quoc lo") == 0 || data.ToLower().IndexOf("tinh lo") == 0 || data.ToLower().IndexOf("huyen lo") == 0 || data.ToLower().IndexOf("huong lo") == 0 || data.ToLower().IndexOf("duong") == 0 || data.ToLower().IndexOf("ngo") == 0 || data.ToLower().IndexOf("nghach") == 0 || data.ToLower().IndexOf("kiet") == 0)
                {
                    result = 0;
                    return false;
                }
                int nStart = -1;
                int nEnd = data.Length;
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] > '0' && data[i] <= '9')
                    {
                        nStart = i;
                        break;
                    }
                }
                if (nStart < 0)
                {
                    result = 0;
                    return false;
                }
                for (int i = nStart + 1; i < data.Length; i++)
                {
                    if (data[i] < '0' || data[i] > '9')
                    {
                        nEnd = i;
                        break;
                    }
                }
                if (data.Length - nEnd > shift)
                {
                    result = 0;
                    return false;
                }
                if (nStart < nEnd)
                {
                    result = Convert.ToInt16(data.Substring(nStart, nEnd - nStart));
                    return true;
                }
                else
                {
                    result = 0;
                    return false;
                }
            }
            catch
            {
                result = 0;
                return false;
            }
        }

        public static string GetIPAddress()
        {
            try
            {
                if (IPADDRESS_INTERNER.Length > 0)
                    return IPADDRESS_INTERNER;

                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        IPADDRESS_INTERNER = ip.ToString().Trim();
                        if (IPADDRESS_INTERNER.Length > 0)
                            break;
                    }
                }
                return IPADDRESS_INTERNER;
            }
            catch { return "?"; }
        }


        /// <summary>
        /// Tính toán đánh giá từ khóa trong chuỗi
        /// </summary>
        public static float KeySearchCalcRate(int indexID, int lengthCount)
        {
            if (lengthCount < 2)
                return 100f;
            // Đếm số đơn vị
            int unitCount = 0;
            for (int i = 0; i < lengthCount; i++)
                unitCount += (i + 1);
            return (lengthCount - indexID) * 100f / unitCount;
        }


        public static Encoding GetEncoding(string filename)
        {
            try
            {
                // Read the BOM
                byte[] bom = new byte[4];
                using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    file.Read(bom, 0, 4);
                }
                // Analyze the BOM
                if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76)
                    return Encoding.UTF7;
                else if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf)
                    return Encoding.UTF8;
                else if (bom[0] == 0xff && bom[1] == 0xfe)
                    return Encoding.Unicode; //UTF-16LE
                else if (bom[0] == 0xfe && bom[1] == 0xff)
                    return Encoding.BigEndianUnicode; //UTF-16BE
                else if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff)
                    return Encoding.UTF32;
                else
                    return Encoding.ASCII;
            }
            catch { return Encoding.ASCII; }
        }
    }
}
