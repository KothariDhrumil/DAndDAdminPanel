using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Customers.Update;

public sealed class UpdateCustomerRouteSequenceCommandHandler(IRetailDbContext db) : ICommandHandler<UpdateCustomerRouteSequenceCommand>
{
    public async Task<Result> Handle(UpdateCustomerRouteSequenceCommand command, CancellationToken ct)
    {
        var customerIds = command.Updates.Select(x => x.TenantUserId).ToList();
        var customers = await db.TenantCustomerProfiles
            .Where(x => x.RouteId == command.RouteId && customerIds.Contains(x.TenantUserId))
            .OrderBy(c => c.SequenceNo)
            .ToListAsync(ct);


        // Example: swap sequence 1 and 2
        var tempOffset = 10000; // use a large offset to avoid conflicts

        foreach (var c in customers)
        {
            c.SequenceNo += tempOffset; // phase 1 — move them out of unique range
        }
        await db.SaveChangesAsync(ct);


        foreach (var customer in customers)
        {
            var updatedCustomer = command.Updates.FirstOrDefault(x => x.TenantUserId == customer.TenantUserId);
            if (updatedCustomer != null)
            {
                customer.SequenceNo = updatedCustomer.Sequence;
            }
        }
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
