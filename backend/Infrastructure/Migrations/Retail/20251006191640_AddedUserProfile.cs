using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example7.BlazorWASMandWebApi.Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class AddedUserProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_TenantCustomerProfiles_TenantCustomerId",
                schema: "retail",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TenantCustomerProfiles",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_TenantCustomerProfiles_GlobalCustomerId_DataKey",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Orders_TenantCustomerId_DataKey",
                schema: "retail",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TenantCustomerId",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropColumn(
                name: "TenantCustomerId",
                schema: "retail",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantUserId",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                schema: "retail",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_TenantCustomerProfiles",
                schema: "retail",
                table: "TenantCustomerProfiles",
                column: "TenantUserId");

            migrationBuilder.CreateTable(
                name: "LedgerEntries",
                schema: "retail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ReferenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LedgerEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TenantUserProfiles",
                schema: "retail",
                columns: table => new
                {
                    TenantUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    GlobalUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleType = table.Column<int>(type: "int", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantUserProfiles", x => x.TenantUserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TenantCustomerProfiles_CreatedAt",
                schema: "retail",
                table: "TenantCustomerProfiles",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TenantCustomerProfiles_GlobalCustomerId_TenantId",
                schema: "retail",
                table: "TenantCustomerProfiles",
                columns: new[] { "GlobalCustomerId", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TenantCustomerProfiles_UpdatedAt",
                schema: "retail",
                table: "TenantCustomerProfiles",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId_DataKey",
                schema: "retail",
                table: "Orders",
                columns: new[] { "CustomerId", "DataKey" });

            migrationBuilder.CreateIndex(
                name: "IX_LedgerEntries_DataKey",
                schema: "retail",
                table: "LedgerEntries",
                column: "DataKey");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerEntries_EntryDate",
                schema: "retail",
                table: "LedgerEntries",
                column: "EntryDate");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerEntries_TenantUserId_EntryDate",
                schema: "retail",
                table: "LedgerEntries",
                columns: new[] { "TenantUserId", "EntryDate" });

            migrationBuilder.CreateIndex(
                name: "IX_TenantUserProfiles_CreatedAt",
                schema: "retail",
                table: "TenantUserProfiles",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TenantUserProfiles_DataKey",
                schema: "retail",
                table: "TenantUserProfiles",
                column: "DataKey");

            migrationBuilder.CreateIndex(
                name: "IX_TenantUserProfiles_GlobalUserId_TenantId",
                schema: "retail",
                table: "TenantUserProfiles",
                columns: new[] { "GlobalUserId", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TenantUserProfiles_RoleType",
                schema: "retail",
                table: "TenantUserProfiles",
                column: "RoleType");

            migrationBuilder.CreateIndex(
                name: "IX_TenantUserProfiles_TenantId",
                schema: "retail",
                table: "TenantUserProfiles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantUserProfiles_UpdatedAt",
                schema: "retail",
                table: "TenantUserProfiles",
                column: "UpdatedAt");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_TenantCustomerProfiles_CustomerId",
                schema: "retail",
                table: "Orders",
                column: "CustomerId",
                principalSchema: "retail",
                principalTable: "TenantCustomerProfiles",
                principalColumn: "TenantUserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_TenantCustomerProfiles_CustomerId",
                schema: "retail",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "LedgerEntries",
                schema: "retail");

            migrationBuilder.DropTable(
                name: "TenantUserProfiles",
                schema: "retail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TenantCustomerProfiles",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_TenantCustomerProfiles_CreatedAt",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_TenantCustomerProfiles_GlobalCustomerId_TenantId",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_TenantCustomerProfiles_UpdatedAt",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerId_DataKey",
                schema: "retail",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TenantUserId",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "retail",
                table: "TenantCustomerProfiles");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "retail",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "TenantCustomerId",
                schema: "retail",
                table: "TenantCustomerProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "TenantCustomerId",
                schema: "retail",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TenantCustomerProfiles",
                schema: "retail",
                table: "TenantCustomerProfiles",
                column: "TenantCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantCustomerProfiles_GlobalCustomerId_DataKey",
                schema: "retail",
                table: "TenantCustomerProfiles",
                columns: new[] { "GlobalCustomerId", "DataKey" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TenantCustomerId_DataKey",
                schema: "retail",
                table: "Orders",
                columns: new[] { "TenantCustomerId", "DataKey" });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_TenantCustomerProfiles_TenantCustomerId",
                schema: "retail",
                table: "Orders",
                column: "TenantCustomerId",
                principalSchema: "retail",
                principalTable: "TenantCustomerProfiles",
                principalColumn: "TenantCustomerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
