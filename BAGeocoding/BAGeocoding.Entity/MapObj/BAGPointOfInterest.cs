using System.Data;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapObj
{
    public class BAGPointOfInterest : SQLDataUlt
    {
        public int PoiID { get; set; }
        public byte ProvinceID { get; set; }
        public short KindID { get; set; }
        public string KindName { get; set; }
        public string Name { get; set; }
        public short House { get; set; }
        public string Road { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Anchor { get; set; }
        public string Info { get; set; }
        public string Note { get; set; }
        public string ShortKey { get; set; }
        public BAGPoint Coords { get; set; }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                PoiID = base.GetDataValue<int>(dr, "PoiID");
                ProvinceID = base.GetDataValue<byte>(dr, "ProvinceID");
                KindID = base.GetDataValue<short>(dr, "PKDKindID");
                KindName = base.GetDataValue<string>(dr, "PKDName", string.Empty);
                Name = base.GetDataValue<string>(dr, "Name", string.Empty);
                House = base.GetDataValue<short>(dr, "House");
                Road = base.GetDataValue<string>(dr, "Road", string.Empty);
                Address = base.GetDataValue<string>(dr, "Address", string.Empty);
                Tel = base.GetDataValue<string>(dr, "Tel", string.Empty);
                Anchor = base.GetDataValue<string>(dr, "Anchor", string.Empty);
                Info = base.GetDataValue<string>(dr, "Info", string.Empty);
                Note = base.GetDataValue<string>(dr, "Note", string.Empty);
                ShortKey = base.GetDataValue<string>(dr, "ShortKey", string.Empty);

                Coords = new BAGPoint();
                if (Coords.FromDataRow(dr) == false)
                    return false;

                return true;
            }
            catch { return false; }
        }
    }
}
