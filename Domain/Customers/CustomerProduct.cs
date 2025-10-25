using Domain.AbstactClass;

namespace Domain.Customers;

public class CustomerProduct : AuditableEntity
{
    public TenantCustomerProfile Customer { get; set; }
    public Guid CustomerId { get; set; }
    public Product Product { get; set; }
    public int ProductId { get; set; }
    public decimal SalesRate { get; set; }   

}
