using Application.Abstractions.Messaging;
using Application.Abstractions.Persistence;
using Application.Domain.Orders.CreateCustomerOrder;

namespace Application.Domain.Orders.UpdateCustomerOrder;

public sealed class UpdateCustomerOrderCommand : ICommand<bool>
{
    public int OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public decimal? Discount { get; set; }
    public string? Remarks { get; set; }
    public decimal? ParcelCharge { get; set; }
    public List<CustomerOrderDetailDto> CustomerOrderDetails { get; set; } = new();
    public bool IsPreOrder { get; set; }
    public DateTime? OrderDeliveryDate { get; set; }
    public bool? IsDelivered { get; set; }
}
