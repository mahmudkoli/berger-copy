using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class updatecolumnMenutable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "Menus");

            migrationBuilder.AddColumn<string>(
                name: "ActivityNavigationId",
                table: "Menus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityNavigationId",
                table: "Menus");

            migrationBuilder.AddColumn<string>(
                name: "ActivityId",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
