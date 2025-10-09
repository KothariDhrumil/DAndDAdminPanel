namespace Application.Customers.Services
{
    public interface ITenantUserOnboardingService : ITransientService

    {
        Task CreateTenantUserProfileIfMissingAsync(string globalUserId, int tenantId, string? displayName, string phoneNumber, CancellationToken ct);
        Task UpdateTenantUserProfileAsync(Guid globalUserId, string? displayName, CancellationToken ct);

    }
}