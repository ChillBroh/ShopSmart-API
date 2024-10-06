using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System.Numerics;
using webApi.Models;

namespace webApi.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
            _database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);
        }

        public IMongoCollection<ApplicationUser> Users => _database.GetCollection<ApplicationUser>("Users");
        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
        public IMongoCollection<Order> Orders => _database.GetCollection<Order>("Orders");

        public IMongoCollection<RatingAndReview> RatingAndReview => _database.GetCollection<RatingAndReview>("RatingAndReview");
        //public IMongoCollection<Vendor> Vendors => _database.GetCollection<Vendor>("Vendors");
    }
}
