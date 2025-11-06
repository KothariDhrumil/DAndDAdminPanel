using Application.Abstractions.Messaging;

namespace Application.Domain.PriceTiers.Update;

public sealed class UpdateRoutePriceTierCommand : ICommand
{
    public int RouteId { get; set; }
    public int PriceTierId { get; set; }
}
