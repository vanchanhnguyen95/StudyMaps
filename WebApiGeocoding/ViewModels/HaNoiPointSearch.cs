namespace WebApiGeocoding.ViewModels
{
    public class HaNoiPointSearch
    {
        public string Name { get; set; } // Tên địa chỉ đầy đủ
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class GeocodeHaNoiPointSearch
    {
        public string Name { get; set; } // Tên địa chỉ đầy đủ
        public string Street { get; set; } // Tên đường (số nhà, địa danh)
        public string Ward { get; set; }// Tên phường
        public string District { get; set; }// Tên quận
        public string Province { get; set; }// Tên tỉnh, thành phố trực thuộc trung ương
    }

    public class SpeedHaNoiPointSearch
    {
        public string Name { get; set; } // Tên địa chỉ đầy đủ
        public int Speed { get; set; } // Tốc độ tối đa cho phép
    }
}
