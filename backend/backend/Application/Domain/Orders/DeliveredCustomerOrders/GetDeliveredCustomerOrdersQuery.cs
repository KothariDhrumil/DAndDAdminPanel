using Application.Abstractions.Messaging;
using Application.Abstractions.Persistence;
using SharedKernel;

namespace Application.Domain.Orders.DeliveredCustomerOrders;

public sealed class GetDeliveredCustomerOrdersQuery : IQuery<PagedResult<List<CustomerOrderItemDto>>>
{
    public int? RouteId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
