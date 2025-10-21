using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Customers;
using SharedKernel;

namespace Application.Domain.PriceTiers.Create;

public sealed class CreatePriceTierCommandHandler(IRetailDbContext db) : ICommandHandler<CreatePriceTierCommand, int>
{
    public async Task<Result<int>> Handle(CreatePriceTierCommand command, CancellationToken ct)
    {
        var entity = new PriceTier
        {
            Name = command.Name,
            Description = command.Description,
            IsActive = command.IsActive
        };
        db.PriceTiers.Add(entity);
        await db.SaveChangesAsync(ct);
        return Result.Success(entity.Id);
    }
}
