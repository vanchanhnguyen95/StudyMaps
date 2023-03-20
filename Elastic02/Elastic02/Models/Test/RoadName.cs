using Elastic02.Utility;
using Nest;
using System.ComponentModel;

namespace Elastic02.Models.Test
{
    public class RoadNamePush
    {
        [Number(Index = true)]
        public int RoadID { get; set; } = 0;

        [Number(Index = true)]
        public int ProvinceID { get; set; } = 0;

        //[Text(Index = true, Fielddata = true, Analyzer = "my_combined_analyzer")]
        [Text(Index = true, Fielddata = true)]
        public string? RoadName { get; set; } = string.Empty;

        [Text(Index = true, Fielddata = true)]
        public string? NameExt { get; set; } = string.Empty;

        [Text(Index = true, Fielddata = true)]
        public string? Address { get; set; } = string.Empty;

        [Number(Index = true)]
        public decimal Lng { get; set; } = 0;

        [Number(Index = true)]
        public decimal Lat { get; set; } = 0;
    }

    [ElasticsearchType(IdProperty = nameof(Id)), Description("roadname-combined")]
    public class RoadName : RoadNamePush
    {
        //[GeoPoint]
        //[Number(Index = true)]
        //public string Location { get; set; } = string.Empty;

        [Text(Index = true, Fielddata = true)]
        public string? AddressLower { get; set; } = string.Empty;

        //[Text(Index = true, Fielddata = true, Analyzer = "my_combined_analyzer")]
        [Text(Index = true, Fielddata = true)]
        public string Keywords { get; set; } = string.Empty;

        public string? KeywordsAscii { get; set; } = string.Empty;
        public RoadName(RoadNamePush other)
        {
            RoadID = other.RoadID;
            ProvinceID = other.ProvinceID;
            RoadName = other.RoadName;
            NameExt = other.NameExt;
            Address = other.Address;
            AddressLower = other.Address.ToLower();
            Lng = other.Lng;
            Lat = other.Lat;

            //Location = other.Lat.ToString() + ", " + other.Lng.ToString();
            if (!string.IsNullOrEmpty(other.NameExt))
            {
                Keywords = other?.RoadName?.ToString().ToLower() + " , " + other?.NameExt?.ToString().ToLower();
            }
            else
            {
                Keywords = other?.RoadName?.ToString().ToLower() ?? "";
                //Keywords = other.RoadName + " , ";
            }

            if (!string.IsNullOrEmpty(other?.Address))
            {
                Keywords += " , " + other?.Address?.ToString().ToLower();
            }else
            {
                Keywords += " , ";
            }
            //else
            //{
            //    Keywords += other?.Address ?? "";
            //}

            KeywordsAscii = LatinToAscii.Latin2Ascii(Keywords);
        }

