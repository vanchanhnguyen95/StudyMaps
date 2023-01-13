using System;
using System.Collections.Generic;

using BAGeocoding.Entity.Enum.MapTool;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapTool.Plan
{
    public class WRKFolder : SQLDataUlt
    {
        public int UserID { get; set; }
        public string Password { get; set; }
        public string FolderPath { get; set; }

        public byte[] Tobinary()
        {
            try
            {
                // Khởi tạo dữ liệu
                List<byte> resultList = new List<byte>();
                byte[] passwordBff = Constants.UTF8CodePage.GetBytes(Password);
                byte[] folderPathBff = Constants.UTF8CodePage.GetBytes(FolderPath);
                
                // Thông tin tài khoản
                resultList.AddRange(BitConverter.GetBytes(UserID));         // Loại POI
                resultList.AddRange(BitConverter.GetBytes((short)passwordBff.Length));
                if (passwordBff.Length > 0)
                    resultList.AddRange(passwordBff);

                // Đường dẫn thư mục
                resultList.AddRange(BitConverter.GetBytes((short)folderPathBff.Length));
                if (folderPathBff.Length > 0)
                    resultList.AddRange(folderPathBff);
                
                // Trả về kết quả
                return resultList.ToArray();
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKFolder.Tobinary, ex: {0}", ex.ToString()));
                return null;
            }
        }

        public bool FromBinary(byte[] bff)
        {
            try
            {
                int dataIndex = 0;
                int textLength = 0;

                // 1. Lấy tài khoản
                // 1.1 Mã tài khoản
                UserID = BitConverter.ToInt32(bff, dataIndex);
                dataIndex += 4;
                // 1.2 Lấy mật khẩu
                textLength = BitConverter.ToInt16(bff, dataIndex);
                dataIndex += 2;
                Password = Constants.UTF8CodePage.GetString(bff, dataIndex, textLength);
                dataIndex += textLength;
                
                // 2. Lấy đường dẫn thư mục
                textLength = BitConverter.ToInt16(bff, dataIndex);
                dataIndex += 2;
                FolderPath = Constants.UTF8CodePage.GetString(bff, dataIndex, textLength);
                dataIndex += textLength;

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKFolder.FromBinary, ex: {0}", ex.ToString()));
                return false;
            }
        }
    }
}