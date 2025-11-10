using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComuniApi.Migrations
{
    /// <inheritdoc />
    public partial class usuarioadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "ComunidadId", "Email", "NombreCompleto", "PasswordHash", "RolId", "Username" },
                values: new object[] { 2, 1, "", "Administrador", "AQAAAAIAAYagAAAAEJMWmqGEeofwL3f2r0uCFpykRHUwRHd2S3axzTA2Ox0AVE1hxv7oB/FeWzQJcPb/aA==", 2, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
