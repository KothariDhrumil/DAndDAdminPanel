using Application.Abstractions.Messaging;
using Application.Exceptions;
using AuthPermissions.BaseCode.DataLayer.Classes;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Plans.Delete;

public sealed record DeletePlanCommand(int PlanId) : ICommand;

internal sealed class DeletePlanCommandHandler(AuthPermissionsDbContext context)
    : ICommandHandler<DeletePlanCommand>
{
    public async Task<Response> Handle(DeletePlanCommand command, CancellationToken cancellationToken)
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

        return Response.Success();
    }
}

internal sealed class DeletePlanCommandValidator : AbstractValidator<DeletePlanCommand>
{
    public DeletePlanCommandValidator()
    {
        RuleFor(c => c.PlanId).NotEmpty();
    }
}
