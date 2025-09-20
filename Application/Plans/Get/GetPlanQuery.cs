using Application.Abstractions.Messaging;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Plans.Get;


public sealed record GetPlanQuery() : IQuery<List<PlanResponse>>;

internal sealed class GetPlanQueryHandler(AuthPermissionsDbContext context)
    : IQueryHandler<GetPlanQuery, List<PlanResponse>>
{
    public async Task<Result<List<PlanResponse>>> Handle(GetPlanQuery query, CancellationToken cancellationToken)
    {
        List<PlanResponse> plans = await context.Plans
            .Select(PlanItem => new PlanResponse()
            {
                Id = PlanItem.Id,
                Description = PlanItem.Description,
                PlanValidityInDays = PlanItem.PlanValidityInDays,
                PlanRate = PlanItem.PlanRate,
                IsActive = PlanItem.IsActive,
                Name = PlanItem.Name,
                RoleIds = PlanItem.Roles.Select(r => r.RoleId).ToList()
            })
            .ToListAsync(cancellationToken);

        return Result.Success(plans);
    }
}
