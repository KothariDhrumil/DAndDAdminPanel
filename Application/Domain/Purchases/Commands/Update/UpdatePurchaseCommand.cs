using Application.Abstractions.Messaging;

namespace Application.Domain.Purchases.Commands.Update;

public sealed class UpdatePurchaseCommand : ICommand
{
    public int Id { get; set; }
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
    public Guid? PickupSalesmanId { get; set; }
    public decimal? ShippingCost { get; set; }
    public PurchaseTypeDTO Type { get; set; }
    public List<UpdatePurchaseDetailDto> PurchaseDetails { get; set; } = new();
}

public class UpdatePurchaseDetailDto
{
    public int ProductId { get; set; }
    public int Qty { get; set; }
    public decimal Rate { get; set; }
    public decimal? Tax { get; set; }
    public decimal Amount { get; set; }
}

public enum PurchaseTypeDTO
{
    Vendor = 1,
    Route = 2,
    InternalTransfer = 3
}