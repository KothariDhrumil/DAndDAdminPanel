using Domain.Purchase;

namespace Application.Abstractions.Pricing;

public interface ICustomerOrderPriceCalculationService
{
    Task<CustomerOrder> SaveOrUpdateOrderAsync(CustomerOrder customerOrder);
}
