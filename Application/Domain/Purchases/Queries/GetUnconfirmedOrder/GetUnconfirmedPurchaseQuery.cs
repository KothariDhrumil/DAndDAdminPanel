using Application.Abstractions.Messaging;
using Application.Domain.Purchases.Commands.Update;

namespace Application.Domain.Purchases.Queries.GetUnconfirmedOrder;

public sealed record GetUnconfirmedPurchaseQuery(int RouteId, int PurchaseUnitId) : IQuery<UnconfirmedPurchaseResponse?>;

public class UnconfirmedPurchaseResponse
{
    public int? Id { get; set; }
    public int? RouteId { get; set; }
    public int PurchaseUnitId { get; set; }
    public decimal Amount { get; set; }
    public decimal? Discount { get; set; }
    public decimal? Tax { get; set; }
    public decimal? AdditionalTax { get; set; }
    public decimal GrandTotal { get; set; }
    public string? Remarks { get; set; }
    public bool IsPreOrder { get; set; }
    public PurchaseTypeDTO Type { get; set; }
    public List<UnconfirmedPurchaseDetailDto> PurchaseDetails { get; set; } = new();
}
public class UnconfirmedPurchaseDetailDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Qty { get; set; }
    public decimal PurchaseRate { get; set; }
    public decimal? Tax { get; set; }
    public decimal Amount { get; set; }
}
