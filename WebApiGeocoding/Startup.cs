using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System;
using WebApiGeocoding.Repositories;

namespace WebApiGeocoding
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(Configuration.GetConnectionString("MongoDb")));
            services.AddTransient<IHaNoiPointReporitory, HaNoiPointReporitory>();

            //Register the Swagger generator, defining 1 or more Swagger documents
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            //});

            services.AddControllers(options =>
            {
                options.Conventions.Add(new GroupingByNamespaceConvention());
            });

            services.AddSwaggerGen(config =>
            {
                var titlebase = "Geocoding demo";
                var desc = "Geocoding";
                var termsofservice = new Uri("https://bagps.vn/");
                var license = new OpenApiLicense()
                {
                    Name = "MIT"
                };

                var contact = new OpenApiContact()
                {
                    Name = "chanhnv",
                    Email = "chanhnv@bagroup.vn.com",
                    Url = new Uri("https://bagps.vn/")

                };

                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = titlebase + " V 1",
                    Description = desc,
                    Contact = contact,
                    License = license,
                    TermsOfService = termsofservice
                });

                config.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2",
                    Title = titlebase + " V 2",
                    Description = desc,
                    Contact = contact,
                    License = license,
                    TermsOfService = termsofservice
                });

            } );

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "API v2");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
