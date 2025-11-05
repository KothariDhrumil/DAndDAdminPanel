using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using SharedKernel;

namespace Application.Domain.Routes.Update;

public sealed class UpdateRouteCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateRouteCommand>
{
    public async Task<Result> Handle(UpdateRouteCommand command, CancellationToken ct)
    {
        var route = await unitOfWork.Routes.FirstOrDefaultAsync(x => x.Id == command.Id, ct);
        if (route == null)
            return Result.Failure(Error.NotFound("RouteNotFound", "Route not found."));
        route.TenantUserId = Guid.Parse(command.TenantUserId);
        route.IsActive = command.IsActive;
        route.Name = command.Name;
        unitOfWork.Routes.Update(route);
        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }
}
