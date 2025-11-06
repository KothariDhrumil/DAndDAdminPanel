using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example7.BlazorWASMandWebApi.Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class AddPurchaseUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseUnits",
                schema: "retail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsInternal = table.Column<bool>(type: "bit", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTaxable = table.Column<bool>(type: "bit", nullable: false),
                    TenantUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseUnits_TenantUserProfiles_TenantUserId",
                        column: x => x.TenantUserId,
                        principalSchema: "retail",
                        principalTable: "TenantUserProfiles",
                        principalColumn: "TenantUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Purchase",
                schema: "retail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteId = table.Column<int>(type: "int", nullable: true),
                    PurchaseUnitId = table.Column<int>(type: "int", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderPickupDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdditionalTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GrandTotal = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PickupSalesmanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchase_PurchaseUnits_PurchaseUnitId",
                        column: x => x.PurchaseUnitId,
                        principalSchema: "retail",
                        principalTable: "PurchaseUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Purchase_Routes_RouteId",
                        column: x => x.RouteId,
                        principalSchema: "retail",
                        principalTable: "Routes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Purchase_TenantUserProfiles_PickupSalesmanId",
                        column: x => x.PickupSalesmanId,
                        principalSchema: "retail",
                        principalTable: "TenantUserProfiles",
                        principalColumn: "TenantUserId");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseUnitProduct",
                schema: "retail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseUnitId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    PurchaseRate = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseUnitProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseUnitProduct_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "retail",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseUnitProduct_PurchaseUnits_PurchaseUnitId",
                        column: x => x.PurchaseUnitId,
                        principalSchema: "retail",
                        principalTable: "PurchaseUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseDetail",
                schema: "retail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
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
                    table.PrimaryKey("PK_PurchaseDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseDetail_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "retail",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseDetail_Purchase_PurchaseId",
                        column: x => x.PurchaseId,
                        principalSchema: "retail",
                        principalTable: "Purchase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_DataKey",
                schema: "retail",
                table: "Purchase",
                column: "DataKey");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_PickupSalesmanId",
                schema: "retail",
                table: "Purchase",
                column: "PickupSalesmanId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_PurchaseUnitId",
                schema: "retail",
                table: "Purchase",
                column: "PurchaseUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_RouteId",
                schema: "retail",
                table: "Purchase",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDetail_DataKey",
                schema: "retail",
                table: "PurchaseDetail",
                column: "DataKey");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDetail_ProductId",
                schema: "retail",
                table: "PurchaseDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDetail_PurchaseId",
                schema: "retail",
                table: "PurchaseDetail",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseUnitProduct_DataKey",
                schema: "retail",
                table: "PurchaseUnitProduct",
                column: "DataKey");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseUnitProduct_ProductId",
                schema: "retail",
                table: "PurchaseUnitProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseUnitProduct_PurchaseUnitId",
                schema: "retail",
                table: "PurchaseUnitProduct",
                column: "PurchaseUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseUnits_DataKey",
                schema: "retail",
                table: "PurchaseUnits",
                column: "DataKey");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseUnits_IsInternal",
                schema: "retail",
                table: "PurchaseUnits",
                column: "IsInternal");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseUnits_IsTaxable",
                schema: "retail",
                table: "PurchaseUnits",
                column: "IsTaxable");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseUnits_Name",
                schema: "retail",
                table: "PurchaseUnits",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseUnits_TenantUserId",
                schema: "retail",
                table: "PurchaseUnits",
                column: "TenantUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseDetail",
                schema: "retail");

            migrationBuilder.DropTable(
                name: "PurchaseUnitProduct",
                schema: "retail");

            migrationBuilder.DropTable(
                name: "Purchase",
                schema: "retail");

            migrationBuilder.DropTable(
                name: "PurchaseUnits",
                schema: "retail");
        }
    }
}
