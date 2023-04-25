using CpGeoService.Constants;
using CpGeoService.Interfaces;
using CpGeoService.Model;
using Newtonsoft.Json;
using System.Text;

namespace CpGeoService.Repository
{
    public class GeoPBDV2Repository : IGeoPBDV2Repository
    {
        public GeoPBDV2Repository() {}

        public async Task<DataPNC> GeoByAddressAsync(string address)
        {
            address = address == string.Empty ? string.Empty : address;

            try
            {
                InputAddress inputAddres = new InputAddress() { Address = address };
                var contentJson = JsonConvert.SerializeObject(inputAddres);
                var content = new StringContent(contentJson, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    var stringContent = new StringContent(contentJson.ToString(), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(MyConstants.URL_API_GEOBYADDRESS_PBDV2, content);

                    if(!response.IsSuccessStatusCode)
                        return new DataPNC();

                    var result = await response.Content.ReadAsStringAsync();

                    var resultConvert = JsonConvert.DeserializeObject<ResultGeoByAddressPBDV2>(result);
                    if(resultConvert?.data?.province == null)
                        return new DataPNC();

                    return new DataPNC()
                    {
                        Lat = resultConvert?.data.lat??0,
                        Lng = resultConvert? .data.lng ?? 0,
                        Address = $"{resultConvert?.data.building.ToString() ?? ""}, {resultConvert?.data.road ??""}, {resultConvert?.data.commune ??""}, {resultConvert?.data.district ??""}," +
                        $" {resultConvert?.data.province ??null}",
                    };
                }
            }
            catch(Exception)
            {
                return new DataPNC();
            }
        }
    }
}
