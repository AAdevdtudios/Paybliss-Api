using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Paybliss.Migrations
{
    /// <inheritdoc />
    public partial class tier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "tier",
                table: "User",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tier",
                table: "User");
        }
    }
}
