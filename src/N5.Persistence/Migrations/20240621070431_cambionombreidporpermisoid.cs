using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace N5.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class cambionombreidporpermisoid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Permiso",
                newName: "PermisoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PermisoId",
                table: "Permiso",
                newName: "Id");
        }
    }
}
