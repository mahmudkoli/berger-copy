using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class renameFocusDealerEmployeeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeRegId",
                table: "FocusDealers");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "FocusDealers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "FocusDealers");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeRegId",
                table: "FocusDealers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
