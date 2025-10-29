namespace Application.Services.PriceTier;

public interface IPriceTierService
{
    Task<decimal?> GetSalesRateAsync(Guid customerId, int productId, CancellationToken ct);
}
