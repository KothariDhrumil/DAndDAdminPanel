using Application.Abstractions.Messaging;

namespace Application.Domain.Purchases.Queries.GetSummary;

public sealed class GetPurchaseSummaryQuery : IQuery<PurchaseSummaryResponse>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? RouteId { get; set; }
    public int? PurchaseUnitId { get; set; }
}
