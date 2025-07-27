using AuthPermissions;
using AuthPermissions.AspNetCore;
using AuthPermissions.AspNetCore.Services;
using AuthPermissions.AspNetCore.ShardingServices;
using AuthPermissions.AspNetCore.StartupServices;
using AuthPermissions.BaseCode;
using AuthPermissions.BaseCode.DataLayer;
using AuthPermissions.BaseCode.SetupCode;
using AuthPermissions.SupportCode;
using AuthPermissions.SupportCode.AddUsersServices;
using AuthPermissions.SupportCode.AddUsersServices.Authentication;
using AuthPermissions.SupportCode.DownStatusCode;
using Infrastructure.Auth.Jwt;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Net.DistributedFileStoreCache;
using NetCore.AutoRegisterDi;
using RunMethodsSequentially;
using Shared;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Infrastructure.Auth.AuthP;

public static class StartupExtensions
{
    public const string RetailDbContextHistoryName = "Retail-DbContextHistory";

    internal static IServiceCollection AddAuthP(this IServiceCollection services, ConfigurationManager configuration, IWebHostEnvironment webHostEnvironment)
    {
        string? connectionString = configuration.GetConnectionString("IdentityConnection");

        var provider = services.BuildServiceProvider();
        var jwtSettings = provider.GetRequiredService<IOptions<JwtSettings>>().Value;

        services.RegisterAuthPermissions<Permissions>(options =>
        {
            options.TenantType = TenantTypes.HierarchicalTenant;
            options.EncryptionKey = configuration[nameof(AuthPermissionsOptions.EncryptionKey)];
            options.PathToFolderToLock = webHostEnvironment.ContentRootPath;
            options.Configuration = configuration;
            options.SecondPartOfShardingFile = webHostEnvironment.EnvironmentName;

            // This sets up the JWT Token. The config is suitable for using the Refresh Token with your JWT Token
            options.ConfigureAuthPJwtToken = new AuthPJwtConfiguration
            {
                Issuer = "retail",
                Audience = "retail",
                SigningKey = jwtSettings.Key,
                TokenExpires = new TimeSpan(0, jwtSettings.TokenExpirationInMinutes, 0),
                RefreshTokenExpires = TimeSpan.FromDays(jwtSettings.RefreshTokenExpirationInDays)
            };
        })

        //NOTE: This uses the same database as the individual accounts DB
        .UsingEfCoreSqlServer(connectionString)
        .SetupMultiTenantSharding(new ShardingEntryOptions(true)) 
        .IndividualAccountsAuthentication()
        .RegisterAddClaimToUser<AddGlobalChangeTimeClaim>()
        .RegisterAddClaimToUser<AddTenantNameClaim>()
        .RegisterTenantChangeService<RetailTenantChangeService>()
        .AddRolesPermissionsIfEmpty(Example7AppAuthSetupData.RolesDefinition)
        .AddTenantsIfEmpty(Example7AppAuthSetupData.TenantDefinition)
        .AddAuthUsersIfEmpty(Example7AppAuthSetupData.UsersRolesDefinition)
        .RegisterFindUserInfoService<IndividualAccountUserLookup>()
        .RegisterAuthenticationProviderReader<SyncIndividualAccountUsers>()
        .AddSuperUserToIndividualAccounts()
        .SetupAspNetCoreAndDatabase(options =>
        {
            //Migrate individual account database
            options.RegisterServiceToRunInJob<StartupServiceMigrateAnyDbContext<AppIdentityDbContext>>();
            //Add demo users to the database
            options.RegisterServiceToRunInJob<StartupServicesIndividualAccountsAddDemoUsers>();

            //Migrate the application part of the database
            options.RegisterServiceToRunInJob<StartupServiceMigrateAnyDbContext<RetailDbContext>>();
            //This seeds the invoice database (if empty)
            options.RegisterServiceToRunInJob<StartupServiceServiceSeedRetailDatabase>();
        });

        //This is used to set app statue as "Down" and tenant as "Down",
        //plus handling a tenant DataKey change that requires an update of the user's claims
        services.AddDistributedFileStoreCache(options =>
        {
            options.WhichVersion = FileStoreCacheVersions.Class;
            //I override the the default first part of the FileStore cache file because there are many example apps in this repo
            options.FirstPartOfCacheFileName = "Example7CacheFileStore";
            options.JsonSerializerForCacheFile = new JsonSerializerOptions
            {
                //This will make the json in the FileStore json file will be easier to read
                //BUT it will be a bit slower and take up more characters
                WriteIndented = true,              
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
        }, webHostEnvironment);

        //Have to manually register this as its in the SupportCode project
        services.AddSingleton<IGlobalChangeTimeService, GlobalChangeTimeService>(); //used for "update claims on a change" feature
        services.AddSingleton<IDatabaseStateChangeEvent, TenantKeyOrShardChangeService>(); //triggers the "update claims on a change" feature
        services.AddTransient<ISetRemoveStatus, SetRemoveStatus>(); //Used for "down for maintenance" feature  
        services.AddTransient<ISignUpGetShardingEntry, DemoShardOnlyGetDatabaseForNewTenant>(); //handles sharding tenants
        services.AddTransient<IAddNewUserManager, IndividualUserAddUserManager<IdentityUser>>();
        services.AddTransient<ISignInAndCreateTenant, SignInAndCreateTenant>();

        //This registers all the code to handle the shop part of the demo
        //Register RetailDbContext database and some services (included hosted services)
        services.RegisterExample7ShopCode(configuration);
        //Add GenericServices (after registering the RetailDbContext context)
        //services.GenericServicesSimpleSetup<RetailDbContext>(Assembly.GetAssembly(typeof(StartupExtensions)));
        return services;
    }

    private static void RegisterExample7ShopCode(this IServiceCollection services, IConfiguration configuration)
    {
        //Register any services in this project
        services.RegisterAssemblyPublicNonGenericClasses()
            .Where(c => c.Name.EndsWith("Service"))  //optional
            .AsPublicImplementedInterfaces();

        //Register the retail database to the same database used for individual accounts and AuthP database
        services.AddDbContext<RetailDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("ShardingConnection"), dbOptions =>
            dbOptions.MigrationsHistoryTable(RetailDbContextHistoryName)));


    }
}

