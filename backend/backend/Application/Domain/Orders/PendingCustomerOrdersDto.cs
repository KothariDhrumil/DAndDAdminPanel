namespace Application.Domain.Orders;

public class PendingCustomerOrdersDto
{
    public int Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime OrderPlacedDate { get; set; }
    public decimal Amount { get; set; }
    public decimal GrandTotal { get; set; }    
    public required string CustomerName { get; set; }
    public string? RouteName { get; set; }
}
