using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CpGeoService.Model
{
    public class DataPNC
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Address { get; set; }

        //public  DataPNC (double lng, double lat, int building, string road="", string commune = "", string district = "", string province = "")
        //{
        //    Lat = lat;
        //    Lng = lng;
        //    Address = 
        //}
    }

    public class ResultGeoByAddressPNC
    {
        public DataPNC Data { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
    }

    public class DataPBDV2
    {
        public double lng { get; set; }
        public double lat { get; set; }
        public int building { get; set; }
        public string road { get; set; }
        public string commune { get; set; }
        public string district { get; set; }
        public string province { get; set; }
        public bool accurate { get; set; }
    }

    public class ResultGeoByAddressPBDV2
    {
        public DataPBDV2 data { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public string processState { get; set; }
    }

    public class DataMerg : DataPNC
    {
        public string? Dep { get; set; }
        public double? Distance { get; set; } = 0;
    }

    public class Datum
    {
        public List<DataMerg> Location { get; set; }
        //public double Distance { get; set; }
    }

    public class ResultGeoByAddressMerge
    {
        public List<Datum>? Data { get; set; }
        public int Code { get; set; } = 1;
        public string? Message { get; set; }
    }

}
