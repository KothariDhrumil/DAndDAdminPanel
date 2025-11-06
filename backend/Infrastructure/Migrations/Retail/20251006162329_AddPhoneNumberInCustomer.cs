using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example7.BlazorWASMandWebApi.Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class AddPhoneNumberInCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoyaltyPoints",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropColumn(
                name: "Preferences",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.AddColumn<int>(
                name: "LoyaltyPoints",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Preferences",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
