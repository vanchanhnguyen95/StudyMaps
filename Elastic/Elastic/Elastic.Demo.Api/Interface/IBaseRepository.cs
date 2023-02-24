namespace Elastic.Demo.Api.Interface
{
    public interface IBaseRepository
    {
        int NumberOfShards { get; set; }
        int NumberOfReplicas { get; set; }
        Task<long> GetDocumentsCount();
        Task<bool> IsIndexExists();
        Task RefreshIndex();
        Task DeleteIndexAsync(string indexName);
    }
}