        public RoadName(RoadNamePush other, string type= "p")
        {
            RoadID = other.RoadID;
            ProvinceID = other.ProvinceID;
            RoadName = other.RoadName;
            NameExt = other.NameExt;
            Address = other.Address;
            AddressLower = other?.Address?.ToLower();
            Lng = other.Lng;
            Lat = other.Lat;

            //Location = other.Lat.ToString() + ", " + other.Lng.ToString();
            if (!string.IsNullOrEmpty(other.NameExt))
            {
                Keywords = other?.RoadName?.ToString().ToLower() + " , " + other?.NameExt?.ToString().ToLower();
            }
            else
            {
                Keywords = other?.RoadName?.ToString().ToLower() ?? "";
                //Keywords = other.RoadName + " , ";
            }

            if (!string.IsNullOrEmpty(other?.Address))
            {
                Keywords += " , " + other?.Address?.ToString().ToLower();
            }
            else
            {
                Keywords += " , ";
            }

            KeywordsAscii = LatinToAscii.Latin2Ascii(Keywords);

            //else
            //{
            //    Keywords += other?.Address ?? "";
            //}

            //switch (ProvinceID)
            //{
            //    case 1:
            //        Keywords += @", hà giang";
            //        break;
            //    case 2:
            //        Keywords += @", cao bằng";
            //        break;
            //    case 3:
            //        Keywords += @", lai châu";
            //        break;
            //    case 4:
            //        Keywords += @", lào cai";
            //        break;
            //    case 5:
            //        Keywords += @", tuyên quang";
            //        break;
            //    case 6:
            //        Keywords += @", bắc kạn";
            //        break;
            //    case 7:
            //        Keywords += @", lạng sơn";
            //        break;
            //    case 8:
            //        Keywords += @", điện biên";
            //        break;
            //    case 9:
            //        Keywords += @", yên bái";
            //        break;
            //    case 10:
            //        Keywords += @", thái nguyên";
            //        break;
            //    case 11:
            //        Keywords += @", sơn la";
            //        break;
            //    case 12:
            //        Keywords += @", phú thọ";
            //        break;
            //    case 13:
            //        Keywords += @", vĩnh phúc";
            //        break;
            //    case 14:
            //        Keywords += @", bắc giang";
            //        break;
            //    case 15:
            //        Keywords += @", quảng ninh";
            //        break;
            //    case 16:
            //        Keywords += @", hà nội";
            //        break;
            //    case 17:
            //        Keywords += @", bắc ninh";
            //        break;
            //    case 18:
            //        Keywords += @", hưng yên";
            //        break;
            //    case 19:
            //        Keywords += @", hải dương";
            //        break;
            //    case 20:
            //        Keywords += @", hải phòng";
            //        break;
            //    case 21:
            //        Keywords += @", hòa bình";
            //        break;
            //    case 22:
            //        Keywords += @", hà nam";
            //        break;
            //    case 23:
            //        Keywords += @", thái bình";
            //        break;
            //    case 24:
            //        Keywords += @", ninh bình";
            //        break;
            //    case 25:
            //        Keywords += @", nam định";
            //        break;
            //    case 26:
            //        Keywords += @", thanh hóa";
            //        break;
            //    case 27:
            //        Keywords += @", nghệ an";
            //        break;
            //    case 28:
            //        Keywords += @", hà tĩnh";
            //        break;
            //    case 29:
            //        Keywords += @", quảng bình";
            //        break;
            //    case 30:
            //        Keywords += @", quảng trị";
            //        break;
            //    case 31:
            //        Keywords += @", thừa thiên huế";
            //        break;
            //    case 32:
            //        Keywords += @", đà nẵng";
            //        break;
            //    case 33:
            //        Keywords += @", quảng nam";
            //        break;
            //    case 34:
            //        Keywords += @", kon tum";
            //        break;
            //    case 35:
            //        Keywords += @", quảng ngãi";
            //        break;
            //    case 36:
            //        Keywords += @", gia lai";
            //        break;
            //    case 37:
            //        Keywords += @", bình định";
            //        break;
            //    case 38:
            //        Keywords += @", phú yên";
            //        break;
            //    case 39:
            //        Keywords += @", đak lak";
            //        break;
            //    case 40:
            //        Keywords += @", khánh hòa";
            //        break;
            //    case 41:
            //        Keywords += @", đak nong";
            //        break;
            //    case 42:
            //        Keywords += @", bình phước";
            //        break;
            //    case 43:
            //        Keywords += @", lâm đồng";
            //        break;
            //    case 44:
            //        Keywords += @", ninh thuận";
            //        break;
            //    case 45:
            //        Keywords += @", tây ninh";
            //        break;
            //    case 46:
            //        Keywords += @", bình dương";
            //        break;
            //    case 47:
            //        Keywords += @", đồng nai";
            //        break;
            //    case 48:
            //        Keywords += @", bình thuận";
            //        break;
            //    case 49:
            //        Keywords += @", long an";
            //        break;
            //    case 50:
            //        Keywords += @", hồ chí minh";
            //        break;

            //    case 51:
            //        Keywords += @", vũng tàu";
            //        break;
            //    case 52:
            //        Keywords += @", an giang";
            //        break;
            //    case 53:
            //        Keywords += @", đồng tháp";
            //        break;
            //    case 54:
            //        Keywords += @", tiền giang";
            //        break;
            //    case 55:
            //        Keywords += @", kiên giang";
            //        break;
            //    case 56:
            //        Keywords += @", cần thơ";
            //        break;
            //    case 57:
            //        Keywords += @", vĩnh long";
            //        break;
            //    case 58:
            //        Keywords += @", bến tre";
            //        break;
            //    case 59:
            //        Keywords += @", hậu giang";
            //        break;
            //    case 60:
            //        Keywords += @", sóc trăng";
            //        break;
            //    case 61:
            //        Keywords += @", trà vinh";
            //        break;
            //    case 62:
            //        Keywords += @", bạc liêu";
            //        break;
            //    case 63:
            //        Keywords += @", cà mau";
            //        break;
            //}    

        }

    }
}
