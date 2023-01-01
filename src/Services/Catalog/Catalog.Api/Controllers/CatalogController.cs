using System.Net;

using Catalog.Api.Model;
using Catalog.Api.Repositories;

using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [Route("api/v1/[controller]/")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ILogger<CatalogController> _logger;
        private readonly IProductRepository _repository;

        public CatalogController(ILogger<CatalogController> logger, IProductRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IReadOnlyCollection<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IReadOnlyCollection<Product>?>> GetProducts()
        {
            return Ok(await _repository.GetProducts());
        }


        [HttpGet]
        [Route("[action]/")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IReadOnlyCollection<Product>?>> GetProductById([FromQuery] string id)
        {
            var product = await _repository.GetProductById(id);

            if (product is null) return NotFound($"Product with [{id}] does not exist in the database.");

            return Ok(product);
        }


        [HttpGet]
        [Route("[action]/")]
        [ProducesResponseType(typeof(IReadOnlyCollection<Product>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IReadOnlyCollection<Product>?>> GetProductsByName([FromQuery] string name)
        {
            var products = await _repository.GetProductsByName(name);

            if (products is null || !products.Any()) return NotFound($"Products with name [{name}] do not exist in the database.");

            return Ok(products);
        }


        [HttpGet]
        [Route("[action]/")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IReadOnlyCollection<Product>?>> GetProductsByCategory([FromQuery] string name)
        {
            var products = await _repository.GetProductsByCategory(name);

            if (products is null || !products.Any()) return NotFound($"Products with category [{name}] do not exist in the database.");

            return Ok(products);
        }


        [HttpPost]
        [Route("[action]/")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult<bool>> CreateProduct([FromBody] Product newProduct)
        {
            bool isAdded = await _repository.CreateProduct(newProduct);

            if (!isAdded) return NoContent();

            return Ok($"Product [{newProduct.Id} - {newProduct.Name}] has been saved to the database.");
        }


        [HttpPut]
        [Route("[action]/")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public async Task<ActionResult<bool>> UpdateProduct([FromBody] Product productUpdate)
        {
            bool products = await _repository.UpdateProduct(productUpdate);

            if (!products) return Conflict($"Failed to update product [{productUpdate.Name}].");

            return Ok($"Product [{productUpdate.Name}] has been updated successfully.");
        }


        [HttpDelete]
        [Route("[action]/")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IReadOnlyCollection<Product>?>> DeleteProduct([FromQuery] string name)
        {
            bool isDeleted = await _repository.DeleteProduct(name);

            if (!isDeleted) return NotFound($"Product [{name}] does not exist in the database.");

            return Ok($"Product [{name}] has been removed from the database.");
        }
    }
}
