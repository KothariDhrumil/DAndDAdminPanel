using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Domain.UserTypes.Delete;

public sealed class DeleteUserTypeCommandHandler(IRetailDbContext db) : ICommandHandler<DeleteUserTypeCommand>
{
    public async Task<Result> Handle(DeleteUserTypeCommand command, CancellationToken ct)
    {
        var type = await db.UserTypes.FindAsync(new object[] { command.UserTypeId }, ct);
        if (type == null)
            return Result.Failure(Error.NotFound("UserTypeNotFound", "User type not found."));
        db.UserTypes.Remove(type);
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
