using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example7.BlazorWASMandWebApi.Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class AddPostDeliveryAccounting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OutstandingBalance",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "decimal(9,2)",
                precision: 9,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "RouteId",
                schema: "retail",
                table: "CustomerOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Ledgers",
                schema: "retail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountType = table.Column<int>(type: "int", nullable: false),
                    LedgerType = table.Column<int>(type: "int", nullable: false),
                    OperationType = table.Column<int>(type: "int", nullable: false),
                    OperationId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    PaymentMode = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PerformedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ledgers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                schema: "retail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QtyPurchased = table.Column<int>(type: "int", nullable: false),
                    QtySold = table.Column<int>(type: "int", nullable: false),
                    Return = table.Column<int>(type: "int", nullable: false),
                    Waste = table.Column<int>(type: "int", nullable: false),
                    InEating = table.Column<int>(type: "int", nullable: false),
                    ItemLoss = table.Column<int>(type: "int", nullable: false),
                    Sample = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stocks_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "retail",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stocks_Routes_RouteId",
                        column: x => x.RouteId,
                        principalSchema: "retail",
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrders_RouteId",
                schema: "retail",
                table: "CustomerOrders",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_AccountId",
                schema: "retail",
                table: "Ledgers",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_AccountId_Date",
                schema: "retail",
                table: "Ledgers",
                columns: new[] { "AccountId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_DataKey",
                schema: "retail",
                table: "Ledgers",
                column: "DataKey");

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_Date",
                schema: "retail",
                table: "Ledgers",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_DataKey",
                schema: "retail",
                table: "Stocks",
                column: "DataKey");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_Date",
                schema: "retail",
                table: "Stocks",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_ProductId",
                schema: "retail",
                table: "Stocks",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_RouteId",
                schema: "retail",
                table: "Stocks",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_RouteId_ProductId_Date",
                schema: "retail",
                table: "Stocks",
                columns: new[] { "RouteId", "ProductId", "Date" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerOrders_Routes_RouteId",
                schema: "retail",
                table: "CustomerOrders",
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
                name: "FK_CustomerOrders_Routes_RouteId",
                schema: "retail",
                table: "CustomerOrders");

            migrationBuilder.DropTable(
                name: "Ledgers",
                schema: "retail");

            migrationBuilder.DropTable(
                name: "Stocks",
                schema: "retail");

            migrationBuilder.DropIndex(
                name: "IX_CustomerOrders_RouteId",
                schema: "retail",
                table: "CustomerOrders");

            migrationBuilder.DropColumn(
                name: "OutstandingBalance",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropColumn(
                name: "RouteId",
                schema: "retail",
                table: "CustomerOrders");
        }
    }
}
