using Domain.AbstactClass;
using Domain.Customers;

namespace Domain.Purchase
{
    public class PurchaseUnit : AuditableEntity
    {
        public required string Name { get; set; }
        // Indicates if this is our own warehouse or an external manufacturing unit
        public bool IsInternal { get; set; }
        public string? Address { get; set; }
        public bool IsTaxable { get; set; }
        public TenantUserProfile? TenantUser { get; set; }
        public Guid? TenantUserId { get; set; } 
        public ICollection<PurchaseUnitProduct> Products { get; set; } = new List<PurchaseUnitProduct>();
        public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
    }
}
