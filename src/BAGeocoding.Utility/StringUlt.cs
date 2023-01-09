using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Json;

namespace BAGeocoding.Utility
{
    /// <summary>
    /// Summary description for Utility
    /// </summary>
    public class StringUlt
    {
        private static object LockObj = new object();

        public static string ValidateSearch(string key)
        {
            key = key.Replace("%", string.Empty);
            key = key.Replace("_", "[_]");
            return key;
        }

        /// <summary>
        /// Check Email
        /// </summary>
        public static bool ValidateEmail(string emailaddress)
        {
            try
            {
                string match = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                Regex reg = new Regex(match);
                if (reg.IsMatch(emailaddress.Trim())) 
                    return true;
                else 
                    return false;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra validate IPAddress
        /// </summary>
        public static bool ValidateIPAddress(string address)
        {
            if (!Regex.IsMatch(address, @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b"))
                return false;

            IPAddress dummy;
            return IPAddress.TryParse(address, out dummy);
        }

        public static string ValidateNumber(string data)
        {
            Regex rgx = new Regex("\\s+");
            data = rgx.Replace(data, "");
            data = data.Replace(".", "");
            data = data.Replace(",", "");
            return data;
        }

        public static bool ValidateSIMIMEI(string data)
        {
            Regex reg1 = new Regex(@"^([1-9][0-9]{15})$");
            Regex reg2 = new Regex(@"^([1-9][0-9]{18})$");
            data = ValidateNumber(data);
            return reg1.IsMatch(data) || reg2.IsMatch(data);
        }

        /// <summary>
        /// Nối các chuỗi
        /// </summary>
        public static string Join(string separator, params string[] strings)
        {
            try
            {
                if ((String.CompareOrdinal(separator, String.Empty) == 0) && (strings.Length == 2))
                    return (string)strings[0] + (string)strings[1];
                else
                    return String.Join(separator, strings);
            }
            catch (Exception ex)
            {
                LogFile.WriteError("StringUlt.Join, ex: " + ex.ToString());
                return "";
            }
        }
        
        /// <summary>
        /// Khởi tạo chuỗi bất kỳ
        /// </summary>
        public static string GeneratePassword(int length)
        {
            try
            {
                string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@#$%&";
                StringBuilder res = new StringBuilder();
                Random rnd = new Random();
                while (0 < length)
                {
                    res.Append(valid[rnd.Next(valid.Length)]);
                    length--;
                }
                return res.ToString();
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// Mã hóa mật khẩu
        /// </summary>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static string EncryptPassword(string Password)
        {
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] hashBytes = encoding.GetBytes(Password);

            //Compute the SHA-1 hash
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                byte[] cryptPassword = sha1.ComputeHash(hashBytes);
                return BitConverter.ToString(cryptPassword);
            }
        }

        /// <summary>
        /// Hiệu chỉnh lại số điện thoại
        /// </summary>
        public static string AdjustSIMNumber(string simnumber)
        {
            try
            {
                if (simnumber == null || simnumber.Length == 0)
                    return simnumber;
                Regex digitsOnly = new Regex(@"[^\d]");
                simnumber = digitsOnly.Replace(simnumber, "");

                if (simnumber.Length == 0)
                    return simnumber;
                else if (simnumber[0] != '0')
                    return '0' + simnumber;
                else
                    return simnumber;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("StringUlt.AdjustSIMNumber(" + simnumber + "), ex: " + ex.ToString());
                return simnumber;
            }
        }

        public static List<byte> GetBytes(string data, int leng)
        {
            if (data == null)
                data = string.Empty;

            List<byte> bffResult = new List<byte>();
            bffResult.AddRange(Constants.UTF8CodePage.GetBytes(data));
            if (leng == 1)
            {
                if (bffResult.Count > byte.MaxValue)
                    bffResult.RemoveRange(byte.MaxValue, bffResult.Count - byte.MaxValue);
                bffResult.Insert(0, (byte)bffResult.Count);
            }
            else if (leng == 2)
            {
                if (bffResult.Count > short.MaxValue)
                    bffResult.RemoveRange(short.MaxValue, bffResult.Count - short.MaxValue);
                bffResult.InsertRange(0, BitConverter.GetBytes((short)bffResult.Count));
            }
            else if (leng == 4)
                bffResult.InsertRange(0, BitConverter.GetBytes(bffResult.Count));
            return bffResult;
        }

        public static string FromBytes(byte[] bff, int typ, ref int idx)
        {
            int len = 0;
            if (CheckFromBytes(bff, typ, ref idx) == false)
                return string.Empty;

            if (typ == 1)
                len = bff[idx];
            else if (typ == 2)
                len = BitConverter.ToInt16(bff, idx);
            else if (typ == 4)
                len = BitConverter.ToInt32(bff, idx);
            idx += typ;

            if (CheckFromBytes(bff, len, ref idx) == false)
                return string.Empty;
            idx += len;
            return Constants.UTF8CodePage.GetString(bff, idx - len, len);
        }

        private static bool CheckFromBytes(byte[] bff, int len, ref int idx)
        {
            if (bff.Length < idx + len)
                idx = -1;
            return idx > -1;
        }

        public static List<string> SplitKeySearch(string input)
        {
            List<string> resultList = new List<string>();
            List<string> segmentKey = input.Replace(".", " ").ToLower().Split(' ').ToList();
            for (int i = 0; i < segmentKey.Count; i++)
            {
                segmentKey[i] = segmentKey[i].Trim();
                if (segmentKey[i].Length == 0)
                    continue;
                else if (segmentKey[i].Equals("tp") == true)
                    continue;
                else if (segmentKey[i].Equals("tx") == true)
                    continue;
                else if (segmentKey[i].Equals("q") == true)
                    continue;
                else if (segmentKey[i].Equals("h") == true)
                    continue;
                else if (segmentKey[i].Equals("tt") == true)
                    continue;
                else if (segmentKey[i].Equals("p") == true)
                    continue;
                else if (segmentKey[i].Equals("x") == true)
                    continue;
                else if (resultList.Exists(item => item == segmentKey[i]) == false)
                    resultList.Add(segmentKey[i]);
            }
            return resultList;
        }
        
        /// <summary>
        /// Phân tích cắt chuỗi
        /// </summary>
        public static List<string> StringAnalyze(string dataInput, char splitStr)
        {
            try
            {
                int start = 0;
                int indexID = 0;
                bool multiFlag = false;
                List<string> resultList = new List<string>();
                while (indexID < dataInput.Length)
                {
                    if (dataInput[indexID] == '"')
                        multiFlag = !multiFlag;
                    if (dataInput[indexID] == splitStr && multiFlag == false)
                    {
                        if (start == indexID)
                            resultList.Add(string.Empty);
                        else
                            resultList.Add(dataInput.Substring(start, indexID - start).Replace("\"", string.Empty));
                        start = indexID + 1;
                    }
                    indexID += 1;
                }
                if (indexID > start)
                    resultList.Add(dataInput.Substring(start, indexID - start).Replace("\"", string.Empty));
                else if (indexID == start)
                    resultList.Add(string.Empty);
                return resultList;
            }
            catch { return null; }
        }

        /// <summary>
        /// Có lỗi
        /// </summary>
        public static bool IsError(string strError, ref string error)
        {
            error = strError;
            return false;
        }
        

        public static bool IsPhoneMobile(string phoneNumber, bool exists = true, bool multi = true)
        {
            phoneNumber = phoneNumber.Trim();
            if (exists == false && phoneNumber.Length == 0)
                return true;
            Regex regMobil01 = new Regex(@"^([0][1][0-9]{9})$");                 // Số điện thoại bình thường: Ví dụ 01684213641
            Regex regMobil08 = new Regex(@"^([0][8][6,8,9][0-9]{7})$");          // Số điện thoại bình thường: Ví dụ 086xxxxxxx, 088xxxxxxx, 089xxxxxxx
            Regex regMobil09 = new Regex(@"^([0][9][0-9]{8})$");                 // Số điện thoại bình thường: Ví dụ 0936014801
            string[] data = phoneNumber.Trim().Split(',');
            if (multi == false && data.Length > 1)
                return false;
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = ValidateNumber(data[i]);
                if (regMobil09.IsMatch(data[i].Trim()) == true)
                    continue;
                else if (regMobil01.IsMatch(data[i].Trim()) == true)
                    continue;
                else if (regMobil08.IsMatch(data[i].Trim()) == true)
                    continue;
                else
                    return false;
            }
            return true;
        }


        /// <summary>
        /// Chuyển EnumItemAttribute sang string
        /// </summary>
        public static string ConvertEnumToString(System.Enum value)
        {
            EnumItemAttribute data = value.GetType()
                .GetFields().ToList()
                .Select(fi => new { Fi = fi, Ta = ObjextExtender.GetAttribute<EnumItemAttribute>(fi) })
                .Where(t => t.Ta != null)
                .Select(t =>
                {
                    // Lấy FieldName của Enum
                    t.Ta.FieldName = ObjextExtender.GetAttribute<EnumItemAttribute>(t.Fi).FieldName;
                    // Giá trị của Enum
                    t.Ta.Value = t.Fi.GetRawConstantValue();
                    // return TAttribute
                    return t.Ta;
                }).ToList()
            .Find(item => item.FieldName.ToString() == value.ToString());
            return (data != null && string.IsNullOrEmpty(data.Name) == false) ? data.Name : string.Empty;
        }

        /// <summary>
        /// Lấy danh sách enum
        /// </summary>
        public static List<EnumItemAttribute> GetListEnumAttribute(System.Enum value)
        {
            List<EnumItemAttribute> data = value.GetType()
                        .GetFields().ToList()
                        .Select(fi => new { Fi = fi, Ta = fi.GetAttribute<EnumItemAttribute>() })
                        .Where(t => t.Ta != null)
                        .Select(t =>
                        {
                            // Lấy FieldName của Enum
                            t.Ta.FieldName = t.Fi.GetAttribute<EnumItemAttribute>().FieldName;
                            // Giá trị của Enum
                            t.Ta.Value = t.Fi.GetRawConstantValue();
                            // return TAttribute
                            return t.Ta;
                        }).ToList();
            return data;
        }

        /// <summary>
        /// Đọc file JavaScript
        /// </summary>
        public static string ReadFileJs(string folder, string fileName)
        {
            try
            {
                StreamReader reader = new StreamReader(folder + fileName, Encoding.ASCII);
                string str = reader.ReadToEnd();
                reader.Close();
                return str;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("StringUlt.ReadFileJs(" + folder + ", " + fileName + "), ex: " + ex.ToString());
                return "";
            }
        }

        /// <summary>
        /// Kiểm tra url request
        /// </summary>
        public static bool CheckURL(string url)
        {
            try
            {
                //LogFile.WriteData(string.Format("Root: '{0}'", Constants.WEBSITE_ROOT_URL));
                //LogFile.WriteData(string.Format("Url: '{0}'", url));
                url = url.ToLower();
                if (url.IndexOf(Constants.WEBSITE_ROOT_URL) == 0)
                    return true;
                else
                    return false;
            }
            catch { return false; }
        }

        public static string Stream2String(Stream data)
        {
            try
            {
                lock (LockObj)
                {
                    long fileLength = 0;
                    byte[] readBuffer = new byte[4096];
                    int bytesRead;
                    List<byte> ListData = new List<byte>();

                    //Chuyển toàn bộ ảnh vào buffer
                    while ((bytesRead = data.Read(readBuffer, 0, 4096)) > 0)
                    {
                        fileLength += bytesRead;
                        //if (fileLength >= Constant.MAXDATAFROMCLIENT)
                        //    break;
                        for (int i = 0; i < bytesRead; i++)
                            ListData.Add(readBuffer[i]);
                    }
                    //Chuyển đổi dữ liệu về string
                    return Encoding.UTF8.GetString(ListData.ToArray(), 0, ListData.Count);
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("Stream2String, ex: {0}", ex.ToString()));
                return string.Empty;
            }
        }

        /// <summary>
        /// Ghi dữ liệu đối tượng ra Json
        /// </summary>
        public static string Object2Json<T>(T a)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                ser.WriteObject(ms, a);
                byte[] json = ms.ToArray();
                ms.Close();
                return Encoding.UTF8.GetString(json, 0, json.Length);
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("Object2Json, ex: {0}", ex.ToString()));
                return string.Empty;
            }
        }

        /// <summary>
        /// Biến dữ liệu Json ra đối tượng
        /// </summary>
        public static T Json2Object<T>(string d)
        {
            try
            {
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(d));
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                return (T)ser.ReadObject(ms);
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("Json2Object, ex: {0}", ex.ToString()));
                return default(T);
            }
        }
    }
}