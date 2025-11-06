using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example7.BlazorWASMandWebApi.Infrastructure.Migrations.Identity
{
    /// <inheritdoc />
    public partial class MinorChangeInApplicationDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                table: "Field",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                table: "Field",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Field",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<string>(
                name: "DataKey",
                table: "Field",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataKey",
                table: "Field");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Field",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Field",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Field",
                newName: "Created");
        }
    }
}
