using Application.Abstractions.Messaging;

namespace Application.Domain.Purchases.Commands.Delete;

public sealed class DeletePurchaseCommand : ICommand
{
    public int Id { get; set; }
}
