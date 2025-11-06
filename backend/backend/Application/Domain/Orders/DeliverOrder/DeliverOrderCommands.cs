using Application.Abstractions.Messaging;

namespace Application.Domain.Orders.DeliverOrder;

public sealed class DeliverExistingOrderCommand : ICommand<bool>
{
    public int OrderId { get; set; }
}
