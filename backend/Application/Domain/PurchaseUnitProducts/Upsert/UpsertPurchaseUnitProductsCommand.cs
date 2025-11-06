using Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace Application.Domain.PurchaseUnitProducts.Upsert;

public sealed class UpsertPurchaseUnitProductsCommand : ICommand
{
    public int PurchaseUnitId { get; set; }
    public List<PurchaseUnitProductUpsertDto> Products { get; set; } = new();
}

public sealed class PurchaseUnitProductUpsertDto
{
    public int ProductId { get; set; }
    public decimal PurchaseRate { get; set; }
    public decimal? Tax { get; set; }
    public bool IsActive { get; set; }
}
