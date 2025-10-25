using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example7.BlazorWASMandWebApi.Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class PendingMigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PurchaseUnitProduct_IsActive",
                schema: "retail",
                table: "PurchaseUnitProduct",
                column: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PurchaseUnitProduct_IsActive",
                schema: "retail",
                table: "PurchaseUnitProduct");
        }
    }
}
