using Application.Abstractions.Messaging;

namespace Application.Domain.PurchaseUnits.Delete;

public sealed class DeletePurchaseUnitCommand : ICommand
{
    public int Id { get; set; }
}
