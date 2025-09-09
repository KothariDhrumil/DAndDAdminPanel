using Application.Abstractions.Messaging;
using Application.Exceptions;
using AuthPermissions.BaseCode.DataLayer.Classes;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Plans.Delete;

public sealed record DeletePlanCommand(int PlanId) : ICommand<string>;

internal sealed class DeletePlanCommandHandler(AuthPermissionsDbContext context)
    : ICommandHandler<DeletePlanCommand, string>
{
    public async Task<Response<string>> Handle(DeletePlanCommand command, CancellationToken cancellationToken)
    {
        Plan? PlanItem = await context.Plans
            .SingleOrDefaultAsync(t => t.Id == command.PlanId, cancellationToken);

        if (PlanItem is null)
        {
            throw new ApiException("Plan not found");
        }

        context.Plans.Remove(PlanItem);

        //PlanItem.Raise(new PlanItemDeletedDomainEvent(PlanItem.Id));

        await context.SaveChangesAsync(cancellationToken);

        return new Response<string>();
    }
}

internal sealed class DeletePlanCommandValidator : AbstractValidator<DeletePlanCommand>
{
    public DeletePlanCommandValidator()
    {
        RuleFor(c => c.PlanId).NotEmpty();
    }
}
