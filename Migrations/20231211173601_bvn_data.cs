using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Paybliss.Migrations
{
    /// <inheritdoc />
    public partial class bvn_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "bvn",
                table: "User",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bvn",
                table: "User");
        }
    }
}
