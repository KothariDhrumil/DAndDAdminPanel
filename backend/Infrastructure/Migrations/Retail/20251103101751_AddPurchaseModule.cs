using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class AddPurchaseModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchase_PurchaseUnits_PurchaseUnitId",
                schema: "retail",
                table: "Purchase");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchase_Routes_RouteId",
                schema: "retail",
                table: "Purchase");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchase_TenantUserProfiles_PickupSalesmanId",
                schema: "retail",
                table: "Purchase");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseDetail_Products_ProductId",
                schema: "retail",
                table: "PurchaseDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseDetail_Purchase_PurchaseId",
                schema: "retail",
                table: "PurchaseDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseDetail",
                schema: "retail",
                table: "PurchaseDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Purchase",
                schema: "retail",
                table: "Purchase");

            migrationBuilder.RenameTable(
                name: "PurchaseDetail",
                schema: "retail",
                newName: "PurchaseDetails",
                newSchema: "retail");

            migrationBuilder.RenameTable(
                name: "Purchase",
                schema: "retail",
                newName: "Purchases",
                newSchema: "retail");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseDetail_PurchaseId",
                schema: "retail",
                table: "PurchaseDetails",
                newName: "IX_PurchaseDetails_PurchaseId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseDetail_ProductId",
                schema: "retail",
                table: "PurchaseDetails",
                newName: "IX_PurchaseDetails_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseDetail_DataKey",
                schema: "retail",
                table: "PurchaseDetails",
                newName: "IX_PurchaseDetails_DataKey");

            migrationBuilder.RenameIndex(
                name: "IX_Purchase_RouteId",
                schema: "retail",
                table: "Purchases",
                newName: "IX_Purchases_RouteId");

            migrationBuilder.RenameIndex(
                name: "IX_Purchase_PurchaseUnitId",
                schema: "retail",
                table: "Purchases",
                newName: "IX_Purchases_PurchaseUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Purchase_PickupSalesmanId",
                schema: "retail",
                table: "Purchases",
                newName: "IX_Purchases_PickupSalesmanId");

            migrationBuilder.RenameIndex(
                name: "IX_Purchase_DataKey",
                schema: "retail",
                table: "Purchases",
                newName: "IX_Purchases_DataKey");

            migrationBuilder.AlterColumn<decimal>(
                name: "Tax",
                schema: "retail",
                table: "Purchases",
                type: "decimal(9,2)",
                precision: 9,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                schema: "retail",
                table: "Purchases",
                type: "decimal(9,2)",
                precision: 9,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AdditionalTax",
                schema: "retail",
                table: "Purchases",
                type: "decimal(9,2)",
                precision: 9,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPreOrder",
                schema: "retail",
                table: "Purchases",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "retail",
                table: "Purchases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseDetails",
                schema: "retail",
                table: "PurchaseDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Purchases",
                schema: "retail",
                table: "Purchases",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_IsConfirmed",
                schema: "retail",
                table: "Purchases",
                column: "IsConfirmed");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_IsPreOrder",
                schema: "retail",
                table: "Purchases",
                column: "IsPreOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_PurchaseDate",
                schema: "retail",
                table: "Purchases",
                column: "PurchaseDate");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_PurchaseUnitId_PurchaseDate",
                schema: "retail",
                table: "Purchases",
                columns: new[] { "PurchaseUnitId", "PurchaseDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_RouteId_PurchaseDate",
                schema: "retail",
                table: "Purchases",
                columns: new[] { "RouteId", "PurchaseDate" });

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseDetails_Products_ProductId",
                schema: "retail",
                table: "PurchaseDetails",
                column: "ProductId",
                principalSchema: "retail",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseDetails_Purchases_PurchaseId",
                schema: "retail",
                table: "PurchaseDetails",
                column: "PurchaseId",
                principalSchema: "retail",
                principalTable: "Purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_PurchaseUnits_PurchaseUnitId",
                schema: "retail",
                table: "Purchases",
                column: "PurchaseUnitId",
                principalSchema: "retail",
                principalTable: "PurchaseUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Routes_RouteId",
                schema: "retail",
                table: "Purchases",
                column: "RouteId",
                principalSchema: "retail",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_TenantUserProfiles_PickupSalesmanId",
                schema: "retail",
                table: "Purchases",
                column: "PickupSalesmanId",
                principalSchema: "retail",
                principalTable: "TenantUserProfiles",
                principalColumn: "TenantUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseDetails_Products_ProductId",
                schema: "retail",
                table: "PurchaseDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseDetails_Purchases_PurchaseId",
                schema: "retail",
                table: "PurchaseDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_PurchaseUnits_PurchaseUnitId",
                schema: "retail",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Routes_RouteId",
                schema: "retail",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_TenantUserProfiles_PickupSalesmanId",
                schema: "retail",
                table: "Purchases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Purchases",
                schema: "retail",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_IsConfirmed",
                schema: "retail",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_IsPreOrder",
                schema: "retail",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_PurchaseDate",
                schema: "retail",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_PurchaseUnitId_PurchaseDate",
                schema: "retail",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_RouteId_PurchaseDate",
                schema: "retail",
                table: "Purchases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseDetails",
                schema: "retail",
                table: "PurchaseDetails");

            migrationBuilder.DropColumn(
                name: "IsPreOrder",
                schema: "retail",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "retail",
                table: "Purchases");

            migrationBuilder.RenameTable(
                name: "Purchases",
                schema: "retail",
                newName: "Purchase",
                newSchema: "retail");

            migrationBuilder.RenameTable(
                name: "PurchaseDetails",
                schema: "retail",
                newName: "PurchaseDetail",
                newSchema: "retail");

            migrationBuilder.RenameIndex(
                name: "IX_Purchases_RouteId",
                schema: "retail",
                table: "Purchase",
                newName: "IX_Purchase_RouteId");

            migrationBuilder.RenameIndex(
                name: "IX_Purchases_PurchaseUnitId",
                schema: "retail",
                table: "Purchase",
                newName: "IX_Purchase_PurchaseUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Purchases_PickupSalesmanId",
                schema: "retail",
                table: "Purchase",
                newName: "IX_Purchase_PickupSalesmanId");

            migrationBuilder.RenameIndex(
                name: "IX_Purchases_DataKey",
                schema: "retail",
                table: "Purchase",
                newName: "IX_Purchase_DataKey");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseDetails_PurchaseId",
                schema: "retail",
                table: "PurchaseDetail",
                newName: "IX_PurchaseDetail_PurchaseId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseDetails_ProductId",
                schema: "retail",
                table: "PurchaseDetail",
                newName: "IX_PurchaseDetail_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseDetails_DataKey",
                schema: "retail",
                table: "PurchaseDetail",
                newName: "IX_PurchaseDetail_DataKey");

            migrationBuilder.AlterColumn<decimal>(
                name: "Tax",
                schema: "retail",
                table: "Purchase",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,2)",
                oldPrecision: 9,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                schema: "retail",
                table: "Purchase",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,2)",
                oldPrecision: 9,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AdditionalTax",
                schema: "retail",
                table: "Purchase",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,2)",
                oldPrecision: 9,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Purchase",
                schema: "retail",
                table: "Purchase",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseDetail",
                schema: "retail",
                table: "PurchaseDetail",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchase_PurchaseUnits_PurchaseUnitId",
                schema: "retail",
                table: "Purchase",
                column: "PurchaseUnitId",
                principalSchema: "retail",
                principalTable: "PurchaseUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchase_Routes_RouteId",
                schema: "retail",
                table: "Purchase",
                column: "RouteId",
                principalSchema: "retail",
                principalTable: "Routes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchase_TenantUserProfiles_PickupSalesmanId",
                schema: "retail",
                table: "Purchase",
                column: "PickupSalesmanId",
                principalSchema: "retail",
                principalTable: "TenantUserProfiles",
                principalColumn: "TenantUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseDetail_Products_ProductId",
                schema: "retail",
                table: "PurchaseDetail",
                column: "ProductId",
                principalSchema: "retail",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseDetail_Purchase_PurchaseId",
                schema: "retail",
                table: "PurchaseDetail",
                column: "PurchaseId",
                principalSchema: "retail",
                principalTable: "Purchase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
