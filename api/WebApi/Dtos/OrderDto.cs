﻿using Core.Entities;

namespace WebApi.Dtos;

public class OrderDto
{
    public required int Id { get; set; }
    public DateTime OrderDate { get; set; } 
    public required string BuyerEmail { get; set; }
    public required ShippingAddress ShippingAddress { get; set; }
    public required string DeliveryMethod { get; set; }
    public decimal ShippingPrice { get; set; }
    public required PaymentSummary PaymentSummary { get; set; }
    public required List<OrderItemDto> OrderItems { get; set; } = new();
    public decimal SubTotal { get; set; }
    public decimal Total { get; set; }
    public required string OrderStatus { get; set; }
    public required string PaymentIntentId { get; set; }
}
