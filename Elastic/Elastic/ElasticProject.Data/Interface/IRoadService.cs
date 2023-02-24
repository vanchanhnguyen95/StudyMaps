using ElasticProject.Data.Entity.MapObj;

namespace ElasticProject.Data.Interface
{
    public interface IRoadService
    {
        Task<List<BGCElasticRequestCreate>> GetRoads(string indexName);
        Task<string> InsertBulkElasticRequest(string indexName, List<BGCElasticRequestCreate> elasticRequestCreates);
        Task<List<string>> GetDataSuggestion(string indexName, string keyWord, int size);
    }
}
