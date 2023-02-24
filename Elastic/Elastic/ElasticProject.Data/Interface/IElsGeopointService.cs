using ElasticProject.Data.Entity.MapObj;

namespace ElasticProject.Data.Interface
{
    public interface IElsGeopointService
    {
        Task<string> BulkAsync(string indexName, List<ElasticRequestPushGeopoint> elasticRequestCreates);
    }
}
