using AuthPermissions.BaseCode.DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.DesignFactory
{
    public class AuthPermissionsDesignTimeFactory
       : IDesignTimeDbContextFactory<AuthPermissionsDbContext>
    {
        public AuthPermissionsDbContext CreateDbContext(string[] args)
        {
            // Use your local dev connection string here
            var connectionString = "Server=.;Database=YourAuthPDb;Trusted_Connection=True;TrustServerCertificate=True;";

            var optionsBuilder = new DbContextOptionsBuilder<AuthPermissionsDbContext>();
            optionsBuilder.UseSqlServer(connectionString, sql =>
            {
                // 🔥 this line is critical
                sql.MigrationsAssembly(typeof(AuthPermissionsDesignTimeFactory).Assembly.GetName().Name);
            });

            return new AuthPermissionsDbContext(optionsBuilder.Options);
        }
    }
}
