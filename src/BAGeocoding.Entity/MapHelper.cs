using System;
using System.Collections.Generic;
using System.Text;

using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.Enum;

namespace BAGeocoding.Entity
{
    public static class MapHelper
    {
        private const double _MilesToNautical = 0.8684;
        private const double _MilesToKilometers = 1.609344;
        // hằng số delta tọa độ: delta= 180/pi*R(bán kính trái đất)
        private const double _DeltaCoordinate = 0.0000089805297042448772;

        //ban kinh trai dat - km 
        private const double R = Math.PI * 6371 / 180;
        private const double SquareR = R * R;
        private const int ConvertKmToMeter = 1000;

        /// <summary>
        /// Converts degrees to Radians.
        /// </summary>
        /// <returns>Returns a radian from degrees.</returns>
        public static double ToRadian(this double degree) { return (degree * Math.PI / 180.0); }

        /// <summary>
        /// To degress from a radian value.
        /// </summary>
        /// <returns>Returns degrees from radians.</returns>
        public static double ToDegree(this double radian) { return (radian / Math.PI * 180.0); }

        /// <summary>
        /// Calculates the distance between two points of latitude and longitude.
        /// Great Link - http://www.movable-type.co.uk/scripts/latlong.html
        /// </summary>
        /// <param name="coordinate1">First coordinate.</param>
        /// <param name="coordinate2">Second coordinate.</param>
        /// <param name="unitsOfLength">Sets the return value unit of length.</param>
        public static double Distance(double _OldLongitude, double _OldLatitude, double Longitude, double Latitude, EnumLengthUnit unitsOfLength)
        {
            if (_OldLongitude == 0 || _OldLatitude == 0 || Longitude == 0 || Latitude == 0)
                return 0;
            else if (_OldLongitude == Longitude && _OldLatitude == Latitude)
                return 0;

            var theta = _OldLongitude - Longitude;
            var distance = Math.Sin(_OldLatitude.ToRadian()) * Math.Sin(Latitude.ToRadian()) +
                           Math.Cos(_OldLatitude.ToRadian()) * Math.Cos(Latitude.ToRadian()) *
                           Math.Cos(theta.ToRadian());

            distance = Math.Acos(distance).ToDegree() * 60 * 1.1515;

            switch (unitsOfLength)
            {
                case EnumLengthUnit.Kilometer: 
                    return distance * _MilesToKilometers;
                case EnumLengthUnit.NauticalMiles: 
                    return distance * _MilesToNautical;
                default: 
                    return distance;
            }
        }

        public static double Distance(BAGPoint old, BAGPoint current)
        {
            return Distance(old.Lng, old.Lat, current.Lng, current.Lat, EnumLengthUnit.Kilometer);
        }

        /// <summary>
        /// Tinh khoang cach tuong doi giua hai diem tuong doi gan nhau so voi ban kinh trai dat
        /// </summary>
        /// <param name="a"> toa do diem dau </param>
        /// <param name="b"> toa do diem cuoi </param>
        /// <returns></returns>
        public static double DistanceFloor(double _OldLongitude, double _OldLatitude, double Longitude, double Latitude)
        {
            //double x = (b.Lng - a.Lng) * Math.Cos((a.Lat + b.Lat) / 2);
            double x = (Longitude - _OldLongitude);
            double y = (Latitude - _OldLatitude);
            return Math.Sqrt(x * x + y * y) * R;
        }

        public static double DistanceFloorSquare(double _OldLongitude, double _OldLatitude, double Longitude, double Latitude)
        {
            double x = (Longitude - _OldLongitude);
            double y = (Latitude - _OldLatitude);
            return (x * x + y * y) * R * R;
        }

        public static double DistanceFloorToMeter(double _OldLongitude, double _OldLatitude, double Longitude, double Latitude)
        {
            return DistanceFloor(_OldLongitude, _OldLatitude, Longitude, Latitude) * ConvertKmToMeter;
        }

        public static double DistanceFloor(BAGPoint old, BAGPoint current)
        {
            return DistanceFloor(old.Lng, old.Lat, current.Lng, current.Lat);
        }

        public static double DistanceFloorToMeter(BAGPoint old, BAGPoint current)
        {
            return DistanceFloor(old, current) * ConvertKmToMeter;
        }

        public static double DistanceInMeter(double _OldLongitude, double _OldLatitude, double Longitude, double Latitude)
        {
            return ConvertKmToMeter * Distance(_OldLongitude, _OldLatitude, Longitude, Latitude, EnumLengthUnit.Kilometer);
        }

        /// <summary>
        /// Tính Cosin của đường ABC
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static double Cosin(double a, double b, double c)
        {
            return (Math.Pow(b, 2) + Math.Pow(c, 2) - Math.Pow(a, 2)) / (2 * b * c);
        }

