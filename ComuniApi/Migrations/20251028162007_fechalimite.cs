using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComuniApi.Migrations
{
    /// <inheritdoc />
    public partial class fechalimite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaLimite",
                table: "EdoCuentas",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaLimite",
                table: "EdoCuentas");
        }
    }
}
