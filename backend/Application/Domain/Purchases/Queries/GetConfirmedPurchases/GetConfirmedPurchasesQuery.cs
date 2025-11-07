using Application.Abstractions.Messaging;
using Application.Domain.Purchases.Queries.GetAll;

namespace Application.Domain.Purchases.Queries.GetConfirmedPurchases;

public sealed class GetConfirmedPurchasesQuery : IQuery<List<PurchaseResponse>>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? RouteId { get; set; }
    public int? PurchaseUnitId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
