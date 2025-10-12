using Application.Abstractions.Messaging;
using Application.Exceptions;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.TenantPlans.GetActivePlan;

public sealed record GetActiveTenantPlanByIdQuery(int TenantId) : IQuery<TenentPlanResponse?>;

internal sealed class GetActiveTenantPlanByIdQueryHandler(AuthPermissionsDbContext context)
    : IQueryHandler<GetActiveTenantPlanByIdQuery, TenentPlanResponse?>
{
    public async Task<Result<TenentPlanResponse?>> Handle(GetActiveTenantPlanByIdQuery query, CancellationToken cancellationToken)
    {
        TenentPlanResponse? TenantPlan = await context
            .TenantPlans
            .AsNoTracking()
            .Where(TenantPlanItem => TenantPlanItem.TenentId == query.TenantId &&
                                     TenantPlanItem.IsActive)
            .Select(TenantPlanItem => new TenentPlanResponse
            {
                Id = TenantPlanItem.Id,
                IsActive = TenantPlanItem.IsActive,
                Remarks = TenantPlanItem.Remarks,
                TenantPlanId = query.TenantId,
                ValidFrom = TenantPlanItem.ValidFrom,
                ValidTo = TenantPlanItem.ValidTo,
                PlanName = TenantPlanItem.Plan.Name,
                Roles = TenantPlanItem.Roles.Select(x => x.RoleId).ToList()
            })
            .SingleOrDefaultAsync(cancellationToken);

        return Result.Success(TenantPlan);
    }
}

public sealed record GetTenantPlanByIdQuery(int TenantId) : IQuery<List<TenentPlanResponse?>>;

internal sealed class GetTenantPlanByIdQueryHandler(AuthPermissionsDbContext context)
    : IQueryHandler<GetTenantPlanByIdQuery, List<TenentPlanResponse?>>
{
    public async Task<Result<List<TenentPlanResponse?>>> Handle(GetTenantPlanByIdQuery query, CancellationToken cancellationToken)
    {
        var TenantPlan = await context
            .TenantPlans
            .AsNoTracking()
            .Where(TenantPlanItem => TenantPlanItem.TenentId == query.TenantId)
            .Select(TenantPlanItem => new TenentPlanResponse
            {
                Id = TenantPlanItem.Id,
                IsActive = TenantPlanItem.IsActive,
                Remarks = TenantPlanItem.Remarks,
                TenantPlanId = query.TenantId,
                ValidFrom = TenantPlanItem.ValidFrom,
                ValidTo = TenantPlanItem.ValidTo,
                PlanName = TenantPlanItem.Plan.Name,

            })
            .ToListAsync(cancellationToken);

        return Result.Success(TenantPlan);
    }
}
