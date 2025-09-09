namespace Application.Plans.GetById;

public sealed class PlanResponse
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    public int PlanValidityInDays { get; set; }
    public int PlanRate { get; set; } = 0;
    public bool IsActive { get; set; }
}
