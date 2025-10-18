using Application.Domain.TeantUser.Update;

namespace Application.Customers.Services
{
    public interface ITenantUserOnboardingService : ITransientService

    {
        Task CreateTenantUserProfileIfMissingAsync(string globalUserId, int tenantId, string firstName, string lastName, string phoneNumber, CancellationToken ct);
        Task UpdateTenantUserProfileAsync(UpdateTenantUserModel updateTenantUserModel, CancellationToken ct);

    }
}