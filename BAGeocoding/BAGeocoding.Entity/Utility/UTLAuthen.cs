using System;
using System.Data;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Utility
{
    public class UTLAuthen : SQLDataUlt
    {
        public int AuthenID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int DataExt { get; set; }
        public int CreateTime { get; set; }

        public int QuotaBA { get; set; }
        public int QuotaGG { get; set; }

        public byte State { get; set; }

        public UTLAuthen()
        {
            Username = string.Empty;
            Password = string.Empty;
        }

        public bool FromDataSimple(DataRow dr)
        {
            try
            {
                AuthenID = base.GetDataValue<int>(dr, "AuthenID", 0);
                Username = base.GetDataValue<string>(dr, "Username", string.Empty);
                Password = base.GetDataValue<string>(dr, "Password", string.Empty);
                DataExt = base.GetDataValue<int>(dr, "DataExt", 0);

                QuotaBA = base.GetDataValue<int>(dr, "QuotaBA", 0);
                QuotaGG = base.GetDataValue<int>(dr, "QuotaGG", 0);

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("UTLAuthen.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool Equal(UTLAuthen other)
        {
            if (Username != other.Username)
                return false;
            else if (Password != other.Password)
                return false;
            else
                return true;
        }

        public override string ToString()
        {
            return string.Format("Username: '{0}', Password: '{1}', QuotaBA: {2}, QuotaGG: {3}", Username, Password, QuotaBA, QuotaGG);
        }
    }
}
