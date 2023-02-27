using Nest;

namespace Elastic02.Services
{
    public interface IElasticGeoRepository<T> where T : class
    {
        Task<CreateIndexResponse> CreateIndexGeoAsync();
        Task<bool> BulkAsync(IEnumerable<T> objects);
        Task<List<string>> GetDataSearchGeo(double lat, double ln, GeoDistanceType type, string distance, int pageSize);
    }
}
