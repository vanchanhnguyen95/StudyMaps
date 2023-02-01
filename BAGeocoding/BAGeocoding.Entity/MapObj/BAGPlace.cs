using System;
using System.Collections.Generic;
using System.Data;

using RTree.Engine.Entity;

using BAGeocoding.Entity.Enum;

using BAGeocoding.Utility;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BAGeocoding.Entity.MapObj
{
    public class BAGPlace : SQLDataUlt
    {
        public int PlaceID { get; set; }
        public byte TypeID { get; set; }
        public EnumBAGPlaceType EnumTypeID { get { return (EnumBAGPlaceType)TypeID; } set { TypeID = (byte)value; } }
        public int ParentID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public BAGPoint Center { get; set; }
        public List<BAGPoint> PointList { get; set; }
        
        public string LngStr { get; set; }
        public string LatStr { get; set; }


        public int RealID { get; set; }
        
        public BAGPlace()
        {
            Center = new BAGPoint();
            PointList = new List<BAGPoint>();
            
            LngStr = string.Empty;
            LatStr = string.Empty;
        }

        public BAGPlace(BAGPlace other)
        {
            PlaceID = other.PlaceID;
            TypeID = other.TypeID;
            ParentID = other.ParentID;
            Name = other.Name;
            Address = other.Address;
            Description = other.Description;
            Center = new BAGPoint(other.Center);
        }

        public bool FromDataRow(DataRow dr, DataTable dt)
        {
            try
            {
                PlaceID = base.GetDataValue<int>(dr, "PlaceID");
                TypeID = base.GetDataValue<byte>(dr, "TypeID");
                ParentID = base.GetDataValue<int>(dr, "ParentID");
                Name = base.GetDataValue<string>(dr, "Name", string.Empty);
                Address = base.GetDataValue<string>(dr, "Address", string.Empty);
                Description = base.GetDataValue<string>(dr, "Description", string.Empty);
                Center = new BAGPoint(base.GetDataValue<double>(dr, "Lng"), base.GetDataValue<double>(dr, "Lat"));
                
                PointList = new List<BAGPoint>();
                DataRow[] rowList = dt.Select(string.Format("PlaceID = {0}", PlaceID));
                for (int i = 0; i < rowList.Length; i++)
                {
                    BAGPoint point = new BAGPoint();
                    if (point.FromDataRow(rowList[i]) == false)
                        return false;
                    PointList.Add(point);
                }

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BAGPlace.FromDataRow, ex: {0}", ex.ToString()));
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

        public void UpdateInfo(BAGPlace other)
        {
            if (other == null)
            {
                Name = string.Empty;
                Address = string.Empty;
                Description = string.Empty;
            }
            else
            {
                Name = other.Name;
                Address = other.Address;
                Description = other.Description;
            }
        }
        
        public byte[] ToBinary()
        {
            List<byte> resultList = new List<byte>();

            // Thông chung
            resultList.AddRange(BitConverter.GetBytes(PlaceID));
            if (EnumTypeID != EnumBAGPlaceType.Urban)
                resultList.AddRange(BitConverter.GetBytes(ParentID));

            // Tên
            byte[] bffName = Constants.UnicodeCodePage.GetBytes(Name);
            resultList.Add((byte)bffName.Length);
            resultList.AddRange(bffName);

            // Địa chỉ
            if (EnumTypeID == EnumBAGPlaceType.Urban)
            {
                byte[] bffAddress = Constants.UnicodeCodePage.GetBytes(Address);
                resultList.AddRange(BitConverter.GetBytes((short)bffAddress.Length));
                resultList.AddRange(bffAddress);
            }

            if (EnumTypeID == EnumBAGPlaceType.Plot)
            {
                // Tọa độ tâm
                resultList.AddRange(BitConverter.GetBytes(Center.Lng));
                resultList.AddRange(BitConverter.GetBytes(Center.Lat));

                // Tọa độ vùng
                resultList.AddRange(BitConverter.GetBytes((short)PointList.Count));
                for (int i = 0; i < PointList.Count; i++)
                {
                    resultList.AddRange(BitConverter.GetBytes(PointList[i].Lng));
                    resultList.AddRange(BitConverter.GetBytes(PointList[i].Lat));
                }
            }

            return resultList.ToArray();
        }

        public override string ToString()
        {
            return string.Format("PlaceID: {0}, TypeID: {1}, ParentID: {2}, Name: {3}, RealID: {4}", PlaceID, TypeID, ParentID, Name, RealID);
        }
    }

    public class BAGPlaceV2 : SQLDataUlt
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int PlaceID { get; set; }
        public byte TypeID { get; set; }
        public EnumBAGPlaceType EnumTypeID { get { return (EnumBAGPlaceType)TypeID; } set { TypeID = (byte)value; } }
        public int ParentID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public BAGPoint Center { get; set; }
        public List<BAGPoint> PointList { get; set; }

        public string LngStr { get; set; }
        public string LatStr { get; set; }


        public int RealID { get; set; }

        public BAGPlaceV2()
        {
            Center = new BAGPoint();
            PointList = new List<BAGPoint>();

            LngStr = string.Empty;
            LatStr = string.Empty;
        }

        public BAGPlaceV2(BAGPlaceV2 other)
        {
            PlaceID = other.PlaceID;
            TypeID = other.TypeID;
            ParentID = other.ParentID;
            Name = other.Name;
            Address = other.Address;
            Description = other.Description;
            Center = new BAGPoint(other.Center);
        }

        public bool FromDataRow(DataRow dr, DataTable dt)
        {
            try
            {
                PlaceID = base.GetDataValue<int>(dr, "PlaceID");
                TypeID = base.GetDataValue<byte>(dr, "TypeID");
                ParentID = base.GetDataValue<int>(dr, "ParentID");
                Name = base.GetDataValue<string>(dr, "Name", string.Empty);
                Address = base.GetDataValue<string>(dr, "Address", string.Empty);
                Description = base.GetDataValue<string>(dr, "Description", string.Empty);
                Center = new BAGPoint(base.GetDataValue<double>(dr, "Lng"), base.GetDataValue<double>(dr, "Lat"));

                PointList = new List<BAGPoint>();
                DataRow[] rowList = dt.Select(string.Format("PlaceID = {0}", PlaceID));
                for (int i = 0; i < rowList.Length; i++)
                {
                    BAGPoint point = new BAGPoint();
                    if (point.FromDataRow(rowList[i]) == false)
                        return false;
                    PointList.Add(point);
                }

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BAGPlace.FromDataRow, ex: {0}", ex.ToString()));
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

        public void UpdateInfo(BAGPlace other)
        {
            if (other == null)
            {
                Name = string.Empty;
                Address = string.Empty;
                Description = string.Empty;
            }
            else
            {
                Name = other.Name;
                Address = other.Address;
                Description = other.Description;
            }
        }

        public byte[] ToBinary()
        {
            List<byte> resultList = new List<byte>();

            // Thông chung
            resultList.AddRange(BitConverter.GetBytes(PlaceID));
            if (EnumTypeID != EnumBAGPlaceType.Urban)
                resultList.AddRange(BitConverter.GetBytes(ParentID));

            // Tên
            byte[] bffName = Constants.UnicodeCodePage.GetBytes(Name);
            resultList.Add((byte)bffName.Length);
            resultList.AddRange(bffName);

            // Địa chỉ
            if (EnumTypeID == EnumBAGPlaceType.Urban)
            {
                byte[] bffAddress = Constants.UnicodeCodePage.GetBytes(Address);
                resultList.AddRange(BitConverter.GetBytes((short)bffAddress.Length));
                resultList.AddRange(bffAddress);
            }

            if (EnumTypeID == EnumBAGPlaceType.Plot)
            {
                // Tọa độ tâm
                resultList.AddRange(BitConverter.GetBytes(Center.Lng));
                resultList.AddRange(BitConverter.GetBytes(Center.Lat));

                // Tọa độ vùng
                resultList.AddRange(BitConverter.GetBytes((short)PointList.Count));
                for (int i = 0; i < PointList.Count; i++)
                {
                    resultList.AddRange(BitConverter.GetBytes(PointList[i].Lng));
                    resultList.AddRange(BitConverter.GetBytes(PointList[i].Lat));
                }
            }

            return resultList.ToArray();
        }

        public override string ToString()
        {
            return string.Format("PlaceID: {0}, TypeID: {1}, ParentID: {2}, Name: {3}, RealID: {4}", PlaceID, TypeID, ParentID, Name, RealID);
        }
    }
}