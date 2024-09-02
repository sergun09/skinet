using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Stripe;
using WebApi.Extensions;
using WebApi.SignalR;

namespace WebApi.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PaymentController> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IConfiguration _config;

        private readonly string _whSecret = "";

        public PaymentController(IPaymentService paymentService, IUnitOfWork unitOfWork, ILogger<PaymentController> logger, IHubContext<NotificationHub> hubContext, IConfiguration config)
        {
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _hubContext = hubContext;
            _config = config;
            _whSecret = _config["StripeSettings:WhSecret"]!;
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
            return Ok(await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync());
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = ConstructStripeEvent(json);

                if (stripeEvent.Data.Object is not PaymentIntent intent)
                    return BadRequest("Data de l'event invalide");

                await HandlePaymentIntentSucceededAsync(intent);

                return Ok();
            }
            catch(StripeException e)
            {
                _logger.LogError("Une erreur inattendu s'est produite avec le Webhook !");
                return StatusCode(StatusCodes.Status500InternalServerError, "Une erreur inattendu s'est produite !");
            }
        }

        private async Task HandlePaymentIntentSucceededAsync(PaymentIntent intent)
        {
            if(intent.Status == "succeeded")
            {
                var spec = new OrderSpecification(intent.Id, true);

                var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

                if (order is null) throw new Exception("Commande pas trouvée");

                var orderAmount = Decimal.ToInt64((order.SubTotal + order.DeliveryMethod.Price) * 100) ;

                if (orderAmount != intent.Amount)
                {
                    order.OrderStatus = OrderStatus.PaymentMismatch;
                }
                else
                {
                    order.OrderStatus = OrderStatus.PaymentReceived;
                }

                await _unitOfWork.SaveChanges();

                var connectionId = NotificationHub.GetConnectionIdByEmail(order.BuyerEmail);
                if (!string.IsNullOrEmpty(connectionId))
                {
                    await _hubContext.Clients.Client(connectionId).SendAsync("OrderCompleteNotification", order.ToDto());
                }
            }
        }

        private Event ConstructStripeEvent(string json)
        {
            try
            {
                return EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _whSecret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Impossible de créer l'évent Stripe !");
                throw new StripeException("Signature Invalide !");
            }
        }
    }
}
