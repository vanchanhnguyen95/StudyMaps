using System;

using BAGeocoding.Bll;

using BAGeocoding.Entity.Utility;

using BAGeocoding.Utility;

namespace BAGeocoding.Service
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            AppEnv appEn = new AppEnv(Context);
            Constants.DEFAULT_SPLIT_KEYS = appEn.GetAppSetting("SplitKeys")[0];
            Constants.DEFAULT_SPLIT_DATA = appEn.GetAppSetting("SplitData")[0];
            Constants.DEFAULT_DIRECTORY_DATA = appEn.GetAppSetting("Data");
            Constants.DEFAULT_DIRECTORY_LOGS = appEn.GetAppSetting("Logs");

            Constants.DISTANCE_INTERSECT_ROAD = DataUtl.ConvertMeterToLngLat(appEn.GetValueAsNumber("DistanceIntersectRoad"));
            Constants.DISTANCE_INTERSECT_EPSILON = DataUtl.ConvertMeterToLngLat(appEn.GetValueAsNumber("DistanceIntersectEpsilon"));
            DataUtl.ConvertDistanceIntersect(appEn.GetValueAsString("DistanceIntersectRange"));

            RunningParams.TestSpeed = Convert.ToBoolean(appEn.GetAppSetting("ServiceTestSpeed"));
            RunningParams.DataSpeed = Convert.ToBoolean(appEn.GetAppSetting("ServiceDataSpeed"));
            RunningParams.CheckAuthen = Convert.ToBoolean(appEn.GetAppSetting("CheckAuthenticate"));
            RunningParams.AuthHeaderInfo = new UTLAuthHeader(appEn.GetAppSetting("User"), appEn.GetAppSetting("Pass"));
            RunningParams.GOOGLE_GEOCODE_KEY = appEn.GetAppSetting("GoogleKey");
            RunningParams.ProvinceRoadByLevel = appEn.GetAppSetting("RoadByLevel");
            RunningParams.DistrictPriorityStr = appEn.GetAppSetting("PriorityStr");
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}