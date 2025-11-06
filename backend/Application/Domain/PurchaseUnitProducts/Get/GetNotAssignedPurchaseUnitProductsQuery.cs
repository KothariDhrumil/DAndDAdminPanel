using Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace Application.Domain.PurchaseUnitProducts.Get;

public sealed record GetNotAssignedPurchaseUnitProductsQuery(int PurchaseUnitId) : IQuery<List<NotAssignedPurchaseUnitProductResponse>>;

public sealed class NotAssignedPurchaseUnitProductResponse
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
}
