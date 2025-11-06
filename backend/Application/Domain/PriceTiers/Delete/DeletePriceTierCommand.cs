using Application.Abstractions.Messaging;

namespace Application.Domain.PriceTiers.Delete;

public sealed class DeletePriceTierCommand : ICommand
{
    public int Id { get; set; }
}
