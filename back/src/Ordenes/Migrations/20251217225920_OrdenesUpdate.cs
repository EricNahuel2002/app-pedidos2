using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordenes.Migrations
{
    /// <inheritdoc />
    public partial class OrdenesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NombreMenu",
                table: "Ordenes",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreMenu",
                table: "Ordenes");
        }
    }
}
