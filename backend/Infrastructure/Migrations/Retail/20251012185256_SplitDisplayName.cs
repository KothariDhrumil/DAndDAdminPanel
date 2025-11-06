using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example7.BlazorWASMandWebApi.Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class SplitDisplayName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DisplayName",
                schema: "retail",
                table: "TenantUserProfiles",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "DisplayName",
                schema: "retail",
                table: "TenantCustomerProfiles",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "retail",
                table: "TenantUserProfiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "retail",
                table: "TenantUserProfiles");

            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.RenameColumn(
                name: "LastName",
                schema: "retail",
                table: "TenantUserProfiles",
                newName: "DisplayName");

            migrationBuilder.RenameColumn(
                name: "LastName",
                schema: "retail",
                table: "TenantCustomerProfiles",
                newName: "DisplayName");
        }
    }
}
