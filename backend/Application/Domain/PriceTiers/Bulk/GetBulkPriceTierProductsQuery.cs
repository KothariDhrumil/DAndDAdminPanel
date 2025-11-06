using Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace Application.Domain.PriceTiers.Bulk;

public sealed record GetBulkPriceTierProductsQuery : IQuery<List<PriceTierProductBulkResponse>>;

public sealed class PriceTierProductBulkResponse
{
    public int PriceTierId { get; set; }
    public string PriceTierName { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal SalesRate { get; set; }
}
