using AuthPermissions.BaseCode.DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Auth.AuthP;

public class AuthPermissionsDesignTimeFactory
    : IDesignTimeDbContextFactory<AuthPermissionsDbContext>
{
    public AuthPermissionsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AuthPermissionsDbContext>();

        // Use the same provider that you configure at runtime
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\mssqllocaldb;Database=AuthPermissionsDesign;Trusted_Connection=True;MultipleActiveResultSets=true",
            dbOptions =>
            {
                dbOptions.MigrationsHistoryTable("AuthP_History", "authp");
                dbOptions.MigrationsAssembly("AuthPermissions.SqlServer");
            });

        return new AuthPermissionsDbContext(optionsBuilder.Options);
    }
}
