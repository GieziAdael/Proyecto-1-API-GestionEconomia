using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_GestionEconomia.Migrations
{
    /// <inheritdoc />
    public partial class quintaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FKCodigoUsuario_Hash",
                table: "OrganizacionesEconomicas",
                newName: "FKCodigoUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FKCodigoUsuario",
                table: "OrganizacionesEconomicas",
                newName: "FKCodigoUsuario_Hash");
        }
    }
}
