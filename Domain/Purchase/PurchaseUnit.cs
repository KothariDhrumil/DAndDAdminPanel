using Domain.AbstactClass;
using Domain.Customers;

namespace Domain.Purchase
{
    public class PurchaseUnit : AuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        // Indicates if this is our own warehouse or an external manufacturing unit
        public bool IsInternal { get; set; }
        public string? Address { get; set; }

        public TenantUserProfile TenantUserProfile { get; set; }
        public Guid TenantUserProfileId { get; set; }

        public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
    }
}
