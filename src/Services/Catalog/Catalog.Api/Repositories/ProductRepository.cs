using Catalog.Api.Data;
using Catalog.Api.Model;

using MongoDB.Driver;

namespace Catalog.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IProductDbContext _context;
        private readonly ILogger<ProductRepository> _logger;


        public ProductRepository(IProductDbContext context, ILogger<ProductRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task<bool> CreateProduct(Product product)
        {
            try
            {
                //Check if database is valid
                if (product is null) return false;

                //Add product to database
                await _context.Products.InsertOneAsync(product);

                _logger.LogInformation($"Product=[{product.Id}] has been added to the database");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }


        public async Task<bool> DeleteProduct(string productId)
        {
            try
            {
                //Get products from database and filter according to criteria
                FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, productId);

                var deleteResult = await _context.Products.DeleteOneAsync(filter);

                if (!deleteResult.IsAcknowledged && deleteResult.DeletedCount <= 0)
                    return false;

                _logger.LogInformation($"Product[{productId}] has been deleted from the database.");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return false;
            }
        }


        public async Task<IReadOnlyCollection<Product>?> GetProducts()
        {
            try
            {
                //Get products from database
                List<Product>? products = await _context.Products.Find(p => true).ToListAsync();

                //Check if result is empty
                if (products is null) return null;

                _logger.LogInformation($"Products returned: [{products.Count}]");

                //Return products found
                return (IReadOnlyCollection<Product>?)products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }


        public async Task<IReadOnlyCollection<Product>?> GetProductsByCategory(string category)
        {
            //Get products from database and filter according to criteria
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, category);

            List<Product>? foundCategories = await _context.Products.Find(filter).ToListAsync();

            //Check if result is empty
            if (foundCategories is null) return null;

            _logger.LogInformation($"Categories returned from database");

            foreach (var item in foundCategories)
                _logger.LogInformation($"Product=[{item.Id} - {item.Category}]");

            //Return categories found
            return foundCategories;
        }


        public async Task<Product?> GetProductById(string id)
        {
            try
            {
                //Get products from database and filter according to criteria
                FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);

                Product? product = await _context.Products.Find(filter).FirstOrDefaultAsync();

                //Check if result is empty
                if (product is null) return null;

                _logger.LogInformation($"Products returned from database");
                _logger.LogInformation($"[{product.Id} - {product.Name}] - {product.Price:c2}");

                //Return product found
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }


        public async Task<IReadOnlyCollection<Product>?> GetProductsByName(string name)
        {
            try
            {
                //Get products from database and filter according to criteria
                FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);

                List<Product>? products = await _context.Products.Find(filter).ToListAsync();

                //Check if result is empty
                if (products is null) return null;

                _logger.LogInformation($"Products returned from database");

                foreach (var item in products)
                    _logger.LogInformation($"Product=[{item.Id} - {item.Category}]");

                //Return product found
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }


        public async Task<bool> UpdateProduct(Product product)
        {
            try
            {
                var updateResult = await _context.Products.ReplaceOneAsync(toUpdate => toUpdate.Id!.Equals(product.Id), product);

                if (!updateResult.IsAcknowledged && updateResult.ModifiedCount <= 0)
                    return false;

                _logger.LogInformation("Successfully updated product!");

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return false;
            }
        }
    }
}