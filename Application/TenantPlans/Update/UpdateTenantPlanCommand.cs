using Application.Abstractions.Messaging;
using Application.Exceptions;
using AuthPermissions.BaseCode.DataLayer.Classes;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.TenantPlans.Update;

public sealed class UpdateTenantPlanCommand : ICommand
{
    public int TenantPlanId { get; set; }
    public int PlanId { get; set; }
    public int TenantId { get; set; }
    public bool IsActive { get; set; }
    public int? InactiveStatusId { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public string Remarks { get; set; } = string.Empty;
    

    internal sealed class UpdateTenantPlanCommandHandler(
    AuthPermissionsDbContext context)
    : ICommandHandler<UpdateTenantPlanCommand>
    {
        public async Task<Result> Handle(UpdateTenantPlanCommand command, CancellationToken cancellationToken)
        {
            TenantPlan? TenantPlan = await context.TenantPlans
                .SingleOrDefaultAsync(t => t.Id == command.TenantPlanId, cancellationToken)
                ?? throw new ApiException(GenericErrors.NotFound.Description);

            if (TenantPlan.PlanId != command.PlanId)
            {
                //TO DO : Update features...

            }
            TenantPlan.PlanId = command.PlanId;
            TenantPlan.IsActive = command.IsActive;
            TenantPlan.ValidFrom = command.ValidFrom;
            TenantPlan.ValidTo = command.ValidTo;
            TenantPlan.Remarks = command.Remarks;

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(TenantPlan.Id);
        }
    }
}