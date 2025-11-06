using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class AddWarehouseStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Return",
                schema: "retail",
                table: "Stocks",
                newName: "QtyReturnedToGodown");

            migrationBuilder.RenameColumn(
                name: "QtyPurchased",
                schema: "retail",
                table: "Stocks",
                newName: "QtyReceivedFromGodown");

            migrationBuilder.RenameColumn(
                name: "ItemLoss",
                schema: "retail",
                table: "Stocks",
                newName: "OpeningBalance");

            migrationBuilder.AddColumn<int>(
                name: "ClosingBalance",
                schema: "retail",
                table: "Stocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Loss",
                schema: "retail",
                table: "Stocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseUnitId",
                schema: "retail",
                table: "Purchases",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "ShippingCost",
                schema: "retail",
                table: "Purchases",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StockTransactions",
                schema: "retail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    RouteId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseStocks",
                schema: "retail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OpeningBalanceQty = table.Column<int>(type: "int", precision: 9, scale: 2, nullable: false),
                    QtyPurchased = table.Column<int>(type: "int", precision: 9, scale: 2, nullable: false),
                    QtyTransferredToRoutes = table.Column<int>(type: "int", precision: 9, scale: 2, nullable: false),
                    RouteReturn = table.Column<int>(type: "int", precision: 9, scale: 2, nullable: false),
                    Waste = table.Column<int>(type: "int", precision: 9, scale: 2, nullable: false),
                    Loss = table.Column<int>(type: "int", precision: 9, scale: 2, nullable: false),
                    Sample = table.Column<int>(type: "int", precision: 9, scale: 2, nullable: false),
                    ClosingBalanceQty = table.Column<int>(type: "int", precision: 9, scale: 2, nullable: false),
                    PurchaseUnitId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseStocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseStocks_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "retail",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseStocks_PurchaseUnits_PurchaseUnitId",
                        column: x => x.PurchaseUnitId,
                        principalSchema: "retail",
                        principalTable: "PurchaseUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_DataKey",
                schema: "retail",
                table: "StockTransactions",
                column: "DataKey");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_Type",
                schema: "retail",
                table: "StockTransactions",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseStocks_DataKey",
                schema: "retail",
                table: "WarehouseStocks",
                column: "DataKey");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseStocks_Date",
                schema: "retail",
                table: "WarehouseStocks",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseStocks_ProductId",
                schema: "retail",
                table: "WarehouseStocks",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseStocks_PurchaseUnitId",
                schema: "retail",
                table: "WarehouseStocks",
                column: "PurchaseUnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockTransactions",
                schema: "retail");

            migrationBuilder.DropTable(
                name: "WarehouseStocks",
                schema: "retail");

            migrationBuilder.DropColumn(
                name: "ClosingBalance",
                schema: "retail",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "Loss",
                schema: "retail",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "ShippingCost",
                schema: "retail",
                table: "Purchases");

            migrationBuilder.RenameColumn(
                name: "QtyReturnedToGodown",
                schema: "retail",
                table: "Stocks",
                newName: "Return");

            migrationBuilder.RenameColumn(
                name: "QtyReceivedFromGodown",
                schema: "retail",
                table: "Stocks",
                newName: "QtyPurchased");

            migrationBuilder.RenameColumn(
                name: "OpeningBalance",
                schema: "retail",
                table: "Stocks",
                newName: "ItemLoss");

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseUnitId",
                schema: "retail",
                table: "Purchases",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
