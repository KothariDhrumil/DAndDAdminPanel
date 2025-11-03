using Application.Abstractions.Messaging;

namespace Application.Domain.Purchases.Commands.Create;

public sealed class CreatePurchaseCommand : ICommand<int>
{
    public int? RouteId { get; set; }
    public int PurchaseUnitId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public DateTime? OrderPickupDate { get; set; }
    public decimal Amount { get; set; }
    public decimal? Discount { get; set; }
    public decimal? Tax { get; set; }
    public decimal? AdditionalTax { get; set; }
    public decimal GrandTotal { get; set; }
    public string? Remarks { get; set; }
    public bool IsPreOrder { get; set; }
    public Guid? PickupSalesmanId { get; set; }
    public global::Domain.Purchase.PurchaseType Type { get; set; }
    public List<PurchaseDetailDto> PurchaseDetails { get; set; } = new();
}

public class PurchaseDetailDto
{
    public int ProductId { get; set; }
    public int Qty { get; set; }
    public decimal Rate { get; set; }
    public decimal? Tax { get; set; }
    public decimal Amount { get; set; }
}
