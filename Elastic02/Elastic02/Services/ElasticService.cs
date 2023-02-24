using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elastic02.Models;
using Nest;

namespace Elastic02.Services
{
    public class ElasticService<T> : IElasticService<T> where T : class
    {
        private readonly IElasticRepository<T> _elasticRepository;

        public ElasticService(IElasticRepository<T> elasticRepository) => _elasticRepository = elasticRepository;

        public async Task AddDocumentAsync(T value) => await _elasticRepository.AddDocumentAsync(value);

        public async Task<bool> CreateIndexAsync()
        {
            var result = await _elasticRepository.CreateIndexAsync();

            return result.IsValid;

        }

        public async Task DeleteDocumentAsync(T value) => await _elasticRepository.DeleteDocumentAsync(value);

        public async Task DeleteIndexAsync() => await _elasticRepository.DeleteIndexAsync();

        public async Task DeleteIndexAsync(string indexName) => await _elasticRepository.DeleteIndexAsync(indexName);
         
        public async Task<List<IndexDetail>> GetAllIndicesAsync()
        {
            var response = await _elasticRepository.GetAllIndicesAsync();

            return response.Indices.Select(q => new IndexDetail
            {
                Key = q.Key.Name,
                Values = q.Value.Settings.ToDictionary(e => e.Key, v => v.Value)
            }).ToList();
        }

        public async Task<T> GetDocumentAsync(string id) => await _elasticRepository.GetDocumentAsync(id);

        public async Task<(long Count, IEnumerable<T> Documents)> GetDocumentsAsync(GridQueryModel gridQueryModel) => await _elasticRepository.GetDocumentsAsync(gridQueryModel);

        public async Task<long> GetDocumentsCount() => await _elasticRepository.GetDocumentsCount();

        public async Task<IndexDetail> GetIndexAsync()
        {
            var response = await _elasticRepository.GetIndexAsync();

            return response.Indices?.Select(q => new IndexDetail
            {
                Key = q.Key.Name,
                Values = q.Value.Settings.ToDictionary(e => e.Key, v => v.Value)
            }).FirstOrDefault();
        }

        public async Task<List<GroupStats>> GroupByAsync(string fieldName) => await _elasticRepository.GroupByAsync(fieldName);

        public async Task<bool> IsIndexExists() => await _elasticRepository.IsIndexExists();

        public async Task RefreshIndex() => await _elasticRepository.RefreshIndex();

        public async Task<CommonStats> StatsAggregationAsync(string key, string fieldName)
        {
            var statsResult = await _elasticRepository.StatsAggregationAsync(key, fieldName);
            statsResult.Median = await _elasticRepository.MedianAggregationAsync(key, fieldName);
            return statsResult;
        }

        public async Task UpdateDocumentAsync(T value) => await _elasticRepository.UpdateDocumentAsync(value);

        public async Task UpsertDocumentAsync(T value) => await _elasticRepository.UpsertDocumentAsync(value);

        public async Task<bool> CreateIndexGeoAsync()
        {
            var result = await _elasticRepository.CreateIndexGeoAsync();

            return result.IsValid;
        }

        public async Task<bool> BulkAsync(IEnumerable<T> objects) => await _elasticRepository.BulkAsync(objects);
       
    }
}
