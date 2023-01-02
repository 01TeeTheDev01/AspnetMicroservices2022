using Basket.Api.Model;
using Basket.Api.Services;

using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers
{
    [Route("api/v1/[controller]/")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly ILogger<BasketController> _logger;

        public BasketController(IBasketRepository basketRepository, ILogger<BasketController> logger)
        {
            _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("[action]/")]
        [ProducesResponseType(typeof(ShoppingBasket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ShoppingBasket), StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<ShoppingBasket>> GetBasket([FromQuery] string userName)
        {
            try
            {
                if (string.IsNullOrEmpty(userName)) return BadRequest();

                var result = await _basketRepository.GetBasket(userName);

                //Create new basket if user's a first time shopper
                if (result is null) return Ok(new ShoppingBasket(userName));

                _logger.LogInformation("[{0}] has [{1}] cart(s).", userName, result.BasketItems.Count);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{0}", ex.Message);
                throw;
            }
        }


        [HttpPut]
        [Route("[action]/")]
        [ProducesResponseType(typeof(ShoppingBasket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ShoppingBasket), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ShoppingBasket), StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<ShoppingBasket?>> UpdateBasket([FromBody] ShoppingBasket basket)
        {
            try
            {
                if (basket is null) return BadRequest();

                ShoppingBasket? result = await _basketRepository.UpdateBasket(basket);

                if (result is null) return Problem($"Failed to update basket.");

                _logger.LogInformation("[{0}'s] basket has been updated.", result.UserName);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{0}", ex.Message);
                throw;
            }
        }

        [HttpDelete]
        [Route("[action]/")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteBasket([FromQuery] string userName)
        {
            try
            {
                if (string.IsNullOrEmpty(userName)) return BadRequest();

                await _basketRepository.DeleteBasket(userName);

                _logger.LogInformation("[{0}] has delete their basket.", userName);

                return Ok("Basket has been deleted from the database.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{0}", ex.Message);
                throw;
            }
        }
    }
}
