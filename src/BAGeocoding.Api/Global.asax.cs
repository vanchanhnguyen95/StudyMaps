using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using BAGeocoding.Bll;
using BAGeocoding.Entity.Utility;
using BAGeocoding.Utility;

namespace BAGeocoding.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

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
            Debug.WriteLine("Main processing starting");
            MainProcessing.InitData();
            Debug.WriteLine("Main processing finished");
        }
    }
}
