using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_GestionEconomia.Migrations
{
    /// <inheritdoc />
    public partial class primeraMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrganizacionesEconomicas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreOrg = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordOrg_Hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoEconomia = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizacionesEconomicas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    Password_Hash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Economias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumMovimiento = table.Column<int>(type: "int", nullable: false),
                    DescripcionMovimiento = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IngresoEgreso = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FechaMovimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrganizacionEconomicaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Economias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Economias_OrganizacionesEconomicas_OrganizacionEconomicaId",
                        column: x => x.OrganizacionEconomicaId,
                        principalTable: "OrganizacionesEconomicas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Organizadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tipoUsuario = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: true),
                    OrganizacionesEconomicasId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizadores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizadores_OrganizacionesEconomicas_OrganizacionesEconomicasId",
                        column: x => x.OrganizacionesEconomicasId,
                        principalTable: "OrganizacionesEconomicas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Organizadores_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Economias_OrganizacionEconomicaId",
                table: "Economias",
                column: "OrganizacionEconomicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizadores_OrganizacionesEconomicasId",
                table: "Organizadores",
                column: "OrganizacionesEconomicasId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizadores_UsuarioId",
                table: "Organizadores",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Economias");

            migrationBuilder.DropTable(
                name: "Organizadores");

            migrationBuilder.DropTable(
                name: "OrganizacionesEconomicas");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
