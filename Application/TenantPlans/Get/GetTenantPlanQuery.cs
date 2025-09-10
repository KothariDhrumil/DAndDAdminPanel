using Application.Abstractions.Messaging;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.TenantPlans.Get;


public sealed record GetTenantPlanQuery() : IQuery<List<TenantPlanResponse>>;

internal sealed class GetTenantPlanQueryHandler(AuthPermissionsDbContext context)
    : IQueryHandler<GetTenantPlanQuery, List<TenantPlanResponse>>
{
    public async Task<Response<List<TenantPlanResponse>>> Handle(GetTenantPlanQuery query, CancellationToken cancellationToken)
    {
        List<TenantPlanResponse> TenantPlan = await context
            .TenantPlans
            .AsNoTracking()
            .Include(x => x.Plan)
            .Include(x => x.Tenant)
            .Where(x => x.IsActive)
            .Select(TenantPlanItem => new TenantPlanResponse()
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

        return new Response<List<TenantPlanResponse>>(TenantPlan);
    }
}
