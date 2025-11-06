using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example7.BlazorWASMandWebApi.Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class NullableTenantUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseUnits_TenantUserProfiles_TenantUserId",
                schema: "retail",
                table: "PurchaseUnits");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantUserId",
                schema: "retail",
                table: "PurchaseUnits",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseUnits_TenantUserProfiles_TenantUserId",
                schema: "retail",
                table: "PurchaseUnits",
                column: "TenantUserId",
                principalSchema: "retail",
                principalTable: "TenantUserProfiles",
                principalColumn: "TenantUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseUnits_TenantUserProfiles_TenantUserId",
                schema: "retail",
                table: "PurchaseUnits");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantUserId",
                schema: "retail",
                table: "PurchaseUnits",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseUnits_TenantUserProfiles_TenantUserId",
                schema: "retail",
                table: "PurchaseUnits",
                column: "TenantUserId",
                principalSchema: "retail",
                principalTable: "TenantUserProfiles",
                principalColumn: "TenantUserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
