using Application.Abstractions.Messaging;
using Application.Exceptions;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.TenantPlans.GetActivePlan;

public sealed record GetActiveTenantPlanByIdQuery(int TenantId) : IQuery<TenantPlanResponse>;

internal sealed class GetTenantPlanByIdQueryHandler(AuthPermissionsDbContext context)
    : IQueryHandler<GetActiveTenantPlanByIdQuery, TenantPlanResponse>
{
    public async Task<Response<TenantPlanResponse>> Handle(GetActiveTenantPlanByIdQuery query, CancellationToken cancellationToken)
    {
        TenantPlanResponse? TenantPlan = await context
            .TenantPlans
            .AsNoTracking()
            .Where(TenantPlanItem => TenantPlanItem.TenentId == query.TenantId && 
                                     TenantPlanItem.IsActive)
            .Select(TenantPlanItem => new TenantPlanResponse
            {
                Id = TenantPlanItem.Id,
                IsActive = TenantPlanItem.IsActive,
                Remarks = TenantPlanItem.Remarks,
                TenantPlanId = query.TenantId,
                ValidFrom = TenantPlanItem.ValidFrom,
                ValidTo = TenantPlanItem.ValidTo,
                PlanName = TenantPlanItem.Plan.Name,

            })
            .SingleOrDefaultAsync(cancellationToken);

        return TenantPlan is null ? throw new ApiException("TenantPlan not found") : new Response<TenantPlanResponse>(TenantPlan);
    }
}
