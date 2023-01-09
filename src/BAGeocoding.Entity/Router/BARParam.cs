using System;
using System.Collections.Generic;

namespace BAGeocoding.Entity.Router
{
    public class BARParamS
    {
        public BARParamPoint from { get; set; }
        public BARParamPoint to { get; set; }
        public int opts { get; set; }
        public int back { get; set; }
        public byte gg { get; set; }
        public string key { get; set; }

        public bool IsValid()
        {
            if (from.IsValid() == false)
                return false;
            else if (to.IsValid() == false)
                return false;
            else if (back < 0)
                return false;
            else if (key == null)
                return false;
            else if (key.Length == 0)
                return false;
            else
                return true;
        }

        public BARParamS() 
        {
            from = new BARParamPoint();
            to = new BARParamPoint();
            key = string.Empty;
        }

        public BARParamS(BARParamSExt ext, int idx)
        {
            from = new BARParamPoint(ext.froms[idx]);
            to = new BARParamPoint(ext.to);
            opts = ext.opts;
            back = ext.back;
            gg = ext.gg;
            key = ext.key;

            Adjust();
        }

        public void Adjust()
        {
            if (opts == 0)
                return;
            // 1. Nếu yêu cầu tìm đường bỏ qua cao tốc
            if ((opts & 1) > 0)
            {
                // .1 Điểm đầu phải bỏ qua cao tốc
                if ((from.typ & 1) == 0)
                    from.typ = (byte)(from.typ + 1);
                // .2 Điểm đến phải bỏ qua cao tốc
                if ((to.typ & 1) == 0)
                    to.typ = (byte)(to.typ + 1);
            }
            // 2. Nếu yêu cầu tìm đường bỏ qua phà/đò
            if ((opts & 2) > 0)
            {
                // .1 Điểm đầu phải bỏ qua phà/đò
                if ((from.typ & 2) == 0)
                    from.typ = (byte)(from.typ + 2);
                // .2 Điểm đến phải bỏ qua phà/đò
                if ((to.typ & 2) == 0)
                    to.typ = (byte)(to.typ + 2);
            }
        }
    }

    public class BARParamSExt
    {
        public List<BARParamPoint> froms { get; set; }
        public BARParamPoint to { get; set; }
        public int opts { get; set; }
        public int back { get; set; }
        public byte gg { get; set; }
        public string key { get; set; }

        public bool IsValid()
        {
            if (froms == null)
                return false;
            else if (froms.Count == 0)
                return false;
            for (int i = 0; i < froms.Count; i++)
            {
                if (froms[i].IsValid() == false)
                    return false;
            }
            if (to.IsValid() == false)
                return false;
            else if (back < 0)
                return false;
            else if (key == null)
                return false;
            else if (key.Length == 0)
                return false;
            else
                return true;
        }
    }

    public class BARParamPoint
    {
        public byte typ { get; set; }
        public double lng { get; set; }
        public double lat { get; set; }

        public BARParamPoint() { }

        public BARParamPoint(BARParamPoint other)
        {
            typ = other.typ;
            lng = other.lng;
            lat = other.lat;
        }

        public BARParamPoint(string other)
        {
            string[] data = other.Split(',');
            typ = Convert.ToByte(data[0]);
            lng = Convert.ToDouble(data[1]);
            lat = Convert.ToDouble(data[2]);
        }

        public bool IsValid()
        {
            if (lng == 0)
                return false;
            else if (lat == 0)
                return false;
            else
                return true;
        }
    }
}