using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example7.BlazorWASMandWebApi.Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class AddFieldsInCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "CourierChargesApplicable",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "CreditLimit",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "GSTName",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GSTNumber",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "OpeningBalance",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "TaxEnabled",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropColumn(
                name: "CourierChargesApplicable",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropColumn(
                name: "CreditLimit",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropColumn(
                name: "GSTName",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropColumn(
                name: "GSTNumber",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropColumn(
                name: "OpeningBalance",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropColumn(
                name: "TaxEnabled",
                schema: "retail",
                table: "TenantCustomerProfiles");
        }
    }
}
