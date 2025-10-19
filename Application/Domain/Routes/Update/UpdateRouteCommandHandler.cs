using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Routes.Update;

public sealed class UpdateRouteCommandHandler(IRetailDbContext db) : ICommandHandler<UpdateRouteCommand>
{
    public async Task<Result> Handle(UpdateRouteCommand command, CancellationToken ct)
    {
        var route = await db.Routes.SingleOrDefaultAsync(x => x.Id == command.Id, ct);
        if (route == null)
            return Result.Failure(Error.NotFound("RouteNotFound", "Route not found."));
        route.TenantUserId = Guid.Parse(command.TenantUserId);
        route.IsActive = command.IsActive;
        route.Name = command.Name;        
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
