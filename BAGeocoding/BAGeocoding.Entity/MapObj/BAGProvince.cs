﻿using System;
using System.Collections.Generic;
using System.Data;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.Enum.MapObject;

using BAGeocoding.Utility;

using RTree.Engine.Entity;

namespace BAGeocoding.Entity.MapObj
{
    public class BAGProvince : SQLDataUlt
    {
        public short ProvinceID { get; set; }
        public string VName { get; set; }
        public string EName { get; set; }
        public string Abbreviate { get; set; }
        public string Description { get; set; }
        public string LngStr { get; set; }
        public string LatStr { get; set; }
        public string GeoStr { get; set; }
        public List<BAGPoint> PointList { get; set; }

        public int DataExt { get; set; }
        public int DataExtOriginal { get; set; }
        public bool IsDataSearch { get { return DataExtGet(EnumMOBProvinceDataExt.DataSearch); } set { DataExtSet(EnumMOBProvinceDataExt.DataSearch, value); } }
        public bool IsRoadByLevel { get { return DataExtGet(EnumMOBProvinceDataExt.RoadByLevel); } set { DataExtSet(EnumMOBProvinceDataExt.RoadByLevel, value); } }
        public bool IsForSTaxi { get { return DataExtGet(EnumMOBProvinceDataExt.ForSTaxi); } set { DataExtSet(EnumMOBProvinceDataExt.ForSTaxi, value); } }

        public short SortOrder { get; set; }
        public short SortOrderOriginal { get; set; }
        
        public BAGProvince()
        {
            LngStr = string.Empty;
            LatStr = string.Empty;
            GeoStr = string.Empty;
        }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                ProvinceID = base.GetDataValue<short>(dr, "ProvinceID");
                VName = base.GetDataValue<string>(dr, "VName", string.Empty);
                EName = base.GetDataValue<string>(dr, "EName", string.Empty);
                Abbreviate = base.GetDataValue<string>(dr, "Abbreviate", string.Empty);
                Description = base.GetDataValue<string>(dr, "Description", string.Empty);
                PointList = new List<BAGPoint>();

