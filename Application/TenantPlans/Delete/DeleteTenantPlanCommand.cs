using Application.Abstractions.Messaging;
using Application.Exceptions;
using AuthPermissions.BaseCode.DataLayer.Classes;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.TenantPlans.Delete;

public sealed record DeleteTenantPlanCommand(int TenantPlanId) : ICommand<string>;

internal sealed class DeleteTenantPlanCommandHandler(AuthPermissionsDbContext context)
    : ICommandHandler<DeleteTenantPlanCommand, string>
{
    public async Task<Response<string>> Handle(DeleteTenantPlanCommand command, CancellationToken cancellationToken)
    {
        TenantPlan? TenantPlanItem = await context.TenantPlans
            .SingleOrDefaultAsync(t => t.Id == command.TenantPlanId, cancellationToken);

        if (TenantPlanItem is null)
        {
            throw new ApiException("TenantPlan not found");
        }

        context.TenantPlans.Remove(TenantPlanItem);

        //TenantPlanItem.Raise(new TenantPlanItemDeletedDomainEvent(TenantPlanItem.Id));

        await context.SaveChangesAsync(cancellationToken);

        return new Response<string>();
    }
}

internal sealed class DeleteTenantPlanCommandValidator : AbstractValidator<DeleteTenantPlanCommand>
{
    public DeleteTenantPlanCommandValidator()
    {
        RuleFor(c => c.TenantPlanId).NotEmpty();
    }
}
