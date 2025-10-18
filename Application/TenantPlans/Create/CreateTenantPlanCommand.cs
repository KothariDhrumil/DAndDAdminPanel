using Application.Abstractions.Messaging;
using Application.Exceptions;
using AuthPermissions.AdminCode;
using AuthPermissions.BaseCode.DataLayer.Classes;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.TenantPlans.Create;

public sealed class CreateTenantPlanCommand : ICommand<int>
{
    public int PlanId { get; set; }
    public int TenantId { get; set; }
    public bool IsActive { get; set; }
    public int? InactiveStatusId { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public string Remarks { get; set; } = string.Empty;

    // Effective roles to assign to this tenant plan (from UI)
    public List<int> Roles { get; set; } = new();

    internal sealed class CreateTenantPlanCommandHandler(
        AuthPermissionsDbContext context,
        IAuthTenantAdminService authTenantAdmin,
        IAuthUsersAdminService authUsersAdminService)
        : ICommandHandler<CreateTenantPlanCommand, int>
    {
        private readonly IAuthUsersAdminService authUsersAdminService = authUsersAdminService;

        public async Task<Result<int>> Handle(CreateTenantPlanCommand command, CancellationToken cancellationToken)
        {
            // Load roles by ids provided by UI (effective roles)
            var effectiveRoles = command.Roles?.Count > 0
                ? await context.RoleToPermissions
                    .Where(x => command.Roles.Contains(x.RoleId))
                    .ToListAsync(cancellationToken)
                : new List<RoleToPermissions>();

            var tenantPlan = new TenantPlan()
            {
                IsActive = command.IsActive,
                TenentId = command.TenantId,
                ValidFrom = command.ValidFrom,
                ValidTo = command.ValidTo,
                Remarks = command.Remarks,
                PlanId = command.PlanId,
                Roles = effectiveRoles
            };

            context.TenantPlans.Add(tenantPlan);
            

            // If this plan is active, update the tenant's roles so permissions are applied
            if (command.IsActive)
            {
                var roleIds = effectiveRoles.Select(r => r.RoleId).ToList();
                var status = await authTenantAdmin.UpdateTenantRolesAsync(command.TenantId, roleIds);
                if (status.HasErrors)
                    throw new ApiException(status.GetAllErrors());                
            }
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(tenantPlan.Id);
        }
    }
}
public class CreateTenantPlanCommandValidator : AbstractValidator<CreateTenantPlanCommand>
{
    public CreateTenantPlanCommandValidator()
    {
        RuleFor(c => c.PlanId).NotNull().GreaterThan(0);
        RuleFor(c => c.TenantId).NotNull().GreaterThan(0);
        RuleFor(c => c.ValidFrom).LessThan(c => c.ValidTo);
    }
}