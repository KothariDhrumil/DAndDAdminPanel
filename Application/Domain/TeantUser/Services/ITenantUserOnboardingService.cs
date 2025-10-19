using Application.Domain.TeantUser.Update;

namespace Application.Customers.Services
{
    public interface ITenantUserOnboardingService : ITransientService

    {
        Task CreateTenantUserProfileIfMissingAsync(string globalUserId, int tenantId, string firstName, string lastName, string phoneNumber, int userTypeId, CancellationToken ct);
        Task<List<string>> GetProfilesByRoleTypeId(int userTypeId);
        Task UpdateTenantUserProfileAsync(UpdateTenantUserModel updateTenantUserModel, CancellationToken ct);

    }
}