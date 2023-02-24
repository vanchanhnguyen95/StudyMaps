using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticProject.Data.Entity.MapObj
{
    public class BGCLngLat
    {
        [JsonProperty("lng")]
        public double Lng { get; set; }
        [JsonProperty("lat")]
        public double Lat { get; set; }

        public BGCLngLat() { }

        public BGCLngLat(BGCLngLat other)
        {
            Lng = other.Lng;
            Lat = other.Lat;
        }

        public BGCLngLat(double lng, double lat)
        {
            //Lng = Math.Round(lng, Constants.ROUND_DOUBLE_DIGIT);
            //Lat = Math.Round(lat, Constants.ROUND_DOUBLE_DIGIT);
            Lng = Math.Round(lng, 8);
            Lat = Math.Round(lat, 8);
        }

        public BGCLngLat(object lng, object lat, bool rev = false)
        {
            if (rev == true)
            {
                Lat = Math.Round(Convert.ToDouble(lng), 8);
                Lng = Math.Round(Convert.ToDouble(lat), 8);
            }
            else
            {
                Lng = Math.Round(Convert.ToDouble(lng), 8);
                Lat = Math.Round(Convert.ToDouble(lat), 8);
            }
        }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                Lng = Math.Round(Convert.ToDouble(dr["Lng"], new CultureInfo("en-US")), 8);
                Lat = Math.Round(Convert.ToDouble(dr["Lat"], new CultureInfo("en-US")), 8);

                return true;
            }
            catch { return false; }
        }
    }
}
