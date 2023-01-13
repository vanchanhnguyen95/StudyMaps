using System;
using System.Collections.Generic;
using System.Data;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapObj
{
    public class BAGKeyRate : SQLDataUlt
    {
        public int ObjectID { get; set; }
        public byte IndexID { get; set; }
        public byte Percent { get; set; }
        public short ReferenceID { get; set; }

        public BAGKeyRate() { }

        public BAGKeyRate(BAGKeyRate other) 
        {
            ObjectID = other.ObjectID;
            IndexID = other.IndexID;
            Percent = other.Percent;
            ReferenceID = other.ReferenceID;
        }

        public bool FromDataRow(DataRow dr, bool fl)
        {
            try
            {
                ObjectID = base.GetDataValue<int>(dr, "ObjectID");
                IndexID = base.GetDataValue<byte>(dr, "IndexID");
                Percent = base.GetDataValue<byte>(dr, "RatePercent");
                if (fl == true)
                    ReferenceID = base.GetDataValue<short>(dr, "ReferenceID");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("UTLSearchKey.FromDataRow, ex: " + ex.ToString());
                return false;
            }
        }

        public void UpdateRate(BAGKeyRate other)
        {
            Percent = (byte)(Percent + other.Percent);
        }

        public byte[] ToBinary(bool fl)
        {
            List<byte> result = new List<byte>();
            result.Add(IndexID);
            result.Add(Percent);
            if (fl == true)
                result.AddRange(BitConverter.GetBytes(ReferenceID));
            return result.ToArray();
        }
    }
}
