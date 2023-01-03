using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApiGeocoding.Models;
using WebApiGeocoding.ViewModels;

namespace WebApiGeocoding.Repositories
{
    public interface IHaNoiPointReporitory
    {
        // Create
        Task<ObjectId> Create(HaNoiPoint haNoiPoint);

        // Read
        Task<IEnumerable<HaNoiPointSearch>> AutoSuggestSearch(string keyword);
        Task<GeocodeHaNoiPointSearch> SearchByLatLong(double latitude, double longitude);
        Task<GeocodeHaNoiPointSearch> SearchByName(string name);
        Task<SpeedHaNoiPointSearch> SearchSpeed(double latitude, double longitude);

        Task<HaNoiPoint> Get(ObjectId objectId);
        Task<IEnumerable<HaNoiPoint>> Get();

        // Update
        Task<bool> UpdateSpeed(double latitude, double longitude, HaNoiPoint haNoiPoint);

        // Delete
        Task<bool> Delete(ObjectId objectId);
    }

    public class HaNoiPointReporitory : IHaNoiPointReporitory
    {
        private readonly IMongoCollection<HaNoiPoint> _haNoiPoints;

        public HaNoiPointReporitory(IMongoClient client)
        {
            var database = client.GetDatabase("geo-db");
            var collectionPoint = database.GetCollection<HaNoiPoint>(nameof(HaNoiPoint));

            _haNoiPoints = collectionPoint;
        }

        public async Task<IEnumerable<HaNoiPointSearch>> AutoSuggestSearch(string keyword)
        {
            var queryExpr = new BsonRegularExpression(new Regex(keyword, RegexOptions.None));

            var builder = Builders<HaNoiPoint>.Filter;
            var filter = builder.Regex("Name", queryExpr);
            List<HaNoiPoint> matchedDocuments = await _haNoiPoints.FindSync(filter).ToListAsync();
            var result = matchedDocuments.Select(
                o => new HaNoiPointSearch()
                { Latitude = o.Latitude, Longitude = o.Longitude, Name = o.Name }).Take(10);

            return (IEnumerable<HaNoiPointSearch>)result;
        }

        public async Task<GeocodeHaNoiPointSearch> SearchByLatLong(double latitude, double longitude)
        {
            var location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(
                new GeoJson2DGeographicCoordinates(longitude, latitude));
            var filter = Builders<HaNoiPoint>.Filter.Eq(c => c.Location, location);
            HaNoiPoint p = await _haNoiPoints.Find(filter).FirstOrDefaultAsync();
            if (p == null) return new GeocodeHaNoiPointSearch();

            GeocodeHaNoiPointSearch result = new GeocodeHaNoiPointSearch() { Name = p.Name, Street = p.Street, Ward = p.Ward, District = p.District, Province = p.Province };

            return result;
        }

        public async Task<GeocodeHaNoiPointSearch> SearchByName(string name)
        {
            var filter = Builders<HaNoiPoint>.Filter.Eq(c => c.Name, name);
            HaNoiPoint p = await _haNoiPoints.Find(filter).FirstOrDefaultAsync();

            if (p == null) return new GeocodeHaNoiPointSearch();

            GeocodeHaNoiPointSearch result = new GeocodeHaNoiPointSearch() { Name = p.Name, Street = p.Street, Ward = p.Ward, District = p.District, Province = p.Province };
            return result;
        }

        public async Task<ObjectId> Create(HaNoiPoint haNoiPoint)
        {
            await _haNoiPoints.InsertOneAsync(haNoiPoint);

            return haNoiPoint.Id;
        }

        public async Task<HaNoiPoint> Get(ObjectId objectId)
        {
            var filter = Builders<HaNoiPoint>.Filter.Eq(c => c.Id, objectId);
            var point = await _haNoiPoints.Find(filter).FirstOrDefaultAsync();

            return point;
        }

        public async Task<IEnumerable<HaNoiPoint>> Get()
        {
            var haNoiPoints = await _haNoiPoints.Find(_ => true).ToListAsync();

            return haNoiPoints;
        }

        public async Task<bool> UpdateSpeed(double latitude, double longitude, HaNoiPoint haNoiPoint)
        {
            var location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(
               new GeoJson2DGeographicCoordinates(longitude, latitude));
            var filter = Builders<HaNoiPoint>.Filter.Eq(c => c.Location, location);
            var update = Builders<HaNoiPoint>.Update
                //.Set(c => c.Latitude, haNoiPoint.Latitude)
                //.Set(c => c.Longitude, haNoiPoint.Longitude)
                //.Set(c => c.Street, haNoiPoint.Street)
                //.Set(c => c.Ward, haNoiPoint.Ward)
                //.Set(c => c.District, haNoiPoint.District)
                //.Set(c => c.Name, haNoiPoint.Name)
                //.Set(c => c.Location, haNoiPoint.Location)
                .Set(c => c.Speed, haNoiPoint.Speed);
            var result = await _haNoiPoints.UpdateOneAsync(filter, update);

            return result.ModifiedCount == 1;
        }

        public async Task<bool> Delete(ObjectId objectId)
        {
            var filter = Builders<HaNoiPoint>.Filter.Eq(c => c.Id, objectId);
            var result = await _haNoiPoints.DeleteOneAsync(filter);

            return result.DeletedCount == 1;
        }

        public async Task<SpeedHaNoiPointSearch> SearchSpeed(double latitude, double longitude)
        {
            var location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(
               new GeoJson2DGeographicCoordinates(longitude, latitude));
            var filter = Builders<HaNoiPoint>.Filter.Eq(c => c.Location, location);
            HaNoiPoint p = await _haNoiPoints.Find(filter).FirstOrDefaultAsync();
            if (p == null) return new SpeedHaNoiPointSearch();

            SpeedHaNoiPointSearch result = new SpeedHaNoiPointSearch() { Name = p.Name, Speed = p.Speed };

            return result;
        }
    }
}
