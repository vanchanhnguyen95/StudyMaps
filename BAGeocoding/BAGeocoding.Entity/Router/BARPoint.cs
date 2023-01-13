using System;
using System.Collections.Generic;
using System.Data;

using BAGeocoding.Utility;

using RTree.Engine.Entity;

namespace BAGeocoding.Entity.Router
{
    public class BARPoint
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

        public BARPoint() { }

        public BARPoint(BARPoint other) 
        {
            Lng = other.Lng;
            Lat = other.Lat;
        }

        public BARPoint(double lng, double lat)
        {
            Lng = Math.Round(lng, Digit);
            Lat = Math.Round(lat, Digit);
        }

        public BARPoint(string coordStr)
        {
            try
            {
                string[] tempData = coordStr.Split(',');
                Lng = Math.Round(Convert.ToDouble(tempData[0]), Digit);
                Lat = Math.Round(Convert.ToDouble(tempData[1]), Digit);
            }
            catch
            {
                Lng = 0;
                Lat = 0;
            }
        }

        public static BARPoint operator -(BARPoint P1, BARPoint P2)
        {
            return new BARPoint((P1.Lng - P2.Lng) * Math.Cos((P1.Lat + P2.Lat) * 0.5 * PIdiv180), P1.Lat - P2.Lat);
        }

        public static double operator *(BARPoint P1, BARPoint P2)
        {
            return (P1.Lng * P2.Lng + P1.Lat * P2.Lat);
        }

        public double Distance(BARPoint P0)
        {
            BARPoint NP = this - P0;
            return Math.Round(Math.Sqrt(NP * NP) * PIdiv180 * Radius, 2);
        }

        public void AddPoint(BARPoint other)
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

        public string StringLngLat()
        {
            return string.Format("{0},{1}", Lng, Lat);
        }

        public override string ToString()
        {
            return string.Format("Lat, Lng: ({0}, {1})", Lat, Lng);
        }
    }
}
