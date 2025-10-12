using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_GestionEconomia.Migrations
{
    /// <inheritdoc />
    public partial class sextaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FKIdUsuario",
                table: "Organizadores",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FKIdUsuario",
                table: "Organizadores");
        }
    }
}
