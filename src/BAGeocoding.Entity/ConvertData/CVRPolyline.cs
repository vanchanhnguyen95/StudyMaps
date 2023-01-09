using System;
using System.Collections.Generic;
using System.Data;

using System.Text.RegularExpressions;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.ConvertData
{
    public class CVRPolyline : SQLDataUlt
    {
        public int SegmentID { get; set; }
        public string VName { get; set; }
        public short StartLeft { get; set; }
        public short StartRight { get; set; }
        public short EndLeft { get; set; }
        public short EndRight { get; set; }
        public byte Visible { get; set; }
        public byte State { get; set; }
        public string Note { get; set; }
        public List<BAGPoint> PointList { get; set; }

        public CVRPolyline() { }

        public CVRPolyline(BAGSegment other)
        {
            SegmentID = other.SegmentID;
            VName = other.VName;
            StartLeft = other.StartLeft;
            StartRight = other.StartRight;
            EndLeft = other.EndLeft;
            EndRight = other.EndRight;
            Visible = 1;
            State = 1;
            Note = string.Empty;
            PointList = new List<BAGPoint>();
            for (int i = 0; i < other.PointList.Count; i++)
                PointList.Add(new BAGPoint(other.PointList[i]));
        }
        
        public bool FromDataRow(DataRow dr)
        {
            try
            {

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("CVRPolyline.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromString(EnumCVRText2MifKind kindID, string input)
        {
            string[] data;
            try
            {
                if (input.Length == 0)
                    return true;
                else if (kindID == EnumCVRText2MifKind.MapToolPOI)
                    data = Regex.Split(input, "#@#");
                else
                    return false;

                int indexID = 0;
                if (kindID == EnumCVRText2MifKind.MapToolPOI)
                {
                    SegmentID = Convert.ToInt32(data[indexID++].Trim());
                    VName = data[indexID++].Trim();
                    indexID = 8;
                    StartLeft = Convert.ToInt16(data[indexID++].Trim());
                    EndLeft = Convert.ToInt16(data[indexID++].Trim());
                    StartRight = Convert.ToInt16(data[indexID++].Trim());
                    EndRight = Convert.ToInt16(data[indexID++].Trim());
                    indexID = 33;
                    Visible = Convert.ToByte(data[indexID++].Trim());
                    State = Convert.ToByte(data[indexID++].Trim());
                    indexID = 37;
                    string[] lngStr = data[indexID++].Trim().Split('@');
                    string[] latStr = data[indexID++].Trim().Split('@');
                    PointList = new List<BAGPoint>();
                    for (int i = 0; i < lngStr.Length; i++)
                        PointList.Add(new BAGPoint(lngStr[i], latStr[i]));
                    Note = data[indexID++].Trim();
                }

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("CVRPolyline.FromString, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public override string ToString()
        {
            //return string.Format("{0},\"{1}\",{2},{3},{4},{5},{6},{7},\"{8}\"", SegmentID, VName, StartLeft, EndLeft, StartRight, EndRight, Visible, State, Note);
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", SegmentID, VName.Replace(",", "-"), StartLeft, EndLeft, StartRight, EndRight, Visible, State, Note.Replace(",", "-"));
        }
    }
}