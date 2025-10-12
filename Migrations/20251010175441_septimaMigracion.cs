using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_GestionEconomia.Migrations
{
    /// <inheritdoc />
    public partial class septimaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizadores_Usuarios_UsuarioId",
                table: "Organizadores");

            migrationBuilder.DropIndex(
                name: "IX_Organizadores_UsuarioId",
                table: "Organizadores");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Organizadores");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Organizadores",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizadores_UsuarioId",
                table: "Organizadores",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizadores_Usuarios_UsuarioId",
                table: "Organizadores",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }
    }
}
