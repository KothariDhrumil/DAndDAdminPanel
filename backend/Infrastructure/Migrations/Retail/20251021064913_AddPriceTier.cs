using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example7.BlazorWASMandWebApi.Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class AddPriceTier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceTiers",
                schema: "retail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceTiers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerPriceTiers",
                schema: "retail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PriceTierId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
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
                name: "PriceTierProducts",
                schema: "retail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriceTierId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SalesRate = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceTierProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceTierProducts_PriceTiers_PriceTierId",
                        column: x => x.PriceTierId,
                        principalSchema: "retail",
                        principalTable: "PriceTiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriceTierProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "retail",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoutePriceTiers",
                schema: "retail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    PriceTierId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
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
                name: "IX_PriceTierProducts_DataKey",
                schema: "retail",
                table: "PriceTierProducts",
                column: "DataKey");

            migrationBuilder.CreateIndex(
                name: "IX_PriceTierProducts_PriceTierId_ProductId",
                schema: "retail",
                table: "PriceTierProducts",
                columns: new[] { "PriceTierId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_PriceTierProducts_ProductId",
                schema: "retail",
                table: "PriceTierProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceTiers_DataKey",
                schema: "retail",
                table: "PriceTiers",
                column: "DataKey");

            migrationBuilder.CreateIndex(
                name: "IX_PriceTiers_IsActive",
                schema: "retail",
                table: "PriceTiers",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_PriceTiers_Name",
                schema: "retail",
                table: "PriceTiers",
                column: "Name");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerPriceTiers",
                schema: "retail");

            migrationBuilder.DropTable(
                name: "PriceTierProducts",
                schema: "retail");

            migrationBuilder.DropTable(
                name: "RoutePriceTiers",
                schema: "retail");

            migrationBuilder.DropTable(
                name: "PriceTiers",
                schema: "retail");
        }
    }
}
