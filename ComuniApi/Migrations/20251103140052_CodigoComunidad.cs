using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComuniApi.Migrations
{
    /// <inheritdoc />
    public partial class CodigoComunidad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoComunidad",
                table: "Comunidades",
                type: "varchar(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "-")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Comunidades",
                keyColumn: "Id",
                keyValue: 1,
                column: "CodigoComunidad",
                value: "WUEBOS");

            migrationBuilder.CreateIndex(
                name: "IX_Comunidades_CodigoComunidad",
                table: "Comunidades",
                column: "CodigoComunidad",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comunidades_CodigoComunidad",
                table: "Comunidades");

            migrationBuilder.DropColumn(
                name: "CodigoComunidad",
                table: "Comunidades");
        }
    }
}
