namespace Application.TenantPlans.Get;

public class TenantPlanInfo
{
    public int Id { get; set; }
    public int PlanId { get; set; }

    public bool IsActive { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }

    public string Remarks { get; set; }

    public string PlanName { get; set; }
    public int PlanRate { get; set; }

    public DateTime CreatedOn { get; set; }
    public string Tenant { get; set; }
}
