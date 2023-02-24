using ElasticProject.Data.Entity;
using ElasticProject.Data.Entity.MapObj;
using Nest;

namespace ElasticProject.Data.Interface
{
    public interface IElasticsearchService
    {
        Task<string> CreateIndex(string indexName);
        Task InsertDocument(string indexName, Cities cities);
        Task<string> DeleteIndex(string indexName);
        Task DeleteByIdDocument(string indexName, Cities cities);
        Task InsertBulkDocuments(string indexName, List<Cities> cities);
        Task<Cities> GetDocument(string indexName, string id);
        Task<List<Cities>> GetDocuments(string indexName);
        Task<List<Cities>> GetAllDocument(string indexName);
    }
}
