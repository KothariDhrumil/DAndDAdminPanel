using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example7.BlazorWASMandWebApi.Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class AttachingUserType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserTypeId",
                schema: "retail",
                table: "TenantUserProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TenantUserProfiles_UserTypeId",
                schema: "retail",
                table: "TenantUserProfiles",
                column: "UserTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantUserProfiles_UserTypes_UserTypeId",
                schema: "retail",
                table: "TenantUserProfiles",
                column: "UserTypeId",
                principalSchema: "retail",
                principalTable: "UserTypes",
                principalColumn: "UserTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantUserProfiles_UserTypes_UserTypeId",
                schema: "retail",
                table: "TenantUserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_TenantUserProfiles_UserTypeId",
                schema: "retail",
                table: "TenantUserProfiles");

            migrationBuilder.DropColumn(
                name: "UserTypeId",
                schema: "retail",
                table: "TenantUserProfiles");
        }
    }
}
