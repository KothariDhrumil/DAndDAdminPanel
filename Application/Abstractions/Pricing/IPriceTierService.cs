using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstractions.Pricing;

public interface IPriceTierService
{
    Task<decimal?> GetSalesRateAsync(Guid customerId, int productId, CancellationToken ct);
}
