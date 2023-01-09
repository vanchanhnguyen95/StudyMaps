using System;
using System.Collections.Generic;
using System.Data;

using RTree.Engine.Entity;

using BAGeocoding.Entity.Enum;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapObj
{
    public class BAGGridView : SQLDataUlt
    {
        public int TemplateID { get; set; }
        public int GridID { get; set; }
        public string Name { get; set; }
        public short ProvinceID { get; set; }
        public string LngStr { get; set; }
        public string LatStr { get; set; }
        public List<BAGPoint> PointList { get; set; }

        public BAGGridView()
        {           
            LngStr = string.Empty;
            LatStr = string.Empty;
            PointList = new List<BAGPoint>();
        }

        public bool FromDataRow(DataRow dr, DataTable dt)
        {
            try
            {
                GridID = base.GetDataValue<int>(dr, "GridID");
                ProvinceID = base.GetDataValue<short>(dr, "ProvinceID");
                Name = base.GetDataValue<string>(dr, "Name", string.Empty);
                
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BAGSegment.FromDataRow, ex: {0}", ex.ToString()));
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
    }
}