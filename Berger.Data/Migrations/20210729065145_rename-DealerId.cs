using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class renameDealerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "DealerId",
                table: "Payments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DealerId",
                table: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
