namespace Domain.Customers;

public class TenantUserProfile: UserProfile
{
    /// <summary>
    /// Global user identifier (FK of AspNetUsers.Id)
    /// </summary>
    public Guid GlobalUserId { get; set; }

    public UserType RoleType { get; set; }

}

public enum UserType
{
    Admin = 1,
    Salesman = 2,
    Accountant = 3,
    DeliveryPerson = 4,
    Manager = 5
}