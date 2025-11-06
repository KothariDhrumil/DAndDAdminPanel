using Application.Abstractions.Messaging;

namespace Application.Domain.PriceTiers.Update;

public sealed class UpdatePriceTierActiveStatusCommand : ICommand
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
}
