using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class UpdatePainterandpainterCallandaddPainterstatuslog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dealer",
                table: "AttachedDealerPainters");

            migrationBuilder.AddColumn<string>(
                name: "PainterNo",
                table: "Painters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SaleGroup",
                table: "PainterCalls",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Territory",
                table: "PainterCalls",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zone",
                table: "PainterCalls",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DealerId",
                table: "AttachedDealerPainters",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PainterNo",
                table: "Painters");

            migrationBuilder.DropColumn(
                name: "SaleGroup",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "Territory",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "Zone",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "DealerId",
                table: "AttachedDealerPainters");

            migrationBuilder.AddColumn<int>(
                name: "Dealer",
                table: "AttachedDealerPainters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
