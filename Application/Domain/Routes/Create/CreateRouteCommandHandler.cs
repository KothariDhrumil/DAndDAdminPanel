using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Customers;
using SharedKernel;

namespace Application.Domain.Routes.Create;

public sealed class CreateRouteCommandHandler(IRetailDbContext db) : ICommandHandler<CreateRouteCommand, string>
{
    public async Task<Result<string>> Handle(CreateRouteCommand command, CancellationToken ct)
    {
        var entity = new Route
        {
            Name = command.Name,
            TenantUserId = Guid.Parse(command.TenantUserId),
            IsActive = command.IsActive,
        };
        db.Routes.Add(entity);
        await db.SaveChangesAsync(ct);
        return Result.Success(entity.Name);
    }
}
