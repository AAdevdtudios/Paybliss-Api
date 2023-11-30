using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Paybliss.Migrations
{
    /// <inheritdoc />
    public partial class bvnVals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "custormerId",
                table: "User",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "accountName",
                table: "AccountDetails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "bank",
                table: "AccountDetails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "bank_name",
                table: "AccountDetails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "currency",
                table: "AccountDetails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "reference",
                table: "AccountDetails",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "custormerId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "accountName",
                table: "AccountDetails");

            migrationBuilder.DropColumn(
                name: "bank",
                table: "AccountDetails");

            migrationBuilder.DropColumn(
                name: "bank_name",
                table: "AccountDetails");

            migrationBuilder.DropColumn(
                name: "currency",
                table: "AccountDetails");

            migrationBuilder.DropColumn(
                name: "reference",
                table: "AccountDetails");
        }
    }
}
