using Application.Abstractions.Messaging;
using Application.Exceptions;
using AuthPermissions.BaseCode.DataLayer.Classes;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Plans.Update;

public class UpdatePlanCommand : ICommand<int>
{
    public int PlanId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int PlanValidityInDays { get; set; }
    public int PlanRate { get; set; }
    public bool IsActive { get; set; }
    public bool IsApplyToAllUsers { get; set; }
    public List<int> RoleIds { get; set; } = new();
}

internal sealed class UpdatePlanCommandHandler(
    AuthPermissionsDbContext context, IDateTimeProvider dateTimeProvider)
    : ICommandHandler<UpdatePlanCommand, int>
{
     
    public async Task<Result<int>> Handle(UpdatePlanCommand command, CancellationToken cancellationToken)
    {
        var plan = await context.Plans
            .SingleOrDefaultAsync(t => t.Id == command.PlanId, cancellationToken);
        
        if (plan == null)
        {
            throw new ApiException(GenericErrors.NotFound.Description);
        }

        plan.Name = command.Name;
        plan.IsActive = command.IsActive;
        plan.PlanRate = command.PlanRate;
        plan.PlanValidityInDays = command.PlanValidityInDays;

        // update the roles
        if (command.RoleIds != null && command.RoleIds.Count > 0)
        {
            var roles = await context.RoleToPermissions
                .Where(x => command.RoleIds.Contains(x.RoleId))
                .ToListAsync(cancellationToken);
            plan.Roles = roles;
        }

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(plan.Id);
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