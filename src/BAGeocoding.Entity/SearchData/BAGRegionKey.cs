using System;
using System.Data;

using BAGeocoding.Utility;
using BAGeocoding.Entity.Enum;

namespace BAGeocoding.Entity.SearchData
{
    public class BAGRegionKey : SQLDataUlt
    {
        public byte TypeID { get; set; }
        public EnumBAGRegionType EnumTypeID { get { return (EnumBAGRegionType)TypeID; } set { TypeID = (byte)value; } }
        public int KeyID { get; set; }
        public string KeyStr { get; set; }
        public int ObjectID { get; set; }
        public int Count { get; set; }

        public byte IndexID { get; set; }
        public float Rate { get; set; }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                TypeID = base.GetDataValue<byte>(dr, "TypeID");
                KeyID = base.GetDataValue<int>(dr, "KeyID");
                KeyStr = base.GetDataValue<string>(dr, "KeyStr", string.Empty);
                ObjectID = base.GetDataValue<int>(dr, "ObjectID");
                Count = base.GetDataValue<int>(dr, "Count");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BAGRegionKey.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }
    }
}