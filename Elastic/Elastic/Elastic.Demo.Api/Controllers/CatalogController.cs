using Elastic.Demo.Api.Models;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System;

namespace Elastic.Demo.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogController : Controller
    {
        private readonly ElasticSearchFactory _esFactory;
        public CatalogController()
        {
            _esFactory = new ElasticSearchFactory();
        }

        [HttpGet]
        [Route("/GetCatalog")]
        public async Task<List<Catalog>> Get()
        {
            return await DoSearchAsync("");
        }

        private async Task<List<Catalog>> DoSearchAsync(string name = "")
        {
            var response = await (_esFactory.ElasticSearchClient().SearchAsync<Catalog>(s => s
                        .Index("catalogs")
                        .Size(50)
                        .Query(q => q
                          .Match(m => m
                            .Field(f => f.Name)
                            .Query(name)
                          )
                        )
                      ));
            return response.Hits.Select(s => s.Source).ToList();
        }

        // POST: Catalog/Create  
        [HttpPost]
        [Route("/CreateCatalog")]
        public async Task<Catalog> Create(Catalog catalog)
        {
            catalog.Id = Guid.NewGuid().ToString();
            var uris = new[]
            {
                new Uri("http://elastic:NlPYZIUsFzkRivtq-wWC@localhost:9200")
            };

            var connectionPool = new SniffingConnectionPool(uris);
            var settings = new ConnectionSettings(connectionPool)
                .DefaultIndex("catalog");

            var client = new ElasticClient(settings);

            var indexResponse = client.IndexDocument(catalog);

            var asyncIndexResponse = await client.IndexDocumentAsync(catalog);
            return null;
        }
    }
}
