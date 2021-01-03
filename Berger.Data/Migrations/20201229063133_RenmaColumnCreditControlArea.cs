using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class RenmaColumnCreditControlArea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditControlArea1",
                table: "CreditControlAreas");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "CreditControlAreas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "CreditControlAreas");

            migrationBuilder.AddColumn<double>(
                name: "CreditControlArea1",
                table: "CreditControlAreas",
                type: "float",
                nullable: true);
        }
    }
}
