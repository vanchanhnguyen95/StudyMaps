using BAGeocoding.Api.Contracts;
using BAGeocoding.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Runtime;

namespace BAGeocoding.Api.Services
{

    public interface IBAGDistrictAddService
    {
        // CreateAsync
        Task<BAGDistrictAdd> CreateAsync(BAGDistrictAdd baGDistrict);
    }

    public class BAGDistrictAddService
    {
        private readonly IMongoCollection<BAGDistrictAdd> _collection;
        public BAGDistrictAddService(IOptions<GeoDatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(
            databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                databaseSettings.Value.DatabaseName);

            _collection = mongoDatabase.GetCollection<BAGDistrictAdd>(
                databaseSettings.Value.BAGDistrictAddsCollectionName);
        }

        public async Task<BAGDistrictAdd> CreateAsync(BAGDistrictAdd baGDistrict)
        {
            await _collection.InsertOneAsync(baGDistrict);
            return baGDistrict;
        }
    }

}
