using System.Collections.Generic;

using BAGeocoding.Dal.MapImg;

using BAGeocoding.Entity.MapImg;

namespace BAGeocoding.Bll.MapImg
{
    public class RequestURLManager
    {
        public static List<BAGRequestURL> GetAll(bool actived = true)
        {
            return RequestURLDAO.GetAll(actived);
        }

        /// <summary>
        /// Xử lý đăng ký sử dụng bản đồ
        /// </summary>
        public static bool Create(BAGRequestURL request)
        {
            return RequestURLDAO.Create(request);
        }

        /// <summary>
        /// Hủy đăng ký sử dụng bản đồ
        /// </summary>
        public static bool Delete(BAGRequestURL request)
        {
            return RequestURLDAO.Delete(request);
        }
    }
}