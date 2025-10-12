using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_GestionEconomia.Migrations
{
    /// <inheritdoc />
    public partial class segundaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Economias_OrganizacionesEconomicas_OrganizacionEconomicaId",
                table: "Economias");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizadores_OrganizacionesEconomicas_OrganizacionesEconomicasId",
                table: "Organizadores");

            migrationBuilder.DropIndex(
                name: "IX_Organizadores_OrganizacionesEconomicasId",
                table: "Organizadores");

            migrationBuilder.DropIndex(
                name: "IX_Economias_OrganizacionEconomicaId",
                table: "Economias");

            migrationBuilder.DropColumn(
                name: "OrganizacionesEconomicasId",
                table: "Organizadores");

            migrationBuilder.DropColumn(
                name: "OrganizacionEconomicaId",
                table: "Economias");

            migrationBuilder.AddColumn<string>(
                name: "CodigoUsuario",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FKCodigoEconomia",
                table: "Organizadores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FKCodigoUsuario",
                table: "OrganizacionesEconomicas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FKCodigoEconomia",
                table: "Economias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoUsuario",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "FKCodigoEconomia",
                table: "Organizadores");

            migrationBuilder.DropColumn(
                name: "FKCodigoUsuario",
                table: "OrganizacionesEconomicas");

            migrationBuilder.DropColumn(
                name: "FKCodigoEconomia",
                table: "Economias");

            migrationBuilder.AddColumn<int>(
                name: "OrganizacionesEconomicasId",
                table: "Organizadores",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizacionEconomicaId",
                table: "Economias",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizadores_OrganizacionesEconomicasId",
                table: "Organizadores",
                column: "OrganizacionesEconomicasId");

            migrationBuilder.CreateIndex(
                name: "IX_Economias_OrganizacionEconomicaId",
                table: "Economias",
                column: "OrganizacionEconomicaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Economias_OrganizacionesEconomicas_OrganizacionEconomicaId",
                table: "Economias",
                column: "OrganizacionEconomicaId",
                principalTable: "OrganizacionesEconomicas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizadores_OrganizacionesEconomicas_OrganizacionesEconomicasId",
                table: "Organizadores",
                column: "OrganizacionesEconomicasId",
                principalTable: "OrganizacionesEconomicas",
                principalColumn: "Id");
        }
    }
}
