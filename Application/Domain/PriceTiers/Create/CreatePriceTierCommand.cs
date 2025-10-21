using Application.Abstractions.Messaging;

namespace Application.Domain.PriceTiers.Create;

public sealed class CreatePriceTierCommand : ICommand<int>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
