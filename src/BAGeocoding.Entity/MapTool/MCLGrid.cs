using System;
using System.Collections.Generic;
using System.Data;

using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;
using RTree.Engine.Entity;

namespace BAGeocoding.Entity.MapTool
{
    public class MCLGrid : SQLDataUlt
    {
        public int GridID { get; set; }
        public short DistrictID { get; set; }
        public string Name { get; set; }
        public string CoordsEncrypt { get; set; }
        public string CoordsOrignal { get; set; }
        public int SortOrder { get; set; }
        public List<BAGPoint> PointList { get; set; }

        public string ProvinceStr { get; set; }


        public MCLGrid()
        {
            Name = string.Empty;
            CoordsEncrypt = string.Empty;
            CoordsOrignal = string.Empty;
            PointList = new List<BAGPoint>();
            
            ProvinceStr = string.Empty;
        }

        public bool FromDataBase(DataRow dr)
        {
            try
            {
                GridID = base.GetDataValue<int>(dr, "GridID");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("MCLGrid.FromDataBase, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                GridID = base.GetDataValue<int>(dr, "GridID");
                DistrictID = base.GetDataValue<short>(dr, "DistrictID", 0);
                Name = base.GetDataValue<string>(dr, "Name", string.Empty);
                CoordsEncrypt = base.GetDataValue<string>(dr, "CoordsEncrypt", string.Empty);
                CoordsOrignal = base.GetDataValue<string>(dr, "CoordsOrignal", string.Empty);
                SortOrder = base.GetDataValue<int>(dr, "SortOrder");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("MCLGrid.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public void PointGenerate()
        {
            if (PointList.Count > 0)
                return;
            string[] coords = CoordsOrignal.Split(',');
            for (int i = 0; i < coords.Length; i++)
            {
                string[] geos = coords[i].Split(' ');
                PointList.Add(new BAGPoint(Convert.ToDouble(geos[0]), Convert.ToDouble(geos[1])));
            }
        }

        public BAGPoint Center()
        {
            BAGPoint center = new BAGPoint(0, 0);
            for (int i = 0; i < PointList.Count - 1; i++)
            {
                center.Lng += PointList[i].Lng;
                center.Lat += PointList[i].Lat;
            }
            center.Lng = center.Lng / (PointList.Count - 1);
            center.Lat = center.Lat / (PointList.Count - 1);
            return center;
        }

        public RTRectangle GetRectangle()
        {
            if (PointList.Count == 0)
            {
                string[] pointCount = CoordsOrignal.Split(',');
                for (int i = 0; i < pointCount.Length; i++)
                {
                    string[] pointData = pointCount[i].Split(' ');
                    PointList.Add(new BAGPoint(Convert.ToDouble(pointData[0]), Convert.ToDouble(pointData[1])));
                }
            }
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
    }
}