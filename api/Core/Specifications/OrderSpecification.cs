using Core.Entities;
using System.Text.Json.Serialization.Metadata;

namespace Core.Specifications;

public class OrderSpecification : BaseSpecification<Order>
{
    public OrderSpecification(string email) : base(o => o.BuyerEmail == email)
    {
        AddInclude(o => o.DeliveryMethod);
        AddInclude(o => o.OrderItems);
        AddOrderByDescending(o => o.OrderDate);
    }

    public OrderSpecification(string email, int id) : base(o => o.BuyerEmail == email && o.Id == id)
    {
        AddIncludeString("OrderItems");
        AddIncludeString("DeliveryMethod");
    }

    public OrderSpecification(string paymentIntentId, bool isPaymentIntent) : 
        base(o => o.PaymentIntentId == paymentIntentId)
    {
        AddIncludeString("OrderItems");
        AddIncludeString("DeliveryMethod");
    }

    public OrderSpecification(OrderSpecParams orderSpecParams) : base(o => 
        string.IsNullOrEmpty(orderSpecParams.Status) || o.OrderStatus == ParseStatus(orderSpecParams.Status)
    )
    {
        AddIncludeString("OrderItems");
        AddIncludeString("DeliveryMethod");
        AddPaging(orderSpecParams.PageSize * (orderSpecParams.PageIndex - 1), orderSpecParams.PageSize);
        AddOrderByDescending(o => o.OrderDate);
    }

    public OrderSpecification(int id) : base(o => o.Id == id)
    {
        AddIncludeString("OrderItems");
        AddIncludeString("DeliveryMethod");
    }

    private static OrderStatus? ParseStatus(string status)
    {
        if (Enum.TryParse<OrderStatus>(status, true, out var result))
        {
            return result;
        }
        return null;
    }
}
