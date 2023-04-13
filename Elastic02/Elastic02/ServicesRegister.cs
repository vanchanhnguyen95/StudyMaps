using Elastic02.Services;
using Elastic02.Services.Test;

namespace Elastic02
{
    public static class ServicesRegister
    {
        public static void RegisterService(this IServiceCollection services)
        {
            services.AddScoped(typeof(IElasticRepository<>), typeof(ElasticRepository<>));
            services.AddScoped(typeof(IElasticService<>), typeof(ElasticService<>));
            services.AddScoped(typeof(IElasticGeoRepository<>), typeof(ElasticGeoRepository<>));
            services.AddScoped(typeof(IHaNoiRoadService), typeof(HaNoiRoadService));
            services.AddScoped(typeof(IHaNoiShapeService), typeof(HaNoiShapeService));
            services.AddScoped(typeof(IVietNamShapeService), typeof(VietNameShapeService));
            services.AddScoped(typeof(IRoadNameService), typeof(RoadNameService));
            services.AddScoped(typeof(ILogService), typeof(LogService));

            //uilder.Services.AddScoped(typeof(IElasticRepository<>), typeof(ElasticRepository<>));
            //builder.Services.AddScoped(typeof(IElasticService<>), typeof(ElasticService<>));
            //builder.Services.AddScoped(typeof(IElasticGeoRepository<>), typeof(ElasticGeoRepository<>));
            ////builder.Services.AddScoped(typeof(IGeoService), typeof(GeoService));
            //builder.Services.AddScoped(typeof(IHaNoiRoadService), typeof(HaNoiRoadService));
            //builder.Services.AddScoped(typeof(IHaNoiShapeService), typeof(HaNoiShapeService));
            //builder.Services.AddScoped(typeof(IVietNamShapeService), typeof(VietNameShapeService));
            //builder.Services.AddScoped(typeof(IRoadNameService), typeof(RoadNameService));
            //builder.Services.AddScoped(typeof(ILogService), typeof(LogService));
        }
    }
}
