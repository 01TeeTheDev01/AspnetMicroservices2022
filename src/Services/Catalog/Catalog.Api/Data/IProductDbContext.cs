using Catalog.Api.Model;

using MongoDB.Driver;

namespace Catalog.Api.Data
{
    public interface IProductDbContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
