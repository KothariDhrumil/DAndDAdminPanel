using Application.Abstractions.Messaging;
using Application.Exceptions;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.TenantPlans.GetActivePlan;

public sealed record GetActiveTenantPlanByIdQuery(int TenantId) : IQuery<ActiveTenentPlanResponse>;

internal sealed class GetTenantPlanByIdQueryHandler(AuthPermissionsDbContext context)
    : IQueryHandler<GetActiveTenantPlanByIdQuery, ActiveTenentPlanResponse>
{
    public async Task<Result<ActiveTenentPlanResponse>> Handle(GetActiveTenantPlanByIdQuery query, CancellationToken cancellationToken)
    {
        ActiveTenentPlanResponse? TenantPlan = await context
            .TenantPlans
            .AsNoTracking()
            .Where(TenantPlanItem => TenantPlanItem.TenentId == query.TenantId &&
                                     TenantPlanItem.IsActive)
            .Select(TenantPlanItem => new ActiveTenentPlanResponse
            {
                Id = TenantPlanItem.Id,
                IsActive = TenantPlanItem.IsActive,
                Remarks = TenantPlanItem.Remarks,
                TenantPlanId = query.TenantId,
                ValidFrom = TenantPlanItem.ValidFrom,
                ValidTo = TenantPlanItem.ValidTo,
                PlanName = TenantPlanItem.Plan.Name,
                Roles = TenantPlanItem.Plan.Roles.Select(x => new AuthPermissions.AdminCode.RoleWithPermissionNamesDto()
                {

                }).ToList()


            })
            .SingleOrDefaultAsync(cancellationToken);

        return TenantPlan is not null ? Result.Success(TenantPlan) : throw new ApiException("TenantPlan not found");
    }
}
