using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/carts")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        
        [HttpGet]
        public async Task<ActionResult<ShoppingCart>> GetCartById(string id)
        {
            var cart = await _cartService.GetCartAsync(id);

            return Ok(cart ?? new ShoppingCart() { Id = id });
        }
        
        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateCart(ShoppingCart cart)
        {
            var updatedCart = await _cartService.SetCartAsync(cart);

            if (updatedCart == null)
                return BadRequest("Problème avec la mise à jour de votre panier");

            return Ok(updatedCart);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCart(string id)
        {
            var result = await _cartService.DeleteAsync(id);

            return Ok(result ? Ok() : BadRequest("Problème avec la supression de votre panier")) ;
        }

    }
}
