using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepository;

        public PaymentController(IPaymentService paymentService, IGenericRepository<DeliveryMethod> deliveryMethod)
        {
            _paymentService = paymentService;
            _deliveryMethodRepository = deliveryMethod;
        }

        [Authorize]
        [HttpPost("{cartId}")]
        public async Task<ActionResult<ShoppingCart>> CreateOrUpdatePaymentIntent(string cartId)
        {
            var cart = await _paymentService.CreateOrUpdatePaymentIntent(cartId);

            if (cart is null) return BadRequest("Problème avec votre panier");

            return Ok(cart);
        }

        [HttpGet("delivery-methods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await _deliveryMethodRepository.GetAllAsync());
        }

    }
}
