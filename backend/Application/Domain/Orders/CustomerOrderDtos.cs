namespace Application.Domain.Orders;

public sealed class CustomerOrderDetailItemDto
{
    public string ProductName { get; set; }
    public int ProductId { get; set; }
    public int Qty { get; set; }
    public decimal SalesRate { get; set; }
    public decimal Amount { get; set; }

}

public sealed class CustomerOrderItemDto
{
    public int Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime OrderPlacedDate { get; set; }
    public DateTime OrderDeliveryDate { get; set; }
    public bool IsDelivered { get; set; }
    public decimal Amount { get; set; }
    public decimal? Discount { get; set; }
    public decimal? Tax { get; set; }
    public decimal GrandTotal { get; set; }
    public decimal? ParcelCharge { get; set; }
    public bool IsPreOrder { get; set; }
    public required string CustomerName { get; set; }
    public string? RouteName { get; set; }

    public Guid CreatedBy { get; set; }
}


public class GetCustomerOrderByIdDto
{
    public int Id { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = null!;
    public int? RouteId { get; set; }
    public decimal Amount { get; set; }
    public decimal? Discount { get; set; }
    public decimal GrandTotal { get; set; }
    public string? Remarks { get; set; }
    public string? InvoiceNumber { get; set; }
    public decimal? ParcelCharge { get; set; }
    public bool IsPreOrder { get; set; }

    public List<CustomerOrderDetailItemDto> CustomerOrderDetails { get; set; } = new();
}