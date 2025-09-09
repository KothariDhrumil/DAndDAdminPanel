using Application.Abstractions.Messaging;
using Application.Exceptions;
using AuthPermissions.BaseCode.DataLayer.Classes;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Plans.Update;

public sealed record UpdatePlanCommand(
    int PlanId,
    string Name) : ICommand<int>;


internal sealed class UpdatePlanCommandHandler(
    AuthPermissionsDbContext context)
    : ICommandHandler<UpdatePlanCommand, int>
{
    public async Task<Response<int>> Handle(UpdatePlanCommand command, CancellationToken cancellationToken)
    {
        Plan? PlanItem = await context.Plans
            .SingleOrDefaultAsync(t => t.Id == command.PlanId, cancellationToken);

        if (PlanItem is null)
        {
            throw new ApiException(GenericErrors.NotFound.Description);
        }

        PlanItem.Name = command.Name;

        await context.SaveChangesAsync(cancellationToken);

        return new Response<int>(PlanItem.Id);
    }
}