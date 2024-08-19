using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController(ICartService cartService, IUnitOfWork unitOfWork) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrdersForUser()
        {
            var spec = new OrderSpecification(User.GetEmail());

            var orders = await unitOfWork.Repository<Order>().ListAsync(spec);

            var ordersToReturn = orders.Select(o => o.ToDto()).ToList();

            return Ok(ordersToReturn);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            var spec = new OrderSpecification(User.GetEmail(), id);

            var order = await unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            if (order == null) return NotFound("Votre commande n'a pas été trouvée !");

            return Ok(order.ToDto());
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto orderDto)
        {
            var email = User.GetEmail();

            var cart = await cartService.GetCartAsync(orderDto.CartId);

            if (cart is null) return BadRequest("Panier non trouvé !");

            if (cart.PaymentIntentId == null) return BadRequest("Pas de paiement pour ce panier");

            var items = new List<OrderItem>();

            foreach (var item in cart.Items)
            {
                var productItem = await unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);
                if (productItem is null) return BadRequest("Problème avec votre commande !");

                var itemOrdered = new ProductItemOrdered()
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    PictureUrl = productItem.PictureUrl
                };

                var orderItem = new OrderItem()
                {
                    ItemOrdered = itemOrdered,
                    Price = productItem.Price,
                    Quantity = item.Quantity
                };

                items.Add(orderItem);
            }

            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(orderDto.DeliveryMethodId);

            if (deliveryMethod is null) return BadRequest("La méthode de livraison est vide !");

            var order = new Order()
            {
                OrderItems = items,
                DeliveryMethod = deliveryMethod,
                ShippingAddress = orderDto.ShippingAddress,
                SubTotal = items.Sum(x => x.Price * x.Quantity),
                PaymentSummary = orderDto.PaymentSummary,
                PaymentIntentId = cart.PaymentIntentId,
                BuyerEmail = email
            };


            unitOfWork.Repository<Order>().Add(order);

            if (!await unitOfWork.SaveChanges()) 
            {
                return BadRequest("Problème avec la création de votre commande !");
            }

            return order;
        } 
    }
}
