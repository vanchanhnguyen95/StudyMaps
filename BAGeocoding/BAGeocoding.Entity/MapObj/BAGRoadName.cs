using System.Data;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapObj
{
    public class BAGRoadName : SQLDataUlt
    {
        public int RoadID { get; set; }
        public byte ProvinceID { get; set; }
        public string RoadName { get; set; }
        public string NameExt { get; set; }
        public string Address { get; set; }
        public BAGPoint Coords { get; set; }

        public BAGRoadName()
        {
            Coords = new BAGPoint();
        }

        public BAGRoadName(BAGRoadName other)
        {
            RoadID = other.RoadID;
            ProvinceID = other.ProvinceID;
            RoadName = other.RoadName;
            NameExt = other.NameExt;
            Address = other.Address;
            Coords = new BAGPoint(other.Coords);
        }

        public bool FromDataCreate(DataRow dr)
        {
            try
            {
                ProvinceID = base.GetDataValue<byte>(dr, "ProvinceID");
                RoadName = base.GetDataValue<string>(dr, "RoadName", string.Empty);
                NameExt = base.GetDataValue<string>(dr, "DistrictName", string.Empty);
                Coords = new BAGPoint();
                if (Coords.FromDataRow(dr) == false)
                    return false;

                return true;
            }
            catch { return false; }
        }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                RoadID = base.GetDataValue<int>(dr, "RoadID");
                ProvinceID = base.GetDataValue<byte>(dr, "ProvinceID");
                RoadName = base.GetDataValue<string>(dr, "RoadName", string.Empty);
                NameExt = base.GetDataValue<string>(dr, "NameExt", string.Empty);
                Address = base.GetDataValue<string>(dr, "Address", string.Empty);
                Coords = new BAGPoint();
                if (Coords.FromDataRow(dr) == false)
                    return false;

                return true;
            }
            catch { return false; }
        }
    }

    public class BAGRoadNameV2 : SQLDataUlt
    {
        public int RoadID { get; set; }
        public byte ProvinceID { get; set; }
        public string RoadName { get; set; }
        public string NameExt { get; set; }
        public string Address { get; set; }
        public BAGPointV2 Coords { get; set; }

        public BAGRoadNameV2()
        {
            Coords = new BAGPointV2();
        }

        public BAGRoadNameV2(BAGRoadNameV2 other)
        {
            RoadID = other.RoadID;
            ProvinceID = other.ProvinceID;
            RoadName = other.RoadName;
            NameExt = other.NameExt;
            Address = other.Address;
            Coords = new BAGPointV2(other.Coords);
        }

        public bool FromDataCreate(DataRow dr)
        {
            try
            {
                ProvinceID = base.GetDataValue<byte>(dr, "ProvinceID");
                RoadName = base.GetDataValue<string>(dr, "RoadName", string.Empty);
                NameExt = base.GetDataValue<string>(dr, "DistrictName", string.Empty);
                Coords = new BAGPointV2();
                if (Coords.FromDataRow(dr) == false)
                    return false;

                return true;
            }
            catch { return false; }
        }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                RoadID = base.GetDataValue<int>(dr, "RoadID");
                ProvinceID = base.GetDataValue<byte>(dr, "ProvinceID");
                RoadName = base.GetDataValue<string>(dr, "RoadName", string.Empty);
                NameExt = base.GetDataValue<string>(dr, "NameExt", string.Empty);
                Address = base.GetDataValue<string>(dr, "Address", string.Empty);
                Coords = new BAGPointV2();
                if (Coords.FromDataRow(dr) == false)
                    return false;

                return true;
            }
            catch { return false; }
        }
    }
}