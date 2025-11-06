using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Customers;
using SharedKernel;

namespace Application.Domain.UserTypes.Create;

public sealed class CreateUserTypeCommandHandler(IRetailDbContext db) : ICommandHandler<CreateUserTypeCommand, int>
{
    public async Task<Result<int>> Handle(CreateUserTypeCommand command, CancellationToken ct)
    {
        var entity = new UserType
        {
            Name = command.Name,
            Description = command.Description,
            IsActive = command.IsActive,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        db.UserTypes.Add(entity);
        await db.SaveChangesAsync(ct);
        return Result.Success(entity.Id);
    }
}
