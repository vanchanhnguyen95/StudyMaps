using System;
using System.Collections.Generic;
using System.Data;

using BAGeocoding.Utility;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BAGeocoding.Entity.MapObj
{
    public class BAGPolyline : SQLDataUlt
    {
        public int PolylineID { get; set; }
        public string Name { get; set; }
        public List<BAGPoint> PointList { get; set; }
        public string LngStr { get; set; }
        public string LatStr { get; set; }

        public bool IsDone { get; set; }

        public BAGPolyline()
        {
            PointList = new List<BAGPoint>();
            Name = string.Empty;
            LngStr = string.Empty;
            LatStr = string.Empty;
        }

        public bool FromDataSimple(DataRow dr)
        {
            try
            {
                PolylineID = base.GetDataValue<int>(dr, "PolylineID");

                string[] lngStr = base.GetDataValue<string>(dr, "LngStr", string.Empty).Split('@');
                string[] latStr = base.GetDataValue<string>(dr, "LatStr", string.Empty).Split('@');

                PointList = new List<BAGPoint>();
                for (int i = 0; i < lngStr.Length; i++)
                    PointList.Add(new BAGPoint(lngStr[i], latStr[i]));

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BAGPolyline.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }
    }

    public class BAGPolylineV2 : SQLDataUlt
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int PolylineID { get; set; }
        public string Name { get; set; }
        public List<BAGPointV2> PointList { get; set; }
        public string LngStr { get; set; }
        public string LatStr { get; set; }

        public bool IsDone { get; set; }

        public BAGPolylineV2()
        {
            PointList = new List<BAGPointV2>();
            Name = string.Empty;
            LngStr = string.Empty;
            LatStr = string.Empty;
        }

        public bool FromDataSimple(DataRow dr)
        {
            try
            {
                PolylineID = base.GetDataValue<int>(dr, "PolylineID");

                string[] lngStr = base.GetDataValue<string>(dr, "LngStr", string.Empty).Split('@');
                string[] latStr = base.GetDataValue<string>(dr, "LatStr", string.Empty).Split('@');

                PointList = new List<BAGPointV2>();
                for (int i = 0; i < lngStr.Length; i++)
                    PointList.Add(new BAGPointV2(lngStr[i], latStr[i]));

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BAGPolyline.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }
    }
}