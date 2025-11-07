using Application.Abstractions.Messaging;

namespace Application.Domain.Purchases.Commands.Confirm;

public sealed class ConfirmPurchaseCommand : ICommand
{
    public int PurchaseId { get; set; }
}
