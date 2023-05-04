using System;
using System.Collections.Generic;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;

namespace BAGeocoding.Bll
{
    public class MapUtilityManager
    {
        /// <summary>
        /// Kiểm tra điểm trong vùng
        /// </summary>
        public static bool CheckInsidePolygon(List<BAGPoint> pg, BAGPoint pp)
        {
            int i, j = pg.Count - 1;
            bool oddNodes = false;

            for (i = 0; i < pg.Count; i++)
            {
                if (pg[i].Lat < pp.Lat && pg[j].Lat >= pp.Lat || pg[j].Lat < pp.Lat && pg[i].Lat >= pp.Lat)
                {
                    if (pg[i].Lng + (pp.Lat - pg[i].Lat) / (pg[j].Lat - pg[i].Lat) * (pg[j].Lng - pg[i].Lng) < pp.Lng)
                        oddNodes = !oddNodes;
                }
                j = i;
            }

            return oddNodes;
        }

        public static bool CheckInsidePolygonV2(List<BAGPoint> pg, BAGPointV2 pp)
        {
            int i, j = pg.Count - 1;
            bool oddNodes = false;

            for (i = 0; i < pg.Count; i++)
            {
                if (pg[i].Lat < pp.Lat && pg[j].Lat >= pp.Lat || pg[j].Lat < pp.Lat && pg[i].Lat >= pp.Lat)
                {
                    if (pg[i].Lng + (pp.Lat - pg[i].Lat) / (pg[j].Lat - pg[i].Lat) * (pg[j].Lng - pg[i].Lng) < pp.Lng)
                        oddNodes = !oddNodes;
                }
                j = i;
            }

            return oddNodes;
        }

        /// <summary>
        /// Tính độ dài của đoạn thẳng
        /// </summary>
        public static float Magnitude(BAGPoint p1, BAGPoint p2)
        {
            return (float)Math.Sqrt(Math.Pow(p2.Lng - p1.Lng, 2) + Math.Pow(p2.Lat - p1.Lat, 2));
        }

        public static float Magnitude(BAGPointV2 p1, BAGPointV2 p2)
        {
            return (float)Math.Sqrt(Math.Pow(p2.Lng - p1.Lng, 2) + Math.Pow(p2.Lat - p1.Lat, 2));
        }

        /// <summary>
        /// Khoảng cách từ một điểm đến một đường (p3 so với đường p1-p2)
        /// </summary>
        public static BAGDistance DistancePointLine(BAGPoint p1, BAGPoint p2, BAGPoint p3)
        {
            double LineMag = Magnitude(p2, p1);

            double U = (((p3.Lng - p1.Lng) * (p2.Lng - p1.Lng)) + ((p3.Lat - p1.Lat) * (p2.Lat - p1.Lat))) / (LineMag * LineMag);

            if (U < 0.0f)
                return new BAGDistance(EnumBAGAnchor.Left, p1, Magnitude(p3, p1));
            else if (U > 1.0f)
                return new BAGDistance(EnumBAGAnchor.Right, p2, Magnitude(p3, p2));
            else
            {
                BAGPoint p0 = new BAGPoint(p1.Lng + U * (p2.Lng - p1.Lng), p1.Lat + U * (p2.Lat - p1.Lat));
                return new BAGDistance(EnumBAGAnchor.Middle, p0, Magnitude(p3, p0));
            }
        }

        /// <summary>
        /// Kiểm tra xem điểm bên trái đường (p3 so với đường p1-p2)
        /// </summary>
        public static bool IsLeft(BAGPoint p1, BAGPoint p2, BAGPoint p3)
        {
            return ((p2.Lng - p1.Lng) * (p3.Lat - p1.Lat) - (p3.Lng - p1.Lng) * (p2.Lat - p1.Lat)) > 0.0f;
        }

        public static bool IsLeft(BAGPointV2 p1, BAGPointV2 p2, BAGPointV2 p3)
        {
            return ((p2.Lng - p1.Lng) * (p3.Lat - p1.Lat) - (p3.Lng - p1.Lng) * (p2.Lat - p1.Lat)) > 0.0f;
        }

        /// <summary>
        /// Tỷ lệ của điểm chia đôi đoạn đường
        /// </summary>
        public static float PosPercent(List<BAGPoint> list, BAGPoint point, EnumBAGAnchor pos, int idx)
        {
            try
            {
                float d1 = 0.0f;
                float d2 = 0.0f;
                if (pos == EnumBAGAnchor.Right)
                    idx++;
                for (int i = 0; i < idx - 1; i++)
                    d1 += Magnitude(list[i], list[i + 1]);
                if (pos == EnumBAGAnchor.Middle)
                    d1 += Magnitude(list[idx - 1], point);
                for (int i = 0; i < list.Count - 1; i++)
                    d2 += Magnitude(list[i], list[i + 1]);

                return d1 / d2;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PosPercent, ex: " + ex.ToString());
                return 0.0f;
            }
        }

        public static float PosPercent(List<BAGPointV2> list, BAGPointV2 point, EnumBAGAnchor pos, int idx)
        {
            try
            {
                float d1 = 0.0f;
                float d2 = 0.0f;
                if (pos == EnumBAGAnchor.Right)
                    idx++;
                for (int i = 0; i < idx - 1; i++)
                    d1 += Magnitude(list[i], list[i + 1]);
                if (pos == EnumBAGAnchor.Middle)
                    d1 += Magnitude(list[idx - 1], point);
                for (int i = 0; i < list.Count - 1; i++)
                    d2 += Magnitude(list[i], list[i + 1]);

                return d1 / d2;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("PosPercent, ex: " + ex.ToString());
                return 0.0f;
            }
        }

        /// <summary>
        /// Tính toán số nhà
        /// </summary>
        public static short CalBuilding(bool serial, int start, int end, float percen)
        {
            try
            {
                // 1. Nếu cả đoạn chỉ có 1 số nhà -> Trả về luôn số nhà
                if (end == start)
                    return (short)start;

                // 2. Lấy số nhà sai lệch và hiệu chỉnh theo biên độ (Chẵn - Lẽ)
                float tmp = (end - start) * percen;
                int bld = (int)Math.Round(tmp);
                
                if (serial == false && bld % 2 != 0)
                {
                    if (bld > tmp)
                        bld -= 1;
                    else
                        bld += 1;
                }
                
                // 3. Tính toán số nhà
                if (start < end)
                {
                    if (start + bld > end)
                        return (short)end;
                    else
                        return (short)(start + bld);
                }
                else
                {
                    if (start + bld < end)
                        return (short)end;
                    else
                        return (short)(start + bld);
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteError("CalBuilding, ex: " + ex.ToString());
                return 0;
            }
        }
    }
}
