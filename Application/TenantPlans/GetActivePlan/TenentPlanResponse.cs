using AuthPermissions.AdminCode;

namespace Application.TenantPlans.GetActivePlan;

public sealed class TenentPlanResponse
{
    public int Id { get; set; }
    public int TenantPlanId { get; set; }

    public bool IsActive { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }

    public string Remarks { get; set; }

    public string PlanName { get; set; }
    public int PlanRate { get; set; }

    public DateTime CreatedOn { get; set; }

    public List<RoleWithPermissionNamesDto> Roles { get; set; }
    
}
