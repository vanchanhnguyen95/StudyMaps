using System;
using System.Collections.Generic;
using System.Data;

using BAGeocoding.Utility;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RTree.Engine.Entity;

namespace BAGeocoding.Entity.MapObj
{
    public class BAGCommune : SQLDataUlt
    {
        public short CommuneID { get; set; }
        public short DistrictID { get; set; }
        public string VName { get; set; }
        public string EName { get; set; }
        public string Description { get; set; }
        public string LngStr { get; set; }
        public string LatStr { get; set; }
        public string GeoStr { get; set; }
        public List<BAGPoint> PointList { get; set; }


        public byte ProvinceID { get; set; }

        public BAGCommune()
        {
            LngStr = string.Empty;
            LatStr = string.Empty;
            GeoStr = string.Empty;
        }

        public bool FromDataManager(DataRow dr)
        {
            try
            {
                CommuneID = base.GetDataValue<short>(dr, "CommuneID");
                DistrictID = base.GetDataValue<short>(dr, "DistrictID");
                VName = base.GetDataValue<string>(dr, "VName", string.Empty);
                EName = base.GetDataValue<string>(dr, "EName", string.Empty);
                Description = base.GetDataValue<string>(dr, "Description", string.Empty);
                PointList = new List<BAGPoint>();

                ProvinceID = base.GetDataValue<byte>(dr, "ProvinceID");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BAGCommune.FromDataManager, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataRow(DataRow dr, DataTable dt)
        {
            try
            {
                CommuneID = base.GetDataValue<short>(dr, "CommuneID");
                DistrictID = base.GetDataValue<short>(dr, "DistrictID");
                VName = base.GetDataValue<string>(dr, "VName", string.Empty);
                EName = base.GetDataValue<string>(dr, "EName", string.Empty);
                Description = base.GetDataValue<string>(dr, "Description", string.Empty);

                PointList = new List<BAGPoint>();
                DataRow[] rowList = dt.Select(string.Format("ObjectID = {0}", CommuneID));
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
                LogFile.WriteError(string.Format("BAGCommune.FromDataRow, ex: {0}", ex.ToString()));
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

        public byte[] ToBinary()
        {
            List<byte> resultList = new List<byte>();
            resultList.AddRange(BitConverter.GetBytes(CommuneID));

            // Tên xã/phường
            byte[] bffVName = Constants.UnicodeCodePage.GetBytes(VName);
            resultList.Add((byte)bffVName.Length);
            resultList.AddRange(bffVName);

            // Thông tin quận/huyện
            resultList.AddRange(BitConverter.GetBytes(DistrictID));

            // Danh sách điểm
            resultList.AddRange(BitConverter.GetBytes((short)PointList.Count));
            for (int i = 0; i < PointList.Count; i++)
            {
                resultList.AddRange(BitConverter.GetBytes(PointList[i].Lng));
                resultList.AddRange(BitConverter.GetBytes(PointList[i].Lat));
            }

            // Trả về kết quả
            return resultList.ToArray();
        }
    }

    public class BAGCommuneV2 : SQLDataUlt
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public short CommuneID { get; set; }
        public short DistrictID { get; set; }
        public string VName { get; set; }
        public string EName { get; set; }
        public string Description { get; set; }
        public string LngStr { get; set; }
        public string LatStr { get; set; }
        public string GeoStr { get; set; }
        public List<BAGPointV2> PointList { get; set; }


        public byte ProvinceID { get; set; }

        public BAGCommuneV2()
        {
            LngStr = string.Empty;
            LatStr = string.Empty;
            GeoStr = string.Empty;
        }

        public bool FromDataManager(DataRow dr)
        {
            try
            {
                CommuneID = base.GetDataValue<short>(dr, "CommuneID");
                DistrictID = base.GetDataValue<short>(dr, "DistrictID");
                VName = base.GetDataValue<string>(dr, "VName", string.Empty);
                EName = base.GetDataValue<string>(dr, "EName", string.Empty);
                Description = base.GetDataValue<string>(dr, "Description", string.Empty);
                PointList = new List<BAGPointV2>();

                ProvinceID = base.GetDataValue<byte>(dr, "ProvinceID");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BAGCommune.FromDataManager, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataRow(DataRow dr, DataTable dt)
        {
            try
            {
                CommuneID = base.GetDataValue<short>(dr, "CommuneID");
                DistrictID = base.GetDataValue<short>(dr, "DistrictID");
                VName = base.GetDataValue<string>(dr, "VName", string.Empty);
                EName = base.GetDataValue<string>(dr, "EName", string.Empty);
                Description = base.GetDataValue<string>(dr, "Description", string.Empty);

                PointList = new List<BAGPointV2>();
                DataRow[] rowList = dt.Select(string.Format("ObjectID = {0}", CommuneID));
                for (int i = 0; i < rowList.Length; i++)
                {
                    BAGPointV2 point = new BAGPointV2();
                    if (point.FromDataRow(rowList[i]) == false)
                        return false;
                    PointList.Add(point);
                }

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BAGCommune.FromDataRow, ex: {0}", ex.ToString()));
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

        public byte[] ToBinary()
        {
            List<byte> resultList = new List<byte>();
            resultList.AddRange(BitConverter.GetBytes(CommuneID));

            // Tên xã/phường
            byte[] bffVName = Constants.UnicodeCodePage.GetBytes(VName);
            resultList.Add((byte)bffVName.Length);
            resultList.AddRange(bffVName);

            // Thông tin quận/huyện
            resultList.AddRange(BitConverter.GetBytes(DistrictID));

            // Danh sách điểm
            resultList.AddRange(BitConverter.GetBytes((short)PointList.Count));
            for (int i = 0; i < PointList.Count; i++)
            {
                resultList.AddRange(BitConverter.GetBytes(PointList[i].Lng));
                resultList.AddRange(BitConverter.GetBytes(PointList[i].Lat));
            }

            // Trả về kết quả
            return resultList.ToArray();
        }
    }

}