using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example7.BlazorWASMandWebApi.Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class UpdateInTenantCustomerProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_GlobalCustomerId_DataKey",
                schema: "retail",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "GlobalCustomerId",
                schema: "retail",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "TenantCustomerId",
                schema: "retail",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TenantCustomerId_DataKey",
                schema: "retail",
                table: "Orders",
                columns: new[] { "TenantCustomerId", "DataKey" });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_TenantCustomerProfiles_TenantCustomerId",
                schema: "retail",
                table: "Orders",
                column: "TenantCustomerId",
                principalSchema: "retail",
                principalTable: "TenantCustomerProfiles",
                principalColumn: "TenantCustomerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_TenantCustomerProfiles_TenantCustomerId",
                schema: "retail",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_TenantCustomerId_DataKey",
                schema: "retail",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TenantCustomerId",
                schema: "retail",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "GlobalCustomerId",
                schema: "retail",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_GlobalCustomerId_DataKey",
                schema: "retail",
                table: "Orders",
                columns: new[] { "GlobalCustomerId", "DataKey" });
        }
    }
}
