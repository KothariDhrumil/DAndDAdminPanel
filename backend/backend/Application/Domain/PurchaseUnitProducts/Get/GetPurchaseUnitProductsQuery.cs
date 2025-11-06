using Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace Application.Domain.PurchaseUnitProducts.Get;

public sealed record GetPurchaseUnitProductsQuery(int PurchaseUnitId) : IQuery<List<PurchaseUnitProductResponse>>;

public sealed class PurchaseUnitProductResponse
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal PurchaseRate { get; set; }
    public decimal? Tax { get; set; }
    public bool IsActive { get; set; }
}
