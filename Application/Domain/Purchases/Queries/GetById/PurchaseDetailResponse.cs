namespace Application.Domain.Purchases.Queries.GetById;

public class PurchaseDetailResponse
{
    public int Id { get; set; }
    public int? RouteId { get; set; }
    public string? RouteName { get; set; }
    public int PurchaseUnitId { get; set; }
    public string PurchaseUnitName { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public DateTime? OrderPickupDate { get; set; }
    public decimal Amount { get; set; }
    public decimal? Discount { get; set; }
    public decimal? Tax { get; set; }
    public decimal? AdditionalTax { get; set; }
    public decimal GrandTotal { get; set; }
    public string? Remarks { get; set; }
    public bool IsConfirmed { get; set; }
    public bool IsPreOrder { get; set; }
    public Guid? PickupSalesmanId { get; set; }
    public string? PickupSalesmanName { get; set; }
    public global::Domain.Purchase.PurchaseType Type { get; set; }
    public List<PurchaseDetailItemResponse> PurchaseDetails { get; set; } = new();
}

public class PurchaseDetailItemResponse
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Qty { get; set; }
    public decimal Rate { get; set; }
    public decimal? Tax { get; set; }
    public decimal Amount { get; set; }
}
