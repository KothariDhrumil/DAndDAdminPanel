namespace Application.Domain.Orders.UndeliveredCustomerOrder;

public sealed class UndeliveredCustomerOrderItemDto
{
    public int Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime OrderPlacedDate { get; set; }
    public DateTime OrderDeliveryDate { get; set; }
    public decimal Amount { get; set; }
    public decimal? Discount { get; set; }
    public string? Remarks { get; set; }
    public decimal? ParcelCharge { get; set; }
    public List<UndeliveredCustomerOrderDetailItemDto> CustomerOrderDetails { get; set; } = new();
}

public sealed class UndeliveredCustomerOrderDetailItemDto
{
    public required int ProductId { get; set; }
    public required string ProductName { get; set; }
    public required int Qty { get; set; }
    public required decimal SalesRate { get; set; }
}