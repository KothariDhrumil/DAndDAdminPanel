using Application.Abstractions.Authentication;
using Application.Abstractions.Persistence;
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
using Domain;
using Infrastructure.Auth.Jwt;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Net.DistributedFileStoreCache;
using RunMethodsSequentially;
using Shared;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Infrastructure.Auth.AuthP;

public static class DependencyInjection
{
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
                Issuer = jwtSettings.RetailKey,
                Audience = jwtSettings.RetailKey,
                SigningKey = jwtSettings.Key,
                TokenExpires = new TimeSpan(0, jwtSettings.TokenExpirationInMinutes, 0),
                RefreshTokenExpires = TimeSpan.FromDays(jwtSettings.RefreshTokenExpirationInDays)
            };
        })

        //NOTE: This uses the same database as the individual accounts DB
        .UsingEfCoreSqlServer(connectionString)
        .SetupMultiTenantSharding(new ShardingEntryOptions(true))
        .IndividualAccountsAuthentication<ApplicationUser>()
        .RegisterAddClaimToUser<AddGlobalChangeTimeClaim>()
        .RegisterAddClaimToUser<AddTenantNameClaim>()
        .RegisterTenantChangeService<RetailTenantChangeService>()
        .AddAuthUsersIfEmpty(AppAuthSetupData.UsersRolesDefinition)
        .AddRolesPermissionsIfEmpty(AppAuthSetupData.RolesDefinition)
        .RegisterFindUserInfoService<IndividualAccountUserLookup>()
        .RegisterAuthenticationProviderReader<SyncIndividualAccountUsers>()
        .AddSuperUserToIndividualAccounts<ApplicationUser>()
        .SetupAspNetCoreAndDatabase(options =>
        {
            //Migrate individual account database
            options.RegisterServiceToRunInJob<StartupServiceMigrateAnyDbContext<AppIdentityDbContext>>();



            //Migrate the application part of the database
            options.RegisterServiceToRunInJob<StartupServiceMigrateAnyDbContext<RetailDbContext>>();

        });

        //This is used to set app statue as "Down" and tenant as "Down",
        //plus handling a tenant DataKey change that requires an update of the user's claims
        services.AddDistributedFileStoreCache(options =>
        {
            options.WhichVersion = FileStoreCacheVersions.Class;
            //I override the the default first part of the FileStore cache file because there are many example apps in this repo
            options.FirstPartOfCacheFileName = "CacheFileStore";
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
        services.AddTransient<IAddNewUserManager, IndividualUserAddUserManager<ApplicationUser>>();
        services.AddTransient<ISignInAndCreateTenant, SignInAndCreateTenant>();
        services.AddTransient<ITenantRetailDbContextFactory, TenantRetailDbContextFactory>();
        services.AddTransient<ICustomerTokenService, CustomerTokenService>();


        return services;
    }

}

