using Application.Abstractions.Messaging;
using Application.Exceptions;
using AuthPermissions.BaseCode.DataLayer.Classes;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Numerics;

namespace Application.Plans.Update;

public class UpdatePlanCommand : ICommand<int>
{
    public int PlanId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int PlanValidityInDays { get; set; }
    public int PlanRate { get; set; }
    public bool IsActive { get; set; }
    //public bool IsBillable { get; set; }
    public bool IsApplyToAllUsers { get; set; }
    public List<int> RoleIds { get; set; }
}

internal sealed class UpdatePlanCommandHandler(
    AuthPermissionsDbContext context, IDateTimeProvider dateTimeProvider)
    : ICommandHandler<UpdatePlanCommand, int>
{
     
    public async Task<Result<int>> Handle(UpdatePlanCommand command, CancellationToken cancellationToken)
    {
        Plan? Plan = await context.Plans
            .SingleOrDefaultAsync(t => t.Id == command.PlanId, cancellationToken) ?? throw new ApiException(GenericErrors.NotFound.Description);

        Plan.Name = command.Name;
        Plan.IsActive = command.IsActive;
        Plan.PlanRate = command.PlanRate;
        Plan.PlanValidityInDays = command.PlanValidityInDays;

        // update the roles
        var roles = await context.RoleToPermissions
            .Where(x => command.RoleIds.Contains(x.RoleId))
            .ToListAsync(cancellationToken);
        Plan.Roles = roles;

        //// if the plan is billable then it must be active
        //if (command.IsBillable && !command.IsActive)
        //    throw new ApiException("A billable plan must be active.");
        //// if the plan is not billable then it can't be active
        //if (!command.IsBillable && command.IsActive)
        //    throw new ApiException("A non-billable plan can't be active.");
        //// if the plan is not billable then it can't be applied to all users
        //if (!command.IsBillable && command.IsApplyToAllUsers)
        //    throw new ApiException("A non-billable plan can't be applied to all users.");
        //// if the plan is billable and is not active then it can't be applied to all users
        //if (command.IsBillable && !command.IsActive && command.IsApplyToAllUsers)
        //    throw new ApiException("A billable plan that is not active can't be applied to all users.");
        
        // if the plan is to be applied to all users, then update all active tenant plans that are using this plan
        if (command.IsApplyToAllUsers)
        {
            IQueryable<TenantPlan> companyPlans = context.TenantPlans.Where(x => x.PlanId == command.PlanId && 
                                                                                 x.IsActive && 
                                                                                 x.ValidFrom.Date <= dateTimeProvider.Now.Date &&
                                                                                 x.ValidTo.Date >= dateTimeProvider.Now.Date);

            foreach (var companyPlan in companyPlans)
            {
                // TODO : Update to all users
                //companyPlan.ValidTo = dateTimeProvider.Now.AddDays(command.PlanValidityInDays);
                //context.TenantPlans.Update(companyPlan);

            }
        }

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(Plan.Id);
    }
}

public class UpdatePlanCommandValidator : AbstractValidator<UpdatePlanCommand>
{
    public UpdatePlanCommandValidator()
    {
        RuleFor(c => c.PlanId).GreaterThan(0);
        RuleFor(c => c.Name).NotEmpty().MaximumLength(255);
        RuleFor(c => c.PlanValidityInDays).GreaterThan(0);
        RuleFor(c => c.PlanRate).GreaterThanOrEqualTo(0);
    }
}