        /// <summary>
        /// check điểm trong hình chữ nhật
        /// chính là hình chữ nhật ngoại tiếp đường tròn
        /// </summary>
        /// <param name="point">điểm cần check</param>
        /// <param name="radius">bán kính</param>
        /// <param name="center">tâm đường tròn</param>
        public static bool CheckPointInRectangle(BAGPoint point, BAGPoint center, double radius)
        {
            // độ rộng vĩ độ
            double deltaLat = radius * _DeltaCoordinate;
            // độ rộng vĩ độ
            double deltaLng = deltaLat / (Math.Cos(center.Lat * Math.PI / 180));
            // check điểm có nằm trong khoảng vĩ độ
            if (point.Lat > center.Lat + deltaLat || point.Lat < center.Lat - deltaLat)
                return false;
            // check điểm có nằm trong khoảng kinh độ
            if (point.Lng > center.Lng + deltaLng || point.Lng < center.Lng - deltaLng)
                return false;
            return true;
        }

        /// <summary>
        /// Hàm kiểm tra một điểm có nằm trong một đa giác hay không
        /// theo thuật toán xét xe nằm trên hay dưới đường thẳng của đa giac
        /// </summary>
        /// <param name="point">điểm cần check</param>
        /// <param name="Polygon">list điểm của đa giác</param>
        public static bool CheckPointInsidePolygon(BAGPoint point, BAGPoint[] Polygon)
        {
            int cn = 0;
            for (int i = 0; i < Polygon.Length - 1; i++)
            {
                if (((Polygon[i].Lng <= point.Lng) && (Polygon[i + 1].Lng > point.Lng))
                 || ((Polygon[i].Lng > point.Lng) && (Polygon[i + 1].Lng <= point.Lng)))
                {
                    double vt = (point.Lng - Polygon[i].Lng) / (Polygon[i + 1].Lng - Polygon[i].Lng);
                    if (point.Lat < Polygon[i].Lat + vt * (Polygon[i + 1].Lat - Polygon[i].Lat))
                        ++cn;
                }
            }
            return ((cn & 1) == 1);
        }

        /// <summary>
        /// Xác định hướng
        /// </summary>
        public static byte Bearing(double distance, byte oldBearing, double oldLongitude, double oldLatitude, double longitude, double latitude)
        {
            //If longitude and latitude are not valid, don't change car's direction
            if (longitude == 0 | latitude == 0) return oldBearing;

            //If distance between two cars is too small, retur old Bearing
            if (distance < 0.02)
            {
                return oldBearing;
            }

            byte _Bearing = 0;
            //Calculate new direction
            double DeltaX = latitude - oldLatitude;
            double DeltaY = longitude - oldLongitude;
            double S = Math.Sqrt(Math.Pow(DeltaX, 2) + Math.Pow(DeltaY, 2));
            double G = Math.Acos(DeltaX / S);
            if (DeltaY < 0) G = 2 * Math.PI - G;
            G = Math.Round(4 * G / Math.PI);
            if (G > 7 || G < 0) G = 0;

            try
            { _Bearing = (byte)G; }
            catch
            { _Bearing = 0; }

            return _Bearing;
        }

        public static byte Bearing(double distance, byte oldBearing, BAGPoint old, BAGPoint now)
        {
            return Bearing(distance, oldBearing, old.Lng, old.Lat, now.Lng, now.Lat);
        }

        public static string PolylineAlgorithmEncode(this IEnumerable<BAGPoint> points)
        {
            var str = new StringBuilder();

            var encodeDiff = (Action<int>)(diff =>
            {
                int shifted = diff << 1;
                if (diff < 0)
                    shifted = ~shifted;

                int rem = shifted;

                while (rem >= 0x20)
                {
                    str.Append((char)((0x20 | (rem & 0x1f)) + 63));

                    rem >>= 5;
                }

                str.Append((char)(rem + 63));
            });

            int lastLat = 0;
            int lastLng = 0;

            foreach (var point in points)
            {
                int lat = (int)Math.Round(point.Lat * 1E5);
                int lng = (int)Math.Round(point.Lng * 1E5);

                encodeDiff(lat - lastLat);
                encodeDiff(lng - lastLng);

                lastLat = lat;
                lastLng = lng;
            }

            return str.ToString();
        }

        public static IEnumerable<BAGPoint> PolylineAlgorithmDecode(this string encodedPoints)
        {
            if (string.IsNullOrEmpty(encodedPoints))
                throw new ArgumentNullException("encodedPoints");

            char[] polylineChars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            while (index < polylineChars.Length)
            {
                // calculate next latitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylineChars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length)
                    break;

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                //calculate next longitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylineChars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length && next5bits >= 32) break;

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                yield return new BAGPoint(Convert.ToDouble(currentLng) / 1E5, Convert.ToDouble(currentLat) / 1E5);
            }
        }

        public static double CalAngle(BAGPoint A, BAGPoint B)
        {
            double DeltaX = B.Lat - A.Lat;
            double DeltaY = B.Lng - A.Lng;
            return Math.Atan2(DeltaY, DeltaX) * (180 / Math.PI);
        }
    }
}