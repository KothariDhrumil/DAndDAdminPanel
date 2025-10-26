using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Persistence;
using Domain.Purchase;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Orders;

public sealed class UpdateCustomerOrderCommand : ICommand<bool>
{
    public int? TenantId { get; set; }
    public int OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public decimal? Discount { get; set; }
    public string? Remarks { get; set; }
    public decimal? ParcelCharge { get; set; }
    public List<CustomerOrderDetailDto> CustomerOrderDetails { get; set; } = new();
    public bool IsPreOrder { get; set; }
    public DateTime? OrderDeliveryDate { get; set; }
    public bool? IsDelivered { get; set; }
}

internal sealed class UpdateCustomerOrderCommandHandler(
    IRetailDbContext db)
    : ICommandHandler<UpdateCustomerOrderCommand, bool>
{
    public async Task<Result<bool>> Handle(UpdateCustomerOrderCommand command, CancellationToken ct)
    {
        //if (!command.TenantId.HasValue)
        //    return Result.Failure<bool>(Error.Validation("TenantMissing", "Tenant id not resolved."));

        //var db = await factory.CreateAsync(command.TenantId.Value, ct);


        var order = await db.CustomerOrders
            .Include(o => o.CustomerOrderDetails)
            .FirstOrDefaultAsync(o => o.Id == command.OrderId, ct);
        if (order == null)
            return Result.Failure<bool>(Error.NotFound("OrderNotFound", "Order not found."));

        order.CustomerId = command.CustomerId;
        order.Discount = command.Discount;
        order.Remarks = command.Remarks;
        order.ParcelCharge = command.ParcelCharge;
        order.IsPreOrder = command.IsPreOrder;
        if (command.OrderDeliveryDate.HasValue)
            order.OrderDeliveryDate = command.OrderDeliveryDate.Value;
        if (command.IsDelivered.HasValue)
            order.IsDelivered = command.IsDelivered.Value;

        // Update details: simple replace
        order.CustomerOrderDetails.Clear();
        foreach (var d in command.CustomerOrderDetails)
        {
            order.CustomerOrderDetails.Add(new CustomerOrderDetail
            {
                ProductId = d.ProductId,
                Qty = d.Qty,
                Rate = 0,
                Amount = 0
            });
        }
        await db.SaveChangesAsync(ct);
        return Result.Success(true);
    }
}
