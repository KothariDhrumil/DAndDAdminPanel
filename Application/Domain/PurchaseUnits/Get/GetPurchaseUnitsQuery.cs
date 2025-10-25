using Application.Abstractions.Messaging;
using Domain.Purchase;

namespace Application.Domain.PurchaseUnits.Get;

public sealed record GetPurchaseUnitsQuery : IQuery<List<GetPurchaseUnitResponse>>;

public sealed class GetPurchaseUnitResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsInternal { get; set; }
    public string? Address { get; set; }
    public bool IsTaxable { get; set; }
    public Guid? TenantUserId { get; set; }
    public string TenantUser { get; set; }
    public bool IsActive { get; set; }
}
