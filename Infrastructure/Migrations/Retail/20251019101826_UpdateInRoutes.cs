using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example7.BlazorWASMandWebApi.Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class UpdateInRoutes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_TenantUserProfiles_TenantUserId",
                schema: "retail",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_TeantUserId",
                schema: "retail",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "TeantUserId",
                schema: "retail",
                table: "Routes");

            migrationBuilder.AlterColumn<string>(
                name: "TenantUserId",
                schema: "retail",
                table: "Routes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantUserId1",
                schema: "retail",
                table: "Routes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Routes_TenantUserId1",
                schema: "retail",
                table: "Routes",
                column: "TenantUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_TenantUserProfiles_TenantUserId1",
                schema: "retail",
                table: "Routes",
                column: "TenantUserId1",
                principalSchema: "retail",
                principalTable: "TenantUserProfiles",
                principalColumn: "TenantUserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_TenantUserProfiles_TenantUserId1",
                schema: "retail",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_TenantUserId1",
                schema: "retail",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "TenantUserId1",
                schema: "retail",
                table: "Routes");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantUserId",
                schema: "retail",
                table: "Routes",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "TeantUserId",
                schema: "retail",
                table: "Routes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_TeantUserId",
                schema: "retail",
                table: "Routes",
                column: "TeantUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_TenantUserProfiles_TenantUserId",
                schema: "retail",
                table: "Routes",
                column: "TenantUserId",
                principalSchema: "retail",
                principalTable: "TenantUserProfiles",
                principalColumn: "TenantUserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
