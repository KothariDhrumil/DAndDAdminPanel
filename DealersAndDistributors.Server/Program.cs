using AuthPermissions;
using AuthPermissions.BaseCode;
using AuthPermissions.AspNetCore;
using AuthPermissions.AspNetCore.Services;
using DealersAndDistributors.Server.PermissionsCode;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using RunMethodsSequentially;
using AuthPermissions.AspNetCore.StartupServices;
using AuthPermissions.BaseCode.SetupCode;
using AuthPermissions.AspNetCore.ShardingServices;
using AuthPermissions.SupportCode.DownStatusCode;
using AuthPermissions.BaseCode.DataLayer;
using Net.DistributedFileStoreCache;
using System.Reflection;
using GenericServices.Setup;
using Example6.MvcWebApp.Sharding.Data;
using DealersAndDistributors.Server.Services;
using DealersAndDistributors.Server.EfCoreCode;
using Example6.SingleLevelSharding.AppStart;
using DealersAndDistributors.Server.AppStart;
using DealersAndDistributors.Server.Models;
using DealersAndDistributors.Server.ClaimsChangeCode;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("IdentityConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
        options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Configure Authentication using JWT token with refresh capability
var jwtData = new JwtSetupData();
builder.Configuration.Bind("JwtData", jwtData);
//The solution to getting the nameidentifier claim to have the user's Id was found in https://stackoverflow.com/a/70315108/1434764
JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtData.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtData.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtData.SigningKey)),
            ClockSkew = TimeSpan.Zero //The default is 5 minutes, but we want a quick expires for JTW refresh

        };

        //This code came from https://www.blinkingcaret.com/2018/05/30/refresh-tokens-in-asp-net-core-web-api/
        //It returns a useful header if the JWT Token has expired
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    context.Response.Headers.Add("Token-Expired", "true");
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.RegisterAuthPermissions<Example6Permissions>(options =>
{
    //This tells AuthP that you don't have multiple instances of your app running,
    //so it can run the startup services without a global lock
    options.UseLocksToUpdateGlobalResources = false;

    options.TenantType = TenantTypes.HierarchicalTenant;
    options.EncryptionKey = builder.Configuration[nameof(AuthPermissionsOptions.EncryptionKey)];
    options.PathToFolderToLock = builder.Environment.WebRootPath;
    options.SecondPartOfShardingFile = builder.Environment.EnvironmentName;
    options.Configuration = builder.Configuration;

    options.ConfigureAuthPJwtToken = new AuthPJwtConfiguration
    {
        Issuer = jwtData.Issuer,
        Audience = jwtData.Audience,
        SigningKey = jwtData.SigningKey,
        TokenExpires = new TimeSpan(0, 5, 0), //Quick Token expiration because we use a refresh token
        RefreshTokenExpires = new TimeSpan(1, 0, 0, 0) //Refresh token is valid for one day
    };
})
    //NOTE: This uses the same database as the individual accounts DB
    .UsingEfCoreSqlServer(connectionString)
    //AuthP version 5 and above: Use this method to configure sharding
    .SetupMultiTenantSharding(new ShardingEntryOptions(true))
    .IndividualAccountsAuthentication()
    .RegisterAddClaimToUser<AddTenantNameClaim>()
    .RegisterAddClaimToUser<AddGlobalChangeTimeClaim>()
    .RegisterTenantChangeService<ShardingTenantChangeService>()
    .AddRolesPermissionsIfEmpty(Example6AppAuthSetupData.RolesDefinition)
    .AddTenantsIfEmpty(Example6AppAuthSetupData.TenantDefinition)
    .AddAuthUsersIfEmpty(Example6AppAuthSetupData.UsersRolesDefinition)
    .RegisterFindUserInfoService<IndividualAccountUserLookup>()
    .RegisterAuthenticationProviderReader<SyncIndividualAccountUsers>()
    .AddSuperUserToIndividualAccounts()
    .SetupAspNetCoreAndDatabase(options =>
    {
        //Migrate individual account database
        options.RegisterServiceToRunInJob<StartupServiceMigrateAnyDbContext<ApplicationDbContext>>();
        //Add demo users to the database (if no individual account exist)
        options.RegisterServiceToRunInJob<StartupServicesIndividualAccountsAddDemoUsers>();

        //Migrate the application part of the database
        options.RegisterServiceToRunInJob<StartupServiceMigrateAnyDbContext<ShardingSingleDbContext>>();
        //This seeds the invoice database (if empty)
        options.RegisterServiceToRunInJob<StartupServiceSeedShardingDbContext>();
    });

//This is used for a) hold the sharding entries and b) to set a tenant as "Down",
builder.Services.AddDistributedFileStoreCache(options =>
{
    options.WhichVersion = FileStoreCacheVersions.Class;
    options.FirstPartOfCacheFileName = "Example6CacheFileStore";
}, builder.Environment);

//manually add services from the AuthPermissions.SupportCode project
builder.Services.AddSingleton<IGlobalChangeTimeService, GlobalChangeTimeService>(); //used for "update claims on a change" feature
builder.Services.AddSingleton<IDatabaseStateChangeEvent, TenantKeyOrShardChangeService>(); //triggers the "update claims on a change" feature
builder.Services.AddScoped<IDatabaseStateChangeEvent, RoleChangedDetectorService>();
builder.Services.AddScoped<IDatabaseStateChangeEvent, EmailChangeDetectorService>();
builder.Services.AddTransient<ISetRemoveStatus, SetRemoveStatus>();


//thanks to: https://www.c-sharpcorner.com/article/authentication-and-authorization-in-asp-net-5-with-jwt-and-swagger/
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiWithToken.IndividualAccounts", Version = "v1" });

    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securitySchema);

    var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };

    c.AddSecurityRequirement(securityRequirement);
});


//AuthP version 5 and above: REMOVE THIS LINE. This now done via the SetupMultiTenantSharding extension method
//builder.Services.AddTransient<IAccessDatabaseInformation, AccessDatabaseInformation>();

builder.Services.RegisterExample6Invoices(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//WARNING: To make this example easy to use I wipe the FileStore cache on startup.
//In normal use you might not want to do this. 
var fsCache = app.Services.GetRequiredService<IDistributedFileStoreCacheClass>();
fsCache.ClearAll();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UsePermissionsChange();   //Example of updating the user's Permission claim when the database change in app using JWT Token for Authentication / Authorization
app.UseAddEmailClaimToUsers();//Example of adding an extra Email 

app.MapControllers();
app.UseDownForMaintenance(TenantTypes.HierarchicalTenant);
//app.MapFallbackToFile("/index.html");

app.Run();
