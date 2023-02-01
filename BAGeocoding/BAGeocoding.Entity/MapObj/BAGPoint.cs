using BAGeocoding.Utility;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RTree.Engine.Entity;
using System.Data;

namespace BAGeocoding.Entity.MapObj
{
    public class BAGPoint
    {
        public const double PIdiv180 = Math.PI / 180.0;
        public const double Radius = 6367000;
        public const int Digit = 8;

        public double Lng { get; set; }
        public double Lat { get; set; }

        public double D2Start { get; set; }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                Lng = Math.Round(Convert.ToDouble(dr["Lng"], Constants.DEFAULT_CULTUREINFO), Digit);
                Lat = Math.Round(Convert.ToDouble(dr["Lat"], Constants.DEFAULT_CULTUREINFO), Digit);

                return true;
            }
            catch { return false; }
        }

        public BAGPoint() { }

        public BAGPoint(BAGPoint other) 
        {
            Lng = other.Lng;
            Lat = other.Lat;
        }

        public BAGPoint(double lng, double lat)
        {
            Lng = Math.Round(lng, Digit);
            Lat = Math.Round(lat, Digit);
        }

        public BAGPoint(object lng, object lat, bool rev = false)
        {
            if (rev == true)
            {
                Lat = Math.Round(Convert.ToDouble(lng), Digit);
                Lng = Math.Round(Convert.ToDouble(lat), Digit);
            }
            else
            {
                Lng = Math.Round(Convert.ToDouble(lng), Digit);
                Lat = Math.Round(Convert.ToDouble(lat), Digit);
            }
        }

        public static BAGPoint operator -(BAGPoint P1, BAGPoint P2)
        {
            return new BAGPoint((P1.Lng - P2.Lng) * Math.Cos((P1.Lat + P2.Lat) * 0.5 * PIdiv180), P1.Lat - P2.Lat);
        }

        public static double operator *(BAGPoint P1, BAGPoint P2)
        {
            return (P1.Lng * P2.Lng + P1.Lat * P2.Lat);
        }

        public double Distance(BAGPoint P0)
        {
            BAGPoint NP = this - P0;
            return Math.Sqrt(NP * NP) * PIdiv180 * Radius;
        }

        public void AddPoint(BAGPoint other)
        {
            Lng += other.Lng;
            Lat += other.Lat;
        }

        public bool IsValid()
        {
            if (Lng == 0 && Lat == 0)
                return false;
            else
                return true;
        }

        public RTRectangle ToRectangle(double exp)
        {
            return new RTRectangle(Lng - exp, Lat - exp, Lng + exp, Lat + exp);
        }

        public byte[] ToBinary()
        {
            List<byte> resultList = new List<byte>();
            resultList.AddRange(BitConverter.GetBytes(Lng));
            resultList.AddRange(BitConverter.GetBytes(Lat));
            return resultList.ToArray();
        }

        public double[] ToArray()
        {
            return new double[2] { Lng, Lat };
        }

        public override string ToString()
        {
            return string.Format("Lat, Lng: ({0}, {1})", Lat, Lng);
        }
    }

    public class BAGPointV2
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public const double PIdiv180 = Math.PI / 180.0;
        public const double Radius = 6367000;
        public const int Digit = 8;

        public double Lng { get; set; }
        public double Lat { get; set; }

        public double D2Start { get; set; }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                Lng = Math.Round(Convert.ToDouble(dr["Lng"], Constants.DEFAULT_CULTUREINFO), Digit);
                Lat = Math.Round(Convert.ToDouble(dr["Lat"], Constants.DEFAULT_CULTUREINFO), Digit);

                return true;
            }
            catch { return false; }
        }

        public BAGPointV2() { }

        public BAGPointV2(BAGPointV2 other)
        {
            Lng = other.Lng;
            Lat = other.Lat;
        }

        public BAGPointV2(double lng, double lat)
        {
            Lng = Math.Round(lng, Digit);
            Lat = Math.Round(lat, Digit);
        }

        public BAGPointV2(object lng, object lat, bool rev = false)
        {
            if (rev == true)
            {
                Lat = Math.Round(Convert.ToDouble(lng), Digit);
                Lng = Math.Round(Convert.ToDouble(lat), Digit);
            }
            else
            {
                Lng = Math.Round(Convert.ToDouble(lng), Digit);
                Lat = Math.Round(Convert.ToDouble(lat), Digit);
            }
        }

        public static BAGPointV2 operator -(BAGPointV2 P1, BAGPointV2 P2)
        {
            return new BAGPointV2((P1.Lng - P2.Lng) * Math.Cos((P1.Lat + P2.Lat) * 0.5 * PIdiv180), P1.Lat - P2.Lat);
        }

        public static double operator *(BAGPointV2 P1, BAGPointV2 P2)
        {
            return (P1.Lng * P2.Lng + P1.Lat * P2.Lat);
        }

        public double Distance(BAGPointV2 P0)
        {
            BAGPointV2 NP = this - P0;
            return Math.Sqrt(NP * NP) * PIdiv180 * Radius;
        }

        public void AddPoint(BAGPointV2 other)
        {
            Lng += other.Lng;
            Lat += other.Lat;
        }

        public bool IsValid()
        {
            if (Lng == 0 && Lat == 0)
                return false;
            else
                return true;
        }

        public RTRectangle ToRectangle(double exp)
        {
            return new RTRectangle(Lng - exp, Lat - exp, Lng + exp, Lat + exp);
        }

        public byte[] ToBinary()
        {
            List<byte> resultList = new List<byte>();
            resultList.AddRange(BitConverter.GetBytes(Lng));
            resultList.AddRange(BitConverter.GetBytes(Lat));
            return resultList.ToArray();
        }

        public double[] ToArray()
        {
            return new double[2] { Lng, Lat };
        }

        public override string ToString()
        {
            return string.Format("Lat, Lng: ({0}, {1})", Lat, Lng);
        }
    }
}
