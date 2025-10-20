using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using AuthPermissions.AspNetCore.GetDataKeyCode;
using AuthPermissions.BaseCode.CommonCode;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using Domain;
using Domain.AbstactClass;
using Domain.Accounting;
using Domain.Customers;
using Domain.Orders;
using Domain.Todos;
using Infrastructure.DomainEvents;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Persistence.Contexts;

public class RetailDbContext : DbContext, IRetailDbContext
{
    private readonly IDomainEventsDispatcher domainEventsDispatcher;
    private readonly IUserContext userContext;
    private readonly IDateTimeProvider dateTimeProvider;

    public string DataKey { get; }

    public RetailDbContext(DbContextOptions<RetailDbContext> options,
        IGetShardingDataFromUser shardingDataKeyAndConnect,
         IDomainEventsDispatcher domainEventsDispatcher,
         IUserContext userContext,
         IDateTimeProvider dateTimeProvider)
        : base(options)
    {
        // The DataKey is null when: no one is logged in, its a background service, or user hasn't got an assigned tenant
        // In these cases its best to set the data key that doesn't match any possible DataKey 
        DataKey = shardingDataKeyAndConnect?.DataKey ?? "stop any user without a DataKey to access the data";

        if (shardingDataKeyAndConnect?.ConnectionString != null)
            //NOTE: If no connection string is provided the DbContext will use the connection it was provided when it was registered
            //If you don't want that to happen, then remove the if above and the connection will be set to null (and fail) 
            Database.SetConnectionString(shardingDataKeyAndConnect.ConnectionString);

        this.domainEventsDispatcher = domainEventsDispatcher;
        this.userContext = userContext;
        this.dateTimeProvider = dateTimeProvider;
    }

