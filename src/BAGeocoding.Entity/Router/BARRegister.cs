using System;
using System.Collections;
using System.Data;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Router
{
    public class BARRegister : SQLDataUlt
    {
        public int RegisterID { get; set; }
        public string KeyStr { get; set; }
        public string IPStr { get; set; }
        public Hashtable IPAddress { get; set; }

        public BARRegister()
        {
            KeyStr = string.Empty;
            IPAddress = new Hashtable();
        }

        public BARRegister(BARRegister other)
        {
            KeyStr = other.KeyStr;
            IPAddress = (Hashtable)other.IPAddress.Clone();
        }
        
        public bool FromDataSimple(DataRow dr)
        {
            try
            {
                RegisterID = base.GetDataValue<int>(dr, "RegisterID", 0);
                KeyStr = base.GetDataValue<string>(dr, "KeyStr", string.Empty);
                IPStr = base.GetDataValue<string>(dr, "IPAddress", string.Empty);
                IPAddress.Add(IPStr, RegisterID);
                
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BARRegister.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public void UpdateIP(BARRegister other)
        {
            if (IPAddress.ContainsKey(other.IPStr) == false)
                IPAddress.Add(other.IPStr, RegisterID);
        }

        public override string ToString()
        {
            return string.Format("RegisterID: {0}, KeyStr: {1}, IPStr: {2}", RegisterID, KeyStr, IPStr);
        }
    }
}