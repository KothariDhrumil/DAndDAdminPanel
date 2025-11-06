using Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace Application.Domain.PriceTiers.Bulk;

public sealed class BulkUpsertPriceTierProductsCommand : ICommand
{
    public List<PriceTierProductUpsertDto> Products { get; set; } = new();
}

public sealed class PriceTierProductUpsertDto
{
    public int PriceTierId { get; set; }
    public int ProductId { get; set; }
    public decimal SalesRate { get; set; }
}
