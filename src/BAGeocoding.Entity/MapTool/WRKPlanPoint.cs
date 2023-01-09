using System;
using System.Collections.Generic;
using System.Data;

using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;
using BAGeocoding.Entity.Enum.MapTool;

namespace BAGeocoding.Entity.MapTool
{
    public class WRKPlanPoint : WRKPlanObjBase
    {
        public long PLNPoiID { get; set; }
        public short KindID { get; set; }
        public BAGPoint Coords { get; set; }
        public string ImageSrc { get; set; }
        public string SyncTokenID { get; set; }

        public byte State { get; set; }
        public byte PrcApv { get; set; }
        public EnumMTLApvDataState EnumPrcApv { get { return (EnumMTLApvDataState)PrcApv; } set { PrcApv = (byte)value; } }

        public byte[] ImageBff { get; set; }

        public WRKPlanPoint() : base()
        {
            ImageSrc = string.Empty;
            SyncTokenID = string.Empty;
        }

        public bool FromDataNote(DataRow dr)
        {
            try
            {
                PLNPoiID = base.GetDataValue<long>(dr, "PLNObjID");
                if (base.FromDataNote(dr) == false)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanPoint.FromDataNote, ex: {0}", ex.ToString()));
                return false;
            }
        }
                
        public bool FromDataRow(DataRow dr)
        {
            try
            {
                PLNPoiID = base.GetDataValue<long>(dr, "PLNPoiID");
                if (base.FromDataRow(dr) == false)
                    return false;
                KindID = base.GetDataValue<short>(dr, "KindID");
                Coords = new BAGPoint
                {
                    Lng = base.GetDataValue<double>(dr, "Lng"),
                    Lat = base.GetDataValue<double>(dr, "Lat")
                };
                ImageSrc = base.GetDataValue<string>(dr, "ImageSrc");                
                SyncTokenID = base.GetDataValue<string>(dr, "SyncTokenID");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanPoint.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public byte[] Tobinary()
        {
            try
            {
                // Khởi tạo dữ liệu
                List<byte> resultList = new List<byte>();
                byte[] namBff = Constants.UTF8CodePage.GetBytes(Name);
                byte[] noteBff = Constants.UTF8CodePage.GetBytes(NoteOld);
                byte[] imageBff = Constants.UTF8CodePage.GetBytes(ImageSrc);

                // Thông tin cơ bản
                resultList.AddRange(BitConverter.GetBytes(PLNPoiID));       // Key của server
                resultList.AddRange(BitConverter.GetBytes(KindID));         // Loại POI
                resultList.AddRange(BitConverter.GetBytes((short)namBff.Length));
                if (namBff.Length > 0)
                    resultList.AddRange(namBff);

                // Grid
                resultList.AddRange(BitConverter.GetBytes(GridEdit));
                resultList.AddRange(BitConverter.GetBytes(Convert.ToInt32(GridView)));

                // Đường dẫn ảnh
                resultList.AddRange(BitConverter.GetBytes((short)imageBff.Length));
                if (imageBff.Length > 0)
                    resultList.AddRange(imageBff);

                // Ghi chú
                resultList.AddRange(BitConverter.GetBytes((short)noteBff.Length));
                if (noteBff.Length > 0)
                    resultList.AddRange(noteBff);

                // Thông tin thao tác
                resultList.AddRange(BitConverter.GetBytes(ActionID));
                resultList.Add(ApprovedState);

                // Tọa độ
                resultList.AddRange(BitConverter.GetBytes(Coords.Lat));
                resultList.AddRange(BitConverter.GetBytes(Coords.Lng));

                // Trả về kết quả
                return resultList.ToArray();
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanPoint.Tobinary, ex: {0}", ex.ToString()));
                return null;
            }
        }

        public byte[] TobinaryUploadImage()
        {
            try
            {
                // Khởi tạo dữ liệu
                List<byte> resultList = new List<byte>();
                byte[] syncTokenBff = Constants.UTF8CodePage.GetBytes(SyncTokenID);

                // Thông tin mã đồng bộ
                resultList.AddRange(BitConverter.GetBytes((short)syncTokenBff.Length));
                if (syncTokenBff.Length > 0)
                    resultList.AddRange(syncTokenBff);

                // Trạng thái đồng bộ
                resultList.Add(State);

                // Trả về kết quả
                return resultList.ToArray();
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanPoint.TobinaryUploadImage, ex: {0}", ex.ToString()));
                return null;
            }
        }

        public bool FromBinaryCommitData(byte[] bff, ref int idx)
        {
            try
            {
                // Mã điểm của hệ thống server
                PLNPoiID = BitConverter.ToInt64(bff, idx);
                idx += 8;
                // Mã thao tác
                ActionID = BitConverter.ToInt32(bff, idx);
                idx += 4;
                // Kiểm tra để lấy thông tin POI
                int textLength = 0;
                if (IsDelete() == false)
                {
                    // Loại điểm
                    KindID = BitConverter.ToInt16(bff, idx);
                    idx += 2;
                    // Tên điểm
                    textLength = BitConverter.ToInt16(bff, idx);
                    idx += 2;
                    Name = Constants.UTF8CodePage.GetString(bff, idx, textLength);
                    idx += textLength;
                    // Grid
                    GridEdit = BitConverter.ToInt32(bff, idx);
                    idx += 4;
                    GridView = BitConverter.ToInt32(bff, idx).ToString();
                    idx += 4;
                    // Ghi chú
                    textLength = BitConverter.ToInt16(bff, idx);
                    idx += 2;
                    NoteNew = Constants.UTF8CodePage.GetString(bff, idx, textLength);
                    idx += textLength;
                    // Tọa độ
                    Coords = new BAGPoint
                    {
                        Lat = BitConverter.ToDouble(bff, idx),
                        Lng = BitConverter.ToDouble(bff, idx + 8)
                    };
                    idx += 16;
                    // Mã đồng bộ
                    textLength = BitConverter.ToInt16(bff, idx);
                    idx += 2;
                    SyncTokenID = Constants.UTF8CodePage.GetString(bff, idx, textLength);
                    idx += textLength;
                }
                else
                {
                    // Ghi chú
                    textLength = BitConverter.ToInt16(bff, idx);
                    idx += 2;
                    NoteNew = Constants.UTF8CodePage.GetString(bff, idx, textLength);
                    idx += textLength;
                }
                // Thông tin thao tác
                EditorID = BitConverter.ToInt32(bff, idx);
                idx += 4;
                EditTime = BitConverter.ToInt32(bff, idx);
                idx += 4;

                // Trả về trạng thái
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanPoint.FromBinaryCommitData, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromBinaryUploadImage(byte[] bff, ref int idx)
        {
            try
            {
                // Mã đồng bộ
                int dataLength = BitConverter.ToInt16(bff, idx);
                idx += 2;
                SyncTokenID = Constants.UTF8CodePage.GetString(bff, idx, dataLength);
                idx += dataLength;
                // Dữ liệu ảnh
                dataLength = BitConverter.ToInt32(bff, idx);
                idx += 4;
                ImageBff = new byte[dataLength];
                Array.Copy(bff, idx, ImageBff, 0, ImageBff.Length);
                idx += dataLength;

                // Trả về trạng thái
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanPoint.FromBinaryUploadImage, ex: {0}", ex.ToString()));
                return false;
            }
        }
    }
}