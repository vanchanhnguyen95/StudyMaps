using ElasticProject.Data.Entity.MapObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticProject.Bll.MapObj
{
    public class BGCRoadNameManager
    {
        /// <summary>
        /// Lấy danh sách điểm theo tỉnh
        /// </summary>
        public static List<BGCRoadName> GetByProvince(int provinceID)
        {
            //return BGCRoadNameDAO.GetByProvince(provinceID);
            return new List<BGCRoadName>();
        }

        /// <summary>
        /// Chuyển đổi sang dữ liệu cập nhật ElasticSearch
        /// </summary>
        //public static List<BGCElasticRequestCreate> Conver2Elastic(List<BGCRoadName> source)
        //{
        //    if (source == null)
        //        return new List<BGCElasticRequestCreate>();

        //    List<BGCElasticRequestCreate> result = new List<BGCElasticRequestCreate>();
        //    for (int i = 0; i < source.Count; i++)
        //        result.Add(new BGCElasticRequestCreate(source[i]));
        //    return result;
        //}
    }
}
