using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class RenameColumnDealerOpening : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeId",
                table: "DealerOpenings");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "DealerOpenings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "DealerOpenings");

            migrationBuilder.AddColumn<string>(
                name: "EmployeId",
                table: "DealerOpenings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
