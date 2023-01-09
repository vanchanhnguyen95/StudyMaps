using System;
using System.Collections.Generic;
using System.Data;

using BAGeocoding.Utility;

using RTree.Engine.Entity;

namespace BAGeocoding.Entity.MapObj
{
    public class BAGTile : SQLDataUlt
    {
        public int TileID{get;set;}
        public short CommuneID { get; set; }
        public short DistrictID { get; set; }
        public string VName { get; set; }
        public string EName { get; set; }
        public string LngStr { get; set; }
        public string LatStr { get; set; }
        public string GeoStr { get; set; }
        public List<BAGPoint> PointList { get; set; }

        public BAGTile()
        {
            LngStr = string.Empty;
            LatStr = string.Empty;
            GeoStr = string.Empty;
        }

        public bool FromDataRow(DataRow dr, DataTable dt, ref int dx)
        {
            try
            {
                TileID = base.GetDataValue<int>(dr, "TileID");
                CommuneID = base.GetDataValue<short>(dr, "CommuneID");

                PointList = new List<BAGPoint>();
                int pointCount = base.GetDataValue<int>(dr, "PointCount");
                for (int i = 0; i < pointCount; i++)
                {
                    BAGPoint point = new BAGPoint();
                    if (point.FromDataRow(dt.Rows[dx + i]) == false)
                        return false;
                    PointList.Add(point);
                }
                dx += pointCount;
                //DataRow[] rowList = dt.Select(string.Format("ObjectID = {0}", TileID));
                //for (int i = 0; i < rowList.Length; i++)
                //{
                //    BAGPoint point = new BAGPoint();
                //    if (point.FromDataRow(rowList[i]) == false)
                //        return false;
                //    PointList.Add(point);
                //}

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BAGTile.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public RTRectangle GetRectangle()
        {
            double minlng = PointList[0].Lng;
            double minlat = PointList[0].Lat;
            double maxlng = PointList[0].Lng;
            double maxlat = PointList[0].Lat;
            for (int i = 1; i < PointList.Count; i++)
            {
                minlng = Math.Min(minlng, PointList[i].Lng);
                minlat = Math.Min(minlat, PointList[i].Lat);
                maxlng = Math.Max(maxlng, PointList[i].Lng);
                maxlat = Math.Max(maxlat, PointList[i].Lat);
            }
            return new RTRectangle(minlng, minlat, maxlng, maxlat);
        }

        public byte[] ToBinary()
        {
            List<byte> resultList = new List<byte>();
            resultList.AddRange(BitConverter.GetBytes(TileID));
            resultList.AddRange(BitConverter.GetBytes(CommuneID));

            // Danh sách điểm
            resultList.AddRange(BitConverter.GetBytes((short)PointList.Count));
            for (int i = 0; i < PointList.Count; i++)
            {
                resultList.AddRange(BitConverter.GetBytes(PointList[i].Lng));
                resultList.AddRange(BitConverter.GetBytes(PointList[i].Lat));
            }

            // Trả về kết quả
            return resultList.ToArray();
        }
    }
}