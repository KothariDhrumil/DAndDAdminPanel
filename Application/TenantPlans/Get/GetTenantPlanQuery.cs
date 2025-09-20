using Application.Abstractions.Messaging;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.TenantPlans.Get;


public sealed record GetTenantPlanQuery() : IQuery<List<TenantPlanInfo>>;

internal sealed class GetTenantPlanQueryHandler(AuthPermissionsDbContext context)
    : IQueryHandler<GetTenantPlanQuery, List<TenantPlanInfo>>
{
    public async Task<Result<List<TenantPlanInfo>>> Handle(GetTenantPlanQuery query, CancellationToken cancellationToken)
    {
        List<TenantPlanInfo> TenantPlan = await context
            .TenantPlans
            .AsNoTracking()
            .Include(x => x.Plan)
            .Include(x => x.Tenant)
            .Where(x => x.IsActive)
            .Select(TenantPlanItem => new TenantPlanInfo()
            {
                Id = TenantPlanItem.Id,
                PlanId = TenantPlanItem.PlanId,
                Remarks = TenantPlanItem.Remarks,
                ValidTo = TenantPlanItem.ValidTo,
                ValidFrom = TenantPlanItem.ValidFrom,
                PlanName = TenantPlanItem.Plan.Name,
                PlanRate = TenantPlanItem.Plan.PlanRate,
                Tenant = TenantPlanItem.Tenant.TenantFullName,
                IsActive = TenantPlanItem.IsActive,
            })
            .ToListAsync(cancellationToken);

        return Result.Success(TenantPlan);
    }
}
