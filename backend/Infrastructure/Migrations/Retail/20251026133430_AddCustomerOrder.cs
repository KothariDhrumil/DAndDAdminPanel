using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example7.BlazorWASMandWebApi.Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class AddCustomerOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerOrders",
                schema: "retail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderPlacedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelivered = table.Column<bool>(type: "bit", nullable: false),
                    SalesManId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: true),
                    Tax = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: true),
                    GrandTotal = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParcelCharge = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: true),
                    IsPreOrder = table.Column<bool>(type: "bit", nullable: false),
                    PayerCustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerOrders_TenantCustomerProfiles_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "retail",
                        principalTable: "TenantCustomerProfiles",
                        principalColumn: "TenantUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerOrders_TenantCustomerProfiles_PayerCustomerId",
                        column: x => x.PayerCustomerId,
                        principalSchema: "retail",
                        principalTable: "TenantCustomerProfiles",
                        principalColumn: "TenantUserId");
                    table.ForeignKey(
                        name: "FK_CustomerOrders_TenantUserProfiles_SalesManId",
                        column: x => x.SalesManId,
                        principalSchema: "retail",
                        principalTable: "TenantUserProfiles",
                        principalColumn: "TenantUserId");
                });

            migrationBuilder.CreateTable(
                name: "CustomerOrderDetails",
                schema: "retail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerOrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    CGST = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: true),
                    IGST = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerOrderDetails_CustomerOrders_CustomerOrderId",
                        column: x => x.CustomerOrderId,
                        principalSchema: "retail",
                        principalTable: "CustomerOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerOrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "retail",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderDetails_CustomerOrderId",
                schema: "retail",
                table: "CustomerOrderDetails",
                column: "CustomerOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderDetails_DataKey",
                schema: "retail",
                table: "CustomerOrderDetails",
                column: "DataKey");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderDetails_ProductId",
                schema: "retail",
                table: "CustomerOrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrders_CustomerId",
                schema: "retail",
                table: "CustomerOrders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrders_DataKey",
                schema: "retail",
                table: "CustomerOrders",
                column: "DataKey");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrders_IsDelivered",
                schema: "retail",
                table: "CustomerOrders",
                column: "IsDelivered");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrders_OrderDeliveryDate",
                schema: "retail",
                table: "CustomerOrders",
                column: "OrderDeliveryDate");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrders_OrderPlacedDate",
                schema: "retail",
                table: "CustomerOrders",
                column: "OrderPlacedDate");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrders_PayerCustomerId",
                schema: "retail",
                table: "CustomerOrders",
                column: "PayerCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrders_SalesManId",
                schema: "retail",
                table: "CustomerOrders",
                column: "SalesManId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerOrderDetails",
                schema: "retail");

            migrationBuilder.DropTable(
                name: "CustomerOrders",
                schema: "retail");
        }
    }
}
