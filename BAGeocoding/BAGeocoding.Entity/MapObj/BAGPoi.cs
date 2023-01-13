using System;
using System.Data;

using RTree.Engine.Entity;

namespace BAGeocoding.Entity.MapObj
{
    public class BAGPoi
    {
        public int PoiID { get; set; }
        public string VName { get; set; }
        public string EName { get; set; }
        public BAGPoint Coords { get; set; }
        public string VInfo { get; set; }
        public string EInfo { get; set; }

        public bool FromDataRow(DataRow drPOI)
        {
            try
            {
                PoiID = Convert.ToInt32(drPOI["ID"]);
                VName = drPOI["Name"].ToString();
                Coords = new BAGPoint();
                if (Coords.FromDataRow(drPOI) == false)
                    return false;
                VInfo = drPOI["Info"].ToString();

                return true;
            }
            catch { return false; }
        }

        public RTRectangle GetRectangle()
        {
            return new RTRectangle(Coords.Lng, Coords.Lat, Coords.Lng, Coords.Lat);
        }
    }
}
