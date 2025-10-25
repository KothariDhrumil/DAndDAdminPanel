using Application.Abstractions.Messaging;
using System;

namespace Application.Domain.PurchaseUnits.Create;

public sealed class CreatePurchaseUnitCommand : ICommand<int>
{
    public string Name { get; set; } = string.Empty;
    public bool IsInternal { get; set; }
    public string? Address { get; set; }
    public bool IsTaxable { get; set; }
    public Guid? TenantUserId { get; set; }
}
