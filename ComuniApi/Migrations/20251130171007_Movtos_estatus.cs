using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ComuniApi.Migrations
{
    /// <inheritdoc />
    public partial class Movtos_estatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstatusId",
                table: "EdoCuentas",
                type: "int",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.CreateTable(
                name: "MovtosEstatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovtosEstatus", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "MovtosEstatus",
                columns: new[] { "Id", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Creado" },
                    { 2, "Confirmado" },
                    { 3, "Cancelado" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EdoCuentas_EstatusId",
                table: "EdoCuentas",
                column: "EstatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_EdoCuentas_MovtosEstatus_EstatusId",
                table: "EdoCuentas",
                column: "EstatusId",
                principalTable: "MovtosEstatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EdoCuentas_MovtosEstatus_EstatusId",
                table: "EdoCuentas");

            migrationBuilder.DropTable(
                name: "MovtosEstatus");

            migrationBuilder.DropIndex(
                name: "IX_EdoCuentas_EstatusId",
                table: "EdoCuentas");

            migrationBuilder.DropColumn(
                name: "EstatusId",
                table: "EdoCuentas");
        }
    }
}
