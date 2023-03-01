using Nest;

namespace Elastic02.Services
{
    public interface IElasticGeoRepository<T> where T : class
    {
        Task<CreateIndexResponse> CreateIndexGeoAsync();
        Task<bool> BulkAsync(IEnumerable<T> objects);
    }
}
