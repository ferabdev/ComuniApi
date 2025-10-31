using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComuniApi.Migrations
{
    /// <inheritdoc />
    public partial class Comunidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ComunidadId",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "Comunidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Direccion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Correo = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaxUsers = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comunidades", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Comunidades",
                columns: new[] { "Id", "Correo", "Direccion", "MaxUsers", "Nombre" },
                values: new object[] { 1, "", "Direccion test", 0, "Comunidad test" });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_ComunidadId",
                table: "Usuarios",
                column: "ComunidadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Comunidades_ComunidadId",
                table: "Usuarios",
                column: "ComunidadId",
                principalTable: "Comunidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Comunidades_ComunidadId",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "Comunidades");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_ComunidadId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "ComunidadId",
                table: "Usuarios");
        }
    }
}
