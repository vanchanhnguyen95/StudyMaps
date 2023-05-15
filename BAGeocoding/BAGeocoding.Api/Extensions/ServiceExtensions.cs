using BAGeocoding.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace BAGeocoding.Api.Extensions
{
    public static class ServiceExtensions
    {
        //static string XmlCommentsFileName
        //{
        //    get
        //    {
        //        var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
        //        return fileName;
        //    }
        //}

        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            return services.AddVersioning()
                .AddSwaggerVersioning();
        }

        private static IServiceCollection AddVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(setup =>
            {
                setup.DefaultApiVersion = new ApiVersion(1, 0);
                setup.AssumeDefaultVersionWhenUnspecified = true;
                setup.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

            return services;
        }

        private static IServiceCollection AddSwaggerVersioning(this IServiceCollection services)
        {
            services.AddSwaggerGen(options => {
                // for further customization
                //options.OperationFilter<DefaultValuesFilter>();
            });
            services.ConfigureOptions<ConfigureSwaggerOptions>();

            return services;
        }
    }
}
