using Application.Abstractions.Messaging;
using Application.Domain.Purchases.Commands.Create;
using Application.Domain.Purchases.Commands.Update;

namespace Application.Domain.Purchases.Commands.DeliverDirectPurchase;

public sealed class DeliverDirectPurchaseCommand : ICommand<int>
{
    public int? RouteId { get; set; }
    public int PurchaseUnitId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal? Discount { get; set; }
    public decimal? Tax { get; set; }
    public decimal? AdditionalTax { get; set; }
    public string? Remarks { get; set; }
    public bool IsPreOrder { get; set; }
    public PurchaseTypeDTO Type { get; set; }
    public List<PurchaseDetailDto> PurchaseDetails { get; set; } = new();
    public Guid PerformedByUserId { get; set; }
}
