using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class AddNewFieldsToPlayerAndTeamEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Teams",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Money",
                table: "Teams",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TeamValue",
                table: "Teams",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Players",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "Players",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Money",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "TeamValue",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Players");
        }
    }
}
