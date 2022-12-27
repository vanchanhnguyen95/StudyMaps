using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspMongoApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;


namespace AspMongoApi.Services
{
    public class StudentsService
    {
        private readonly IMongoCollection<Student> _studentsCollection;

        public StudentsService(IOptions<StudentsDbSettings> studentsDatabaseSettings) {
            var mongoClient = new MongoClient(
                studentsDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                studentsDatabaseSettings.Value.DatabaseName);

            _studentsCollection = mongoDatabase.GetCollection<Student>(
                studentsDatabaseSettings.Value.CollectionName);
        }

        public async Task<List<Student>> GetAsync() =>
            await _studentsCollection.Find(_ => true).ToListAsync();

        public async Task<Student?> GetAsync(string id) =>
            await _studentsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Student newStudent) =>
            await _studentsCollection.InsertOneAsync(newStudent);

        public async Task UpdateAsync(string id, Student updatedStudent) =>
            await _studentsCollection.ReplaceOneAsync(x => x.Id == id, updatedStudent);

        public async Task RemoveAsync(string id) =>
            await _studentsCollection.DeleteOneAsync(x => x.Id == id);
        }
}