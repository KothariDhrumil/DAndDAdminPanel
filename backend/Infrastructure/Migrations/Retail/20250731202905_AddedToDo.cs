using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example7.BlazorWASMandWebApi.Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class AddedToDo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopSales",
                schema: "retail");

            migrationBuilder.DropTable(
                name: "ShopStocks",
                schema: "retail");

            migrationBuilder.CreateTable(
                name: "TodoItems",
                schema: "retail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Labels = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItems", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_DataKey",
                schema: "retail",
                table: "TodoItems",
                column: "DataKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoItems",
                schema: "retail");

            migrationBuilder.CreateTable(
                name: "ShopStocks",
                schema: "retail",
                columns: table => new
                {
                    ShopStockId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantItemId = table.Column<int>(type: "int", nullable: false),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    NumInStock = table.Column<int>(type: "int", nullable: false),
                    RetailPrice = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    StockName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopStocks", x => x.ShopStockId);
                    table.ForeignKey(
                        name: "FK_ShopStocks_RetailOutlets_TenantItemId",
                        column: x => x.TenantItemId,
                        principalSchema: "retail",
                        principalTable: "RetailOutlets",
                        principalColumn: "RetailOutletId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopSales",
                schema: "retail",
                columns: table => new
                {
                    ShopSaleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopStockId = table.Column<int>(type: "int", nullable: false),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    NumSoldReturned = table.Column<int>(type: "int", nullable: false),
                    ReturnReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopSales", x => x.ShopSaleId);
                    table.ForeignKey(
                        name: "FK_ShopSales_ShopStocks_ShopStockId",
                        column: x => x.ShopStockId,
                        principalSchema: "retail",
                        principalTable: "ShopStocks",
                        principalColumn: "ShopStockId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopSales_DataKey",
                schema: "retail",
                table: "ShopSales",
                column: "DataKey");

            migrationBuilder.CreateIndex(
                name: "IX_ShopSales_ShopStockId",
                schema: "retail",
                table: "ShopSales",
                column: "ShopStockId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopStocks_DataKey",
                schema: "retail",
                table: "ShopStocks",
                column: "DataKey");

            migrationBuilder.CreateIndex(
                name: "IX_ShopStocks_TenantItemId",
                schema: "retail",
                table: "ShopStocks",
                column: "TenantItemId");
        }
    }
}
