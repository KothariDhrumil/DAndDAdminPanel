using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Domain.Purchases.Commands.Update;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Purchases.Queries.GetUnconfirmedOrder;

public class GetUnconfirmedPurchaseQueryHandler : IQueryHandler<GetUnconfirmedPurchaseQuery, UnconfirmedPurchaseResponse?>
{
    private readonly IRetailDbContext _db;
    public GetUnconfirmedPurchaseQueryHandler(IRetailDbContext db)
    {
        _db = db;
    }

    public async Task<Result<UnconfirmedPurchaseResponse?>> Handle(GetUnconfirmedPurchaseQuery query, CancellationToken cancellationToken)
    {
        var order = await _db.Purchases
            .Where(x => x.RouteId == query.RouteId
                && x.PurchaseUnitId == query.PurchaseUnitId
                && !x.IsConfirmed)
            .Select(x => new UnconfirmedPurchaseResponse
            {
                Id = x.Id,
                PurchaseUnitId = x.PurchaseUnitId,
                Amount = x.Amount,
                Discount = x.Discount,
                Tax = x.Tax,
                GrandTotal = x.GrandTotal,
                Remarks = x.Remarks,
                Type = (PurchaseTypeDTO)x.Type,
                RouteId = x.RouteId,
                PurchaseDetails = x.PurchaseDetails.Select(pd => new UnconfirmedPurchaseDetailDto
                {
                    Amount = pd.Amount,
                    Tax = pd.Tax,
                    ProductId = pd.ProductId,
                    ProductName = pd.Product.Name,
                    Qty = pd.Qty,
                    PurchaseRate = pd.Rate

                }).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        return Result.Success(order);
    }
}
