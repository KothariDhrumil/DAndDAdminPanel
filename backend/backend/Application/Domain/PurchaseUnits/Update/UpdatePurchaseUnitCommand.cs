using Application.Abstractions.Messaging;
using System;

namespace Application.Domain.PurchaseUnits.Update;

public sealed class UpdatePurchaseUnitCommand : ICommand
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsInternal { get; set; }
    public string? Address { get; set; }
    public bool IsTaxable { get; set; }
    public Guid? TenantUserId { get; set; }
}
