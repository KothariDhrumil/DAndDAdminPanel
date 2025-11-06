using Domain.AbstactClass;

namespace Domain.Customers;

public class CustomerRoute : AuditableEntity
{
    public TenantCustomerProfile Customer { get; set; }
    public Guid CustomerId { get; set; }
    public Route Route { get; set; }
    public int RouteId { get; set; }
    public int OrderId { get; set; }
}