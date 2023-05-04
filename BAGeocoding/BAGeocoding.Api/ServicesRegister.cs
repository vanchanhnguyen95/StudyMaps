using BAGeocoding.Api.Interfaces;
using BAGeocoding.Api.Services;

namespace BAGeocoding.Api
{
    public static class ServicesRegister
    {
        public static void RegisterService(this IServiceCollection services)
        {
            services.AddScoped<IGeoService, GeoService>();
            services.AddScoped(typeof(IVietNamShapeService), typeof(VietNameShapeService));
            services.AddScoped(typeof(IRoadNameService), typeof(RoadNameService));
        }
    }
}
