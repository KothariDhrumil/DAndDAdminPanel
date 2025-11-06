using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example7.BlazorWASMandWebApi.Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class RemovedStaticUserTypeAndAddedDynamic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TenantUserProfiles_RoleType",
                schema: "retail",
                table: "TenantUserProfiles");

            migrationBuilder.DropColumn(
                name: "RoleType",
                schema: "retail",
                table: "TenantUserProfiles");

            migrationBuilder.CreateTable(
                name: "UserTypes",
                schema: "retail",
                columns: table => new
                {
                    UserTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypes", x => x.UserTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTypes_DataKey",
                schema: "retail",
                table: "UserTypes",
                column: "DataKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTypes",
                schema: "retail");

            migrationBuilder.AddColumn<int>(
                name: "RoleType",
                schema: "retail",
                table: "TenantUserProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TenantUserProfiles_RoleType",
                schema: "retail",
                table: "TenantUserProfiles",
                column: "RoleType");
        }
    }
}
