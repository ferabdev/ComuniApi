using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComuniApi.Migrations
{
    /// <inheritdoc />
    public partial class conceptoid_edctas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConceptoId",
                table: "EdoCuentas",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENdKRNY7Pi/okq/nwdtVGyv8nBnfQYVWghN3KwoVHf5yqBooKxBtraDW1OIoCFagbg==");

            migrationBuilder.CreateIndex(
                name: "IX_EdoCuentas_ConceptoId",
                table: "EdoCuentas",
                column: "ConceptoId");

            migrationBuilder.AddForeignKey(
                name: "FK_EdoCuentas_Conceptos_ConceptoId",
                table: "EdoCuentas",
                column: "ConceptoId",
                principalTable: "Conceptos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EdoCuentas_Conceptos_ConceptoId",
                table: "EdoCuentas");

            migrationBuilder.DropIndex(
                name: "IX_EdoCuentas_ConceptoId",
                table: "EdoCuentas");

            migrationBuilder.DropColumn(
                name: "ConceptoId",
                table: "EdoCuentas");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEcpKfy6XatUxQmYnTp6GeasWJ4MbuTSE2+amRofhH+t7QnJf0S2WrBJLMbTGClb4Q==");
        }
    }
}
