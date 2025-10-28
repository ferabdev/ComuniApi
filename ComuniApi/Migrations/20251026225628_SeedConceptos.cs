using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ComuniApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedConceptos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Conceptos",
                columns: new[] { "Id", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Mantenimiento" },
                    { 2, "Proyecto Extraordinario" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Conceptos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Conceptos",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
