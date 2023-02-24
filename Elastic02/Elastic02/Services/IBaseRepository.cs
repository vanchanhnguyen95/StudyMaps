using System.Threading.Tasks;

namespace Elastic02.Services
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
