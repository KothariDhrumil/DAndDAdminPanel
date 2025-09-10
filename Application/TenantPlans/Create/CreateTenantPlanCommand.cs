using Application.Abstractions.Messaging;
using AuthPermissions.BaseCode.DataLayer.Classes;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using FluentValidation;
using FluentValidation.Internal;
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

    internal sealed class CreateTenantPlanCommandHandler(
        AuthPermissionsDbContext context)
        : ICommandHandler<CreateTenantPlanCommand, int>
    {
        public async Task<Response<int>> Handle(CreateTenantPlanCommand command, CancellationToken cancellationToken)
        {

            // TODO :first get the plan details from plan table 
            // and update the plan
            var features = await context.Plans.Where(x => x.Id == command.PlanId).Select(x => x.Features).FirstOrDefaultAsync();

            var TenantPlan = new TenantPlan()
            {
                IsActive = command.IsActive,
                TenentId = command.TenantId,
                ValidFrom = command.ValidFrom,
                ValidTo = command.ValidTo,
                Remarks = command.Remarks,
                PlanId = command.PlanId,
                Permissions = features
            };

            // TODO : generate Invoice once plan is assigned 
            //if (planRate > 0)
            //{
            //    var invoiceNo = await invoiceSettingRepositoryAsync.GenerateInvoiceNo();
            //    companyPlan.Invoice = new Invoice()
            //    {
            //        InvoiceNo = invoiceNo,
            //        Status = Domain.Enums.InvoiceStatus.UnPaid,
            //        Amount = planRate
            //    };
            //}

            context.TenantPlans.Add(TenantPlan);

            await context.SaveChangesAsync(cancellationToken);

            return new Response<int>(TenantPlan.Id, string.Empty);
        }
    }
}
public class CreateTenantPlanCommandValidator : AbstractValidator<CreateTenantPlanCommand>
{
    public CreateTenantPlanCommandValidator()
    {
        RuleFor(c => c.PlanId).NotNull().GreaterThan(0);

    }
}