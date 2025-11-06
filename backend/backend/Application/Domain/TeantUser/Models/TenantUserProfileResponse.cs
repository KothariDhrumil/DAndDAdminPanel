namespace Application.Domain.TeantUser.Models;

public sealed class TenantUserProfileResponse
{
    public string UserId { get; set; }
    public Guid tenantUserId { get; set; }
    public string UserType { get; set; }
    public int? UserTypeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string[] RoleNames { get; set; }
}
