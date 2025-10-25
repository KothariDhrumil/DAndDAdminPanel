using Domain.AbstactClass;
using Domain.Customers;

namespace Domain.Purchase
{
    public class Purchase : AuditableEntity
    {
        public int? RouteId { get; set; }
        public Route Route { get; set; } = default!;

        // PurchaseUnit can represent a warehouse OR a manufacturing unit
        public int PurchaseUnitId { get; set; }
        public PurchaseUnit PurchaseUnit { get; set; } = default!;

        public DateTime PurchaseDate { get; set; }
        public DateTime? OrderPickupDate { get; set; }

        public decimal Amount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Tax { get; set; }
        public decimal? AdditionalTax { get; set; }
        public decimal GrandTotal { get; set; }

        public string? Remarks { get; set; }
        public bool IsConfirmed { get; set; }

        public Guid? PickupSalesmanId { get; set; }
        public TenantUserProfile? PickupSalesman { get; set; }

        public ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();
    }
}
