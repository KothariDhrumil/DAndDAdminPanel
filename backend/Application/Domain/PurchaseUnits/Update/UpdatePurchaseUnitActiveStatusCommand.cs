using Application.Abstractions.Messaging;

namespace Application.Domain.PurchaseUnits.Update;

public sealed class UpdatePurchaseUnitActiveStatusCommand : ICommand
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
}
