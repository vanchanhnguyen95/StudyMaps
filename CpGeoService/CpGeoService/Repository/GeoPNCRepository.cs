using CpGeoService.Constants;
using CpGeoService.Interfaces;
using CpGeoService.Model;
using Newtonsoft.Json;
using System.Text;

namespace CpGeoService.Repository
{
    public class GeoPNCRepository : IGeoPNCRepository
    {
        public GeoPNCRepository() { }

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
                    var response = await client.PostAsync(MyConstants.URL_API_GEOBYADDRESS_PNC, content);

                    if (!response.IsSuccessStatusCode)
                        return new DataPNC();

                    var result = await response.Content.ReadAsStringAsync();
                    var resultConvert = JsonConvert.DeserializeObject<ResultGeoByAddressPNC>(result);

                    if(resultConvert?.Code == 1)
                        return new DataPNC()
                        {
                            Lat = resultConvert?.Data.Lat ?? 0,
                            Lng = resultConvert?.Data.Lng ?? 0,
                            Address = resultConvert?.Data?.Address ?? null
                        };
                

                    return new DataPNC();
                }
            }
            catch (Exception)
            {
                return new DataPNC();
            }
        }
    }
}
