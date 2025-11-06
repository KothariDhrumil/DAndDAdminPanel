using Application.Abstractions.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstractions.Persistence;

public interface ITenantRetailDbContextFactory
{
    Task<IRetailDbContext> CreateAsync(int tenantId, CancellationToken ct);
}
