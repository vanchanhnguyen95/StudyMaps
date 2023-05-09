using System;
using System.IO;

namespace BAGeocoding.Utility
{
    /// <summary>
    /// Summary description for LogFile
    /// </summary>
    public class LogFile
    {
        /// <summary>
        /// Ghi lỗi
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteError(string msg)
        {
            try
            {
                StreamWriter f = new StreamWriter(Constants.DEFAULT_DIRECTORY_LOGS + "Logs.txt", true);
                f.WriteLine(DateTime.Now.ToString() + ": " + msg);
                f.Close();
            }
            catch { }
        }

        public static async void WriteErrorV2(string msg)
        {
            try
            {
                StreamWriter f = new StreamWriter(Constants.DEFAULT_DIRECTORY_LOGS + "Logs.txt", true);
                await f.WriteLineAsync(DateTime.Now.ToString() + ": " + msg);
                f.Close();
            }
            catch { }
        }

        /// <summary>
        /// Ghi dữ liệu lỗi
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteData(string msg)
        {
            try
            {
                StreamWriter f = new StreamWriter(Constants.DEFAULT_DIRECTORY_LOGS + "Data.txt", true);
                f.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ": " + msg);
                f.Close();
            }
            catch { }
        }

        public static async void WriteDataV2(string msg)
        {
            try
            {
                StreamWriter f = new StreamWriter(Constants.DEFAULT_DIRECTORY_LOGS + "Data.txt", true);
                await f.WriteLineAsync(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ": " + msg);
                f.Close();
            }
            catch { }
        }

