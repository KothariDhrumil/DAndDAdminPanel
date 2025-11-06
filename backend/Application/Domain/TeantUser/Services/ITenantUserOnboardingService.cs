using Application.Domain.TeantUser.Models;
using Application.Domain.TeantUser.Update;

namespace Application.Domain.TeantUser.Services
{
    public interface ITenantUserOnboardingService : ITransientService

    {
        Task CreateTenantUserProfileIfMissingAsync(string globalUserId, int tenantId, string firstName, string lastName, string phoneNumber, int userTypeId, CancellationToken ct);
        Task<List<TenantUserProfileResponse>> GetProfilesByRoleTypeId(int userTypeId);
        Task UpdateTenantUserProfileAsync(UpdateTenantUserModel updateTenantUserModel, CancellationToken ct);

    }
}