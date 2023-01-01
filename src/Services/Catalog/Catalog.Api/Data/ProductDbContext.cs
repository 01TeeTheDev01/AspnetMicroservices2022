using Catalog.Api.Model;

using MongoDB.Driver;

namespace Catalog.Api.Data
{
    public class ProductDbContext : IProductDbContext
    {
        public IMongoCollection<Product> Products { get; }

        public ProductDbContext(IConfiguration config)
        {
            //Create Db client and get connection string from appsettings.json file
            var client = new MongoClient(config.GetValue<string>("DatabaseSettings:ConnectionString"));

            //Get database connstring from appsettings.json file
            var database = client.GetDatabase(config.GetValue<string>("DatabaseSettings:DatabaseName"));

            //Get collection name string from appsettings.json file
            Products = database.GetCollection<Product>(config.GetValue<string>("DatabaseSettings:CollectionName"));

            //Seed init table data
            ProductDbContextSeed.SeedData(Products);
        }
    }
}
