using Elastic.Demo.Api.Models;

namespace Elastic.Demo.Api.Interface
{
    /// <summary>
    /// An interface which helps to do intermediate calculation before projecting calls to the core component. 
    /// </summary>
    /// <typeparam name="T">Any class which decorates with ElasticsearchType and Description attribute.</typeparam>
    public interface IElasticService<T> where T : class
    {
        Task<bool> CreateIndexAsync();
        Task DeleteIndexAsync();
        Task DeleteIndexAsync(string indexName);
        Task RefreshIndex();
        Task<IndexDetail> GetIndexAsync();
        Task<List<IndexDetail>> GetAllIndicesAsync();
        Task AddDocumentAsync(T value);
        Task DeleteDocumentAsync(T value);
        Task UpdateDocumentAsync(T value);
        Task UpsertDocumentAsync(T value);
        Task<T> GetDocumentAsync(string id);
        Task<(long Count, IEnumerable<T> Documents)>
            GetDocumentsAsync(GridQueryModel gridQueryModel);
        Task<long> GetDocumentsCount();
        Task<bool> IsIndexExists();
        Task<CommonStats> StatsAggregationAsync(string key, string fieldName);
        Task<List<GroupStats>> GroupByAsync(string fieldName);
    }
}
