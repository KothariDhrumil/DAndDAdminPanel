using Application.Abstractions.Messaging;
using Application.Exceptions;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using AuthPermissions.BaseCode;
using AuthPermissions.BaseCode.PermissionsCode;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Plans.GetById;

public sealed record GetPlanByIdQuery(int PlanId) : IQuery<PlanDetailsResponse>;

internal sealed class GetPlanByIdQueryHandler(AuthPermissionsDbContext context, AuthPermissionsOptions options)
    : IQueryHandler<GetPlanByIdQuery, PlanDetailsResponse>
{
    public async Task<Result<PlanDetailsResponse>> Handle(GetPlanByIdQuery query, CancellationToken cancellationToken)
    {
        var planEntity = await context.Plans.Include(x => x.Roles)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == query.PlanId, cancellationToken);

        if (planEntity is null)
            throw new ApiException("Plan not found");

        var response = new PlanDetailsResponse
        {
            Id = planEntity.Id,
            Description = planEntity.Description,
            Name = planEntity.Name,
            IsActive = planEntity.IsActive,
            PlanRate = planEntity.PlanRate,
            PlanValidityInDays = planEntity.PlanValidityInDays,
            RoleIds = [.. planEntity.Roles.Select(r => r.RoleId)]
        };

        return Result.Success(response);
    }
}
