using BAGeocoding.Api.Models;
using BAGeocoding.Bll;
using BAGeocoding.Entity.MapObj;
using BAGeocoding.Utility;
using System.Collections;

namespace BAGeocoding.Api.Services
{
    public interface IRegionManagerService
    {
        // AddBAGDistrict
        Task<bool> AddBAGDistrict();
    }


    public class RegionManagerService
    {
        private static Hashtable HTRoadByLevel = new Hashtable();

        private BAGDistrictAddService _baGDistrict;
        public RegionManagerService(BAGDistrictAddService baGDistrict)
        {
            _baGDistrict = baGDistrict;
        }

        // Tải dữ liệu vùng
        //public async Task<bool> LoadData()
        public async Task<bool> AddBAGDistrict()
        {
            try
            {
                // Tạo file tạm
                string filePath = string.Empty;
                if (MainProcessing.FileCopy(Constants.DEFAULT_DIRECTORY_DATA, Constants.DEFAULT_REGION_FILE_NAME, ref filePath) == false)
                    return false;

                // Tiến hành đọc dữ liệu
                using (FileStream stream = new FileStream(filePath, FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        int nFiles = (int)reader.ReadByte();
                        for (int i = 0; i < nFiles; i++)
                        {
                            int nameLength = 0;
                            int objectCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                            if (i == 1)
                            {
                                #region ==================== Đọc dữ liệu quận/huyện ====================
                                for (int j = 0; j < objectCount; j++)
                                {
                                    //Read District
                                    BAGDistrict district = new BAGDistrict();
                                    //Read 2 bytes (ID)
                                    district.DistrictID = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                                    //Read 1 byte (Length of VName)
                                    nameLength = (int)reader.ReadByte();
                                    //Read VName
                                    district.VName = Constants.UnicodeCodePage.GetString(reader.ReadBytes(nameLength));
                                    district.EName = LatinToAscii.Latin2Ascii(district.VName);
                                    //Read 2 bytes (ProvinceID)
                                    district.ProvinceID = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                                    //Read 2 bytes (Length of list Points)
                                    int length = (int)BitConverter.ToInt16(reader.ReadBytes(2), 0);
                                    district.PointList = new List<BAGPoint>();
                                    //Read list Points
                                    for (int k = 0; k < length; k++)
                                        district.PointList.Add(new BAGPoint(BitConverter.ToDouble(reader.ReadBytes(8), 0), BitConverter.ToDouble(reader.ReadBytes(8), 0)));
                                    //Add object
                                    RunningParams.DistrictData.Objs.Add(district.DistrictID, district);
                                    DistrictData districtData = new DistrictData();
                                    districtData.Objs = RunningParams.DistrictData.Objs;

                                    BAGDistrictAdd baGDistrictAdd = new BAGDistrictAdd(district);
                                    await _baGDistrict.CreateAsync(baGDistrictAdd);

                                    //await _districtDataService.CreateAsync(districtData);
                                }
                                #endregion
                            }
                        }
                        reader.Close();
                    }
                    stream.Close();
                }

                // Hủy file tạm
                File.Delete(filePath);

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RegionManager.LoadData, ex: " + ex.ToString());
                return false;
            }
        }

        private static bool CheckMore(short provinceID)
        {
            try
            {
                if (RunningParams.ProvinceRoadByLevel == null)
                    return false;
                else if (RunningParams.ProvinceRoadByLevel.Length == 0)
                    return false;
                else if (HTRoadByLevel.Count == 0)
                {
                    LogFile.WriteProcess("ProvinceRoadByLevel: " + RunningParams.ProvinceRoadByLevel);
                    string[] temp = RunningParams.ProvinceRoadByLevel.Split(',');
                    for (int i = 0; i < temp.Length; i++)
                        HTRoadByLevel.Add(Convert.ToInt16(temp[i]), null);
                }
                return HTRoadByLevel.ContainsKey(provinceID);
            }
            catch (Exception ex)
            {
                LogFile.WriteError("RegionManager.CheckMore, ex: " + ex.ToString());
                return false;
            }
        }

    }
}
