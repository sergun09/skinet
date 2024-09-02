using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using WebApi.Dtos;
using WebApi.Extensions;
using WebApi.RequestHelpers;

namespace WebApi.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class AdminController(IUnitOfWork unitOfWork, IPaymentService paymentService) : ControllerBase
{
    [HttpGet("orders")]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrders([FromQuery] OrderSpecParams specParams)
    {
        var spec = new OrderSpecification(specParams);

        var orders = await unitOfWork.Repository<Order>().ListAsync(spec);

        var count = await unitOfWork.Repository<Order>().CountAsync(spec);

        var ordersDto = orders.Select(o => o.ToDto()).ToList();

        var pagination = new Pagination<OrderDto>
        {
            PageIndex = specParams.PageIndex,
            PageSize = specParams.PageSize,
            Count = count,
            Data = ordersDto
        };

        return Ok(pagination);
    }

    [HttpGet("orders/{id:int}")]
    public async Task<ActionResult<OrderDto>> GetOrder(int id )
    {
        var spec = new OrderSpecification(id);

        var order = await unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

        if(order is null) return NotFound($"La commande avec l'id {id} n'existe pas !");

        return Ok(order.ToDto());
    }

    [HttpPost("orders/refund/{id:int}")]
    public async Task<ActionResult<OrderDto>> RefundOrder(int id)
    {
        var spec = new OrderSpecification(id);

        var order = await unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

        if (order is null) return NotFound($"La commande avec l'id {id} n'existe pas !");

        if (order.OrderStatus == OrderStatus.Pending) return BadRequest("Paiement non reçu pour cette commande !");

        var result = await paymentService.RefundPayment(order.PaymentIntentId);

        if (result == "succeeded")
        {
            order.OrderStatus = OrderStatus.Refunded;
            await unitOfWork.SaveChanges();
            return Ok(order.ToDto());
        }

        return BadRequest("Problème lors du remboursement de la commande");
    }
}
