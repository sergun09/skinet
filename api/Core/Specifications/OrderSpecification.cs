using Core.Entities;

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
}
