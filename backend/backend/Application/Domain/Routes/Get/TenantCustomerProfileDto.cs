namespace Application.Domain.Routes.Get;

public class TenantCustomerProfileDto
{
    public Guid TenantUserId { get; set; }
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
}
