using System;
using System.Data;

using BAGeocoding.Utility;
using BAGeocoding.Entity.Enum;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BAGeocoding.Entity.SearchData
{
    public class BAGSegmentKey : SQLDataUlt
    {
        public short ProvinceID { get; set; }
        public int KeyID { get; set; }
        public string KeyStr { get; set; }
        public int SegmentID { get; set; }
        public int Count { get; set; }

        public byte IndexID { get; set; }
        public float Rate { get; set; }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                ProvinceID = base.GetDataValue<short>(dr, "ProvinceID");
                KeyID = base.GetDataValue<int>(dr, "KeyID");
                KeyStr = base.GetDataValue<string>(dr, "KeyStr", string.Empty);
                SegmentID = base.GetDataValue<int>(dr, "SegmentID");
                Count = base.GetDataValue<int>(dr, "Count");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BAGSegmentKey.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }
    }

    public class BAGSegmentKeyV2 : SQLDataUlt
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public short ProvinceID { get; set; }
        public int KeyID { get; set; }
        public string KeyStr { get; set; }
        public int SegmentID { get; set; }
        public int Count { get; set; }

        public byte IndexID { get; set; }
        public float Rate { get; set; }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                ProvinceID = base.GetDataValue<short>(dr, "ProvinceID");
                KeyID = base.GetDataValue<int>(dr, "KeyID");
                KeyStr = base.GetDataValue<string>(dr, "KeyStr", string.Empty);
                SegmentID = base.GetDataValue<int>(dr, "SegmentID");
                Count = base.GetDataValue<int>(dr, "Count");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BAGSegmentKey.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }
    }
}