using Catalog.Api.Model;

namespace Catalog.Api.Repositories
{
    public interface IProductRepository
    {
        Task<IReadOnlyCollection<Product>?> GetProducts();
        Task<Product?> GetProductById(string id);
        Task<IReadOnlyCollection<Product>?> GetProductsByName(string name);
        Task<IReadOnlyCollection<Product>?> GetProductsByCategory(string category);
        Task<bool> CreateProduct(Product product);
        Task<bool> DeleteProduct(string id);
        Task<bool> UpdateProduct(Product product);
    }
}
