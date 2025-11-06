using Application.Abstractions.Messaging;
using Application.Domain.Orders.CreateCustomerOrder;

namespace Application.Domain.Orders.DeliverOrder;

public sealed class DeliverDirectSalesOrderCommand : ICommand<int>
{
    public Guid CustomerId { get; set; }
    public int? RouteId { get; set; }
    public decimal? Discount { get; set; }
    public string? Remarks { get; set; }
    public decimal? ParcelCharge { get; set; }
    public List<CustomerOrderDetailDto> CustomerOrderDetails { get; set; } = new();
    public bool IsPreOrder { get; set; }
}
