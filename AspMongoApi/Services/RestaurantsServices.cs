using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspMongoApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;

namespace AspMongoApi.Services
{
    public class RestaurantsServices
    {
        private readonly IMongoCollection<Restaurant> _restaurantsCollection;
        public RestaurantsServices(IOptions<RestaurantsDbSetting> restaurantsDatabaseSettings) {
            var mongoClient = new MongoClient(
                restaurantsDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                restaurantsDatabaseSettings.Value.DatabaseName);

            _restaurantsCollection = mongoDatabase.GetCollection<Restaurant>(
                restaurantsDatabaseSettings.Value.CollectionName);
        }

        // public async Task<List<Restaurant>> GetAsync() =>
        //     await _restaurantsCollection.Find(_ => true).ToListAsync();

        public async Task<Restaurant> GetRestaurant(double lat, double lng)
        {
            try
            {
                lat = 40.78147;
                lng = -73.839297;
                var point = GeoJson.Point(GeoJson.Geographic(lng, lat));
                var locationQuery = new FilterDefinitionBuilder<Restaurant>().Near(tag => tag.Location, point,
                    50); //fetch results that are within a 50 metre radius of the point we're searching.
                var query = _restaurantsCollection.Find(locationQuery).Limit(10); //Limit the query to return only the top 10 results.
                return await query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                //do something;
            }
            return null;
        }

        // public async Task<Restaurant?> GetAsync(string id) =>
        //     await _restaurantsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        // public async Task CreateAsync(Restaurant newRestaurant) =>
        //     await _restaurantsCollection.InsertOneAsync(newRestaurant);

        // public async Task UpdateAsync(string id, Restaurant updatedRestaurant) =>
        //     await _restaurantsCollection.ReplaceOneAsync(x => x.Id == id, updatedRestaurant);

        // public async Task RemoveAsync(string id) =>
        //     await _restaurantsCollection.DeleteOneAsync(x => x.Id == id);
    }
}