using Domain.AbstactClass;

namespace Domain.Customers;

public class CustomerPriceTier : AuditableBaseEntity
{
    public Guid CustomerId { get; set; }
    public TenantCustomerProfile Customer { get; set; } = default!;
    public int PriceTierId { get; set; }
    public PriceTier PriceTier { get; set; } = default!;
}
