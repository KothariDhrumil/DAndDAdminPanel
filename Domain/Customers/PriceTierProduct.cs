using Domain.AbstactClass;

namespace Domain.Customers;

public class PriceTierProduct : AuditableBaseEntity
{
    public int PriceTierId { get; set; }
    public PriceTier PriceTier { get; set; } = default!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = default!;
    public decimal SalesRate { get; set; }
}
