namespace Application.Plans.Get;

public class PlanResponse
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    public int PlanValidityInDays { get; set; }
    public int PlanRate { get; set; } = 0;
    public bool IsActive { get; set; }
    // A list of permission names included in this plan
    public List<int> RoleIds { get; set; }
}
