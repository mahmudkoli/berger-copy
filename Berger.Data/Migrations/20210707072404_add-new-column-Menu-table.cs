using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class addnewcolumnMenutable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Menus");

            migrationBuilder.AddColumn<string>(
                name: "ActivityId",
                table: "Menus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackgroundColor",
                table: "Menus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupCode",
                table: "Menus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "GroupCode",
                table: "Menus");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
