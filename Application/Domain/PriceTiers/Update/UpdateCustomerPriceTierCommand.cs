using Application.Abstractions.Messaging;

namespace Application.Domain.PriceTiers.Update;

public sealed class UpdateCustomerPriceTierCommand : ICommand
{
    public Guid TenantUserId { get; set; }
    public int PriceTierId { get; set; }
}
