namespace Application.Domain.Orders;

public sealed class CustomerOrderDetailItemDto
{
    public int ProductId { get; set; }
    public int Qty { get; set; }
    public decimal Rate { get; set; }
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
    public string? Remarks { get; set; }
    public string? InvoiceNumber { get; set; }
    public decimal? ParcelCharge { get; set; }
    public bool IsPreOrder { get; set; }
    public Guid? PayerCustomerId { get; set; }
    public List<CustomerOrderDetailItemDto> CustomerOrderDetails { get; set; } = new();
}
