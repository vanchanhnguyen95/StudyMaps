using System;
using System.Collections.Generic;
using System.Data;

using BAGeocoding.Entity.Enum.MapTool;
using BAGeocoding.Entity.MapTool.Base;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapTool
{
    public class MCLIPad : CatalogBase
    {
        public int DeviceID { get; set; }
        public string IMEINumber { get; set; }
        public string MachineKey { get; set; }
        public string SIMNumber { get; set; }
        public string Description { get; set; }

        public int DataExt { get; set; }

        public string OSVersion { get; set; }
        public string APVersion { get; set; }
        public int UserID { get; set; }

        public MCLIPad() : base() { }

        public MCLIPad(MCLIPad other) : base(other) 
        {
            DeviceID = other.DeviceID;
            IMEINumber = other.IMEINumber;
            MachineKey = other.MachineKey;
            SIMNumber = other.SIMNumber;
            Description = other.Description;
            DataExt = other.DataExt;
        }

        public bool FromDataView(DataRow dr)
        {
            try
            {
                DeviceID = base.GetDataValue<int>(dr, "DeviceID");
                IMEINumber = base.GetDataValue<string>(dr, "IMEINumber", string.Empty);
                MachineKey = base.GetDataValue<string>(dr, "MachineKey", string.Empty);
                SIMNumber = base.GetDataValue<string>(dr, "SIMNumber", string.Empty);
                Description = base.GetDataValue<string>(dr, "Description", string.Empty);

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("MCLIPad.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }
        
        public bool FromDataRow(DataRow dr)
        {
            try
            {
                if (base.FromDataRow(dr) == false)
                    return false;
                DeviceID = base.GetDataValue<int>(dr, "DeviceID");
                IMEINumber = base.GetDataValue<string>(dr, "IMEINumber", string.Empty);
                MachineKey = base.GetDataValue<string>(dr, "MachineKey", string.Empty);
                SIMNumber = base.GetDataValue<string>(dr, "SIMNumber", string.Empty);
                Description = base.GetDataValue<string>(dr, "Description", string.Empty);

                OSVersion = base.GetDataValue<string>(dr, "OSVersion", string.Empty);
                APVersion = base.GetDataValue<string>(dr, "APVersion", string.Empty);
                UserID = base.GetDataValue<int>(dr, "UserID");
                
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("MCLIPad.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        #region ==================== Dữ liệu mở rộng ===============================
        public List<MCLIPadDataExt> DataExtList(bool state)
        {
            List<MCLIPadDataExt> dataExtList = new List<MCLIPadDataExt>();
            List<EnumItemAttribute> atributeList = StringUlt.GetListEnumAttribute(EnumMCLIPadDataExt.Unknown);
            foreach (EnumItemAttribute item in atributeList)
            {
                dataExtList.Add(new MCLIPadDataExt
                {
                    DataExt = (EnumMCLIPadDataExt)item.Value,
                    Name = item.Name,
                    State = state && DataExtGet((EnumMCLIPadDataExt)item.Value)
                });
            }
            return dataExtList;
        }

        public bool DataExtGet(EnumMCLIPadDataExt dataExt)
        {
            return ((DataExt & (long)Math.Pow(2, (int)dataExt)) > 0);
        }

        public void DataExtSet(EnumMCLIPadDataExt dataExt, bool status)
        {
            // Bít đã được bật
            if (((DataExt >> (int)dataExt) & 1) > 0)
            {
                if (status == false)
                    DataExt = DataExt - (int)Math.Pow(2, (int)dataExt);
            }
            // Bít chưa bật
            else
            {
                if (status == true)
                    DataExt = DataExt + (int)Math.Pow(2, (int)dataExt);
            }
        }
        #endregion

        public bool Equal(MCLIPad other)
        {
            if (MachineKey != other.MachineKey)
                return false;
            else if (SIMNumber != other.SIMNumber)
                return false;
            else if (Description != other.Description)
                return false;
            else
                return true;
        }

        public bool EqualDataExt(MCLIPad other)
        {
            if (DataExt != other.DataExt)
                return false;
            else
                return true;
        }
    }

    public class MCLIPadDataExt
    {
        public EnumMCLIPadDataExt DataExt { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }
    }
}