using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Specialized;
using System.Web;

namespace BAGeocoding.Utility
{
    /// <summary>
    /// Summary description for application's environment
    /// </summary>
    public class AppEnv
    {
        private HttpContext context;

        public AppEnv(HttpContext Context)
        {
            context = Context;
        }

        /// <summary>
        /// Get application string
        /// </summary>
        public string GetAppSetting(string setting)
        {
            string val;
            try
            {
                //val = (string)((NameValueCollection)context.GetConfig("appSettings"))[setting];
                val = "";
            }
            catch (NullReferenceException)
            {
                val = string.Empty;
            }

            if (val == null)
                return string.Empty;
            else
                return val.Trim();
        }

        public string GetValueAsString(string key)
        {
            try
            {
                return GetAppSetting(key);
            }
            catch
            {
                return string.Empty;
            }
            
        }

        public int GetValueAsNumber(string key)
        {
            try
            {
                return Convert.ToInt32(GetAppSetting(key));
            }
            catch
            {
                return 0;
            }
        }
    }
}
