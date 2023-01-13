using BAGeocoding.Api.Contracts;
using BAGeocoding.Api.Models;
using BAGeocoding.Entity.MapObj;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Runtime;

namespace BAGeocoding.Api.Services
{

    public interface IDistrictDataService
    {
        // CreateAsync
        Task<DistrictData> CreateAsync(DistrictData districtData);
    }

    public class DistrictDataService
    {
        private readonly IMongoCollection<DistrictData> _collection;
        public DistrictDataService(IOptions<GeoDatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(
            databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                databaseSettings.Value.DatabaseName);

            _collection = mongoDatabase.GetCollection<DistrictData>(
                databaseSettings.Value.DistrictDatasCollectionName);
        }

        public async Task<DistrictData> CreateAsync(DistrictData districtData)
        {
            await _collection.InsertOneAsync(districtData);
            return districtData;
        }
    }

}
