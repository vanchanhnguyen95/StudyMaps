using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

using BAGeocoding.Entity;
using BAGeocoding.Entity.DataService;
using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapTool;
using BAGeocoding.Entity.Router;
using BAGeocoding.Entity.Utility;

using BAGeocoding.Utility;

namespace BAGeocoding.Bll
{
    public class RunningParams
    {
        public static string DIRECTORY_RUNNING = Environment.CurrentDirectory + "\\";
        public static EnumProcessState ProcessState = EnumProcessState.None;
        public static bool ServiceState;
        public static bool TestSpeed = false;
        public static bool DataSpeed = false;
        public static bool CheckAuthen = true;
        public static bool CheckRegister = true;
        public static string InternalHostNormal = "http://127.0.0.1:6969/routing";
        public static string InternalHostNoFerry = "http://127.0.0.1:6968/routing";
        public static string GoogleRouteKey = "abcdef";
        public static string ProvinceRoadByLevel = "";
        public static string DistrictPriorityStr = "";
        public static UTLAuthHeader AuthHeaderInfo;

        public static Size FRM_CHILD_SIZE = new Size(100, 100);
        public static USRUser USER { get; set; }
        public static USRRolePermission PERMISSION { get; set; }
        public static CacheCatalogData CACHE { get; set; }
        public static CacheParamsData PARAMS { get; set; }

        public static DTSProvince ProvinceData = new DTSProvince();
        public static DTSDistrict DistrictData = new DTSDistrict();
        public static DTSCommune CommuneData = new DTSCommune();
        public static DTSTile TileData = new DTSTile();
        public static DTSPlace PlaceData = new DTSPlace();
        public static DTSPoint PointData = new DTSPoint();
        public static DTSRouter RouterData = new DTSRouter();

        public static BAREpsilon ROUTE_EPSILON = new BAREpsilon(20);

        public static Hashtable HTProvincePriority = new Hashtable();
        public static Hashtable RoadSpecial = new Hashtable();
        public static Hashtable DistrictPriority = new Hashtable();

        public static string GOOGLE_GEOCODE_KEY = "";

        public static EnumBAGFileType ImportFileType = EnumBAGFileType.Shp;


        /// <summary>
        /// Ghi dữ liệu đối tượng ra Json
        /// </summary>
        public static string Object2Json<T>(T a)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                ser.WriteObject(ms, a);
                byte[] json = ms.ToArray();
                ms.Close();
                return Encoding.UTF8.GetString(json, 0, json.Length);
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("RunningParams.Object2Json, ex: {0}", ex.ToString()));
                return string.Empty;
            }
        }

        /// <summary>
        /// Biến dữ liệu Json ra đối tượng
        /// </summary>
        public static T Json2Object<T>(string d)
        {
            try
            {
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(d));
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                return (T)ser.ReadObject(ms);
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("RunningParams.Json2Object, ex: {0}", ex.ToString()));
                return default(T);
            }
        }
    }
}
