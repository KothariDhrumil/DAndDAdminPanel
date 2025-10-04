using Domain;

namespace Application.Customers.Services
{
    public interface ICustomerOnboardingService : ITransientService

    {
        Task<Guid> EnsureCustomerAccountAsync(ApplicationUser user, string phone, string email, string? firstName, string? lastName, CancellationToken ct);
        Task EnsureLinkedToTenantAsync(Guid globalCustomerId, int tenantId, string? displayName, CancellationToken ct);
        Task<ApplicationUser> EnsureUserAsync(string phone, string firstName, string lastName, CancellationToken ct);
    }
}