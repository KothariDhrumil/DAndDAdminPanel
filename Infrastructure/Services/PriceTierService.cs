using Application.Abstractions.Data;
using Application.Abstractions.Pricing;
using Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class PriceTierService : IPriceTierService
{
    private readonly IRetailDbContext _db;
    public PriceTierService(IRetailDbContext db)
    {
        _db = db;
    }

    public async Task<decimal?> GetSalesRateAsync(Guid customerId, int productId, CancellationToken ct)
    {
        // 1. Check for customer-specific price tier
        var customerTierId = await _db.CustomerPriceTiers
            .Where(x => x.CustomerId == customerId)
            .Select(x => x.PriceTierId)
            .FirstOrDefaultAsync(ct);
        if (customerTierId != 0)
        {
            var rate = await _db.PriceTierProducts
                .Where(x => x.PriceTierId == customerTierId && x.ProductId == productId)
                .Select(x => (decimal?)x.SalesRate)
                .FirstOrDefaultAsync(ct);
            if (rate.HasValue)
                return rate.Value;
        }
        // 2. Check for route-based price tier
        var routeId = await _db.TenantCustomerProfiles
            .Where(x => x.TenantUserId == customerId)
            .Select(x => x.RouteId)
            .FirstOrDefaultAsync(ct);
        if (routeId != 0)
        {
            var routeTierId = await _db.RoutePriceTiers
                .Where(x => x.RouteId == routeId)
                .Select(x => x.PriceTierId)
                .FirstOrDefaultAsync(ct);
            if (routeTierId != 0)
            {
                var rate = await _db.PriceTierProducts
                    .Where(x => x.PriceTierId == routeTierId && x.ProductId == productId)
                    .Select(x => (decimal?)x.SalesRate)
                    .FirstOrDefaultAsync(ct);
                if (rate.HasValue)
                    return rate.Value;
            }
        }
        // 3. Fallback to product base price
        var basePrice = await _db.Products
            .Where(x => x.Id == productId)
            .Select(x => (decimal?)x.BasePrice)
            .FirstOrDefaultAsync(ct);
        return basePrice;
    }
}
