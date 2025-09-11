namespace Infrastructure.Identity;

public class CreateUserRequest
{
    public string? PhoneNumber { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string TenantName { get; set; } = default!;
    public string Version { get; set; } = default!;
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public int? DesignationId { get; set; }
}

