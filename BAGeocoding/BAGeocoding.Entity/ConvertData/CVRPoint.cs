using System;
using System.Data;

using System.Text.RegularExpressions;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.ConvertData
{
    public class CVRPoint : SQLDataUlt
    {
        public int ObjectID { get; set; }
        public int XNCode { get; set; }
        public string Plate { get; set; }
        public string ImageSrc { get; set; }
        public string Name { get; set; }
        public byte TypeID { get; set; }
        public DateTime TS { get; set; }
        public BAGPoint Point { get; set; }
        public byte Speed { get; set; }
        public byte State { get; set; }
        public string Info { get; set; }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("CVRPoint.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromString(EnumCVRText2MifKind kindID, string input)
        {
            try
            {
                string[] data;
                if (kindID == EnumCVRText2MifKind.CaptureData)
                    data = input.Split('@');
                else if (kindID == EnumCVRText2MifKind.MapToolPOI)
                    data = Regex.Split(input, "#@#");
                else if (kindID == EnumCVRText2MifKind.LogFileTaxi)
                    data = input.Split(',');
                else
                    return false;

                int indexID = 0;
                if (kindID == EnumCVRText2MifKind.CaptureData)
                {
                    Name = data[indexID++];
                    Point = new BAGPoint(data[indexID++], data[indexID++]);
                    Info = data[++indexID];
                }
                else if (kindID == EnumCVRText2MifKind.MapToolPOI)
                {
                    ImageSrc = data[indexID++];
                    TypeID = Convert.ToByte(data[indexID++]);
                    Name = data[indexID++];
                    Point = new BAGPoint(data[indexID++], data[indexID++], true);
                    if (data.Length > indexID)
                        Info = data[indexID++];
                    else
                        Info = string.Empty;
                }
                else if (kindID == EnumCVRText2MifKind.LogFileTaxi)
                {
                    //XNCode = Convert.ToInt32(data[indexID++]);
                    Plate = data[indexID++].Trim();
                    indexID++;//TS = DateTime.ParseExact(data[indexID++], "yyyyMMddHHmmss", new CultureInfo("en-US"));
                    Point = new BAGPoint(data[indexID++], data[indexID++]);
                    Speed = Convert.ToByte(data[indexID++]);
                    //State = Convert.ToByte(data[indexID++]);
                }
                else
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("CVRPoint.FromString, ex: {0}", ex.ToString()));
                return false;
            }
        }
    }
}