using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BAGeocoding.Entity.DataService;
using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.Public;
using BAGeocoding.Entity.Router;
using BAGeocoding.Entity.Utility;

using BAGeocoding.Utility;

namespace BAGeocoding.Bll
{
    public class BAGDecoding
    {
        /// <summary>
        /// Tìm tỉnh thành
        /// </summary>
        public static short SearchProvinceByName(string keyStr)
        {
            try
            {
                if (keyStr.Length < 3)
                    return -1;
                Hashtable htProvinceID = new Hashtable();
                List<string> keyList = DataUtl.ProcessKeyList(keyStr);
                for (int i = 0; i < keyList.Count; i++)
                {
                    // 1. Không có từ khóa thì bỏ qua (AnhPT: Có thể trả về rỗng luôn)
                    if (RunningParams.ProvinceData.Keys.ContainsKey(keyList[i]) == false)
                        continue;
                    // 2. Lấy dữ liệu đánh chỉ mục
                    UTLSearchKey provinceKey = (UTLSearchKey)RunningParams.ProvinceData.Keys[keyList[i]];
                    // 2.1 Nếu chưa có thì thêm vào
                    if (htProvinceID.Count == 0)
                    {
                        foreach (object key in provinceKey.ObjectID.Keys)
                            htProvinceID.Add(key, null);
                    }
                    // 2.2 Nếu đã tồn tại thì lấy cái chung
                    else
                    {
                        Hashtable tempKey = (Hashtable)htProvinceID.Clone();
                        htProvinceID.Clear();
                        foreach (object key in tempKey.Keys)
                        {
                            if (provinceKey.ObjectID.ContainsKey(key) == true)
                                htProvinceID.Add(key, null);
                        }
                        if (htProvinceID.Count == 0)
                            return -1;
                    }
                }

                // Kiểm tra trả về kết quả
                if (htProvinceID.Count == 0)
                    return -1;
                else
                    return htProvinceID.Keys.Cast<short>().ToList()[0];
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGDecoding.SearchProvinceByName, ex: " + ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Tìm quận/huyện theo tỉnh thành
        /// </summary>
        public static short SearchDistrictByName(string keyStr, byte provinceID)
        {
            try
            {
                if (keyStr.Length < 3)
                    return -1;
                Hashtable htDistrictID = new Hashtable();
                List<string> keyList = DataUtl.ProcessKeyList(keyStr);

                for (int i = 0; i < keyList.Count; i++)
                {
                    // 1. Không có từ khóa thì bỏ qua (AnhPT: Có thể trả về rỗng luôn)
                    if (RunningParams.DistrictData.Keys.ContainsKey(keyList[i]) == false)
                        continue;
                    // 2. Lấy dữ liệu đánh chỉ mục
                    UTLSearchKey districtKey = (UTLSearchKey)RunningParams.DistrictData.Keys[keyList[i]];
                    // 2.1 Nếu chưa có thì thêm vào
                    if (htDistrictID.Count == 0)
                    {
                        foreach (object key in districtKey.ObjectID.Keys)
                        {
                            BAGKeyRate keyRate = (BAGKeyRate)districtKey.ObjectID[key];
                            if (keyRate.ReferenceID == provinceID)
                                htDistrictID.Add(key, null);
                        }
                    }
                    // 2.2 Nếu đã tồn tại thì lấy cái chung
                    else
                    {
                        Hashtable tempKey = (Hashtable)htDistrictID.Clone();
                        htDistrictID.Clear();
                        foreach (object key in tempKey.Keys)
                        {
                            if (districtKey.ObjectID.ContainsKey(key) == true)
                                htDistrictID.Add(key, null);
                        }
                        if (htDistrictID.Count == 0)
                            return -1;
                    }
                }

                // Kiểm tra trả về kết quả
                if (htDistrictID.Count == 0)
                    return -1;
                else
                    return htDistrictID.Keys.Cast<short>().ToList()[0];
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGDecoding.SearchDistrictByName, ex: " + ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Tìm đường theo từ khóa
        /// </summary>
        public static RPBLAddressResult SearchRoadByName(DTSSegment seg, BAGSearchKey key, short dis)
        {
            try
            {
                List<string> keyList = DataUtl.ProcessKeyList(key.Road);
                List<BAGKeyRate> keyRateList = SearchSegmentByName(seg, keyList, dis);
                if (keyRateList == null || keyRateList.Count == 0)
                    return null;

                #region ==================== Ưu tiên tên giống nhau (tỉ lệ cao) ====================
                List<int> similarList = new List<int>();
                RPBLAddressResult result = null;
                for (int i = 0; i < keyRateList.Count; i++)
                {
                    if (seg.Objs.ContainsKey(keyRateList[i].ObjectID) == false)
                        continue;
                    // Chỉ xét tỉ lệ 97% trở lên
                    else if (keyRateList[i].Percent < 97)
                        continue;
                    // Nếu tỉ lệ là 100% thì phải kiểm tra đúng chuỗi (Nguyen Dinh Thi <> Nguyen Thi Dinh)
                    else if (keyRateList[i].Percent > 99)
                    {
                        // Kiểm tra cả tên (Tránh từ khóa đảo lộn)
                        BAGSegment segment = (BAGSegment)seg.Objs[keyRateList[i].ObjectID];
                        if (segment.EName.ToLower().Equals(key.Road) == false)
                            continue;
                        similarList.Add(keyRateList[i].ObjectID);
                        // Kiểm tra nếu tiếng Việt => giống cả dấu (Hàng Đậu <> Hàng Dầu)
                        if (key.IsSpecial == true && key.Original.IndexOf(segment.VName.ToLower()) < 0)
                            continue;
                    }
                    if (SearchRoadByNameBuildResult((BAGSegment)seg.Objs[keyRateList[i].ObjectID], key, false, ref result) == true)
                        break;
                }
                // Trường hợp tiếng việt không giống => Cố gắng đưa ra từ gần giống (Hàng Dau => Hàng Đậu OR Hàng Dầu)
                if (result == null && similarList.Count > 0)
                {
                    for (int i = 0; i < similarList.Count; i++)
                    {
                        if (SearchRoadByNameBuildResult((BAGSegment)seg.Objs[similarList[i]], key, true, ref result) == true)
                            break;
                    }
                }
                if (result != null)
                    return result;
                #endregion

                #region ==================== Tìm kết quả gần giống ====================
                return null;
                //for (int i = 0; i < keyRateList.Count; i++)
                //{
                //    if (seg.Objs.ContainsKey(keyRateList[i].ObjectID) == false)
                //        continue;
                //    // Chỉ xét tỉ lệ 33% trở lên
                //    else if (keyRateList[i].Percent < 33)
                //        continue;
                //    else if (SearchRoadByNameBuildResult((BAGSegment)seg.Objs[keyRateList[i].ObjectID], key, ref result) == true)
                //        break;
                //}
                //return result;
                #endregion
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGDecoding.SearchRoadByName, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Tìm đường theo từ khóa
        /// </summary>
        public static RPBLAddressResultV2 SearchRoadByNameV2(DTSSegment seg, BAGSearchKey key, short dis)
        {
            try
            {
                List<string> keyList = DataUtl.ProcessKeyList(key.Road);
                List<BAGKeyRate> keyRateList = SearchSegmentByName(seg, keyList, dis);
                if (keyRateList == null || keyRateList.Count == 0)
                    return null;

                #region ==================== Ưu tiên tên giống nhau (tỉ lệ cao) ====================
                List<int> similarList = new List<int>();
                RPBLAddressResultV2 result = null;
                for (int i = 0; i < keyRateList.Count; i++)
                {
                    if (seg.Objs.ContainsKey(keyRateList[i].ObjectID) == false)
                        continue;
                    // Chỉ xét tỉ lệ 97% trở lên
                    else if (keyRateList[i].Percent < 97)
                        continue;
                    // Nếu tỉ lệ là 100% thì phải kiểm tra đúng chuỗi (Nguyen Dinh Thi <> Nguyen Thi Dinh)
                    else if (keyRateList[i].Percent > 99)
                    {
                        // Kiểm tra cả tên (Tránh từ khóa đảo lộn)
                        BAGSegment segment = (BAGSegment)seg.Objs[keyRateList[i].ObjectID];
                        if (segment.EName.ToLower().Equals(key.Road) == false)
                            continue;
                        similarList.Add(keyRateList[i].ObjectID);
                        // Kiểm tra nếu tiếng Việt => giống cả dấu (Hàng Đậu <> Hàng Dầu)
                        if (key.IsSpecial == true && key.Original.IndexOf(segment.VName.ToLower()) < 0)
                            continue;
                    }
                    if (SearchRoadByNameBuildResultV2((BAGSegment)seg.Objs[keyRateList[i].ObjectID], key, false, ref result) == true)
                        break;
                }
                // Trường hợp tiếng việt không giống => Cố gắng đưa ra từ gần giống (Hàng Dau => Hàng Đậu OR Hàng Dầu)
                if (result == null && similarList.Count > 0)
                {
                    for (int i = 0; i < similarList.Count; i++)
                    {
                        if (SearchRoadByNameBuildResultV2((BAGSegment)seg.Objs[similarList[i]], key, true, ref result) == true)
                            break;
                    }
                }
                if (result != null)
                    return result;
                #endregion

                #region ==================== Tìm kết quả gần giống ====================
                return null;
                //for (int i = 0; i < keyRateList.Count; i++)
                //{
                //    if (seg.Objs.ContainsKey(keyRateList[i].ObjectID) == false)
                //        continue;
                //    // Chỉ xét tỉ lệ 33% trở lên
                //    else if (keyRateList[i].Percent < 33)
                //        continue;
                //    else if (SearchRoadByNameBuildResult((BAGSegment)seg.Objs[keyRateList[i].ObjectID], key, ref result) == true)
                //        break;
                //}
                //return result;
                #endregion
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGDecoding.SearchRoadByName, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách đường tìm kiếm theo từ khóa
        /// </summary>
        public static List<RPBLAddressResultV2> SearchListRoadByName(DTSSegment seg, BAGSearchKey key, short dis)
        {
            try
            {
                List<string> keyList = DataUtl.ProcessKeyList(key.Road);
                List<BAGKeyRate> keyRateList = SearchSegmentByName(seg, keyList, dis);
                if (keyRateList == null || keyRateList.Count == 0)
                    return null;

                #region ==================== Ưu tiên tên giống nhau (tỉ lệ cao) ====================
                List<int> similarList = new List<int>();
                RPBLAddressResultV2 result = null;
                List<RPBLAddressResultV2> listResult = new List<RPBLAddressResultV2>();
                for (int i = 0; i < keyRateList.Count; i++)
                {
                    if (seg.Objs.ContainsKey(keyRateList[i].ObjectID) == false)
                        continue;
                    // Chỉ xét tỉ lệ 97% trở lên
                    else if (keyRateList[i].Percent < 97)
                        continue;
                    // Nếu tỉ lệ là 100% thì phải kiểm tra đúng chuỗi (Nguyen Dinh Thi <> Nguyen Thi Dinh)
                    else if (keyRateList[i].Percent > 99)
                    {
                        // Kiểm tra cả tên (Tránh từ khóa đảo lộn)
                        BAGSegment segment = (BAGSegment)seg.Objs[keyRateList[i].ObjectID];
                        if (segment.EName.ToLower().Equals(key.Road) == false)
                            continue;
                        similarList.Add(keyRateList[i].ObjectID);
                        // Kiểm tra nếu tiếng Việt => giống cả dấu (Hàng Đậu <> Hàng Dầu)
                        if (key.IsSpecial == true && key.Original.IndexOf(segment.VName.ToLower()) < 0)
                            continue;
                    }
                    if (SearchRoadByNameBuildResultV2((BAGSegment)seg.Objs[keyRateList[i].ObjectID], key, false, ref result) == true)
                        listResult.Add(result);

                    //if (i > limit)
                    //    break;
                }
                // Trường hợp tiếng việt không giống => Cố gắng đưa ra từ gần giống (Hàng Dau => Hàng Đậu OR Hàng Dầu)
                //if (result == null && similarList.Count > 0)
                //{
                //    for (int i = 0; i < similarList.Count; i++)
                //    {
                //        if (SearchRoadByNameBuildResultV2((BAGSegment)seg.Objs[similarList[i]], key, true, ref result) == true)
                //            break;
                //    }
                //}
                if (listResult != null)
                    return listResult;
                return null;
                #endregion
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGDecoding.SearchRoadByName, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Tìm đường theo từ khóa (Hà Nội)
        /// </summary>
        public static RPBLAddressResult SearchRoadByNameHaNoi(DTSSegment seg, BAGSearchKey key, short dis, bool all = false)
        {
            try
            {
                List<string> keyList = DataUtl.ProcessKeyList(key.Road);
                List<BAGKeyRate> keyRateList = SearchSegmentByNameHaNoi(seg, keyList, dis, all);
                if (keyRateList == null || keyRateList.Count == 0)
                    return null;

                #region ==================== Ưu tiên tên giống nhau (tỉ lệ cao) ====================
                List<int> similarList = new List<int>();
                RPBLAddressResult result = null;
                for (int i = 0; i < keyRateList.Count; i++)
                {
                    //if (seg.Objs.ContainsKey(keyRateList[i].ObjectID) == false)
                    //    continue;
                    //// Chỉ xét tỉ lệ 97% trở lên
                    //else if (keyRateList[i].Percent < 97)
                    //    continue;
                    //// Nếu tỉ lệ là 100% thì phải kiểm tra đúng chuỗi (Nguyen Dinh Thi <> Nguyen Thi Dinh)
                    //else if (keyRateList[i].Percent > 99)
                    //{
                    //    // Kiểm tra cả tên (Tránh từ khóa đảo lộn)
                    //    BAGSegment segment = (BAGSegment)seg.Objs[keyRateList[i].ObjectID];
                    //    if (segment.EName.ToLower().Equals(key.Road) == false)
                    //        continue;
                    //    similarList.Add(keyRateList[i].ObjectID);
                    //    // Kiểm tra nếu tiếng Việt => giống cả dấu (Hàng Đậu <> Hàng Dầu)
                    //    if (key.IsSpecial == true && key.Original.IndexOf(segment.VName.ToLower()) < 0)
                    //        continue;
                    //}
                    // Kiểm tra cả tên (Tránh từ khóa đảo lộn)
                    BAGSegment segment = (BAGSegment)seg.Objs[keyRateList[i].ObjectID];
                    if (segment.EName.ToLower().Equals(key.Road) == false)
                        continue;
                    similarList.Add(keyRateList[i].ObjectID);
                    // Kiểm tra nếu tiếng Việt => giống cả dấu (Hàng Đậu <> Hàng Dầu)
                    if (key.IsSpecial == true && key.Original.IndexOf(segment.VName.ToLower()) < 0)
                        continue;

                    if (SearchRoadByNameBuildResult((BAGSegment)seg.Objs[keyRateList[i].ObjectID], key, false, ref result) == true)
                        break;
                }
                // Trường hợp tiếng việt không giống => Cố gắng đưa ra từ gần giống (Hàng Dau => Hàng Đậu OR Hàng Dầu)
                if ((result == null || result.Road == null || result.Road.Length == 0) && similarList.Count > 0)
                {
                    for (int i = 0; i < similarList.Count; i++)
                    {
                        if (SearchRoadByNameBuildResult((BAGSegment)seg.Objs[similarList[i]], key, true, ref result) == true)
                            break;
                    }
                }
                if (result != null)
                    return result;
                #endregion

                #region ==================== Tìm kết quả gần giống ====================
                return null;
                //for (int i = 0; i < keyRateList.Count; i++)
                //{
                //    if (seg.Objs.ContainsKey(keyRateList[i].ObjectID) == false)
                //        continue;
                //    // Chỉ xét tỉ lệ 33% trở lên
                //    else if (keyRateList[i].Percent < 33)
                //        continue;
                //    else if (SearchRoadByNameBuildResult((BAGSegment)seg.Objs[keyRateList[i].ObjectID], key, ref result) == true)
                //        break;
                //}
                //return result;
                #endregion
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGDecoding.SearchRoadByNameHaNoi, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Tìm đường theo từ khóa (Hà Nội) V2
        /// </summary>
        public static RPBLAddressResultV2 SearchRoadByNameHaNoiV2(DTSSegment seg, BAGSearchKey key, short dis, bool all = false)
        {
            try
            {
                List<string> keyList = DataUtl.ProcessKeyList(key.Road);
                List<BAGKeyRate> keyRateList = SearchSegmentByNameHaNoi(seg, keyList, dis, all);
                if (keyRateList == null || keyRateList.Count == 0)
                    return null;

                #region ==================== Ưu tiên tên giống nhau (tỉ lệ cao) ====================
                List<int> similarList = new List<int>();
                RPBLAddressResultV2 result = null;
                for (int i = 0; i < keyRateList.Count; i++)
                {
                    if (seg.Objs.ContainsKey(keyRateList[i].ObjectID) == false)
                        continue;
                    // Chỉ xét tỉ lệ 97% trở lên
                    else if (keyRateList[i].Percent < 97)
                        continue;
                    // Nếu tỉ lệ là 100% thì phải kiểm tra đúng chuỗi (Nguyen Dinh Thi <> Nguyen Thi Dinh)
                    else if (keyRateList[i].Percent > 99)
                    {
                        // Kiểm tra cả tên (Tránh từ khóa đảo lộn)
                        BAGSegment segment = (BAGSegment)seg.Objs[keyRateList[i].ObjectID];
                        if (segment.EName.ToLower().Equals(key.Road) == false)
                            continue;
                        similarList.Add(keyRateList[i].ObjectID);
                        // Kiểm tra nếu tiếng Việt => giống cả dấu (Hàng Đậu <> Hàng Dầu)
                        if (key.IsSpecial == true && key.Original.IndexOf(segment.VName.ToLower()) < 0)
                            continue;
                    }
                    if (SearchRoadByNameBuildResultV2((BAGSegment)seg.Objs[keyRateList[i].ObjectID], key, false, ref result) == true)
                        break;
                }
                // Trường hợp tiếng việt không giống => Cố gắng đưa ra từ gần giống (Hàng Dau => Hàng Đậu OR Hàng Dầu)
                if ((result == null || result.Road == null || result.Road.Length == 0) && similarList.Count > 0)
                {
                    for (int i = 0; i < similarList.Count; i++)
                    {
                        if (SearchRoadByNameBuildResultV2((BAGSegment)seg.Objs[similarList[i]], key, true, ref result) == true)
                            break;
                    }
                }
                if (result != null)
                    return result;
                #endregion

                #region ==================== Tìm kết quả gần giống ====================
                return null;
                //for (int i = 0; i < keyRateList.Count; i++)
                //{
                //    if (seg.Objs.ContainsKey(keyRateList[i].ObjectID) == false)
                //        continue;
                //    // Chỉ xét tỉ lệ 33% trở lên
                //    else if (keyRateList[i].Percent < 33)
                //        continue;
                //    else if (SearchRoadByNameBuildResult((BAGSegment)seg.Objs[keyRateList[i].ObjectID], key, ref result) == true)
                //        break;
                //}
                //return result;
                #endregion
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGDecoding.SearchRoadByNameHaNoi, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Tìm đường theo từ khóa (Hà Nội) V2
        /// </summary>
        public static List<RPBLAddressResultV2> SearchListRoadByNameHaNoi(DTSSegment seg, BAGSearchKey key, short dis, bool all = false)
        {
            try
            {
                List<string> keyList = DataUtl.ProcessKeyList(key.Road);
                List<BAGKeyRate> keyRateList = SearchSegmentByNameHaNoi(seg, keyList, dis, all);
                if (keyRateList == null || keyRateList.Count == 0)
                    return null;

                #region ==================== Ưu tiên tên giống nhau (tỉ lệ cao) ====================
                List<int> similarList = new List<int>();
                RPBLAddressResultV2 result = null;
                List<RPBLAddressResultV2> lstResult = new List<RPBLAddressResultV2>();
                for (int i = 0; i < keyRateList.Count; i++)
                {
                    if (seg.Objs.ContainsKey(keyRateList[i].ObjectID) == false)
                        continue;
                    // Chỉ xét tỉ lệ 97% trở lên
                    else if (keyRateList[i].Percent < 97)
                        continue;
                    // Nếu tỉ lệ là 100% thì phải kiểm tra đúng chuỗi (Nguyen Dinh Thi <> Nguyen Thi Dinh)
                    else if (keyRateList[i].Percent > 99)
                    {
                        // Kiểm tra cả tên (Tránh từ khóa đảo lộn)
                        BAGSegment segment = (BAGSegment)seg.Objs[keyRateList[i].ObjectID];
                        if (segment.EName.ToLower().Equals(key.Road) == false)
                            continue;
                        similarList.Add(keyRateList[i].ObjectID);
                        // Kiểm tra nếu tiếng Việt => giống cả dấu (Hàng Đậu <> Hàng Dầu)
                        if (key.IsSpecial == true && key.Original.IndexOf(segment.VName.ToLower()) < 0)
                            continue;
                    }
                    if (SearchRoadByNameBuildResultV2((BAGSegment)seg.Objs[keyRateList[i].ObjectID], key, false, ref result) == true)
                        lstResult.Add(result);
                }
                // Trường hợp tiếng việt không giống => Cố gắng đưa ra từ gần giống (Hàng Dau => Hàng Đậu OR Hàng Dầu)
                //if ((result == null || result.Road == null || result.Road.Length == 0) && similarList.Count > 0)
                //{
                //    for (int i = 0; i < similarList.Count; i++)
                //    {
                //        if (SearchRoadByNameBuildResultV2((BAGSegment)seg.Objs[similarList[i]], key, true, ref result) == true)
                //            break;
                //    }
                //}
                if (lstResult != null || lstResult.Any())
                    return lstResult;

                return null;
                #endregion

            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGDecoding.SearchRoadByNameHaNoi, ex: " + ex.ToString());
                return null;
            }
        }



        /// <summary>
        /// Tìm danh sách đoạn đường theo từ khóa
        /// </summary>
        private static List<BAGKeyRate> SearchSegmentByName(DTSSegment segList, List<string> keyList, short districtID)
        {
            try
            {
                // 1. Tiến hành tìm kiếm
                Hashtable resultHT = new Hashtable();
                for (int i = 0; i < keyList.Count; i++)
                {
                    // 1.1 Từ khóa có dữ liệu
                    if (segList.Keys.ContainsKey(keyList[i]) == true)
                    {
                        UTLSearchKey segmentKey = (UTLSearchKey)segList.Keys[keyList[i]];
                        // 1.1.1 Từ khóa đầu tiên (Rất quan trọng)
                        if (resultHT.Count == 0)
                        {
                            foreach (object keyTmp in segmentKey.ObjectID.Keys)
                            {
                                BAGKeyRate keyRate = new BAGKeyRate((BAGKeyRate)segmentKey.ObjectID[keyTmp]);
                                // Tìm tất cả hoặc trùng quận/huyện
                                if (districtID == -1 || keyRate.ReferenceID == districtID)
                                    resultHT.Add(keyTmp, keyRate);
                            }
                        }
                        // 1.1.2 Các từ khóa tiếp theo
                        else
                        {
                            // 1.1.2.1 Copy lại dữ liệu hiện tại và khởi tạo lại kết quả
                            Hashtable originalKey = (Hashtable)resultHT.Clone();
                            resultHT.Clear();
                            // 1.1.2.2 Duyệt danh sách đối tượng của kết quả cũ
                            foreach (object keyKey in originalKey.Keys)
                            {
                                // Kiểm tra trong kết quả của từ khóa đang xét
                                if (segmentKey.ObjectID.ContainsKey(keyKey) == false)
                                    continue;
                                // Cập nhật tỉ lệ (AnhPT: Cần mở rộng xét thêm thứ tự từ khóa trong chuỗi tìm kiếm và trong thông tin của đối tượng)
                                BAGKeyRate keyRate = new BAGKeyRate((BAGKeyRate)originalKey[keyKey]);
                                keyRate.UpdateRate((BAGKeyRate)segmentKey.ObjectID[keyKey]);
                                // Thêm vào kết quả
                                resultHT.Add(keyKey, keyRate);
                            }
                            // 1.1.2.3 Nếu không còn kết quả nào thì thoát luôn
                            if (resultHT.Count == 0)
                                return null;
                        }
                    }
                    // 1.2 Không có dữ liệu + là số thì bỏ qua (AnhPT: Kiểm tra lại điều kiện này)
                    else if (DataUtl.IsNumberic(keyList[i]))
                        return null;
                }

                // 2. Sắp xếp theo tỉ lệ
                List<BAGKeyRate> keyRateList = new List<BAGKeyRate>();
                foreach (object key in resultHT.Keys)
                {
                    BAGKeyRate keyRate = (BAGKeyRate)resultHT[key];
                    keyRate.ObjectID = Convert.ToInt32(key);
                    keyRateList.Add(keyRate);
                }

                // 3. Trả về kết quả
                return keyRateList.OrderByDescending(item => item.Percent).ToList();
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGDecoding.SearchSegmentByName, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Tìm danh sách đoạn đường theo từ khóa (Hà Nội)
        /// </summary>
        private static List<BAGKeyRate> SearchSegmentByNameHaNoi(DTSSegment segList, List<string> keyList, short districtID, bool alldistrict)
        {
            try
            {
                // 1. Tiến hành tìm kiếm
                Hashtable resultHT = new Hashtable();
                for (int i = 0; i < keyList.Count; i++)
                {
                    // 1.1 Từ khóa có dữ liệu
                    if (segList.Keys.ContainsKey(keyList[i]) == true)
                    {
                        UTLSearchKey segmentKey = (UTLSearchKey)segList.Keys[keyList[i]];
                        // 1.1.1 Từ khóa đầu tiên (Rất quan trọng)
                        if (resultHT.Count == 0)
                        {
                            foreach (object keyTmp in segmentKey.ObjectID.Keys)
                            {
                                BAGKeyRate keyRate = new BAGKeyRate((BAGKeyRate)segmentKey.ObjectID[keyTmp]);
                                // Trùng quận/huyện
                                if (keyRate.ReferenceID == districtID)
                                    resultHT.Add(keyTmp, keyRate);
                                // Nếu không chọn quận huyện + (Tất cả OR Không thuộc Hà tây cũ)
                                else if (districtID == -1 && (RunningParams.DistrictPriority.ContainsKey(keyRate.ReferenceID) == alldistrict))
                                    resultHT.Add(keyTmp, keyRate);
                            }
                        }
                        // 1.1.2 Các từ khóa tiếp theo
                        else
                        {
                            // 1.1.2.1 Copy lại dữ liệu hiện tại và khởi tạo lại kết quả
                            Hashtable originalKey = (Hashtable)resultHT.Clone();
                            resultHT.Clear();
                            // 1.1.2.2 Duyệt danh sách đối tượng của kết quả cũ
                            foreach (object keyKey in originalKey.Keys)
                            {
                                // Kiểm tra trong kết quả của từ khóa đang xét
                                if (segmentKey.ObjectID.ContainsKey(keyKey) == false)
                                    continue;
                                // Cập nhật tỉ lệ (AnhPT: Cần mở rộng xét thêm thứ tự từ khóa trong chuỗi tìm kiếm và trong thông tin của đối tượng)
                                BAGKeyRate keyRate = new BAGKeyRate((BAGKeyRate)originalKey[keyKey]);
                                keyRate.UpdateRate((BAGKeyRate)segmentKey.ObjectID[keyKey]);
                                // Thêm vào kết quả
                                resultHT.Add(keyKey, keyRate);
                            }
                            // 1.1.2.3 Nếu không còn kết quả nào thì thoát luôn
                            if (resultHT.Count == 0)
                                return null;
                        }
                    }
                    // 1.2 Không có dữ liệu + là số thì bỏ qua (AnhPT: Kiểm tra lại điều kiện này)
                    else if (DataUtl.IsNumberic(keyList[i]))
                        return null;
                }

                // 2. Sắp xếp theo tỉ lệ
                List<BAGKeyRate> keyRateList = new List<BAGKeyRate>();
                foreach (object key in resultHT.Keys)
                {
                    BAGKeyRate keyRate = (BAGKeyRate)resultHT[key];
                    keyRate.ObjectID = Convert.ToInt32(key);
                    keyRateList.Add(keyRate);
                }

                // 3. Trả về kết quả
                return keyRateList.OrderByDescending(item => item.Percent).ToList();
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGDecoding.SearchSegmentByNameHaNoi, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Kiểm tra theo số nhà để xác định đoạn đường chính xác
        /// </summary>
        private static bool SearchRoadByNameBuildResult(BAGSegment segmentInfo, BAGSearchKey searchKey, bool ignoreSide, ref RPBLAddressResult result)
        {
            // 1. Khởi tạo kết quả
            int indexPos = 0;
            bool dataFlag = true;
            short deltaTemp = 0;
            short deltaLeft = 0;
            short deltaRight = 0;
            short building = 0;
            short buildingLeft = 0;
            short buildingRight = 0;
            float deltaLength = 0;
            if (result == null)
                result = new RPBLAddressResult();

            // 2. Kiểm tra số nhà
            if (DataUtl.CheckBuilding(segmentInfo.IsSerial, searchKey.Building, segmentInfo.StartLeft, segmentInfo.EndLeft) == true)
            {
                #region ================ Kiểm tra số nhà bên trái ================
                result.Accurate = (searchKey.Building > 0);
                result.Building = searchKey.Building;
                result.Road = segmentInfo.VName;
                result.MinSpeed = segmentInfo.MinSpeed;
                result.MaxSpeed = segmentInfo.MaxSpeed;
                result.DataExt = segmentInfo.DataExt;
                if (searchKey.Building == 0)
                {
                    result.Lng = (float)segmentInfo.PointList[0].Lng;
                    result.Lat = (float)segmentInfo.PointList[0].Lat;
                }
                else if (searchKey.Building == segmentInfo.StartLeft)
                {
                    result.Lng = (float)segmentInfo.PointList[0].Lng;
                    result.Lat = (float)segmentInfo.PointList[0].Lat;
                }
                else if (searchKey.Building == segmentInfo.EndLeft)
                {
                    result.Lng = (float)segmentInfo.PointList[segmentInfo.PointList.Count - 1].Lng;
                    result.Lat = (float)segmentInfo.PointList[segmentInfo.PointList.Count - 1].Lat;
                }
                else
                {
                    if (segmentInfo.StartLeft == segmentInfo.EndLeft)
                        deltaLength = 0;
                    else if (segmentInfo.StartLeft < segmentInfo.EndLeft)
                        deltaLength = segmentInfo.SegLength * (searchKey.Building - segmentInfo.StartLeft) / (segmentInfo.EndLeft - segmentInfo.StartLeft);
                    else
                        deltaLength = segmentInfo.SegLength * (segmentInfo.StartLeft - searchKey.Building) / (segmentInfo.StartLeft - segmentInfo.EndLeft);
                    for (int i = 1; i < segmentInfo.PointList.Count; i++)
                    {
                        if (segmentInfo.PointList[i].D2Start > deltaLength)
                        {
                            double percent = (deltaLength - segmentInfo.PointList[i - 1].D2Start) / (segmentInfo.PointList[i].D2Start - segmentInfo.PointList[i - 1].D2Start);
                            result.Lng = (float)(segmentInfo.PointList[i - 1].Lng + (segmentInfo.PointList[i].Lng - segmentInfo.PointList[i - 1].Lng) * percent);
                            result.Lat = (float)(segmentInfo.PointList[i - 1].Lat + (segmentInfo.PointList[i].Lat - segmentInfo.PointList[i - 1].Lat) * percent);
                            break;
                        }
                    }
                }
                return true;
                #endregion
            }
            // 2.2 Kiểm tra số nhà bên phải
            else if (DataUtl.CheckBuilding(segmentInfo.IsSerial, searchKey.Building, segmentInfo.StartRight, segmentInfo.EndRight) == true)
            {
                #region ================ Kiểm tra số nhà bên phải ================
                result.Accurate = (searchKey.Building > 0);
                result.Building = searchKey.Building;
                result.Road = segmentInfo.VName;
                result.MinSpeed = segmentInfo.MinSpeed;
                result.MaxSpeed = segmentInfo.MaxSpeed;
                result.DataExt = segmentInfo.DataExt;
                if (searchKey.Building == 0)
                {
                    result.Lng = (float)segmentInfo.PointList[0].Lng;
                    result.Lat = (float)segmentInfo.PointList[0].Lat;
                }
                else if (searchKey.Building == segmentInfo.StartRight)
                {
                    result.Lng = (float)segmentInfo.PointList[0].Lng;
                    result.Lat = (float)segmentInfo.PointList[0].Lat;
                }
                else if (searchKey.Building == segmentInfo.EndRight)
                {
                    result.Lng = (float)segmentInfo.PointList[segmentInfo.PointList.Count - 1].Lng;
                    result.Lat = (float)segmentInfo.PointList[segmentInfo.PointList.Count - 1].Lat;
                }
                else
                {
                    if (segmentInfo.StartRight == segmentInfo.EndRight)
                        deltaLength = 0;
                    else if (segmentInfo.StartRight < segmentInfo.EndRight)
                        deltaLength = segmentInfo.SegLength * (searchKey.Building - segmentInfo.StartRight) / (segmentInfo.EndRight - segmentInfo.StartRight);
                    else
                        deltaLength = segmentInfo.SegLength * (segmentInfo.StartRight - searchKey.Building) / (segmentInfo.StartRight - segmentInfo.EndRight);
                    for (int i = 1; i < segmentInfo.PointList.Count; i++)
                    {
                        if (segmentInfo.PointList[i].D2Start > deltaLength)
                        {
                            double percent = (deltaLength - segmentInfo.PointList[i - 1].D2Start) / (segmentInfo.PointList[i].D2Start - segmentInfo.PointList[i - 1].D2Start);
                            result.Lng = (float)(segmentInfo.PointList[i - 1].Lng + (segmentInfo.PointList[i].Lng - segmentInfo.PointList[i - 1].Lng) * percent);
                            result.Lat = (float)(segmentInfo.PointList[i - 1].Lat + (segmentInfo.PointList[i].Lat - segmentInfo.PointList[i - 1].Lat) * percent);
                            break;
                        }
                    }
                }
                return true;
                #endregion
            }
            // 2.3 Trường hợp kết quả đầu tiên
            else if (ignoreSide == true || segmentInfo.IsSerial == true || (searchKey.Building % 2 == segmentInfo.StartLeft % 2) || (searchKey.Building % 2 == segmentInfo.StartRight % 2) || (segmentInfo.StartLeft == 0 && segmentInfo.StartRight == 0))
            {
                #region ================ Kiểm tra số nhà cùng bên ================
                if (result.Road == null || result.Road.Length == 0)
                {
                    // 2.3.1 Lấy số nhà và vị trí điểm
                    // 2.3.1.1 Đường số nhà liên tiếp
                    if (segmentInfo.IsSerial == true)
                    {
                        buildingLeft = DataUtl.DetechBuilding(searchKey.Building, segmentInfo.StartLeft, segmentInfo.EndLeft, segmentInfo.PointList.Count, ref deltaLeft, ref indexPos);
                        buildingRight = DataUtl.DetechBuilding(searchKey.Building, segmentInfo.StartRight, segmentInfo.EndRight, segmentInfo.PointList.Count, ref deltaRight, ref indexPos);
                        if (deltaLeft < deltaRight)
                            result.Building = buildingLeft;
                        else
                            result.Building = buildingRight;
                    }
                    // 2.3.1.2 Số nhà bên trái
                    else if (searchKey.Building % 2 == segmentInfo.StartLeft % 2)
                        result.Building = DataUtl.DetechBuilding(searchKey.Building, segmentInfo.StartLeft, segmentInfo.EndLeft, segmentInfo.PointList.Count, ref deltaTemp, ref indexPos);
                    // 2.3.1.2 Số nhà bên phải
                    else
                        result.Building = DataUtl.DetechBuilding(searchKey.Building, segmentInfo.StartRight, segmentInfo.EndRight, segmentInfo.PointList.Count, ref deltaTemp, ref indexPos);

                    // 2.3.2 Ghi nhận lại kết quả
                    result.Road = segmentInfo.VName;
                    result.MinSpeed = segmentInfo.MinSpeed;
                    result.MaxSpeed = segmentInfo.MaxSpeed;
                    result.DataExt = segmentInfo.DataExt;
                    result.Lng = (float)segmentInfo.PointList[indexPos].Lng;
                    result.Lat = (float)segmentInfo.PointList[indexPos].Lat;
                }
                // 2.4 Các kết quả sau
                else if (segmentInfo.StartLeft > 0 || segmentInfo.StartRight > 0)
                {
                    // 2.4.1 Lấy số nhà và vị trí điểm

                    // 2.4.1.1 Đường số nhà liên tiếp
                    if (segmentInfo.IsSerial == true)
                    {
                        buildingLeft = DataUtl.DetechBuilding(searchKey.Building, segmentInfo.StartLeft, segmentInfo.EndLeft, segmentInfo.PointList.Count, ref deltaLeft, ref indexPos);
                        buildingRight = DataUtl.DetechBuilding(searchKey.Building, segmentInfo.StartRight, segmentInfo.EndRight, segmentInfo.PointList.Count, ref deltaRight, ref indexPos);
                        if (segmentInfo.StartLeft > 0 && deltaLeft < deltaRight)
                            result.Building = buildingLeft;
                        else if (segmentInfo.StartRight > 0 && deltaRight < deltaLeft)
                            result.Building = buildingRight;
                        else
                            dataFlag = false;
                    }
                    // 2.4.1.2 Số nhà bên trái
                    else if (segmentInfo.StartLeft > 0 && searchKey.Building % 2 == segmentInfo.StartLeft % 2)
                        building = DataUtl.DetechBuilding(searchKey.Building, segmentInfo.StartLeft, segmentInfo.EndLeft, segmentInfo.PointList.Count, ref deltaTemp, ref indexPos);
                    // 2.4.1.2 Số nhà bên phải
                    else if (segmentInfo.StartRight > 0 && searchKey.Building % 2 == segmentInfo.StartRight % 2)
                        building = DataUtl.DetechBuilding(searchKey.Building, segmentInfo.StartRight, segmentInfo.EndRight, segmentInfo.PointList.Count, ref deltaTemp, ref indexPos);
                    else
                        dataFlag = false;

                    if (dataFlag == true)
                    {
                        // 2.4.2 Xác định xem kết quả mới có tốt hơn không
                        short min = Math.Min(result.Building, building);
                        short max = Math.Max(result.Building, building);
                        // 2.4.2.1 Nếu số nhà cần tìm bé hơn 2 kết quả (cũ + mới) => Nếu kết quả mới lớn hơn cũ thì bỏ qua
                        if (searchKey.Building < min && building > result.Building)
                            indexPos = -1;
                        // 2.4.2.1 Nếu số nhà cần tìm lớn hơn 2 kết quả (cũ + mới) => Nếu kết quả mới bé hơn cũ thì bỏ qua
                        else if (searchKey.Building > max && building < result.Building)
                            indexPos = -1;
                        // 2.4.2.1 Nếu số nhà cần tìm nằm giữa 2 kết quả (cũ + mới) => Cũ gần hơn thì bỏ qua
                        else if (searchKey.Building > min && searchKey.Building < max && Math.Abs(searchKey.Building - building) > Math.Abs(result.Building - searchKey.Building))
                            indexPos = -1;

                        // 2.4.3 Kiểm tra cập nhật lại kết quả
                        if (indexPos > -1)
                        {
                            result.Building = building;
                            result.Lng = (float)segmentInfo.PointList[indexPos].Lng;
                            result.Lat = (float)segmentInfo.PointList[indexPos].Lat;
                        }
                    }
                }
                #endregion
            }

            return false;
        }

        /// <summary>
        /// Kiểm tra theo số nhà để xác định đoạn đường chính xác V2
        /// </summary>
        private static bool SearchRoadByNameBuildResultV2(BAGSegment segmentInfo, BAGSearchKey searchKey, bool ignoreSide, ref RPBLAddressResultV2 result)
        {
            // 1. Khởi tạo kết quả
            int indexPos = 0;
            bool dataFlag = true;
            short deltaTemp = 0;
            short deltaLeft = 0;
            short deltaRight = 0;
            short building = 0;
            short buildingLeft = 0;
            short buildingRight = 0;
            float deltaLength = 0;
            if (result == null)
                result = new RPBLAddressResultV2();

            // 2. Kiểm tra số nhà
            if (DataUtl.CheckBuilding(segmentInfo.IsSerial, searchKey.Building, segmentInfo.StartLeft, segmentInfo.EndLeft) == true)
            {
                #region ================ Kiểm tra số nhà bên trái ================
                result.Accurate = (searchKey.Building > 0);
                result.Building = searchKey.Building;
                result.Road = segmentInfo.VName;
                result.MinSpeed = segmentInfo.MinSpeed;
                result.MaxSpeed = segmentInfo.MaxSpeed;
                result.DataExt = segmentInfo.DataExt;
                if (searchKey.Building == 0)
                {
                    result.Lng = segmentInfo.PointList[0].Lng;
                    result.Lat = segmentInfo.PointList[0].Lat;
                }
                else if (searchKey.Building == segmentInfo.StartLeft)
                {
                    result.Lng = segmentInfo.PointList[0].Lng;
                    result.Lat = segmentInfo.PointList[0].Lat;
                }
                else if (searchKey.Building == segmentInfo.EndLeft)
                {
                    result.Lng = segmentInfo.PointList[segmentInfo.PointList.Count - 1].Lng;
                    result.Lat = segmentInfo.PointList[segmentInfo.PointList.Count - 1].Lat;
                }
                else
                {
                    if (segmentInfo.StartLeft == segmentInfo.EndLeft)
                        deltaLength = 0;
                    else if (segmentInfo.StartLeft < segmentInfo.EndLeft)
                        deltaLength = segmentInfo.SegLength * (searchKey.Building - segmentInfo.StartLeft) / (segmentInfo.EndLeft - segmentInfo.StartLeft);
                    else
                        deltaLength = segmentInfo.SegLength * (segmentInfo.StartLeft - searchKey.Building) / (segmentInfo.StartLeft - segmentInfo.EndLeft);
                    for (int i = 1; i < segmentInfo.PointList.Count; i++)
                    {
                        if (segmentInfo.PointList[i].D2Start > deltaLength)
                        {
                            double percent = (deltaLength - segmentInfo.PointList[i - 1].D2Start) / (segmentInfo.PointList[i].D2Start - segmentInfo.PointList[i - 1].D2Start);
                            result.Lng = (segmentInfo.PointList[i - 1].Lng + (segmentInfo.PointList[i].Lng - segmentInfo.PointList[i - 1].Lng) * percent);
                            result.Lat = (segmentInfo.PointList[i - 1].Lat + (segmentInfo.PointList[i].Lat - segmentInfo.PointList[i - 1].Lat) * percent);
                            break;
                        }
                    }
                }
                return true;
                #endregion
            }
            // 2.2 Kiểm tra số nhà bên phải
            else if (DataUtl.CheckBuilding(segmentInfo.IsSerial, searchKey.Building, segmentInfo.StartRight, segmentInfo.EndRight) == true)
            {
                #region ================ Kiểm tra số nhà bên phải ================
                result.Accurate = (searchKey.Building > 0);
                result.Building = searchKey.Building;
                result.Road = segmentInfo.VName;
                result.MinSpeed = segmentInfo.MinSpeed;
                result.MaxSpeed = segmentInfo.MaxSpeed;
                result.DataExt = segmentInfo.DataExt;
                if (searchKey.Building == 0)
                {
                    result.Lng = segmentInfo.PointList[0].Lng;
                    result.Lat = segmentInfo.PointList[0].Lat;
                }
                else if (searchKey.Building == segmentInfo.StartRight)
                {
                    result.Lng = segmentInfo.PointList[0].Lng;
                    result.Lat = segmentInfo.PointList[0].Lat;
                }
                else if (searchKey.Building == segmentInfo.EndRight)
                {
                    result.Lng = segmentInfo.PointList[segmentInfo.PointList.Count - 1].Lng;
                    result.Lat = segmentInfo.PointList[segmentInfo.PointList.Count - 1].Lat;
                }
                else
                {
                    if (segmentInfo.StartRight == segmentInfo.EndRight)
                        deltaLength = 0;
                    else if (segmentInfo.StartRight < segmentInfo.EndRight)
                        deltaLength = segmentInfo.SegLength * (searchKey.Building - segmentInfo.StartRight) / (segmentInfo.EndRight - segmentInfo.StartRight);
                    else
                        deltaLength = segmentInfo.SegLength * (segmentInfo.StartRight - searchKey.Building) / (segmentInfo.StartRight - segmentInfo.EndRight);
                    for (int i = 1; i < segmentInfo.PointList.Count; i++)
                    {
                        if (segmentInfo.PointList[i].D2Start > deltaLength)
                        {
                            double percent = (deltaLength - segmentInfo.PointList[i - 1].D2Start) / (segmentInfo.PointList[i].D2Start - segmentInfo.PointList[i - 1].D2Start);
                            result.Lng = (segmentInfo.PointList[i - 1].Lng + (segmentInfo.PointList[i].Lng - segmentInfo.PointList[i - 1].Lng) * percent);
                            result.Lat = (segmentInfo.PointList[i - 1].Lat + (segmentInfo.PointList[i].Lat - segmentInfo.PointList[i - 1].Lat) * percent);
                            break;
                        }
                    }
                }
                return true;
                #endregion
            }
            // 2.3 Trường hợp kết quả đầu tiên
            else if (ignoreSide == true || segmentInfo.IsSerial == true || (searchKey.Building % 2 == segmentInfo.StartLeft % 2) || (searchKey.Building % 2 == segmentInfo.StartRight % 2) || (segmentInfo.StartLeft == 0 && segmentInfo.StartRight == 0))
            {
                #region ================ Kiểm tra số nhà cùng bên ================
                if (result.Road == null || result.Road.Length == 0)
                {
                    // 2.3.1 Lấy số nhà và vị trí điểm
                    // 2.3.1.1 Đường số nhà liên tiếp
                    if (segmentInfo.IsSerial == true)
                    {
                        buildingLeft = DataUtl.DetechBuilding(searchKey.Building, segmentInfo.StartLeft, segmentInfo.EndLeft, segmentInfo.PointList.Count, ref deltaLeft, ref indexPos);
                        buildingRight = DataUtl.DetechBuilding(searchKey.Building, segmentInfo.StartRight, segmentInfo.EndRight, segmentInfo.PointList.Count, ref deltaRight, ref indexPos);
                        if (deltaLeft < deltaRight)
                            result.Building = buildingLeft;
                        else
                            result.Building = buildingRight;
                    }
                    // 2.3.1.2 Số nhà bên trái
                    else if (searchKey.Building % 2 == segmentInfo.StartLeft % 2)
                        result.Building = DataUtl.DetechBuilding(searchKey.Building, segmentInfo.StartLeft, segmentInfo.EndLeft, segmentInfo.PointList.Count, ref deltaTemp, ref indexPos);
                    // 2.3.1.2 Số nhà bên phải
                    else
                        result.Building = DataUtl.DetechBuilding(searchKey.Building, segmentInfo.StartRight, segmentInfo.EndRight, segmentInfo.PointList.Count, ref deltaTemp, ref indexPos);

                    // 2.3.2 Ghi nhận lại kết quả
                    result.Road = segmentInfo.VName;
                    result.MinSpeed = segmentInfo.MinSpeed;
                    result.MaxSpeed = segmentInfo.MaxSpeed;
                    result.DataExt = segmentInfo.DataExt;
                    result.Lng = segmentInfo.PointList[indexPos].Lng;
                    result.Lat = segmentInfo.PointList[indexPos].Lat;
                }
                // 2.4 Các kết quả sau
                else if (segmentInfo.StartLeft > 0 || segmentInfo.StartRight > 0)
                {
                    // 2.4.1 Lấy số nhà và vị trí điểm

                    // 2.4.1.1 Đường số nhà liên tiếp
                    if (segmentInfo.IsSerial == true)
                    {
                        buildingLeft = DataUtl.DetechBuilding(searchKey.Building, segmentInfo.StartLeft, segmentInfo.EndLeft, segmentInfo.PointList.Count, ref deltaLeft, ref indexPos);
                        buildingRight = DataUtl.DetechBuilding(searchKey.Building, segmentInfo.StartRight, segmentInfo.EndRight, segmentInfo.PointList.Count, ref deltaRight, ref indexPos);
                        if (segmentInfo.StartLeft > 0 && deltaLeft < deltaRight)
                            result.Building = buildingLeft;
                        else if (segmentInfo.StartRight > 0 && deltaRight < deltaLeft)
                            result.Building = buildingRight;
                        else
                            dataFlag = false;
                    }
                    // 2.4.1.2 Số nhà bên trái
                    else if (segmentInfo.StartLeft > 0 && searchKey.Building % 2 == segmentInfo.StartLeft % 2)
                        building = DataUtl.DetechBuilding(searchKey.Building, segmentInfo.StartLeft, segmentInfo.EndLeft, segmentInfo.PointList.Count, ref deltaTemp, ref indexPos);
                    // 2.4.1.2 Số nhà bên phải
                    else if (segmentInfo.StartRight > 0 && searchKey.Building % 2 == segmentInfo.StartRight % 2)
                        building = DataUtl.DetechBuilding(searchKey.Building, segmentInfo.StartRight, segmentInfo.EndRight, segmentInfo.PointList.Count, ref deltaTemp, ref indexPos);
                    else
                        dataFlag = false;

                    if (dataFlag == true)
                    {
                        // 2.4.2 Xác định xem kết quả mới có tốt hơn không
                        short min = Math.Min(result.Building, building);
                        short max = Math.Max(result.Building, building);
                        // 2.4.2.1 Nếu số nhà cần tìm bé hơn 2 kết quả (cũ + mới) => Nếu kết quả mới lớn hơn cũ thì bỏ qua
                        if (searchKey.Building < min && building > result.Building)
                            indexPos = -1;
                        // 2.4.2.1 Nếu số nhà cần tìm lớn hơn 2 kết quả (cũ + mới) => Nếu kết quả mới bé hơn cũ thì bỏ qua
                        else if (searchKey.Building > max && building < result.Building)
                            indexPos = -1;
                        // 2.4.2.1 Nếu số nhà cần tìm nằm giữa 2 kết quả (cũ + mới) => Cũ gần hơn thì bỏ qua
                        else if (searchKey.Building > min && searchKey.Building < max && Math.Abs(searchKey.Building - building) > Math.Abs(result.Building - searchKey.Building))
                            indexPos = -1;

                        // 2.4.3 Kiểm tra cập nhật lại kết quả
                        if (indexPos > -1)
                        {
                            result.Building = building;
                            result.Lng = segmentInfo.PointList[indexPos].Lng;
                            result.Lat = segmentInfo.PointList[indexPos].Lat;
                        }
                    }
                }
                #endregion
            }

            return false;
        }

        /// <summary>
        /// Giải mã tọa độ
        /// </summary>
        public static List<BARResultPoint> PolylineDecode(string polylineStr)
        {
            try
            {
                if (string.IsNullOrEmpty(polylineStr) == true)
                    return new List<BARResultPoint>();
                List<BARResultPoint> result = new List<BARResultPoint>();

                var polylineChars = polylineStr.ToCharArray();
                int index = 0;

                var currentLat = 0;
                var currentLng = 0;
                int next5bits;
                int sum;
                int shifter;

                while (index < polylineChars.Length)
                {
                    // calculate next latitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = polylineChars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylineChars.Length);

                    if (index >= polylineChars.Length)
                        break;

                    currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                    //calculate next longitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = polylineChars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylineChars.Length);

                    if (index >= polylineChars.Length && next5bits >= 32) break;

                    currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                    result.Add(new BARResultPoint { lat = (Convert.ToDouble(currentLat) / 1E5), lng = (Convert.ToDouble(currentLng) / 1E5) });
                }
                return result;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGDecoding.SearchSegmentByNameHaNoi, ex: " + ex.ToString());
                return new List<BARResultPoint>();
            }
        }
    }
}