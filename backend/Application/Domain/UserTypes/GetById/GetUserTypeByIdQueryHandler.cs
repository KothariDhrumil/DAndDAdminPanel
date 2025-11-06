using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.UserTypes.GetById;

public sealed class GetUserTypeByIdQueryHandler(IRetailDbContext db) : IQueryHandler<GetUserTypeByIdQuery, GetByIdUserTypeResponseDTO>
{
    public async Task<Result<GetByIdUserTypeResponseDTO>> Handle(GetUserTypeByIdQuery query, CancellationToken ct)
    {
        var type = await db.UserTypes.AsNoTracking()
            .Where(x => x.UserTypeId == query.UserTypeId)
            .Select(x => new GetByIdUserTypeResponseDTO
            {
                UserTypeId = x.UserTypeId,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .SingleOrDefaultAsync(ct);
        return type == null
            ? Result.Failure<GetByIdUserTypeResponseDTO>(Error.NotFound("UserTypeNotFound", "User type not found."))
            : Result.Success(type);
    }
}
