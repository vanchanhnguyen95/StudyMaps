using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BAGeocoding.Utility
{
    public class Constants
    {
        public static CultureInfo DEFAULT_CULTUREINFO = new CultureInfo("en-US");
        public static Encoding TCVN3CodePage = Encoding.UTF7;
        public static Encoding UTF8CodePage = Encoding.UTF8;
        public static Encoding UnicodeCodePage = Encoding.Unicode;

#if RELEASE
        public static string WINAPP_STATE = "Release"; //RELEASE
        public static string WINAPP_VERSION = "1.4.1";
        public static string WEBSITE_VERSION = "1.0.0";
        public static string WEBSERVICE_API_URL = @"http://maptool-api.binhanh.vn";
        public static string WEBSERVICE_IMG_URL = @"http://maptool-img.binhanh.vn";
        public static string WEBSITE_ROOT_URL = @"http://maptool-web.binhanh.vn";
        public static string MAP_TOOL_DATA_PATH = @"D:\MapToolData\";
        public static string DBMS_CONNECTION_STRING_GEOCODING = @"Data Source=192.168.1.15\BASQL;Initial Catalog=BA_Geocoding;User Id=ba_geocoding_rd;Password=NU5juQUrPYGZP8S3EZA5NFnJPsU6tSp7;";
        public static string DBMS_CONNECTION_STRING_MAPIMAGE = @"Data Source=192.168.1.15\BASQL;Initial Catalog=BAMapImage;User Id=ba_geocoding_rd;Password=NU5juQUrPYGZP8S3EZA5NFnJPsU6tSp7;";
        public static string DBMS_CONNECTION_STRING_MAPTOOL = @"Data Source=192.168.1.15\BASQL;Initial Catalog=BA_MapTool;User Id=ba_geocoding_rd;Password=NU5juQUrPYGZP8S3EZA5NFnJPsU6tSp7;";
        public static string DBMS_CONNECTION_STRING_MAPROUTE = @"Data Source=192.168.1.15\BASQL;Initial Catalog=BA_MapRoute;User Id=ba_geocoding_rd;Password=NU5juQUrPYGZP8S3EZA5NFnJPsU6tSp7;";

        public static string LOGIN_USERNAME = "";
        public static string LOGIN_PASSWORD = "";

        // Đường dẫn ghi logs
        public static string DEFAULT_DIRECTORY_LOGS = @"D:\Software\BAGeocoding\WebApi\Logs\";
        public static string DEFAULT_DIRECTORY_DATA = @"D:\Working\BAGeocoding\BAGeocoding.Tool\bin\Release\Data\Result\";
#else
        public static string WINAPP_STATE = "Debug";
        public static string WINAPP_VERSION = "1.0.0";
        public static string WEBSITE_VERSION = "1.0.0";
        public static string WEBSERVICE_API_URL = @"http://maptool-test-api.binhanh.vn";
        public static string WEBSERVICE_IMG_URL = @"http://maptool-test-img.binhanh.vn";
        public static string WEBSITE_ROOT_URL = @"http://localhost/maptool-web";
        public static string MAP_TOOL_DATA_PATH = @"D:\MapToolTest\";

        //public static string DBMS_CONNECTION_STRING_GEOCODING = @"Data Source=192.168.1.52\BASQL;Initial Catalog=BA_Geocoding;User Id=ba_geocoding;Password=ba_geocoding;";
        //public static string DBMS_CONNECTION_STRING_MAPIMAGE = @"Data Source=192.168.1.52\BASQL;Initial Catalog=BAMapImage;User Id=ba_geocoding;Password=ba_geocoding;";
        //public static string DBMS_CONNECTION_STRING_MAPTOOL = @"Data Source=192.168.1.52\BASQL;Initial Catalog=BA_MapTool;User Id=ba_geocoding;Password=ba_geocoding;";
        //public static string DBMS_CONNECTION_STRING_MAPROUTE = @"Data Source=192.168.1.52\BASQL;Initial Catalog=BA_MapRoute;User Id=ba_geocoding;Password=ba_geocoding;";    

        public static string DBMS_CONNECTION_STRING_GEOCODING = @"Data Source=192.168.1.81,31433;Initial Catalog=BA_Geocoding;User Id=sa;Password=bA@sql321;";
        public static string DBMS_CONNECTION_STRING_MAPIMAGE = @"Data Source=192.168.1.81,31433;Initial Catalog=BAMapImage;User Id=sa;Password=bA@sql321;";
        public static string DBMS_CONNECTION_STRING_MAPTOOL = @"Data Source=192.168.1.81,31433;Initial Catalog=BA_MapTool;User Id=sa;Password=bA@sql321;";
        public static string DBMS_CONNECTION_STRING_MAPROUTE = @"Data Source=192.168.1.81,31433;Initial Catalog=BA_MapRoute;User Id=sa;Password=bA@sql321;";

        public static string LOGIN_USERNAME = "admin";
        public static string LOGIN_PASSWORD = "123123";

        // Đường dẫn ghi logs
        //public static string DEFAULT_DIRECTORY_LOGS = @"D:\Working\BAGeocoding\BAGeocoding.Service\Logs\";
        //public static string DEFAULT_DIRECTORY_DATA = @"D:\Working\BAGeocoding\BAGeocoding.Service\App_Data\";

        //public static string DEFAULT_DIRECTORY_LOGS = @"D:\TEST\BAGeocoding\BAGeocoding.Service\Logs\";
        //public static string DEFAULT_DIRECTORY_DATA = @"D:\TEST\BAGeocoding\BAGeocoding.Service\App_Data\";

        public static string DEFAULT_DIRECTORY_LOGS = @"TEST\BAGeocoding\BAGeocoding.Service\Logs\";
        public static string DEFAULT_DIRECTORY_DATA = @"TEST\BAGeocoding\BAGeocoding.Service\App_Data\";
#endif

        public static string ROUTER_CERTIFICATE_KEY = "";

        public static char DEFAULT_SPLIT_KEYS = ',';
        public static char DEFAULT_SPLIT_DATA = '@';

        public static string DEFAULT_REGION_FILE_NAME = "__rg.ba";
        public static string DEFAULT_PLACE_FILE_NAME = "__pl.ba";
        public static string DEFAULT_SEGMENT_FILE_NAME = "__sm.ba";
        public static string DEFAULT_POI_FILE_NAME = "__po.ba";
        public static string DEFAULT_KEY_FILE_NAME = "__ky.ba";
        public static string DEFAULT_FILE_NAME_ROUTE = "__rt.ba";


        public static int SEGMENT_DISTANCE_POINT_ADD = 20;  // 20 met
        public static int PROVINCE_SHIFT_FOR_KEY = 10000;  //10.000
        public static int PROVINCE_SHIFT_FOR_SEGMENT = 10000000;  //10.000.000


        public static double EARTH_RADIUS = 6371000d;

        public static double DISTANCE_INTERSECT = 0.0001f;
        public static double DISTANCE_INTERSECT_ROAD = 0.0002f;  //0.0005f ~ 55.5 (m)
        public static double DISTANCE_INTERSECT_EPSILON = 0.0005f;
        public static List<double> DISTANCE_INTERSECT_LIST = new List<double>();


        public static string WAITING_MESSAGE_LOADING = "Đang tải dữ liệu...";
        public static string WAITING_MESSAGE_SYNCING = "Đang đồng bộ dữ liệu...";
        public static string WAITING_MESSAGE_PROCESSING = "Đang xử lý dữ liệu...";
    }
}