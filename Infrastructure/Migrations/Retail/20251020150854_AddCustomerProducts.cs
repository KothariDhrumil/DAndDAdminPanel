using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example7.BlazorWASMandWebApi.Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class AddCustomerProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerProducts",
                schema: "retail",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SalesRate = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProducts", x => new { x.CustomerId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_CustomerProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "retail",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerProducts_TenantCustomerProfiles_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "retail",
                        principalTable: "TenantCustomerProfiles",
                        principalColumn: "TenantUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProducts_CustomerId",
                schema: "retail",
                table: "CustomerProducts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProducts_DataKey",
                schema: "retail",
                table: "CustomerProducts",
                column: "DataKey");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProducts_IsActive",
                schema: "retail",
                table: "CustomerProducts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProducts_ProductId",
                schema: "retail",
                table: "CustomerProducts",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerProducts",
                schema: "retail");
        }
    }
}
