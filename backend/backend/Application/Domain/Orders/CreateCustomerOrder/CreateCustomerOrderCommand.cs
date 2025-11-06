using Application.Abstractions.Messaging;

namespace Application.Domain.Orders.CreateCustomerOrder;

public sealed class CustomerOrderDetailDto
{
    public int ProductId { get; set; }
    public int Qty { get; set; }
}

public sealed class CreateCustomerOrderCommand : ICommand<int>
{
    public Guid CustomerId { get; set; }
    public decimal? Discount { get; set; }
    public string? Remarks { get; set; }
    public decimal? ParcelCharge { get; set; }
    public List<CustomerOrderDetailDto> CustomerOrderDetails { get; set; } = new();
    public bool IsPreOrder { get; set; }
}
