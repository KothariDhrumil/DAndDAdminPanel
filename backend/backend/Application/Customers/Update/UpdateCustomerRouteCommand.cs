using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Customers.Update
{
    public sealed class UpdateCustomerRouteCommand : ICommand
    {
        public Guid TenantUserId { get; set; }
        public int RouteId { get; set; }

        public sealed class UpdateCustomerRouteCommandHandler(IRetailDbContext db) : ICommandHandler<UpdateCustomerRouteCommand>
        {
            public async Task<Result> Handle(UpdateCustomerRouteCommand command, CancellationToken ct)
            {
                var customer = await db.TenantCustomerProfiles.SingleOrDefaultAsync(x => x.TenantUserId == command.TenantUserId, ct);
                if (customer == null)
                    return Result.Failure(SharedKernel.Error.NotFound("CustomerNotFound", "Customer not found."));

                if (customer.RouteId == command.RouteId)
                    return Result.Success(); // No change needed

                if (customer.SequenceNo == 0)
                {
                    // Assign sequence number if not set
                    var maxSequenceNo = await db.TenantCustomerProfiles
                        .Where(x => x.RouteId == command.RouteId)
                        .MaxAsync(x => (int?)x.SequenceNo, ct) ?? 0;
                    customer.SequenceNo = maxSequenceNo + 1;
                }

                customer.RouteId = command.RouteId;
                await db.SaveChangesAsync(ct);
                return Result.Success();
            }
        }
    }
}
