using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example7.BlazorWASMandWebApi.Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class RemovedCustomerRoute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RouteId",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SequenceNo",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "DataKey",
                schema: "retail",
                table: "CustomerRoutes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldUnicode: false,
                oldMaxLength: 250);

            migrationBuilder.CreateIndex(
                name: "IX_TenantCustomerProfiles_RouteId_SequenceNo",
                schema: "retail",
                table: "TenantCustomerProfiles",
                columns: new[] { "RouteId", "SequenceNo" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantCustomerProfiles_Routes_RouteId",
                schema: "retail",
                table: "TenantCustomerProfiles",
                column: "RouteId",
                principalSchema: "retail",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantCustomerProfiles_Routes_RouteId",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_TenantCustomerProfiles_RouteId_SequenceNo",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropColumn(
                name: "RouteId",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropColumn(
                name: "SequenceNo",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.AlterColumn<string>(
                name: "DataKey",
                schema: "retail",
                table: "CustomerRoutes",
                type: "varchar(250)",
                unicode: false,
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
