using Application.Abstractions.Messaging;
using AuthPermissions.BaseCode.DataLayer.Classes;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using FluentValidation;
using SharedKernel;

namespace Application.Plans.Create;

public sealed class CreatePlanCommand : ICommand<int>
{
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public int PlanValidityInDays { get; set; }
    public int PlanRate { get; set; }
    public bool IsActive { get; set; }
     
    public List<int> RoleIds { get; set; }
        
    internal sealed class CreatePlanCommandHandler(
        AuthPermissionsDbContext context)
        : ICommandHandler<CreatePlanCommand, int>
    {
        public async Task<Response<int>> Handle(CreatePlanCommand command, CancellationToken cancellationToken)
        {
            // TODO : Check for unique / Duplicate name


            var plan = new Plan()
            {
                Name = command.Name,
                IsActive = command.IsActive,
                PlanRate = command.PlanRate,
                PlanValidityInDays = command.PlanValidityInDays,
                Description = command.Description,
                Features = string.Join(",", command.RoleIds)
            };
            context.Plans.Add(plan);

            await context.SaveChangesAsync(cancellationToken);

            return Response.Success(plan.Id);
        }
    }
}
public class CreatePlanCommandValidator : AbstractValidator<CreatePlanCommand>
{
    public CreatePlanCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MaximumLength(255);
    }
}