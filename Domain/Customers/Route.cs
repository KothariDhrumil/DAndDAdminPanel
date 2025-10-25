using Domain.AbstactClass;

namespace Domain.Customers;

public class Route : AuditableEntity
{
    public string Name { get; set; }
    public virtual TenantUserProfile TenantUser { get; set; }
    public Guid TenantUserId { get; set; }
    public bool IsActive { get; set; }


    public int? PriceTierId { get; set; }
    public PriceTier PriceTier { get; set; } = default!;

    // Each route has many customers
    public ICollection<TenantCustomerProfile> Customers { get; set; } = new List<TenantCustomerProfile>();

}
