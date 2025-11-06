using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Customers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.UserTypes.Get;

public sealed class GetUserTypesQueryHandler(IRetailDbContext db) : IQueryHandler<GetUserTypesQuery, List<GetUserTypeResponseDTO>>
{
    public async Task<Result<List<GetUserTypeResponseDTO>>> Handle(GetUserTypesQuery query, CancellationToken ct)
    {
        var types = await db.UserTypes.AsNoTracking()
            .Select(x => new GetUserTypeResponseDTO
            {
                UserTypeId = x.UserTypeId,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .ToListAsync(ct);
        return Result.Success(types);
    }
}
