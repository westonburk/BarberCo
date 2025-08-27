using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberCo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsClosedtoHourstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "Hours",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "Hours");
        }
    }
}
