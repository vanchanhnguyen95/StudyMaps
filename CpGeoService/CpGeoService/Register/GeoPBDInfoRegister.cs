using CpGeoService.Dto;
using Microsoft.Extensions.Options;

namespace CpGeoService.Register
{
    public class GeoPBDInfoRegister : IConfigureOptions<GeoPBDInfo>
    {
        private const string SectionName = "GeoPBDInfo";
        private readonly IConfiguration _configuration;

        public GeoPBDInfoRegister(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(GeoPBDInfo options)
        {
            _configuration
                .GetSection(SectionName)
                .Bind(options);
        }
    }
}
