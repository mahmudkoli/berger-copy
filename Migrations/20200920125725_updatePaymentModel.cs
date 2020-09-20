using Microsoft.EntityFrameworkCore.Migrations;

namespace BergerMsfaApi.Migrations
{
    public partial class updatePaymentModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Account",
                table: "Payments");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Payments",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "Account",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
