using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.PriceTiers.Update;

public sealed class UpdateRoutePriceTierCommandHandler(IRetailDbContext db) : ICommandHandler<UpdateRoutePriceTierCommand>
{
    public async Task<Result> Handle(UpdateRoutePriceTierCommand command, CancellationToken ct)
    {
        var route = await db.Routes.SingleOrDefaultAsync(x => x.Id == command.RouteId, ct);
        if (route == null)
            return Result.Failure(Error.NotFound("RouteNotFound", "Route not found."));
        route.PriceTierId = command.PriceTierId;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
