using System;
using System.Data;

using BAGeocoding.Utility;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BAGeocoding.Entity.MapObj
{
    public class BAGRoadSpecial : SQLDataUlt
    {
        public int IndexID { get; set; }
        public string RoadName { get; set; }
        public string Description { get; set; }
        
        public bool FromDataRow(DataRow dr)
        {
            try
            {
                IndexID = base.GetDataValue<int>(dr, "IndexID");
                RoadName = base.GetDataValue<string>(dr, "RoadName", string.Empty);
                Description = base.GetDataValue<string>(dr, "Description", string.Empty);

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BAGRoadSpecial.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool Equal(BAGRoadSpecial other)
        {
            if (RoadName != other.RoadName)
                return false;
            else if (Description != other.Description)
                return false;
            else
                return true;
        }

        public byte[] ToBinary()
        {
            List<byte> resultList = new List<byte>();
            byte[] bffRoad = Constants.TCVN3CodePage.GetBytes(RoadName);
            resultList.Add((byte)bffRoad.Length);
            resultList.AddRange(bffRoad);
            return resultList.ToArray();
        }
    }

    public class BAGRoadSpecialV2 : SQLDataUlt
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int IndexID { get; set; }
        public string RoadName { get; set; }
        public string Description { get; set; }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                IndexID = base.GetDataValue<int>(dr, "IndexID");
                RoadName = base.GetDataValue<string>(dr, "RoadName", string.Empty);
                Description = base.GetDataValue<string>(dr, "Description", string.Empty);

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BAGRoadSpecial.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool Equal(BAGRoadSpecialV2 other)
        {
            if (RoadName != other.RoadName)
                return false;
            else if (Description != other.Description)
                return false;
            else
                return true;
        }

        public byte[] ToBinary()
        {
            List<byte> resultList = new List<byte>();
            byte[] bffRoad = Constants.TCVN3CodePage.GetBytes(RoadName);
            resultList.Add((byte)bffRoad.Length);
            resultList.AddRange(bffRoad);
            return resultList.ToArray();
        }
    }
}