using Application.Abstractions.Messaging;
using Application.Exceptions;
using AuthPermissions.AdminCode;
using AuthPermissions.BaseCode.DataLayer.Classes;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Linq;

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

    // Effective roles sent by UI
    public List<int> Roles { get; set; } = new();

    internal sealed class UpdateTenantPlanCommandHandler(
    AuthPermissionsDbContext context,
    IAuthTenantAdminService authTenantAdmin,
    IAuthUsersAdminService authUsersAdminService)
    : ICommandHandler<UpdateTenantPlanCommand>
    {


        public async Task<Result> Handle(UpdateTenantPlanCommand command, CancellationToken cancellationToken)
        {
            var tenantPlan = await context.TenantPlans
              .Include(tp => tp.Plan).ThenInclude(p => p.Roles)
              .Include(tp => tp.Roles)
              .SingleOrDefaultAsync(t => t.Id == command.TenantPlanId, cancellationToken)
              ?? throw new ApiException(GenericErrors.NotFound.Description);

            // If plan changed, set new plan
            if (tenantPlan.PlanId != command.PlanId)
            {
                var newPlan = await context.Plans
                    .Include(p => p.Roles)
                    .SingleAsync(p => p.Id == command.PlanId, cancellationToken);

                tenantPlan.PlanId = command.PlanId;
                tenantPlan.Plan = newPlan;
            }

            // Current roles
            var currentRoleIds = tenantPlan.Roles.Select(r => r.RoleId).ToHashSet();
            var incomingRoleIds = (command.Roles ?? new()).ToHashSet();

            // Determine roles to add / remove
            var toAdd = incomingRoleIds.Except(currentRoleIds).ToList();
            var toRemove = currentRoleIds.Except(incomingRoleIds).ToList();

            if (toRemove.Count > 0)
            {
                var removeItems = tenantPlan.Roles.Where(r => toRemove.Contains(r.RoleId)).ToList();
                foreach (var r in removeItems)
                {
                    tenantPlan.Roles.Remove(r);
                }
            }

            if (toAdd.Count > 0)
            {
                var rolesToAdd = await context.RoleToPermissions
                    .Where(x => toAdd.Contains(x.RoleId))
                    .ToListAsync(cancellationToken);
                foreach (var role in rolesToAdd)
                {
                    tenantPlan.Roles.Add(role);
                }
            }

            tenantPlan.IsActive = command.IsActive;
            tenantPlan.ValidFrom = command.ValidFrom;
            tenantPlan.ValidTo = command.ValidTo;
            tenantPlan.Remarks = command.Remarks;

            // If the plan is active, update the tenant roles to reflect effective roles
            if (tenantPlan.IsActive && (toRemove.Count != 0 || toAdd.Count != 0))
            {
                var effectiveRoleIds = tenantPlan.Roles.Select(r => r.RoleId).ToList();
                var status = await authTenantAdmin.UpdateTenantRolesAsync(command.TenantId, effectiveRoleIds);
                if (status.HasErrors)
                    throw new ApiException(status.GetAllErrors());


                //var usersToUpdate = authUsersAdminService
                //      .QueryAuthUsers(command.TenantId).Include(x => x.UserRoles);

                //// If any roles were removed from the tenant plan, remove those roles from all users in this tenant
                //if (toRemove.Count > 0)
                //{
                //    // Find users in this tenant who currently have any of the removed roles
                //    var users = await usersToUpdate.Where(u => u.UserRoles.Any(ur => toRemove.Contains(ur.RoleId)))
                //        .ToListAsync(cancellationToken);

                //    foreach (var user in users)
                //    {
                //        // New user role set = existing minus removed roles
                //        var newUserRoleIds = user.UserRoles
                //            .Select(ur => ur.RoleId)
                //            .Where(roleId => !toRemove.Contains(roleId))
                //            .Distinct()
                //            .ToList();

                //        var userUpdateStatus = await authUsersAdminService.UpdateUserAsync(
                //            user.UserId,
                //            roleIds: newUserRoleIds);

                //        if (userUpdateStatus.HasErrors)
                //            throw new ApiException(userUpdateStatus.GetAllErrors());
                //    }
                //}


                //// add the roles if its added new one
                //if (toAdd.Count > 0)
                //{
                //    var users = await usersToUpdate.ToListAsync();
                //    foreach (var user in users)
                //    {

                //        var userUpdateStatus = await authUsersAdminService.UpdateUserAsync(
                //            user.UserId,
                //            roleIds: effectiveRoleIds);

                //        if (userUpdateStatus.HasErrors)
                //            throw new ApiException(userUpdateStatus.GetAllErrors());
                //    }
                //}

            }

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(tenantPlan.Id);
        }
    }
}

public class UpdateTenantPlanCommandValidator : AbstractValidator<UpdateTenantPlanCommand>
{
    public UpdateTenantPlanCommandValidator()
    {
        RuleFor(x => x.TenantPlanId).GreaterThan(0);
        RuleFor(x => x.PlanId).GreaterThan(0);
        RuleFor(x => x.TenantId).GreaterThan(0);
        RuleFor(x => x.ValidFrom).NotEmpty();
        RuleFor(x => x.ValidTo).NotEmpty()
            .GreaterThan(x => x.ValidFrom).WithMessage("ValidTo must be after ValidFrom");
        RuleFor(x => x.Remarks).MaximumLength(1000);
    }
}