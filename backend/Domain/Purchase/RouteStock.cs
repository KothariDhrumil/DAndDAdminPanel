using Domain.AbstactClass;
using Domain.Customers;

namespace Domain.Purchase;

public class RouteStock : AuditableEntity
{
    public int RouteId { get; set; }
    public Route Route { get; set; } = default!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = default!;
    public DateTime Date { get; set; }
    public int OpeningBalance { get; set; }
    public int QtyReceivedFromGodown { get; set; }
    public int QtySold { get; set; }
    public int QtyReturnedToGodown { get; set; }
    public int Waste { get; set; }
    public int Loss { get; set; }
    public int Sample { get; set; }
    public int InEating { get; set; }
    public int ClosingBalance { get; set; }
}
public class WarehouseStock : AuditableEntity
{

    public int ProductId { get; set; }
    public Product Product { get; set; }
    public DateTime Date { get; set; }
    public int OpeningBalanceQty { get; set; }
    public int QtyPurchased { get; set; }
    public int QtyTransferredToRoutes { get; set; }
    public int RouteReturn { get; set; }
    public int Waste { get; set; }
    public int Loss { get; set; }
    public int Sample { get; set; }
    public int ClosingBalanceQty { get; set; }

    public PurchaseUnit? PurchaseUnit { get; set; }
    public int? PurchaseUnitId { get; set; }
}

public class StockTransaction : AuditableEntity
{
    public int ProductId { get; set; }
    public int? RouteId { get; set; } // null for godown
    public StockTransactionType Type { get; set; } // e.g., Purchase, Sale, Waste, Return
    public int Quantity { get; set; }
    public DateTime TransactionDate { get; set; }
    public int? ReferenceId { get; set; } // OrderId, PurchaseId, etc.
}
public enum StockTransactionType
{
    PurchaseFromVendor,       // Vendor → Godown
    PurchaseFromWarehouse,        // Godown → Route
    SaleFromRoute,          // Route → Customer
    ReturnToGodown,         // Route → Godown
    ReturnToVendor,         // Godown → Vendor
    Waste, Loss, Sample,    // Ot
    SaleToRoute,
}