        /// <summary>
        /// Ghi dữ liệu lỗi
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteRequest(string msg)
        {
            try
            {
                StreamWriter f = new StreamWriter(Constants.DEFAULT_DIRECTORY_LOGS + "Request.txt", true);
                f.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ": " + msg);
                f.Close();
            }
            catch { }
        }

        public static async void WriteRequestV2(string msg)
        {
            try
            {
                StreamWriter f = new StreamWriter(Constants.DEFAULT_DIRECTORY_LOGS + "Request.txt", true);
                await f.WriteLineAsync(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ": " + msg);
                f.Close();
            }
            catch { }
        }

        /// <summary>
        /// Ghi dữ liệu lỗi
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteResponse(string msg)
        {
            try
            {
                StreamWriter f = new StreamWriter(Constants.DEFAULT_DIRECTORY_LOGS + "Response.txt", true);
                f.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ": " + msg);
                f.Close();
            }
            catch { }
        }

        /// <summary>
        /// Ghi dữ liệu lỗi
        /// </summary>
        /// <param name="msg"></param>
        public static async void WriteResponseV2(string msg)
        {
            try
            {
                StreamWriter f = new StreamWriter(Constants.DEFAULT_DIRECTORY_LOGS + "Response.txt", true);
                await f.WriteLineAsync(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ": " + msg);
                f.Close();
            }
            catch { }
        }

        public static void WriteProcess(string msg)
        {
            try
            {
                StreamWriter f = new StreamWriter(Constants.DEFAULT_DIRECTORY_LOGS + "Process.txt", true);
                f.WriteLine(msg);
                f.Close();
            }
            catch { }
        }

        public static async void WriteProcessV2(string msg)
        {
            try
            {
                StreamWriter f = new StreamWriter(Constants.DEFAULT_DIRECTORY_LOGS + "Process.txt", true);
                await f.WriteLineAsync(msg);
                f.Close();
            }
            catch { }
        }

        public static void WriteCommitData(string msg)
        {
            try
            {
                StreamWriter f = new StreamWriter(string.Format(Constants.DEFAULT_DIRECTORY_LOGS + "CommitData-{0:yyyyMMdd}.txt", DateTime.Now), true);
                f.WriteLine(string.Format("-------------------------------------------------------------------- {0:dd/MM/yyyy HH:mm:ss} --------------------------------------------------------------------", DateTime.Now));
                f.WriteLine(msg);
                f.Close();
            }
            catch { }
        }

        public static async void WriteCommitDataV2(string msg)
        {
            try
            {
                StreamWriter f = new StreamWriter(string.Format(Constants.DEFAULT_DIRECTORY_LOGS + "CommitData-{0:yyyyMMdd}.txt", DateTime.Now), true);
                await f.WriteLineAsync(string.Format("-------------------------------------------------------------------- {0:dd/MM/yyyy HH:mm:ss} --------------------------------------------------------------------", DateTime.Now));
                await f.WriteLineAsync(msg);
                f.Close();
            }
            catch { }
        }

        public static void WriteUploadImage(string msg)
        {
            try
            {
                StreamWriter f = new StreamWriter(string.Format(Constants.DEFAULT_DIRECTORY_LOGS + "UploadImage-{0:yyyyMMdd}.txt", DateTime.Now), true);
                f.WriteLine(string.Format("-------------------------------------------------------------------- {0:dd/MM/yyyy HH:mm:ss} --------------------------------------------------------------------", DateTime.Now));
                f.WriteLine(msg);
                f.Close();
            }
            catch { }
        }

        public static async void WriteUploadImageV2(string msg)
        {
            try
            {
                StreamWriter f = new StreamWriter(string.Format(Constants.DEFAULT_DIRECTORY_LOGS + "UploadImage-{0:yyyyMMdd}.txt", DateTime.Now), true);
                await f.WriteLineAsync(string.Format("-------------------------------------------------------------------- {0:dd/MM/yyyy HH:mm:ss} --------------------------------------------------------------------", DateTime.Now));
                await f.WriteLineAsync(msg);
                f.Close();
            }
            catch { }
        }

        public static void WriteTrackMove(string msg)
        {
            try
            {
                StreamWriter f = new StreamWriter(string.Format(Constants.DEFAULT_DIRECTORY_LOGS + "TrackMove-{0:yyyyMMdd}.txt", DateTime.Now), true);
                f.WriteLine(string.Format("-------------------------------------------------------------------- {0:dd/MM/yyyy HH:mm:ss} --------------------------------------------------------------------", DateTime.Now));
                f.WriteLine(msg);
                f.Close();
            }
            catch { }
        }

        public static async void WriteTrackMoveV2(string msg)
        {
            try
            {
                StreamWriter f = new StreamWriter(string.Format(Constants.DEFAULT_DIRECTORY_LOGS + "TrackMove-{0:yyyyMMdd}.txt", DateTime.Now), true);
                await f.WriteLineAsync(string.Format("-------------------------------------------------------------------- {0:dd/MM/yyyy HH:mm:ss} --------------------------------------------------------------------", DateTime.Now));
                await f.WriteLineAsync(msg);
                f.Close();
            }
            catch { }
        }

        public static void WriteStopwatch(string msg)
        {
            try
            {
                StreamWriter f = new StreamWriter(Constants.DEFAULT_DIRECTORY_LOGS + "Stopwatch.txt", true);
                f.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ": " + msg);
                f.Close();
            }
            catch { }
        }

        public static void WriteRestFul(string msg)
        {
            try
            {
                StreamWriter f = new StreamWriter(Constants.DEFAULT_DIRECTORY_LOGS + "RestFulApi.txt", true);
                f.WriteLine(msg);
                f.Close();
            }
            catch { }
        }

        /// <summary>
        /// Ghi dữ liệu truy vấn google
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteGG(string msg)
        {
            try
            {
                StreamWriter f = new StreamWriter(Constants.DEFAULT_DIRECTORY_LOGS + "GoogleGeocoder.txt", true);
                f.WriteLine(string.Format("{0:dd/MM/yyyy HH:mm:ss:fff}: {1}", DateTime.Now, msg));
                f.Close();
            }
            catch { }
        }

        public static void WriteDataLogs(string msg)
        {
            try
            {
                StreamWriter f = new StreamWriter(Constants.DEFAULT_DIRECTORY_LOGS + "DataLogs.txt", true);
                f.WriteLine(msg);
                f.Close();
            }
            catch { }
        }


        public static bool ProcessState(string msg, bool sts = false)
        {
            try
            {
                StreamWriter f = new StreamWriter(Constants.DEFAULT_DIRECTORY_LOGS + "ProcessState.txt", true);
                f.WriteLine(DateTime.Now.ToString() + ": " + msg);
                f.Close();
                return sts;
            }
            catch { return false; }
        }

        public static async Task<bool> ProcessStateV2(string msg, bool sts = false)
        {
            try
            {
                StreamWriter f = new StreamWriter(Constants.DEFAULT_DIRECTORY_LOGS + "ProcessState.txt", true);
                await f.WriteLineAsync(DateTime.Now.ToString() + ": " + msg);
                f.Close();
                return sts;
            }
            catch { return false; }
        }

        public static void WriteFile(string folder, string file, string pre, string data, bool time = false)
        {
            try
            {
                // 1. Xử lý thư mục
                if (folder.Length > 0)
                    folder = string.Format("{0}{1}\\", Constants.DEFAULT_DIRECTORY_LOGS, folder);
                else
                    folder = Constants.DEFAULT_DIRECTORY_LOGS;
                if (Directory.Exists(folder) == false)
                    Directory.CreateDirectory(folder);

                // 2. Xử lý tên file
                if (file.IndexOf(".txt") < 0)
                    file = string.Format("{0}{1}.txt", folder, file);
                else
                    file = string.Format("{0}{1}", folder, file);

                // 3. Xử lý dữ liệu
                if (pre != null && pre.Length > 0)
                    data = string.Format("{0} -> {1}", pre, data);
                if (time == true)
                    data = string.Format("{0:yyyy/MM/dd HH:mm:ss.fff}: {1}", DataUlt.GetDateTimeFull(), data);

                // 4. Tiến hành ghi
                StreamWriter f = new StreamWriter(file, true);
                f.WriteLine(data);
                f.Close();
            }
            catch { }
        }

        public static async void WriteFileV2(string folder, string file, string pre, string data, bool time = false)
        {
            try
            {
                // 1. Xử lý thư mục
                if (folder.Length > 0)
                    folder = string.Format("{0}{1}\\", Constants.DEFAULT_DIRECTORY_LOGS, folder);
                else
                    folder = Constants.DEFAULT_DIRECTORY_LOGS;
                if (Directory.Exists(folder) == false)
                    Directory.CreateDirectory(folder);

                // 2. Xử lý tên file
                if (file.IndexOf(".txt") < 0)
                    file = string.Format("{0}{1}.txt", folder, file);
                else
                    file = string.Format("{0}{1}", folder, file);

                // 3. Xử lý dữ liệu
                if (pre != null && pre.Length > 0)
                    data = string.Format("{0} -> {1}", pre, data);
                if (time == true)
                    data = string.Format("{0:yyyy/MM/dd HH:mm:ss.fff}: {1}", DataUlt.GetDateTimeFull(), data);

                // 4. Tiến hành ghi
                StreamWriter f = new StreamWriter(file, true);
                await f.WriteLineAsync(data);
                f.Close();
            }
            catch { }
        }

        public static void WriteNoDataGeobyAddress(string msg)
        {
            try
            {
                StreamWriter f = new StreamWriter(Constants.DEFAULT_DIRECTORY_LOGS + "NoDataGeobyaddress.txt", true);
                f.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ": " + msg);
                f.Close();
            }
            catch { }
        }

        public static void WriteNoDataAddressByGeo(string msg)
        {
            try
            {
                StreamWriter f = new StreamWriter(Constants.DEFAULT_DIRECTORY_LOGS + "NoDataAddressByGeo.txt", true);
                f.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ": " + msg);
                f.Close();
            }
            catch { }
        }
    }
}