                DataExt = DataExtOriginal = base.GetDataValue<int>(dr, "DataExt");
                SortOrder = SortOrderOriginal = base.GetDataValue<short>(dr, "SortOrder");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BAGProvince.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataRow(DataRow dr, DataTable dt)
        {
            try
            {
                ProvinceID = base.GetDataValue<short>(dr, "ProvinceID");
                VName = base.GetDataValue<string>(dr, "VName", string.Empty);
                EName = base.GetDataValue<string>(dr, "EName", string.Empty);

                PointList = new List<BAGPoint>();
                DataRow[] rowList = dt.Select(string.Format("ObjectID = {0}", ProvinceID));
                for (int i = 0; i < rowList.Length; i++)
                {
                    BAGPoint point = new BAGPoint();
                    if (point.FromDataRow(rowList[i]) == false)
                        return false;
                    PointList.Add(point);
                }

                DataExt = DataExtOriginal = base.GetDataValue<int>(dr, "DataExt");
                SortOrder = SortOrderOriginal = base.GetDataValue<short>(dr, "SortOrder");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BAGProvince.FromDataRow, ex: {0}", ex.ToString()));
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


        public bool IsUpdate()
        {
            if (DataExt != DataExtOriginal)
                return true;
            else if (SortOrder != SortOrderOriginal)
                return true;
            else
                return false;
        }

        public bool DataExtGet(EnumMOBProvinceDataExt dataExt)
        {
            return ((DataExt & (int)Math.Pow(2, (int)dataExt)) > 0);
        }

        public void DataExtSet(EnumMOBProvinceDataExt dataExt, bool status)
        {
            // Bít đã được bật
            if (((DataExt >> (int)dataExt) & 1) > 0)
            {
                if (status == false)
                    DataExt = DataExt - (int)Math.Pow(2, (int)dataExt);
            }
            // Bít chưa bật
            else
            {
                if (status == true)
                    DataExt = DataExt + (int)Math.Pow(2, (int)dataExt);
            }
        }



        public byte[] ToBinary()
        {
            List<byte> resultList = new List<byte>();
            resultList.AddRange(BitConverter.GetBytes(ProvinceID));
            // Tên tỉnh thành
            byte[] bffVName = Constants.UnicodeCodePage.GetBytes(VName);
            resultList.Add((byte)bffVName.Length);
            resultList.AddRange(bffVName);
            resultList.AddRange(BitConverter.GetBytes(DataExt));
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

    public class BAGProvinceV2 : SQLDataUlt
    {
        public short ProvinceID { get; set; }
        public string VName { get; set; }
        public string EName { get; set; }
        public string Abbreviate { get; set; }
        public string Description { get; set; }
        public string LngStr { get; set; }
        public string LatStr { get; set; }
        public string GeoStr { get; set; }
        public List<BAGPointV2> PointList { get; set; }

        public int DataExt { get; set; }
        public int DataExtOriginal { get; set; }
        public bool IsDataSearch { get { return DataExtGet(EnumMOBProvinceDataExt.DataSearch); } set { DataExtSet(EnumMOBProvinceDataExt.DataSearch, value); } }
        public bool IsRoadByLevel { get { return DataExtGet(EnumMOBProvinceDataExt.RoadByLevel); } set { DataExtSet(EnumMOBProvinceDataExt.RoadByLevel, value); } }
        public bool IsForSTaxi { get { return DataExtGet(EnumMOBProvinceDataExt.ForSTaxi); } set { DataExtSet(EnumMOBProvinceDataExt.ForSTaxi, value); } }

        public short SortOrder { get; set; }
        public short SortOrderOriginal { get; set; }

        public BAGProvinceV2()
        {
            LngStr = string.Empty;
            LatStr = string.Empty;
            GeoStr = string.Empty;
        }

        public bool FromDataRow(DataRow dr)
        {
            try
            {
                ProvinceID = base.GetDataValue<short>(dr, "ProvinceID");
                VName = base.GetDataValue<string>(dr, "VName", string.Empty);
                EName = base.GetDataValue<string>(dr, "EName", string.Empty);
                Abbreviate = base.GetDataValue<string>(dr, "Abbreviate", string.Empty);
                Description = base.GetDataValue<string>(dr, "Description", string.Empty);
                PointList = new List<BAGPointV2>();

                DataExt = DataExtOriginal = base.GetDataValue<int>(dr, "DataExt");
                SortOrder = SortOrderOriginal = base.GetDataValue<short>(dr, "SortOrder");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BAGProvince.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataRow(DataRow dr, DataTable dt)
        {
            try
            {
                ProvinceID = base.GetDataValue<short>(dr, "ProvinceID");
                VName = base.GetDataValue<string>(dr, "VName", string.Empty);
                EName = base.GetDataValue<string>(dr, "EName", string.Empty);

                PointList = new List<BAGPointV2>();
                DataRow[] rowList = dt.Select(string.Format("ObjectID = {0}", ProvinceID));
                for (int i = 0; i < rowList.Length; i++)
                {
                    BAGPointV2 point = new BAGPointV2();
                    if (point.FromDataRow(rowList[i]) == false)
                        return false;
                    PointList.Add(point);
                }

                DataExt = DataExtOriginal = base.GetDataValue<int>(dr, "DataExt");
                SortOrder = SortOrderOriginal = base.GetDataValue<short>(dr, "SortOrder");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("BAGProvince.FromDataRow, ex: {0}", ex.ToString()));
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


        public bool IsUpdate()
        {
            if (DataExt != DataExtOriginal)
                return true;
            else if (SortOrder != SortOrderOriginal)
                return true;
            else
                return false;
        }

        public bool DataExtGet(EnumMOBProvinceDataExt dataExt)
        {
            return ((DataExt & (int)Math.Pow(2, (int)dataExt)) > 0);
        }

        public void DataExtSet(EnumMOBProvinceDataExt dataExt, bool status)
        {
            // Bít đã được bật
            if (((DataExt >> (int)dataExt) & 1) > 0)
            {
                if (status == false)
                    DataExt = DataExt - (int)Math.Pow(2, (int)dataExt);
            }
            // Bít chưa bật
            else
            {
                if (status == true)
                    DataExt = DataExt + (int)Math.Pow(2, (int)dataExt);
            }
        }



        public byte[] ToBinary()
        {
            List<byte> resultList = new List<byte>();
            resultList.AddRange(BitConverter.GetBytes(ProvinceID));
            // Tên tỉnh thành
            byte[] bffVName = Constants.UnicodeCodePage.GetBytes(VName);
            resultList.Add((byte)bffVName.Length);
            resultList.AddRange(bffVName);
            resultList.AddRange(BitConverter.GetBytes(DataExt));
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