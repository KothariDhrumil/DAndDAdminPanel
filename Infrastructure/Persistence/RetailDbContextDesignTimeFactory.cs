using Application.Abstractions.Authentication;
using AuthPermissions.AspNetCore.GetDataKeyCode;
using Infrastructure.DomainEvents;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SharedKernel;

namespace Infrastructure.Persistence;

public class RetailDbContextDesignTimeFactory : IDesignTimeDbContextFactory<RetailDbContext>
{
    // Design-time only connection string for EF Core migrations
    // TrustServerCertificate=True is only acceptable here as this is never used in production
    // At runtime, actual connection strings from configuration are used
    private const string DesignTimeConnectionString = "Server=localhost;Database=DesignTime;Trusted_Connection=True;TrustServerCertificate=True;";
    
    public RetailDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<RetailDbContext>();
        
        // Use a connection string for design-time (migrations)
        // This will be replaced at runtime with actual connection strings
        optionsBuilder.UseSqlServer(DesignTimeConnectionString);
        
        // Create stub implementations for design-time
        var stubSharding = new StubGetShardingDataFromUser();
        var stubDispatcher = new StubDomainEventsDispatcher();
        var stubUserContext = new StubUserContext();
        var stubDateTimeProvider = new StubDateTimeProvider();
        
        return new RetailDbContext(optionsBuilder.Options, stubSharding, stubDispatcher, stubUserContext, stubDateTimeProvider);
    }
    
    private sealed class StubGetShardingDataFromUser : IGetShardingDataFromUser
    {
        public string DataKey => "DesignTime";
        public string ConnectionString => DesignTimeConnectionString;
    }
    
    private sealed class StubDomainEventsDispatcher : IDomainEventsDispatcher
    {
        public Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default) => Task.CompletedTask;
    }
    
    private sealed class StubUserContext : IUserContext
    {
        public Guid UserId => Guid.Empty;
    }
    
    private sealed class StubDateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
        public DateTime Now => DateTime.UtcNow;
    }
}
