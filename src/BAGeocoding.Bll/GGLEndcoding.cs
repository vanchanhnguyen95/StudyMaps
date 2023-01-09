using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.MapObj;
using BAGeocoding.Entity.Public;

using BAGeocoding.Utility;

namespace BAGeocoding.Bll
{
    public class GGLEndcoding
    {
        /// <summary>
        /// Tìm kiếm địa chỉ theo tọa độ
        /// </summary>
        public static RPBLAddressResult GeoByAddress(string keyStr, string lanStr)
        {
            try
            {
                if (RunningParams.GOOGLE_GEOCODE_KEY.Length == 0)
                    return null;
                string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false&key={1}", Uri.EscapeDataString(keyStr), RunningParams.GOOGLE_GEOCODE_KEY);
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    XDocument xdoc = XDocument.Load(client.OpenRead(new Uri(requestUri)));
                    XElement element = xdoc.Element("GeocodeResponse").Element("result");
                    return ParsingResponse(element);
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("GGLEndcoding.GeoByAddress({0}, {1}), ex: {2}", keyStr, lanStr, ex.ToString()));
                return null;
            }
        }

        /// <summary>
        /// Tìm kiếm địa chỉ theo tọa độ
        /// </summary>
        public static RPBLAddressResult AddressByGeo(BAGPoint pointObj, string keyStr, EnumBAGLanguage lanStr)
        {
            try
            {
                if (RunningParams.GOOGLE_GEOCODE_KEY.Length == 0)
                    return null;
                //LogFile.WriteGG(string.Format("{0} -> ({1}, {2})", keyStr, pointObj.Lat, pointObj.Lng));
                string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?latlng={0}&sensor=false&key={1}", Uri.EscapeDataString(string.Format("{0},{1}", pointObj.Lat, pointObj.Lng)), RunningParams.GOOGLE_GEOCODE_KEY);
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    XDocument xdoc = XDocument.Load(client.OpenRead(new Uri(requestUri)));
                    XElement element = xdoc.Element("GeocodeResponse").Element("result");
                    return ParsingResponse(element);
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("GGLEndcoding.AddressByGeo({0}, {1}), ex: {2}", pointObj.ToString(), lanStr, ex.ToString()));
                return null;
            }
        }

        /// <summary>
        /// Phân tích kết quả
        /// </summary>
        private static RPBLAddressResult ParsingResponse(XElement source)
        {
            try
            {
                if (source == null)
                {
                    LogFile.WriteError("GGLEndcoding.ParsingResponse -> NULL");
                    return null;
                }

                XElement locationElement = source.Element("geometry").Element("location");
                List<XElement> originalList = source.Elements("address_component").ToList();
                RPBLAddressResult addressResult = new RPBLAddressResult
                {
                    Lng = Convert.ToSingle(locationElement.Element("lng").Value),
                    Lat = Convert.ToSingle(locationElement.Element("lat").Value),
                    Road = string.Empty,
                    Commune = string.Empty,
                    District = string.Empty,
                    Province = string.Empty
                };
                bool stateData = false;
                for (int i = 0; i < originalList.Count; i++)
                {
                    stateData = false;
                    List<XElement> dataList = originalList[i].Elements("type").ToList();
                    for (int j = 0; j < dataList.Count; j++)
                    {
                        switch (dataList[j].Value.ToString().ToLower())
                        {
                            case "street_number":
                                stateData = true;
                                addressResult.Building = Convert.ToInt16(originalList[i].Element("long_name").Value);
                                break;
                            case "route":
                                stateData = true;
                                addressResult.Road = originalList[i].Element("long_name").Value.ToString().Trim();
                                break;
                            case "administrative_area_level_1":
                                stateData = true;
                                addressResult.Province = originalList[i].Element("long_name").Value.ToString().Trim();
                                break;
                            case "administrative_area_level_2":
                                stateData = true;
                                addressResult.District = originalList[i].Element("long_name").Value.ToString().Trim();
                                break;
                            case "locality":
                                stateData = true;
                                if (addressResult.District.Length == 0)
                                    addressResult.District = originalList[i].Element("long_name").Value.ToString().Trim();
                                break;
                            case "administrative_area_level_3":
                                stateData = true;
                                addressResult.Commune = originalList[i].Element("long_name").Value.ToString().Trim();
                                break;
                            default:
                                break;
                        }
                        if (stateData == true)
                            break;
                    }
                }
                //LogFile.WriteData(source.ToString());
                //LogFile.WriteData(addressResult.ToString());

                return addressResult;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("GGLEndcoding.ParsingResponse({0}), ex: {1}", source.ToString(), ex.ToString()));
                return null;
            }
        }
    }
}
