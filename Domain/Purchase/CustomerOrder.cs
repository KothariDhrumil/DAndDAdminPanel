using Domain.AbstactClass;
using Domain.Customers;

namespace Domain.Purchase
{
    public class CustomerOrder : AuditableEntity
    {
        public Guid CustomerId { get; set; }
        public TenantCustomerProfile Customer { get; set; } = default!;

        public int? RouteId { get; set; }
        public Route? Route { get; set; }

        public DateTime OrderPlacedDate { get; set; }
        public DateTime OrderDeliveryDate { get; set; }

        public bool IsDelivered { get; set; }

        public Guid? SalesManId { get; set; }
        public TenantUserProfile? SalesMan { get; set; }

        public decimal Amount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Tax { get; set; }
        public decimal GrandTotal { get; set; }

        public string? Remarks { get; set; }
        public string? InvoiceNumber { get; set; }

        public decimal? ParcelCharge { get; set; }

        public ICollection<CustomerOrderDetail> CustomerOrderDetails { get; set; } = new List<CustomerOrderDetail>();

        // Pre-order or direct sale flag
        public bool IsPreOrder { get; set; }

        // Optional link to a parent payer (payment by higher hierarchy)
        public Guid? PayerCustomerId { get; set; }
        public TenantCustomerProfile? PayerCustomer { get; set; }
    }
}
