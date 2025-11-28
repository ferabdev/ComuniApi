using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComuniApi.Migrations
{
    /// <inheritdoc />
    public partial class Votaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Foros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Votacion",
                table: "Foros",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ForoVotacionOpciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ForoId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Votos = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForoVotacionOpciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForoVotacionOpciones_Foros_ForoId",
                        column: x => x.ForoId,
                        principalTable: "Foros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ForosVotosUsuarios",
                columns: table => new
                {
                    ForoId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    OpcionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForosVotosUsuarios", x => new { x.ForoId, x.UsuarioId });
                    table.ForeignKey(
                        name: "FK_ForosVotosUsuarios_ForoVotacionOpciones_OpcionId",
                        column: x => x.OpcionId,
                        principalTable: "ForoVotacionOpciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ForosVotosUsuarios_Foros_ForoId",
                        column: x => x.ForoId,
                        principalTable: "Foros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ForosVotosUsuarios_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Foros_UsuarioId",
                table: "Foros",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ForosVotosUsuarios_ForoId_UsuarioId",
                table: "ForosVotosUsuarios",
                columns: new[] { "ForoId", "UsuarioId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ForosVotosUsuarios_OpcionId",
                table: "ForosVotosUsuarios",
                column: "OpcionId");

            migrationBuilder.CreateIndex(
                name: "IX_ForosVotosUsuarios_UsuarioId",
                table: "ForosVotosUsuarios",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ForoVotacionOpciones_ForoId",
                table: "ForoVotacionOpciones",
                column: "ForoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Foros_Usuarios_UsuarioId",
                table: "Foros",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Foros_Usuarios_UsuarioId",
                table: "Foros");

            migrationBuilder.DropTable(
                name: "ForosVotosUsuarios");

            migrationBuilder.DropTable(
                name: "ForoVotacionOpciones");

            migrationBuilder.DropIndex(
                name: "IX_Foros_UsuarioId",
                table: "Foros");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Foros");

            migrationBuilder.DropColumn(
                name: "Votacion",
                table: "Foros");
        }
    }
}
