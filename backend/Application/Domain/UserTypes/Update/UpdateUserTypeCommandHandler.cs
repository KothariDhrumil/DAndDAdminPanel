using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.UserTypes.Update;

public sealed class UpdateUserTypeCommandHandler(IRetailDbContext db) : ICommandHandler<UpdateUserTypeCommand>
{
    public async Task<Result> Handle(UpdateUserTypeCommand command, CancellationToken ct)
    {
        var type = await db.UserTypes.FindAsync(new object[] { command.UserTypeId }, ct);
        if (type == null)
            return Result.Failure(Error.NotFound("UserTypeNotFound", "User type not found."));
        type.Name = command.Name;
        type.Description = command.Description;
        type.IsActive = command.IsActive;
        type.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
