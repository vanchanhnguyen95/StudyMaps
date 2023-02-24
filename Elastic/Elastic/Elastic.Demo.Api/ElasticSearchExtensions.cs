using Elastic.Demo.Api.Models;
using Nest;

namespace Elastic.Demo.Api
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticsearch(
            this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["ELKConfiguration:url"];
            var defaultIndex = configuration["ELKConfiguration:index"];

            var settings = new ConnectionSettings(new Uri(url)).BasicAuthentication("elastic", "8+9E-4gYmhFI3hTSBFa6")
                .PrettyJson()
                .DefaultIndex(defaultIndex);

            AddDefaultMappings(settings);

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);

            CreateIndex(client, defaultIndex);
        }

        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            settings
                .DefaultMappingFor<Catalog>(m => m
                    .Ignore(p => p.Name)
                    .Ignore(p => p.Description)
                );
        }

        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.Indices.Create(indexName,
                index => index.Map<Catalog>(x => x.AutoMap())
            );
        }
    }
}
