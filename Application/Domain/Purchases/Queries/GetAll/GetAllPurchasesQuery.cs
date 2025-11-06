using Application.Abstractions.Messaging;

namespace Application.Domain.Purchases.Queries.GetAll;

public sealed class GetAllPurchasesQuery : IQuery<List<PurchaseResponse>>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? RouteId { get; set; }
    public int? PurchaseUnitId { get; set; }
    public bool? IsConfirmed { get; set; }
    public bool? IsPreOrder { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
