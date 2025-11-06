using Domain.AbstactClass;
using Domain.Customers;

namespace Domain.Purchase
{
    public class PurchaseDetail : AuditableEntity
    {
        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; } = default!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;

        public int Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal? Tax { get; set; }
        public decimal Amount { get; set; }
    }
}
