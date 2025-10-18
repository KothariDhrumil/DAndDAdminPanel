namespace Application.Customers.Services
{
    public interface ITenantUserOnboardingService : ITransientService

    {
        Task CreateTenantUserProfileIfMissingAsync(string globalUserId, int tenantId, string firstName, string lastName, string phoneNumber, CancellationToken ct);
        Task UpdateTenantUserProfileAsync(Guid globalUserId, string firstName, string lastName, CancellationToken ct);

    }
}