using MarvelCharacters.API.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarvelCharacters.API.Services.Db
{
    public class MongoDatabase
    {
        private const string CHARACTER_COLLECTION_NAME = "character";

        private readonly ILogger<MongoDatabase> _logger;

        private readonly string _connectionString;

        public MongoDatabase(ILogger<MongoDatabase> logger,
            string connectionString)
        {
            _logger = logger;
            _connectionString = connectionString;
        }

        static IMongoDatabase GetDatabase(string connectionString, string database)
        {
            var client = new MongoClient(connectionString);
            return client.GetDatabase(database);
        }

        /// <inheritdoc />
        public async Task<Character> AddLike(Character data)
        {
            _logger.LogInformation("Adding like to db");

            var collection = GetDatabase(_connectionString, "Marvel")
                .GetCollection<Character>(CHARACTER_COLLECTION_NAME);

            await collection.InsertOneAsync(data, new InsertOneOptions());

            return data;
        }

        /// <inheritdoc />
        public Task RemoveLike(int id)
        {
            _logger.LogInformation("Removing like from db");

            var collection = GetDatabase(_connectionString, "Marvel")
                .GetCollection<Character>(CHARACTER_COLLECTION_NAME);

            return collection.DeleteOneAsync(x => x.Id == id);
        }

        /// <inheritdoc />
        public async Task<List<Character>> GetLikes()
        {
            _logger.LogInformation("Getting characters from db");

            try
            {
                var collection = GetDatabase(_connectionString, "Marvel")
                .GetCollection<Character>(CHARACTER_COLLECTION_NAME);

                return await collection.AsQueryable().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when getting list of characters");
                throw;
            }

        }
    }
}