    public DbSet<RetailOutlet> RetailOutlets => Set<RetailOutlet>();
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<TenantCustomerProfile> TenantCustomerProfiles => Set<TenantCustomerProfile>();
    public DbSet<TenantUserProfile> TenantUserProfiles => Set<TenantUserProfile>();
    public DbSet<LedgerEntry> LedgerEntries => Set<LedgerEntry>();
    public DbSet<UserType> UserTypes => Set<UserType>();
    public DbSet<Route> Routes => Set<Route>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<CustomerProduct> CustomerProducts => Set<CustomerProduct>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // It is currently not possible to define multiple query filters on the same entity - only the last one will be applied.
        // However, you can define a single filter with multiple conditions using the logical AND operator (&& in C#).
        // See https://docs.microsoft.com/en-us/ef/core/querying/filters
        // This way you can chain multiple query filters for the entity.
        modelBuilder
           .AppendGlobalQueryFilter<IDataKeyFilterReadOnly>(s => s.DataKey.StartsWith(DataKey));

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RetailDbContext).Assembly);

        modelBuilder.HasDefaultSchema("retail");

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IDataKeyFilterReadOnly).IsAssignableFrom(entityType.ClrType))
            {
                entityType.AddHierarchicalTenantReadOnlyQueryFilter(this);
            }
            else
            {
                throw new Exception(
                    $"You haven't added the {nameof(IDataKeyFilterReadOnly)} to the entity {entityType.ClrType.Name}");
            }

            foreach (var mutableProperty in entityType.GetProperties())
            {
                if (mutableProperty.ClrType == typeof(decimal))
                {
                    mutableProperty.SetPrecision(9);
                    mutableProperty.SetScale(2);
                }
            }
        }

        // Orders config
        modelBuilder.Entity<Order>()
            .ToTable("Orders", "retail");
        modelBuilder.Entity<Order>()
            .Property(x => x.Total).HasPrecision(9, 2);
        modelBuilder.Entity<Order>()
            .HasIndex(x => new { x.CustomerId, x.DataKey });
        modelBuilder.Entity<Order>()
            .HasIndex(x => x.DataKey);
        modelBuilder.Entity<Order>()
            .HasIndex(x => x.OrderedAt);

        // TenantCustomerProfile config (hierarchical customer profiles)
        modelBuilder.Entity<TenantCustomerProfile>()
            .ToTable("TenantCustomerProfiles", "retail");
        modelBuilder.Entity<TenantCustomerProfile>()
            .HasIndex(x => x.DataKey);
        // PK default value to NEWSEQUENTIALID on SQL Server
        if (Database.IsSqlServer())
        {
            modelBuilder.Entity<TenantCustomerProfile>()
                .Property(x => x.TenantUserId)
                .HasDefaultValueSql("NEWSEQUENTIALID()")
                .ValueGeneratedOnAdd();
        }
        // Uniqueness: one customer per tenant
        modelBuilder.Entity<TenantCustomerProfile>()
            .HasIndex(x => new { x.GlobalCustomerId, x.TenantId })
            .IsUnique();
        modelBuilder.Entity<TenantCustomerProfile>()
            .HasIndex(x => x.TenantId);
        modelBuilder.Entity<TenantCustomerProfile>()
            .HasIndex(x => x.ParentGlobalCustomerId);
        modelBuilder.Entity<TenantCustomerProfile>()
            .HasIndex(x => x.HierarchyPath);
        modelBuilder.Entity<TenantCustomerProfile>()
            .HasIndex(x => x.Depth);
        modelBuilder.Entity<TenantCustomerProfile>()
            .HasIndex(x => x.CreatedAt);
        modelBuilder.Entity<TenantCustomerProfile>()
            .HasIndex(x => x.UpdatedAt);
        // Index for RouteId (for fast queries by route)
        modelBuilder.Entity<TenantCustomerProfile>()
         .HasIndex(c => new { c.RouteId, c.SequenceNo })
         .IsUnique(); // Prevent duplicate sequence numbers per route

        modelBuilder.Entity<TenantCustomerProfile>()
            .HasOne(c => c.Route)
            .WithMany(r => r.Customers)
            .HasForeignKey(c => c.RouteId)
            .OnDelete(DeleteBehavior.Restrict);

        // TenantUserProfile config (staff/users like HR/Admin/Salesman)
        modelBuilder.Entity<TenantUserProfile>()
            .ToTable("TenantUserProfiles", "retail");
        modelBuilder.Entity<TenantUserProfile>()
            .HasIndex(x => x.DataKey);
        // PK default value to NEWSEQUENTIALID on SQL Server
        if (Database.IsSqlServer())
        {
            modelBuilder.Entity<TenantUserProfile>()
                .Property(x => x.TenantUserId)
                .HasDefaultValueSql("NEWSEQUENTIALID()")
                .ValueGeneratedOnAdd();
        }
        modelBuilder.Entity<TenantUserProfile>()
            .HasIndex(x => x.TenantId);
        // Uniqueness: one user profile per tenant
        modelBuilder.Entity<TenantUserProfile>()
            .HasIndex(x => new { x.GlobalUserId, x.TenantId })
            .IsUnique();
        // Auditing / time-based queries
        modelBuilder.Entity<TenantUserProfile>()
            .HasIndex(x => x.CreatedAt);
        modelBuilder.Entity<TenantUserProfile>()
            .HasIndex(x => x.UpdatedAt);

        // LedgerEntry config
        modelBuilder.Entity<LedgerEntry>()
            .ToTable("LedgerEntries", "retail");
        modelBuilder.Entity<LedgerEntry>()
            .Property(x => x.Amount).HasPrecision(18, 2);
        modelBuilder.Entity<LedgerEntry>()
            .HasIndex(x => x.DataKey);
        modelBuilder.Entity<LedgerEntry>()
            .HasIndex(x => x.EntryDate);
        modelBuilder.Entity<LedgerEntry>()
            .HasIndex(x => new { x.TenantUserId, x.EntryDate });
        modelBuilder.Entity<LedgerEntry>()
            .HasIndex(x => new { x.TenantUserId, x.EntryDate });

        // Route config
        modelBuilder.Entity<Route>()
            .ToTable("Routes", "retail");
        modelBuilder.Entity<Route>()
            .HasIndex(x => x.DataKey);
        modelBuilder.Entity<Route>()
            .HasIndex(x => x.Id);
        modelBuilder.Entity<Route>()
            .HasIndex(x => x.TenantUserId);
        modelBuilder.Entity<Route>()
            .HasIndex(x => x.IsActive);

        // CustomerRoute config
        modelBuilder.Entity<CustomerRoute>()
            .ToTable("CustomerRoutes", "retail");
        modelBuilder.Entity<CustomerRoute>()
            .HasIndex(x => x.CustomerId);
        modelBuilder.Entity<CustomerRoute>()
            .HasIndex(x => x.RouteId);
        modelBuilder.Entity<CustomerRoute>()
            .HasIndex(x => x.OrderId);
        modelBuilder.Entity<CustomerRoute>()
            .HasIndex(x => x.DataKey);

        // Product config
        modelBuilder.Entity<Product>()
            .ToTable("Products", "retail");
        modelBuilder.Entity<Product>()
            .HasIndex(x => x.Name);
        modelBuilder.Entity<Product>()
            .HasIndex(x => x.HSNCode);
        modelBuilder.Entity<Product>()
            .HasIndex(x => x.Order);
        modelBuilder.Entity<Product>()
            .HasIndex(x => x.DataKey);

        // Customer Product
        modelBuilder.Entity<CustomerProduct>()
       .HasKey(cp => new { cp.CustomerId, cp.ProductId }); // Composite PK

        modelBuilder.Entity<CustomerProduct>()
            .HasOne(cp => cp.Customer)
            .WithMany(c => c.CustomerProducts)
            .HasForeignKey(cp => cp.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CustomerProduct>()
            .HasOne(cp => cp.Product)
            .WithMany(p => p.CustomerProducts)
            .HasForeignKey(cp => cp.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // CustomerProduct config
        modelBuilder.Entity<CustomerProduct>()
            .ToTable("CustomerProducts", "retail");
        modelBuilder.Entity<CustomerProduct>()
            .HasIndex(x => x.CustomerId);
        modelBuilder.Entity<CustomerProduct>()
            .HasIndex(x => x.ProductId);
        modelBuilder.Entity<CustomerProduct>()
            .HasIndex(x => x.DataKey);
        modelBuilder.Entity<CustomerProduct>()
            .HasIndex(x => x.IsActive);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // When should you publish domain events?
        //
        // 1. BEFORE calling SaveChangesAsync
        //     - domain events are part of the same transaction
        //     - immediate consistency
        // 2. AFTER calling SaveChangesAsync
        //     - domain events are a separate transaction
        //     - eventual consistency
        //     - handlers can fail

        this.MarkWithDataKeyIfNeeded(DataKey);

        UpdateAuditableEntities();

        int result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEventsAsync();

        return result;
    }

    private void UpdateAuditableEntities()
    {

        foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = dateTimeProvider.Now;
                    entry.Entity.CreatedBy = userContext.UserId;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = dateTimeProvider.Now;
                    entry.Entity.UpdatedBy = userContext.UserId;
                    break;
            }
        }
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                List<IDomainEvent> domainEvents = entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        await domainEventsDispatcher.DispatchAsync(domainEvents);
    }
}
