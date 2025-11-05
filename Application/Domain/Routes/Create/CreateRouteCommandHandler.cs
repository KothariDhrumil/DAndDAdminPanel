using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Domain.Customers;
using SharedKernel;

namespace Application.Domain.Routes.Create;

public sealed class CreateRouteCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<CreateRouteCommand, string>
{
    public async Task<Result<string>> Handle(CreateRouteCommand command, CancellationToken ct)
    {
        var entity = new Route
        {
            Name = command.Name,
            TenantUserId = Guid.Parse(command.TenantUserId),
            IsActive = command.IsActive,
            PriceTierId = command.PriceTierId
        };
        await unitOfWork.Routes.AddAsync(entity, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success(entity.Name);
    }
}
