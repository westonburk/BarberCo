using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberCo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangesForSMSConfirmationFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "confirmation_code_hash",
                table: "appointments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "confirmation_failed",
                table: "appointments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "confirmed_on",
                table: "appointments",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "appointments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_on",
                table: "appointments",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "confirmation_code_hash",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "confirmation_failed",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "confirmed_on",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "created_on",
                table: "appointments");
        }
    }
}
