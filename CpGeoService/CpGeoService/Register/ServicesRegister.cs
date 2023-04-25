using CpGeoService.Interfaces;
using CpGeoService.Repository;

namespace CpGeoService.Register
{
    public static class ServicesRegister
    {
        public static void RegisterService(this IServiceCollection services)
        {
            services.AddScoped<IGeoPBDVRepository, GeoPBDVRepository>();
            services.AddScoped<IGeoPBDV2Repository, GeoPBDV2Repository>();
            services.AddScoped<IGeoPNCRepository, GeoPNCRepository>();
            services.AddScoped<IGeocodeRepository, GeocodeRepository>();
        }
    }
}
