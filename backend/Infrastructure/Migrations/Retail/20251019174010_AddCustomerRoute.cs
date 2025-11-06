using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example7.BlazorWASMandWebApi.Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class AddCustomerRoute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserTypeId",
                schema: "retail",
                table: "UserTypes",
                newName: "Id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                schema: "retail",
                table: "UserTypes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "retail",
                table: "UserTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                schema: "retail",
                table: "UserTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "retail",
                table: "Routes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "retail",
                table: "Routes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "retail",
                table: "Routes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                schema: "retail",
                table: "Routes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CustomerRoutes",
                schema: "retail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerRoutes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerRoutes_Routes_RouteId",
                        column: x => x.RouteId,
                        principalSchema: "retail",
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerRoutes_TenantCustomerProfiles_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "retail",
                        principalTable: "TenantCustomerProfiles",
                        principalColumn: "TenantUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRoutes_CustomerId",
                schema: "retail",
                table: "CustomerRoutes",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRoutes_DataKey",
                schema: "retail",
                table: "CustomerRoutes",
                column: "DataKey");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRoutes_OrderId",
                schema: "retail",
                table: "CustomerRoutes",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRoutes_RouteId",
                schema: "retail",
                table: "CustomerRoutes",
                column: "RouteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerRoutes",
                schema: "retail");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "retail",
                table: "UserTypes");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "retail",
                table: "UserTypes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "retail",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "retail",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "retail",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "retail",
                table: "Routes");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "retail",
                table: "UserTypes",
                newName: "UserTypeId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                schema: "retail",
                table: "UserTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
