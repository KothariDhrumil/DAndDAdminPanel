using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example7.BlazorWASMandWebApi.Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class RouteIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TenantCustomerProfiles_RouteId_SequenceNo",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.AlterColumn<int>(
                name: "RouteId",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_TenantCustomerProfiles_RouteId_SequenceNo",
                schema: "retail",
                table: "TenantCustomerProfiles",
                columns: new[] { "RouteId", "SequenceNo" },
                unique: true,
                filter: "[RouteId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TenantCustomerProfiles_RouteId_SequenceNo",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.AlterColumn<int>(
                name: "RouteId",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TenantCustomerProfiles_RouteId_SequenceNo",
                schema: "retail",
                table: "TenantCustomerProfiles",
                columns: new[] { "RouteId", "SequenceNo" },
                unique: true);
        }
    }
}
