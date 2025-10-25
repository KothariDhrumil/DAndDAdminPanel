using Domain.AbstactClass;
using Domain.Customers;

namespace Domain.Purchase
{
    public class PurchaseUnitProduct : AuditableEntity
    {
        public int PurchaseUnitId { get; set; }
        public PurchaseUnit PurchaseUnit { get; set; } = default!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;

        public decimal PurchaseRate { get; set; }
        public decimal? Tax { get; set; }        
    }
}
