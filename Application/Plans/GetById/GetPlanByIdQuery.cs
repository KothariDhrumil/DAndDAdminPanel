using Application.Abstractions.Messaging;
using Application.Exceptions;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Plans.GetById;

public sealed record GetPlanByIdQuery(int PlanId) : IQuery<PlanResponse>;

internal sealed class GetPlanByIdQueryHandler(AuthPermissionsDbContext context)
    : IQueryHandler<GetPlanByIdQuery, PlanResponse>
{
    public async Task<Response<PlanResponse>> Handle(GetPlanByIdQuery query, CancellationToken cancellationToken)
    {
        PlanResponse? Plan = await context.Plans
            .Where(PlanItem => PlanItem.Id == query.PlanId)
            .Select(PlanItem => new PlanResponse
            {
                Id = PlanItem.Id,
                Description = PlanItem.Description,
                Name = PlanItem.Name,
                IsActive = PlanItem.IsActive,
                PlanRate = PlanItem.PlanRate,
                PlanValidityInDays = PlanItem.PlanValidityInDays,
            })
            .SingleOrDefaultAsync(cancellationToken);

        return Plan is null ? throw new ApiException("Plan not found") : Response.Success(Plan);
    }
}
