using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using SharedKernel;

namespace Application.Domain.Routes.Delete;

public sealed class DeleteRouteCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteRouteCommand>
{
    public async Task<Result> Handle(DeleteRouteCommand command, CancellationToken ct)
    {
        var route = await unitOfWork.Routes.FirstOrDefaultAsync(x => x.Id == command.Id, ct);
        if (route == null)
            return Result.Failure(Error.NotFound("RouteNotFound", "Route not found."));
        unitOfWork.Routes.Remove(route);
        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }
}
