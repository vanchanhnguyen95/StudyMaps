using CpGeoService.Dto;
using CpGeoService.Interfaces;
using CpGeoService.Model;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace CpGeoService.Repository
{
    public class GeoPBDVRepository : IGeoPBDVRepository
    {
        private readonly IConfiguration _configuration;
        private GeoPBDInfo _geoPBDInfo;

        public GeoPBDVRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            var geoPBDInfo = _configuration.GetSection("GeoPBDInfo");

            _geoPBDInfo = new GeoPBDInfo();
            _geoPBDInfo.Username = geoPBDInfo["Username"];
            _geoPBDInfo.Password = geoPBDInfo["Password"];
            _geoPBDInfo.UriSring = geoPBDInfo["UriSring"];
            _geoPBDInfo.ActionGeoByAddress = geoPBDInfo["ActionGeoByAddress"];
        }

        public async Task<DataPNC> GeoByAddressAsync(string? address)
        {
            try
            {
                // Construct the SOAP request XML
                XmlDocument soapEnvelopeXml = new XmlDocument();
                soapEnvelopeXml.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
                <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                  <soap:Header>
                    <UTLAuthHeader xmlns=""http://tempuri.org/"">
                      <Username>" + _geoPBDInfo.Username + @"</Username>
                      <Password>" + _geoPBDInfo.Password + @"</Password>
                    </UTLAuthHeader>
                  </soap:Header>
                  <soap:Body>
                    <GeoByAddress xmlns=""http://tempuri.org/"">
                      <keyStr>" + address + @"</keyStr>
                      <lanStr>vi</lanStr>
                    </GeoByAddress>
                  </soap:Body>
                </soap:Envelope>");

                // Create an instance of HttpClient
                HttpClient client = new HttpClient();

                // Create a StringContent object from the SOAP request XML
                StringContent content = new StringContent(soapEnvelopeXml.InnerXml, Encoding.UTF8, "text/xml");

                // Create an HttpRequestMessage with the required headers and content
                var request = new HttpRequestMessage()
                {
                    //RequestUri = new Uri("http://geocoding.binhanh.com.vn/geocoding.asmx"),
                    RequestUri = new Uri(_geoPBDInfo.UriSring),
                    Method = System.Net.Http.HttpMethod.Post,
                    Content = content
                };

                // Add the required SOAPAction header
                //request.Headers.Add("SOAPAction", "http://tempuri.org/GeoByAddress");
                request.Headers.Add("SOAPAction", _geoPBDInfo.ActionGeoByAddress);

                // Send the SOAP request and retrieve the response
                var response = await client.SendAsync(request);

                // Read the SOAP response into an XElement
                var responseContent = await response.Content.ReadAsStringAsync();
                var soapEnvelope = XDocument.Parse(responseContent);
                XNamespace ns = "http://tempuri.org/";

                // Extract the values from the SOAP response
                var lng = soapEnvelope.Descendants(ns + "Lng").Single().Value;
                var lat = soapEnvelope.Descendants(ns + "Lat").Single().Value;
                var building = soapEnvelope.Descendants(ns + "Building").Single().Value;
                var road = soapEnvelope.Descendants(ns + "Road").Single().Value;
                var commune = soapEnvelope.Descendants(ns + "Commune").Single().Value;
                var district = soapEnvelope.Descendants(ns + "District").Single().Value;
                var province = soapEnvelope.Descendants(ns + "Province").Single().Value;
                //var accurate = bool.Parse(soapEnvelope.Descendants(ns + "Accurate").Single().Value);

                DataPNC data = new DataPNC();
                data.Lat = Convert.ToDouble(lat);
                data.Lng = Convert.ToDouble(lng);
                data.Address = $"{building ?? ""}, {road ?? ""}, {commune ?? ""}, {district ?? ""}, {province ?? ""}";

                return data;
            }
            catch(Exception)
            {
                return new DataPNC();
            }
             
        }
    }
}
