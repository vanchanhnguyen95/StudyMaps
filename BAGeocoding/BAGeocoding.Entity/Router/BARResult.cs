using System.Collections.Generic;

using BAGeocoding.Entity.Enum.Route;

namespace BAGeocoding.Entity.Router
{
    public class BARResultS
    {
        public bool state { get; set; }
        public byte errorcode { get; set; }
        public int distance { get; set; }
        public string type { get; set; }

        public BARResultS()
        {
            type = "ba";
        }

        public BARResultS(EnumBARErrorCode errorCode)
        {
            state = false;
            errorcode = (byte)errorCode;
            type = "ba";
        }
    }

    public class BARResultSExt
    {
        public bool state { get; set; }
        public byte errorcode { get; set; }
        public List<BARResultS> results { get; set; }

        public BARResultSExt()
        {

        }

        public BARResultSExt(EnumBARErrorCode errorCode)
        {
            state = false;
            errorcode = (byte)errorCode;
        }
    }

    public class BARResultF
    {
        public bool state { get; set; }
        public byte errorcode { get; set; }
        public int distance { get; set; }
        public string type { get; set; }
        public BARResultPoint from { get; set; }
        public BARResultPoint to { get; set; }
        public List<BARResultPoint> points { get; set; }
        public List<BARResultSegment> segments { get; set; }

        public BARResultF()
        {
            points = new List<BARResultPoint>();
            segments = new List<BARResultSegment>();
            type = "ba";
        }

        public BARResultF(EnumBARErrorCode errorCode)
        {
            state = false;
            errorcode = (byte)errorCode;
            points = new List<BARResultPoint>();
            segments = new List<BARResultSegment>();
            type = "ba";
        }

        public void SAdd(BARSegment segmentInfo, BARPoint pointStart)
        {
            if (segments.Count == 0)
                segments.Add(new BARResultSegment(segmentInfo, pointStart));
            else if (segments[segments.Count - 1].name != segmentInfo.VName)
                segments.Add(new BARResultSegment(segmentInfo, pointStart));
            else
                segments[segments.Count - 1].distance += (int)segmentInfo.Length;
        }
    }

    public class BARResultFExt
    {
        public bool state { get; set; }
        public byte errorcode { get; set; }
        public List<BARResultF> results { get; set; }

        public BARResultFExt()
        {
            state = true;
            results = new List<BARResultF>();
        }

        public BARResultFExt(EnumBARErrorCode errorCode)
        {
            state = false;
            errorcode = (byte)errorCode;
            results = new List<BARResultF>();
        }
    }

    public class BARResultPoint
    {
        public double lng { get; set; }
        public double lat { get; set; }

        public BARResultPoint() { }

        public BARResultPoint(BARParamPoint other)
        {
            lng = other.lng;
            lat = other.lat;
        }

        public BARResultPoint(BARPoint other)
        {
            lng = other.Lng;
            lat = other.Lat;
        }
    }

    public class BARResultSegment
    {
        public string name { get; set; }
        public BARResultPoint start { get; set; }
        public int distance { get; set; }

        public BARResultSegment()
        {
            start = new BARResultPoint();
        }

        public BARResultSegment(BARSegment other, BARPoint point)
        {
            start = new BARResultPoint(point);
            name = other.VName;
            distance = (int)other.Length;
        }
    }
}