using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Purchase;
using SharedKernel;

namespace Application.Domain.PurchaseUnits.Create;

public sealed class CreatePurchaseUnitCommandHandler(IRetailDbContext db) : ICommandHandler<CreatePurchaseUnitCommand, int>
{
    public async Task<Result<int>> Handle(CreatePurchaseUnitCommand command, CancellationToken ct)
    {
        var entity = new PurchaseUnit
        {
            Name = command.Name,
            IsInternal = command.IsInternal,
            Address = command.Address,
            IsTaxable = command.IsTaxable,
            TenantUserId = command.TenantUserId
        };
        db.PurchaseUnits.Add(entity);
        await db.SaveChangesAsync(ct);
        return Result.Success(entity.Id);
    }
}
