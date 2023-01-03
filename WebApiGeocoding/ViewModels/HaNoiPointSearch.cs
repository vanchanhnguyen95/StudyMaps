using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiGeocoding.ViewModels
{
    public class HaNoiPointSearch
    {
        public string Name { get; set; } // Tên địa chỉ đầy đủ
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class SpeedHaNoiPointSearch
    {
        public string Name { get; set; } // Tên địa chỉ đầy đủ
        public int Speed { get; set; } // Tốc độ tối đa cho phép
    }
}
