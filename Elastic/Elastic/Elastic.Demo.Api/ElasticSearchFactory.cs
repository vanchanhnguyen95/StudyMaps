using Elasticsearch.Net;
using Nest;

namespace Elastic.Demo.Api
{
    public class ElasticSearchFactory
    {
        public ElasticClient ElasticSearchClient()
        {
            //var nodes = new Uri[]
            //{
            // new Uri("http://localhost:9200/"),
            //};
            //var connectionPool = new StaticConnectionPool(nodes);
            //var connectionSettings = new ConnectionSettings(connectionPool).DisableDirectStreaming();
            //var elasticClient = new ElasticClient(connectionSettings);
            //return elasticClient;
            //var client = new ElasticClient();
            var nodes = new List<string>() { "http://elastic:NlPYZIUsFzkRivtq-wWC@localhost:9200/" };

            var uris = nodes.Select(n => new Uri(n));

            var connectionPool = new SniffingConnectionPool(uris);
            var settings = new ConnectionSettings(connectionPool)
                .DefaultIndex("Catalog");

            var client = new ElasticClient(settings);


            //var nodes = new List<string>() { "http://danny-desktop:9200", "http://danny-desktop:9201", "http://vinay-desktop:9200" };

            //var uris = nodes.Select(n => new Uri(n));
            //var connectionPool = new SniffingConnectionPool(uris);
            ////var connectionPool = new StickyConnectionPool(uris);

            //var connectionSettings = new ConnectionSettings(connectionPool);

            //var client = new ElasticClient(connectionSettings);

            //var client.IndexExists(Indices.Parse("hd20160518"));

            return client;
        }
    }
}
