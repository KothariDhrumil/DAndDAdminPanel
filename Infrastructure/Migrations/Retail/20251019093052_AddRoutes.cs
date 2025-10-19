using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example7.BlazorWASMandWebApi.Infrastructure.Migrations.Retail
{
    /// <inheritdoc />
    public partial class AddRoutes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Routes",
                schema: "retail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeantUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DataKey = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Routes_TenantUserProfiles_TenantUserId",
                        column: x => x.TenantUserId,
                        principalSchema: "retail",
                        principalTable: "TenantUserProfiles",
                        principalColumn: "TenantUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Routes_DataKey",
                schema: "retail",
                table: "Routes",
                column: "DataKey");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_Id",
                schema: "retail",
                table: "Routes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_IsActive",
                schema: "retail",
                table: "Routes",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_TeantUserId",
                schema: "retail",
                table: "Routes",
                column: "TeantUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_TenantUserId",
                schema: "retail",
                table: "Routes",
                column: "TenantUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Routes",
                schema: "retail");
        }
    }
}
