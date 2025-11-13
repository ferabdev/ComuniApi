using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ComuniApi.Migrations
{
    /// <inheritdoc />
    public partial class Seed_de_estatus_reportes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reportes_ReporteEstatusEntity_EstatusId",
                table: "Reportes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReporteEstatusEntity",
                table: "ReporteEstatusEntity");

            migrationBuilder.RenameTable(
                name: "ReporteEstatusEntity",
                newName: "ReportesEstatus");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "ReportesEstatus",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportesEstatus",
                table: "ReportesEstatus",
                column: "Id");

            migrationBuilder.InsertData(
                table: "ReportesEstatus",
                columns: new[] { "Id", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Pendiente" },
                    { 2, "En Proceso" },
                    { 3, "Resuelto" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Reportes_ReportesEstatus_EstatusId",
                table: "Reportes",
                column: "EstatusId",
                principalTable: "ReportesEstatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reportes_ReportesEstatus_EstatusId",
                table: "Reportes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportesEstatus",
                table: "ReportesEstatus");

            migrationBuilder.DeleteData(
                table: "ReportesEstatus",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ReportesEstatus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ReportesEstatus",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.RenameTable(
                name: "ReportesEstatus",
                newName: "ReporteEstatusEntity");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "ReporteEstatusEntity",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReporteEstatusEntity",
                table: "ReporteEstatusEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reportes_ReporteEstatusEntity_EstatusId",
                table: "Reportes",
                column: "EstatusId",
                principalTable: "ReporteEstatusEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
