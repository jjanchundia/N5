using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace N5.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class idpermisoE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdPermisoE",
                table: "Permiso",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdPermisoE",
                table: "Permiso");
        }
    }
}
