using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComuniApi.Migrations
{
    /// <inheritdoc />
    public partial class superadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Descripcion" },
                values: new object[] { 3, "Super Administrador" });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "ComunidadId", "Email", "NombreCompleto", "PasswordHash", "RolId", "Username" },
                values: new object[] { 1, 1, "", "Administrador Principal", "AQAAAAIAAYagAAAAEEcpKfy6XatUxQmYnTp6GeasWJ4MbuTSE2+amRofhH+t7QnJf0S2WrBJLMbTGClb4Q==", 3, "administrador" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
