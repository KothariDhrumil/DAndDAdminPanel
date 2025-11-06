using Domain.AbstactClass;
using Domain.Customers;

namespace Domain.Purchase
{
    public class CustomerOrderDetail : AuditableEntity
    {
        
        public int CustomerOrderId { get; set; }
        public CustomerOrder CustomerOrder { get; set; } = default!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;

        public int Qty { get; set; }
        public decimal Rate { get; set; }

        public decimal? CGST { get; set; }
        public decimal? IGST { get; set; }
        public decimal Amount { get; set; }
    }
}
