using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example7.BlazorWASMandWebApi.Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class AddHirarchicalCustomers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte>(
                name: "Depth",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "HierarchyPath",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LoyaltyPoints",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentGlobalCustomerId",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Preferences",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_TenantCustomerProfiles_Depth",
                schema: "retail",
                table: "TenantCustomerProfiles",
                column: "Depth");

            migrationBuilder.CreateIndex(
                name: "IX_TenantCustomerProfiles_HierarchyPath",
                schema: "retail",
                table: "TenantCustomerProfiles",
                column: "HierarchyPath");

            migrationBuilder.CreateIndex(
                name: "IX_TenantCustomerProfiles_ParentGlobalCustomerId",
                schema: "retail",
                table: "TenantCustomerProfiles",
                column: "ParentGlobalCustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TenantCustomerProfiles_Depth",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_TenantCustomerProfiles_HierarchyPath",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_TenantCustomerProfiles_ParentGlobalCustomerId",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropColumn(
                name: "Depth",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropColumn(
                name: "HierarchyPath",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropColumn(
                name: "LoyaltyPoints",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropColumn(
                name: "ParentGlobalCustomerId",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropColumn(
                name: "Preferences",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "retail",
                table: "TenantCustomerProfiles");
        }
    }
}
