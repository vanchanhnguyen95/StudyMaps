using System.Xml.Serialization;

namespace CpGeoService.Dto
{
    [XmlRoot(ElementName = "GeoByAddressResult")]
    public class GeoByAddressResult
    {

        [XmlElement(ElementName = "Lng")]
        public double Lng { get; set; }

        [XmlElement(ElementName = "Lat")]
        public double Lat { get; set; }

        [XmlElement(ElementName = "Building")]
        public int Building { get; set; }

        [XmlElement(ElementName = "Road")]
        public string Road { get; set; }

        [XmlElement(ElementName = "Commune")]
        public string Commune { get; set; }

        [XmlElement(ElementName = "District")]
        public string District { get; set; }

        [XmlElement(ElementName = "Province")]
        public string Province { get; set; }

        [XmlElement(ElementName = "Accurate")]
        public bool Accurate { get; set; }
    }
}
