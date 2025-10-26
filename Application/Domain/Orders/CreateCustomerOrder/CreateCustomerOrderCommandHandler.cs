using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Purchase;
using SharedKernel;

namespace Application.Domain.Orders.CreateCustomerOrder;

internal sealed class CreateCustomerOrderCommandHandler(
    IRetailDbContext dbContext)
    : ICommandHandler<CreateCustomerOrderCommand, int>
{
    public async Task<Result<int>> Handle(CreateCustomerOrderCommand command, CancellationToken ct)
    {
        if (command.CustomerOrderDetails == null || !command.CustomerOrderDetails.Any())
            return Result.Failure<int>(Error.Validation("OrderDetailsMissing", "Order must have at least one detail."));

        var order = new CustomerOrder
        {
            CustomerId = command.CustomerId,
            Discount = command.Discount,
            Remarks = command.Remarks,
            ParcelCharge = command.ParcelCharge,
            IsPreOrder = command.IsPreOrder,
            OrderPlacedDate = DateTime.UtcNow,
            CustomerOrderDetails = command.CustomerOrderDetails.Select(d => new CustomerOrderDetail
            {
                ProductId = d.ProductId,
                Qty = d.Qty,
                Rate = 0, // Set as needed
                Amount = 0 // Set as needed
            }).ToList()
        };

        dbContext.CustomerOrders.Add(order);
        await dbContext.SaveChangesAsync(ct);
        return Result.Success(order.Id);
    }
}
