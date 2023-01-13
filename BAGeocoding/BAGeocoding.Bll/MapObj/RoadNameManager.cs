using System;
using System.Collections.Generic;

//using BAGeocoding.Dal.MapObj;

using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;
using BAGeocoding.Entity.RestfulApi.ElasticSearch;

namespace BAGeocoding.Bll.MapObj
{
    public class RoadNameManager
    {
        //public static bool ProcessCreate(BAGProvince provinceInfo)
        //{
        //    try
        //    {
        //        RoadNameDAO.Clear(provinceInfo.ProvinceID);
        //        List<BAGRoadName> roadList = RoadNameDAO.GetForCreate(provinceInfo.ProvinceID);
        //        if (roadList == null || roadList.Count == 0)
        //            return false;

        //        int indexID = 0;
        //        string roadName = string.Empty;
        //        List<BAGRoadName> tempList = new List<BAGRoadName>();
        //        while (indexID < roadList.Count)
        //        {
        //            if (roadList[indexID].RoadName.ToUpper() != roadName)
        //            {
        //                ProcessCreate(tempList);
        //                tempList = new List<BAGRoadName>();
        //                tempList.Add(new BAGRoadName(roadList[indexID]));
        //                roadName = roadList[indexID].RoadName.ToUpper();
        //            }
        //            else
        //                tempList.Add(new BAGRoadName(roadList[indexID]));

        //            indexID += 1;
        //        }

        //        if (tempList.Count > 0)
        //            ProcessCreate(tempList);

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogFile.WriteError("RoadNameManager.ProcessCreate, ex: " + ex.ToString());
        //        return false;
        //    }
        //}

        //private static void ProcessCreate(List<BAGRoadName> roadList)
        //{
        //    try
        //    {
        //        if (roadList.Count == 0)
        //            return;
        //        else if (roadList.Count == 1)
        //            roadList[0].NameExt = string.Empty;
        //        else if (roadList.Count > 1)
        //            RoadNameDAO.Add(new BAGRoadName(roadList[0]) { NameExt = string.Empty });

        //        for (int i = 0; i < roadList.Count; i++)
        //            RoadNameDAO.Add(roadList[i]);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogFile.WriteError("RoadNameManager.ProcessCreate, ex: " + ex.ToString());
        //    }
        //}

        //public static List<BAGRoadName> GetByProvince(BAGProvince provinceInfo)
        //{
        //    return RoadNameDAO.GetByProvince(provinceInfo.ProvinceID);
        //}

        public static List<BAGElasticRequestItem> Conver2Elastic(List<BAGRoadName> source)
        {
            if (source == null)
                return new List<BAGElasticRequestItem>();

            List<BAGElasticRequestItem> result = new List<BAGElasticRequestItem>();
            for (int i = 0; i < source.Count; i++)
                result.Add(new BAGElasticRequestItem(source[i]));
            return result;
        }
    }
}
