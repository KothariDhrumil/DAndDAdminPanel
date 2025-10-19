using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Routes.Delete;

public sealed class DeleteRouteCommandHandler(IRetailDbContext db) : ICommandHandler<DeleteRouteCommand>
{
    public async Task<Result> Handle(DeleteRouteCommand command, CancellationToken ct)
    {
        var route = await db.Routes.SingleOrDefaultAsync(x => x.Id == command.Id, ct);
        if (route == null)
            return Result.Failure(Error.NotFound("RouteNotFound", "Route not found."));
        db.Routes.Remove(route);
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
