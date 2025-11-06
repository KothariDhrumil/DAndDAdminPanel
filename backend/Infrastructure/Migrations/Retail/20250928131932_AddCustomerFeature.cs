using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example7.BlazorWASMandWebApi.Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class AddCustomerFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                schema: "retail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    GlobalCustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TenantCustomerProfiles",
                schema: "retail",
                columns: table => new
                {
                    TenantCustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GlobalCustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantCustomerProfiles", x => x.TenantCustomerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DataKey",
                schema: "retail",
                table: "Orders",
                column: "DataKey");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_GlobalCustomerId_DataKey",
                schema: "retail",
                table: "Orders",
                columns: new[] { "GlobalCustomerId", "DataKey" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderedAt",
                schema: "retail",
                table: "Orders",
                column: "OrderedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TenantCustomerProfiles_DataKey",
                schema: "retail",
                table: "TenantCustomerProfiles",
                column: "DataKey");

            migrationBuilder.CreateIndex(
                name: "IX_TenantCustomerProfiles_GlobalCustomerId_DataKey",
                schema: "retail",
                table: "TenantCustomerProfiles",
                columns: new[] { "GlobalCustomerId", "DataKey" });

            migrationBuilder.CreateIndex(
                name: "IX_TenantCustomerProfiles_TenantId",
                schema: "retail",
                table: "TenantCustomerProfiles",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders",
                schema: "retail");

            migrationBuilder.DropTable(
                name: "TenantCustomerProfiles",
                schema: "retail");
        }
    }
}
