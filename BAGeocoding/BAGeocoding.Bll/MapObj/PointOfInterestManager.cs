using System.Collections.Generic;

//using BAGeocoding.Dal.MapObj;

using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.RestfulApi.ElasticSearch;

namespace BAGeocoding.Bll.MapObj
{
    public class PointOfInterestManager
    {
        //public static List<BAGPointOfInterest> GetByProvince(BAGProvince provinceInfor)
        //{
        //    return PointOfInterestDAO.GetByProvince(provinceInfor.ProvinceID);
        //}

        public static List<BAGElasticRequestItem> Conver2Elastic(List<BAGPointOfInterest> source)
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
