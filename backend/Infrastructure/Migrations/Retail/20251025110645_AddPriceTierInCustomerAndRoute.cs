using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example7.BlazorWASMandWebApi.Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class AddPriceTierInCustomerAndRoute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerPriceTiers",
                schema: "retail");

            migrationBuilder.DropTable(
                name: "RoutePriceTiers",
                schema: "retail");

            migrationBuilder.AddColumn<int>(
                name: "PriceTierId",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PriceTierId",
                schema: "retail",
                table: "Routes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TenantCustomerProfiles_PriceTierId",
                schema: "retail",
                table: "TenantCustomerProfiles",
                column: "PriceTierId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_PriceTierId",
                schema: "retail",
                table: "Routes",
                column: "PriceTierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_PriceTiers_PriceTierId",
                schema: "retail",
                table: "Routes",
                column: "PriceTierId",
                principalSchema: "retail",
                principalTable: "PriceTiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantCustomerProfiles_PriceTiers_PriceTierId",
                schema: "retail",
                table: "TenantCustomerProfiles",
                column: "PriceTierId",
                principalSchema: "retail",
                principalTable: "PriceTiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_PriceTiers_PriceTierId",
                schema: "retail",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantCustomerProfiles_PriceTiers_PriceTierId",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_TenantCustomerProfiles_PriceTierId",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Routes_PriceTierId",
                schema: "retail",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "PriceTierId",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropColumn(
                name: "PriceTierId",
                schema: "retail",
                table: "Routes");

            migrationBuilder.CreateTable(
                name: "CustomerPriceTiers",
                schema: "retail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PriceTierId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPriceTiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerPriceTiers_PriceTiers_PriceTierId",
                        column: x => x.PriceTierId,
                        principalSchema: "retail",
                        principalTable: "PriceTiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerPriceTiers_TenantCustomerProfiles_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "retail",
                        principalTable: "TenantCustomerProfiles",
                        principalColumn: "TenantUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoutePriceTiers",
                schema: "retail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriceTierId = table.Column<int>(type: "int", nullable: false),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutePriceTiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoutePriceTiers_PriceTiers_PriceTierId",
                        column: x => x.PriceTierId,
                        principalSchema: "retail",
                        principalTable: "PriceTiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoutePriceTiers_Routes_RouteId",
                        column: x => x.RouteId,
                        principalSchema: "retail",
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPriceTiers_CustomerId",
                schema: "retail",
                table: "CustomerPriceTiers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPriceTiers_DataKey",
                schema: "retail",
                table: "CustomerPriceTiers",
                column: "DataKey");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPriceTiers_PriceTierId",
                schema: "retail",
                table: "CustomerPriceTiers",
                column: "PriceTierId");

            migrationBuilder.CreateIndex(
                name: "IX_RoutePriceTiers_DataKey",
                schema: "retail",
                table: "RoutePriceTiers",
                column: "DataKey");

            migrationBuilder.CreateIndex(
                name: "IX_RoutePriceTiers_PriceTierId",
                schema: "retail",
                table: "RoutePriceTiers",
                column: "PriceTierId");

            migrationBuilder.CreateIndex(
                name: "IX_RoutePriceTiers_RouteId",
                schema: "retail",
                table: "RoutePriceTiers",
                column: "RouteId");
        }
    }
}
