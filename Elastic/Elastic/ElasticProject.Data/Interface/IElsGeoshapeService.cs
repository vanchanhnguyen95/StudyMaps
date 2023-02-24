using ElasticProject.Data.Entity.MapObj;

namespace ElasticProject.Data.Interface
{
    public interface IElsGeoshapeService
    {
        Task<string> BulkAsyncElasticRequestGeoshape(string indexName, List<ElasticRequestPushGeoshape> geoshapesPush);
    }
}
