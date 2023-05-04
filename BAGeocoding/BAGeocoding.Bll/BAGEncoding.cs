using System;
using System.Collections.Generic;
using System.IO;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.Public;
using BAGeocoding.Utility;

using KDTree;
using RTree.Engine;
using RTree.Engine.Entity;

namespace BAGeocoding.Bll
{
    public class BAGEncoding
    {
        /// <summary>
        /// Lấy vùng theo tọa độ
        /// </summary>
        public static RPBLAddressResult RegionByGeo(RTRectangle rec, BAGPoint pts, EnumBAGLanguage lan)
        {
            try
            {
                short provinceID = 0;
                return RegionByGeo(rec, pts, lan, ref provinceID);
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGEncoding.RegionByGeo, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy vùng theo tọa độ V2
        /// </summary>
        public static RPBLAddressResultV2 RegionByGeoV2(RTRectangle rec, BAGPointV2 pts, EnumBAGLanguage lan)
        {
            try
            {
                short provinceID = 0;
                return RegionByGeoV2(rec, pts, lan, ref provinceID);
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGEncoding.RegionByGeo, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy vùng theo tọa độ
        /// </summary>
        public static RPBLAddressResult RegionByGeo(RTRectangle rec, BAGPoint pts, EnumBAGLanguage lan, ref short pri)
        {
            try
            {
                // Xác định vùng tìm kiếm
                BAGTile tile = TileByGeo(rec, pts);
                if (tile == null)
                    return null;
                // 1. Xác định xã phường
                else if (RunningParams.CommuneData.Objs.ContainsKey(tile.CommuneID) == false)
                    return null;
                BAGCommune commune = (BAGCommune)RunningParams.CommuneData.Objs[tile.CommuneID];
                //LogFile.WriteProcess(string.Format("{0} - {1}", commune.CommuneID, commune.VName));
                // 2. Xác định quận huyện
                if (RunningParams.DistrictData.Objs.ContainsKey(commune.DistrictID) == false)
                    return null;
                BAGDistrict district = (BAGDistrict)RunningParams.DistrictData.Objs[commune.DistrictID];
                // 3. Xác định tỉnh/thành
                if (RunningParams.ProvinceData.Objs.ContainsKey(district.ProvinceID) == false)
                    return null;
                BAGProvince province = (BAGProvince)RunningParams.ProvinceData.Objs[district.ProvinceID];
                pri = province.ProvinceID;

                // Trả về kết quả
                return new RPBLAddressResult
                {
                    Lng = (float)pts.Lng,
                    Lat = (float)pts.Lat,
                    Commune = (lan == EnumBAGLanguage.Vn) ? commune.VName : commune.EName,
                    District = (lan == EnumBAGLanguage.Vn) ? district.VName : district.EName,
                    Province = (lan == EnumBAGLanguage.Vn) ? province.VName : province.EName,

                    DistrictID = district.DistrictID,
                    ProvinceID = province.ProvinceID
                };

                /*
                // 1. Tìm xã/phường theo tọa độ
                BAGCommune commune = CommuneByGeo(rec, pts);
                if (commune == null)
                    return null;
                // 2. Xác định quận/huyện
                else if (RunningParams.DistrictData.Objs.ContainsKey(commune.DistrictID) == false)
                    return null;
                BAGDistrict district = (BAGDistrict)RunningParams.DistrictData.Objs[commune.DistrictID];
                // 3. Xác định tỉnh/thành
                if (RunningParams.ProvinceData.Objs.ContainsKey(district.ProvinceID) == false)
                    return null;
                BAGProvince province = (BAGProvince)RunningParams.ProvinceData.Objs[district.ProvinceID];
                pri = province.ProvinceID;

                // Trả về kết quả
                return new PBLAddressResult
                {
                    Lng = (float)pts.Lng,
                    Lat = (float)pts.Lat,
                    Commune = (lan == EnumBAGLanguage.Vn) ? commune.VName : commune.EName,
                    District = (lan == EnumBAGLanguage.Vn) ? district.VName : district.EName,
                    Province = (lan == EnumBAGLanguage.Vn) ? province.VName : province.EName
                };
                */
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGEncoding.RegionByGeo, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy vùng theo tọa độ V2
        /// </summary>
        public static RPBLAddressResultV2 RegionByGeoV2(RTRectangle rec, BAGPointV2 pts, EnumBAGLanguage lan, ref short pri)
        {
            try
            {
                // Xác định vùng tìm kiếm
                BAGTile tile = TileByGeoV2(rec, pts);
                if (tile == null)
                    return null;
                // 1. Xác định xã phường
                else if (RunningParams.CommuneData.Objs.ContainsKey(tile.CommuneID) == false)
                    return null;
                BAGCommune commune = (BAGCommune)RunningParams.CommuneData.Objs[tile.CommuneID];
                //LogFile.WriteProcess(string.Format("{0} - {1}", commune.CommuneID, commune.VName));
                // 2. Xác định quận huyện
                if (RunningParams.DistrictData.Objs.ContainsKey(commune.DistrictID) == false)
                    return null;
                BAGDistrict district = (BAGDistrict)RunningParams.DistrictData.Objs[commune.DistrictID];
                // 3. Xác định tỉnh/thành
                if (RunningParams.ProvinceData.Objs.ContainsKey(district.ProvinceID) == false)
                    return null;
                BAGProvince province = (BAGProvince)RunningParams.ProvinceData.Objs[district.ProvinceID];
                pri = province.ProvinceID;

                // Trả về kết quả
                return new RPBLAddressResultV2
                {
                    Lng = pts.Lng,
                    Lat = pts.Lat,
                    Commune = (lan == EnumBAGLanguage.Vn) ? commune.VName : commune.EName,
                    District = (lan == EnumBAGLanguage.Vn) ? district.VName : district.EName,
                    Province = (lan == EnumBAGLanguage.Vn) ? province.VName : province.EName,

                    DistrictID = district.DistrictID,
                    ProvinceID = province.ProvinceID
                };
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGEncoding.RegionByGeo, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy khu đô thị theo tọa độ
        /// </summary>
        public static PBLAddressResult PlaceByGeo(RTRectangle rec, BAGPoint pts, EnumBAGLanguage lan)
        {
            try
            {
                // Tiến hành tìm kiếm
                BAGPlace plotInfo = new BAGPlace();
                List<BAGPlace> result = RunningParams.PlaceData.RTree.Intersects(rec);
                if (result == null || result.Count == 0)
                    return null;
                else
                {
                    for (int i = 0; i < result.Count; i++)
                    {
                        if (MapUtilityManager.CheckInsidePolygon(result[i].PointList, pts) == true)
                        {
                            plotInfo = new BAGPlace(result[i]);
                            break;
                        }
                    }
                }

                // 1. Xác định ô đất
                if (plotInfo.PlaceID == 0)
                    return null;
                // 2. Xác định lô đất
                if (RunningParams.PlaceData.Portion.ContainsKey(plotInfo.ParentID) == false)
                    return null;
                BAGPlace portionInfo = (BAGPlace)RunningParams.PlaceData.Portion[plotInfo.ParentID];
                // 3. Xác định khu đô thị
                if (RunningParams.PlaceData.Urban.ContainsKey(portionInfo.ParentID) == false)
                    return null;
                BAGPlace urbanInfo = (BAGPlace)RunningParams.PlaceData.Urban[portionInfo.ParentID];
                
                // Trả về kết quả
                PBLAddressResult address = new PBLAddressResult
                {
                    Lng = (float)plotInfo.Center.Lng,
                    Lat = (float)plotInfo.Center.Lat,
                    Road = string.Empty
                };
                if (plotInfo.Name != null && plotInfo.Name.Length > 0)
                    address.Road = plotInfo.Name;
                if (portionInfo.Name != null && portionInfo.Name.Length > 0)
                {
                    if (address.Road.Length > 0)
                        address.Road += ", ";
                    address.Road += portionInfo.Name;
                }
                if (urbanInfo.Name != null && urbanInfo.Name.Length > 0)
                {
                    if (address.Road.Length > 0)
                        address.Road += ", ";
                    address.Road += urbanInfo.Name;
                }

                return address;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGEncoding.PlaceByGeo, ex: " + ex.ToString());
                return null;
            }
        }

        public static PBLAddressResult PlaceByGeoV2(RTRectangle rec, BAGPointV2 pts, EnumBAGLanguage lan)
        {
            try
            {
                // Tiến hành tìm kiếm
                BAGPlace plotInfo = new BAGPlace();
                List<BAGPlace> result = RunningParams.PlaceData.RTree.Intersects(rec);
                if (result == null || result.Count == 0)
                    return null;
                else
                {
                    for (int i = 0; i < result.Count; i++)
                    {
                        if (MapUtilityManager.CheckInsidePolygonV2(result[i].PointList, pts) == true)
                        {
                            plotInfo = new BAGPlace(result[i]);
                            break;
                        }
                    }
                }

                // 1. Xác định ô đất
                if (plotInfo.PlaceID == 0)
                    return null;
                // 2. Xác định lô đất
                if (RunningParams.PlaceData.Portion.ContainsKey(plotInfo.ParentID) == false)
                    return null;
                BAGPlace portionInfo = (BAGPlace)RunningParams.PlaceData.Portion[plotInfo.ParentID];
                // 3. Xác định khu đô thị
                if (RunningParams.PlaceData.Urban.ContainsKey(portionInfo.ParentID) == false)
                    return null;
                BAGPlace urbanInfo = (BAGPlace)RunningParams.PlaceData.Urban[portionInfo.ParentID];

                // Trả về kết quả
                PBLAddressResult address = new PBLAddressResult
                {
                    Lng = (float)plotInfo.Center.Lng,
                    Lat = (float)plotInfo.Center.Lat,
                    Road = string.Empty
                };
                if (plotInfo.Name != null && plotInfo.Name.Length > 0)
                    address.Road = plotInfo.Name;
                if (portionInfo.Name != null && portionInfo.Name.Length > 0)
                {
                    if (address.Road.Length > 0)
                        address.Road += ", ";
                    address.Road += portionInfo.Name;
                }
                if (urbanInfo.Name != null && urbanInfo.Name.Length > 0)
                {
                    if (address.Road.Length > 0)
                        address.Road += ", ";
                    address.Road += urbanInfo.Name;
                }

                return address;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGEncoding.PlaceByGeo, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Xác định vùng tìm kiếm
        /// </summary>
        private static BAGTile TileByGeo(RTRectangle rec, BAGPoint pts)
        {
            try
            {
                List<BAGTile> result = RunningParams.TileData.RTree.Intersects(rec);
                if (result == null || result.Count == 0)
                    return null;
                else if (result.Count == 1)
                    return result[0];
                else
                {
                    for (int i = 0; i < result.Count - 1; i++)
                    {
                        if (MapUtilityManager.CheckInsidePolygon(result[i].PointList, pts) == true)
                            return result[i];
                    }
                    return result[result.Count - 1];
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGEncoding.TileByGeo, ex: " + ex.ToString());
                return null;
            }
        }

        private static BAGTile TileByGeoV2(RTRectangle rec, BAGPointV2 pts)
        {
            try
            {
                List<BAGTile> result = RunningParams.TileData.RTree.Intersects(rec);
                if (result == null || result.Count == 0)
                    return null;
                else if (result.Count == 1)
                    return result[0];
                else
                {
                    for (int i = 0; i < result.Count - 1; i++)
                    {
                        if (MapUtilityManager.CheckInsidePolygonV2(result[i].PointList, pts) == true)
                            return result[i];
                    }
                    return result[result.Count - 1];
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGEncoding.TileByGeo, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Xác định xã/phường
        /// </summary>
        private static BAGCommune CommuneByGeo(RTRectangle rec, BAGPoint pts)
        {
            try
            {
                List<BAGCommune> result = RunningParams.CommuneData.RTree.Intersects(rec);
                if (result == null || result.Count == 0)
                    return null;
                else if (result.Count == 1)
                    return result[0];
                else
                {
                    for (int i = 0; i < result.Count - 1; i++)
                    {
                        if (MapUtilityManager.CheckInsidePolygon(result[i].PointList, pts) == true)
                            return result[i];
                    }
                    return result[result.Count - 1];
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGEncoding.CommuneByGeo, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Xác định quận/huyện
        /// </summary>
        private static BAGDistrict DistrictByGeo(RTRectangle rec, BAGPoint pts)
        {
            try
            {
                List<BAGCommune> result = RunningParams.CommuneData.RTree.Intersects(rec);
                if (result == null || result.Count == 0)
                    return null;
                // 2. Nếu chỉ có một kết quả (Một xã/phường) => Lấy luôn quận/huyện của xã/phường đó
                else if (result.Count == 1)
                {
                    if (RunningParams.DistrictData.Objs.ContainsKey(result[0].DistrictID) == true)
                        return (BAGDistrict)RunningParams.DistrictData.Objs[result[0].DistrictID];
                    else
                        return null;
                }
                // 3. Trường hợp nhiều hơn
                else
                {
                    // 3.1 Kiểm tra xem các xã phường đó có thuộc một quận/huyện không
                    short districID = 0;
                    for (int i = 0; i < result.Count - 1; i++)
                    {
                        if (districID == -1)
                            break;
                        else if (RunningParams.DistrictData.Objs.ContainsKey(result[i].DistrictID) == false)
                            continue;
                        else if (districID == 0)
                            districID = result[i].DistrictID;
                        else if (districID != result[i].DistrictID)
                            districID = -1;
                    }
                    if (districID > 0)
                        return (BAGDistrict)RunningParams.DistrictData.Objs[districID];

                    // 3.2 Tìm xã/phường của điểm
                    for (int i = 0; i < result.Count - 1; i++)
                    {
                        if (MapUtilityManager.CheckInsidePolygon(result[i].PointList, pts) == true)
                            return (BAGDistrict)RunningParams.DistrictData.Objs[result[i].DistrictID];
                    }

                    // 3.3 Kết quả chính là xã/phường cuối cùng
                    return (BAGDistrict)RunningParams.DistrictData.Objs[result[result.Count - 1].DistrictID];
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGEncoding.DistrictByGeo, ex: " + ex.ToString());
                return null;
            }
        }
        
        /// <summary>
        /// Xác định tỉnh/thành
        /// </summary>
        private static BAGProvince ProvinceByGeo(RTRectangle rec, BAGPoint pts)
        {
            try
            {
                List<BAGCommune> result = RunningParams.CommuneData.RTree.Intersects(rec);
                if (result == null || result.Count == 0)
                    return null;
                // 2. Nếu chỉ có một kết quả (Một xã/phường) => Lấy luôn quận/huyện của xã/phường đó
                else if (result.Count == 1)
                {
                    if (RunningParams.DistrictData.Objs.ContainsKey(result[0].DistrictID) == true)
                    {
                        int provinceID = ((BAGDistrict)RunningParams.DistrictData.Objs[result[0].DistrictID]).ProvinceID;
                        if (RunningParams.ProvinceData.Objs.ContainsKey(provinceID) == true)
                            return (BAGProvince)RunningParams.ProvinceData.Objs[provinceID];
                        else
                            return null;
                    }
                    else
                        return null;
                }
                // 3. Trường hợp nhiều hơn
                else
                {
                    // 3.1 Kiểm tra xem các xã phường đó có thuộc một tỉnh/thành không
                    short provinceID = 0;
                    for (int i = 0; i < result.Count - 1; i++)
                    {
                        if (provinceID == -1)
                            break;
                        else if (RunningParams.DistrictData.Objs.ContainsKey(result[i].DistrictID) == false)
                            continue;
                        else if (provinceID == 0)
                            provinceID = ((BAGDistrict)RunningParams.DistrictData.Objs[result[0].DistrictID]).ProvinceID;
                        else if (provinceID != ((BAGDistrict)RunningParams.DistrictData.Objs[result[0].DistrictID]).ProvinceID)
                            provinceID = -1;
                    }
                    if (provinceID > 0 && RunningParams.ProvinceData.Objs.ContainsKey(provinceID) == true)
                        return (BAGProvince)RunningParams.ProvinceData.Objs[provinceID];

                    // 3.2 Tìm xã/phường của điểm
                    for (int i = 0; i < result.Count - 1; i++)
                    {
                        if (MapUtilityManager.CheckInsidePolygon(result[i].PointList, pts) == true)
                        {
                            provinceID = ((BAGDistrict)RunningParams.DistrictData.Objs[result[i].DistrictID]).ProvinceID;
                            if (RunningParams.ProvinceData.Objs.ContainsKey(provinceID) == true)
                                return (BAGProvince)RunningParams.ProvinceData.Objs[provinceID];
                            else
                                return null;
                        }
                    }

                    // 3.3 Kết quả chính là xã/phường cuối cùng
                    provinceID = ((BAGDistrict)RunningParams.DistrictData.Objs[result[result.Count - 1].DistrictID]).ProvinceID;
                    if (RunningParams.ProvinceData.Objs.ContainsKey(provinceID) == true)
                        return (BAGProvince)RunningParams.ProvinceData.Objs[provinceID];
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGEncoding.DistrictByGeo, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Tìm đường theo tọa độ
        /// </summary>
        public static RPBLAddressResult RoadByGeo(KDTree<BAGPoint> kdt, RTree<BAGSegment> rts, RTRectangle rec, BAGPoint pts, double dis, EnumBAGLanguage lan)
        {
            try
            {
                // Xác định lại khung tìm kiếm
                BAGPoint neighbor = NodeDetect(kdt, pts);
                if (neighbor != null)
                {
                    double lng = pts.Lng - neighbor.Lng;
                    double lat = pts.Lat - neighbor.Lat;
                    double its = Math.Sqrt(lng * lng + lat * lat) * 1.1d;
                    rec = new RTRectangle(pts.Lng - its, pts.Lat - its, pts.Lng + its, pts.Lat + its, 0.0f, 0.0f);
                }

                // Tiến hành tìm kiếm
                BAGDistance distance = SegmentByGeo(rts, rec, pts, dis);
                if (distance != null)
                {
                    RPBLAddressResult result = new RPBLAddressResult();
                    if (distance.IsLeft == true)
                        result.Building = MapUtilityManager.CalBuilding(distance.Segment.IsSerial, distance.Segment.StartLeft, distance.Segment.EndLeft, distance.Percen);
                    else
                        result.Building = MapUtilityManager.CalBuilding(distance.Segment.IsSerial, distance.Segment.StartRight, distance.Segment.EndRight, distance.Percen);
                    result.Road = (lan == EnumBAGLanguage.Vn) ? distance.Segment.VName : distance.Segment.EName;
                    result.MinSpeed = distance.Segment.MinSpeed;
                    result.MaxSpeed = distance.Segment.MaxSpeed;
                    result.DataExt = distance.Segment.DataExt;
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGEncoding.RoadByGeo, ex: " + ex.ToString());
                return null;
            }
        }

        public static RPBLAddressResult RoadByGeoV2(KDTree<BAGPointV2> kdt, RTree<BAGSegmentV2> rts, RTRectangle rec, BAGPointV2 pts, double dis, EnumBAGLanguage lan)
        {
            try
            {
                // Xác định lại khung tìm kiếm
                BAGPointV2 neighbor = NodeDetectV2(kdt, pts);
                if (neighbor != null)
                {
                    double lng = pts.Lng - neighbor.Lng;
                    double lat = pts.Lat - neighbor.Lat;
                    double its = Math.Sqrt(lng * lng + lat * lat) * 1.1d;
                    rec = new RTRectangle(pts.Lng - its, pts.Lat - its, pts.Lng + its, pts.Lat + its, 0.0f, 0.0f);
                }

                // Tiến hành tìm kiếm
                BAGDistanceV2 distance = SegmentByGeoV2(rts, rec, pts, dis);
                if (distance != null)
                {
                    RPBLAddressResult result = new RPBLAddressResult();
                    if (distance.IsLeft == true)
                        result.Building = MapUtilityManager.CalBuilding(distance.Segment.IsSerial, distance.Segment.StartLeft, distance.Segment.EndLeft, distance.Percen);
                    else
                        result.Building = MapUtilityManager.CalBuilding(distance.Segment.IsSerial, distance.Segment.StartRight, distance.Segment.EndRight, distance.Percen);
                    result.Road = (lan == EnumBAGLanguage.Vn) ? distance.Segment.VName : distance.Segment.EName;
                    result.MinSpeed = distance.Segment.MinSpeed;
                    result.MaxSpeed = distance.Segment.MaxSpeed;
                    result.DataExt = distance.Segment.DataExt;
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGEncoding.RoadByGeo, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Xác định node gần nhất để xét rect
        /// </summary>
        private static BAGPoint NodeDetect(KDTree<BAGPoint> kdt, BAGPoint pts)
        {
            try
            {
                NearestNeighbour<BAGPoint> neighborList = kdt.NearestNeighbors(pts.ToArray(), new SquareEuclideanDistanceFunction(), 1, 1e-5f);
                if (neighborList == null)
                    return null;
                while (neighborList.MoveNext())
                {
                    if (neighborList.Current != null)
                        return new BAGPoint(neighborList.Current);
                }
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGEncoding.NodeDetect, ex: " + ex.ToString());
                return null;
            }
        }

        private static BAGPointV2 NodeDetectV2(KDTree<BAGPointV2> kdt, BAGPointV2 pts)
        {
            try
            {
                NearestNeighbour<BAGPointV2> neighborList = kdt.NearestNeighbors(pts.ToArray(), new SquareEuclideanDistanceFunction(), 1, 1e-5f);
                if (neighborList == null)
                    return null;
                while (neighborList.MoveNext())
                {
                    if (neighborList.Current != null)
                        return new BAGPointV2(neighborList.Current);
                }
                return null;
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGEncoding.NodeDetect, ex: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Xác định đoạn đường theo tọa độ
        /// </summary>
        private static BAGDistance SegmentByGeo(RTree<BAGSegment> rts, RTRectangle rec, BAGPoint pts, double dis)
        {
            try
            {
                // Lấy danh sách segment trong phạm vi cho phép
                List<BAGSegment> result = rts.Intersects(rec);
                if (result == null || result.Count == 0)
                    return null;

                // Tính toán khoảng cách điểm đến segment
                BAGDistance distance = result[0].DistanceFrom(pts);
                distance.Segment = result[0];
                for (int i = 1; i < result.Count; i++)
                {
                    BAGDistance tmp = result[i].DistanceFrom(pts);
                    if (tmp.Distance < distance.Distance)
                    {
                        distance = new BAGDistance(tmp.Anchor, tmp.Point, tmp.Distance);
                        distance.Segment = result[i];
                        distance.PointIndex = tmp.PointIndex;
                        if (distance.Distance < Constants.DISTANCE_INTERSECT_EPSILON)
                            break;
                    }
                }

                // Kiểm tra so sánh với khoảng cách chuẩn
                if (distance.Distance < dis)
                {
                    distance.IsLeft = MapUtilityManager.IsLeft(distance.Segment.PointList[distance.PointIndex - 1], distance.Segment.PointList[distance.PointIndex], pts);
                    distance.Percen = MapUtilityManager.PosPercent(distance.Segment.PointList, pts, distance.Anchor, distance.PointIndex);
                    return distance;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGEncoding.SegmentByGeo, ex: " + ex.ToString());
                return null;
            }
        }

        private static BAGDistanceV2 SegmentByGeoV2(RTree<BAGSegmentV2> rts, RTRectangle rec, BAGPointV2 pts, double dis)
        {
            try
            {
                // Lấy danh sách segment trong phạm vi cho phép
                List<BAGSegmentV2> result = rts.Intersects(rec);
                if (result == null || result.Count == 0)
                    return null;

                // Tính toán khoảng cách điểm đến segment
                BAGDistanceV2 distance = result[0].DistanceFrom(pts);
                distance.Segment = result[0];
                for (int i = 1; i < result.Count; i++)
                {
                    BAGDistanceV2 tmp = result[i].DistanceFrom(pts);
                    if (tmp.Distance < distance.Distance)
                    {
                        distance = new BAGDistanceV2(tmp.Anchor, tmp.Point, tmp.Distance);
                        distance.Segment = result[i];
                        distance.PointIndex = tmp.PointIndex;
                        if (distance.Distance < Constants.DISTANCE_INTERSECT_EPSILON)
                            break;
                    }
                }

                // Kiểm tra so sánh với khoảng cách chuẩn
                if (distance.Distance < dis)
                {
                    distance.IsLeft = MapUtilityManager.IsLeft(distance.Segment.PointList[distance.PointIndex - 1], distance.Segment.PointList[distance.PointIndex], pts);
                    distance.Percen = MapUtilityManager.PosPercent(distance.Segment.PointList, pts, distance.Anchor, distance.PointIndex);
                    return distance;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteError("BAGEncoding.SegmentByGeo, ex: " + ex.ToString());
                return null;
            }
        }


        /// <summary>
        /// Backup các file logs vào thư mục riêng
        /// </summary>
        public static void BackupLogs()
        {
            try
            {
                string[] fileList = Directory.GetFiles(Constants.DEFAULT_DIRECTORY_LOGS, "*.txt");
                if (fileList == null || fileList.Length == 0)
                    return;
                string folderName = string.Format("{0:yyyyMMdd-HHmmss}", DateTime.Now);
                if (Directory.Exists(Constants.DEFAULT_DIRECTORY_LOGS + folderName) == false)
                    Directory.CreateDirectory(Constants.DEFAULT_DIRECTORY_LOGS + folderName);
                int indexID = 0;
                for (int i = 0; i < fileList.Length; i++)
                {
                    indexID = fileList[i].LastIndexOf("\\");
                    MoveFile(Constants.DEFAULT_DIRECTORY_LOGS, folderName, fileList[i].Substring(indexID + 1, fileList[i].Length - indexID - 1));
                }
            }
            catch { }
        }

        /// <summary>
        /// Move file sang thư mục khác
        /// </summary>
        private static void MoveFile(string ph, string fd, string fn)
        {
            try
            {
                if (File.Exists(ph + fn) == true)
                {
                    if (File.Exists(ph + fd + "\\" + fn) == true)
                        File.Delete(ph + fd + "\\" + fn);
                    File.Move(ph + fn, ph + fd + "\\" + fn);
                }
            }
            catch { }
        }
    }
}
