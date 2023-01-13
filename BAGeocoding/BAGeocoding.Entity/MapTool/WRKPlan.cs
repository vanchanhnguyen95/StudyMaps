using System;
using System.Collections.Generic;
using System.Data;

using BAGeocoding.Entity.Enum.MapTool;

using BAGeocoding.Utility;
using BAGeocoding.Entity.MapObj;

namespace BAGeocoding.Entity.MapTool
{
    public class WRKPlan : SQLDataUlt
    {
        public int PlanID { get; set; }
        public short ProvinceID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StartTime { get; set; }
        public DateTime StartTimeGMT { get { return DataUtl.GetTimeUnix(StartTime); } set { StartTime = DataUtl.GetUnixTime(value); } }
        public int EndTime { get; set; }
        public DateTime? EndTimeGMT { get { if (EndTime > 0) return DataUtl.GetTimeUnix(EndTime); else return null; } set { if (value != null) EndTime = DataUtl.GetUnixTime(value.Value); else EndTime = 0; } }
        public byte BehaviorID { get; set; }
        public EnumMTLPlanBehavior EnumBehaviorID { get { return (EnumMTLPlanBehavior)BehaviorID; } set { BehaviorID = (byte)value; } }

        public int DataExt { get; set; }
        public byte State { get; set; }

        public string RootURL { get; set; }
        public string ImagePath { get; set; }
        public string LogsPath { get; set; }

        public BAGPoint Center { get; set; }

        public int CreatorID { get; set; }
        public int CreateTime { get; set; }
        public DateTime CreateTimeGMT { get { return DataUtl.GetTimeUnix(CreateTime); } }

        public int EditorID { get; set; }
        public int EditTime { get; set; }
        public DateTime EditTimeGMT { get { return DataUtl.GetTimeUnix(EditTime); } }

        public string PasswordUser { get; set; }
        public string ProvinceName { get; set; }

        public bool FromDataSimple(DataRow dr, string pr = "")
        {
            try
            {
                PlanID = base.GetDataValue<int>(dr, pr + "PlanID");
                Name = base.GetDataValue<string>(dr, pr + "Name");
                BehaviorID = base.GetDataValue<byte>(dr, pr + "BehaviorID");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlan.FromDataSimple, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataLogin(DataRow dr)
        {
            try
            {
                PlanID = base.GetDataValue<int>(dr, "PlanID");
                Name = base.GetDataValue<string>(dr, "Name", string.Empty);
                RootURL = base.GetDataValue<string>(dr, "RootURL", string.Empty);
                BehaviorID = base.GetDataValue<byte>(dr, "BehaviorID");
                
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlan.FromDataLogin, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                PlanID = base.GetDataValue<int>(dr, "PlanID");
                ProvinceID = base.GetDataValue<short>(dr, "ProvinceID");
                Name = base.GetDataValue<string>(dr, "Name", string.Empty);
                StartTime = base.GetDataValue<int>(dr, "StartTime");
                EndTime = base.GetDataValue<int>(dr, "EndTime", 0);
                RootURL = base.GetDataValue<string>(dr, "RootURL", string.Empty);
                ImagePath = base.GetDataValue<string>(dr, "ImagePath", string.Empty);
                LogsPath = base.GetDataValue<string>(dr, "LogsPath", string.Empty);
                Description = base.GetDataValue<string>(dr, "Description", string.Empty);
                BehaviorID = base.GetDataValue<byte>(dr, "BehaviorID");

                DataExt = base.GetDataValue<int>(dr, "DataExt");
                State = base.GetDataValue<byte>(dr, "State");

                Center = new BAGPoint
                {
                    Lng = base.GetDataValue<double>(dr, "CLng", 0),
                    Lat = base.GetDataValue<double>(dr, "CLat", 0)
                };

                CreatorID = base.GetDataValue<int>(dr, "CreatorID");
                CreateTime = base.GetDataValue<int>(dr, "CreateTime");
                EditorID = base.GetDataValue<int>(dr, "EditorID");
                EditTime = base.GetDataValue<int>(dr, "EditTime");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlan.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool Equal(WRKPlan other)
        {
            if (ProvinceID != other.ProvinceID)
                return false;
            else if (Name != other.Name)
                return false;
            else if (StartTime != other.StartTime)
                return false;
            else if (EndTime != other.EndTime)
                return false;
            else if (Description != other.Description)
                return false;
            else
                return true;
        }

        public byte[] ToBinaryLogin()
        {
            try
            {
                // 1. Lấy dữ liệu
                List<byte> resultList = new List<byte>();
                // 1.1 Mã kế hoạch
                resultList.AddRange(BitConverter.GetBytes(PlanID));
                if (PlanID > 0)
                {
                    // 1.2 Tên kế hoạch
                    byte[] nameBff = Constants.UTF8CodePage.GetBytes(Name);
                    resultList.AddRange(BitConverter.GetBytes((short)nameBff.Length));
                    resultList.AddRange(nameBff);
                    // 1.3 Đường link
                    byte[] rootBff = Constants.UTF8CodePage.GetBytes(RootURL.ToLower());
                    resultList.AddRange(BitConverter.GetBytes((short)rootBff.Length));
                    resultList.AddRange(rootBff);
                }

                // 2. Trả về kết quả
                return resultList.ToArray();
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlan.ToBinaryLogin, ex: {0}", ex.ToString()));
                return null;
            }
        }
    }
}