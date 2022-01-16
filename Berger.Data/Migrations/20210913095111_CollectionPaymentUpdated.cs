using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class CollectionPaymentUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Depot",
                table: "Payments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Territory",
                table: "Payments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zone",
                table: "Payments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Depot",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Territory",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Zone",
                table: "Payments");
        }
    }
}
