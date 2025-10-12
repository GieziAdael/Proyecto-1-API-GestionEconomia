using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_GestionEconomia.Migrations
{
    /// <inheritdoc />
    public partial class cuartaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CodigoUsuario_Hash",
                table: "Usuarios",
                newName: "CodigoUsuario");

            migrationBuilder.RenameColumn(
                name: "FKCodigoEconomia_Hash",
                table: "Organizadores",
                newName: "FKCodigoEconomia");

            migrationBuilder.RenameColumn(
                name: "CodigoEconomia_Hash",
                table: "OrganizacionesEconomicas",
                newName: "CodigoEconomia");

            migrationBuilder.RenameColumn(
                name: "FKCodigoEconomia_Hash",
                table: "Economias",
                newName: "FKCodigoEconomia");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CodigoUsuario",
                table: "Usuarios",
                newName: "CodigoUsuario_Hash");

            migrationBuilder.RenameColumn(
                name: "FKCodigoEconomia",
                table: "Organizadores",
                newName: "FKCodigoEconomia_Hash");

            migrationBuilder.RenameColumn(
                name: "CodigoEconomia",
                table: "OrganizacionesEconomicas",
                newName: "CodigoEconomia_Hash");

            migrationBuilder.RenameColumn(
                name: "FKCodigoEconomia",
                table: "Economias",
                newName: "FKCodigoEconomia_Hash");
        }
    }
}
