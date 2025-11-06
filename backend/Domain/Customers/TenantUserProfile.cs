namespace Domain.Customers;

public class TenantUserProfile: UserProfile
{
    /// <summary>
    /// Global user identifier (FK of AspNetUsers.Id)
    /// </summary>
    public Guid GlobalUserId { get; set; }

    public UserType UserType { get; set; }

    public int? UserTypeId { get; set; }
}
