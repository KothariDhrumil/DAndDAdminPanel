using Application.Abstractions.Messaging;

namespace Application.Domain.PurchaseUnits.GetById;

public sealed record GetPurchaseUnitByIdQuery(int Id) : IQuery<PurchaseUnitResponse>;

public sealed class PurchaseUnitResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsInternal { get; set; }
    public string? Address { get; set; }
    public bool IsTaxable { get; set; }
    public Guid? TenantUserId { get; set; }
}
