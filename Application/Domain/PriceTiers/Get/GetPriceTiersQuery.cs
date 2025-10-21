using Application.Abstractions.Messaging;
using Domain.Customers;

namespace Application.Domain.PriceTiers.Get;

public sealed record GetPriceTiersQuery : IQuery<List<PriceTierResponse>>;

public sealed class PriceTierResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
